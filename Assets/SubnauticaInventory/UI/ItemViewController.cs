﻿using System;
using SubnauticaInventory.DataModel;
using SubnauticaInventory.UI.Tooltips;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SubnauticaInventory.UI
{
	public partial class ItemViewController : SimpleUiBehavior, IDropTarget
	{
		[SerializeField] private Image raycastTarget;
		[SerializeField] private FreeModifier[] freeModifiers;
		[SerializeField] private Image itemImage;
		
		private InventoryViewController _owner;
		private bool _isDragging;
		
		public ItemData ItemData { get; private set; }
		
		public void SetData(ItemData itemData, InventoryViewController inventoryViewController)
		{
			ItemData = itemData;
			_owner = inventoryViewController;
			itemImage.sprite = itemData.sprite;
			SetSize(inventoryViewController);
		}

		/// <summary>
		/// Sets this object's size based on the cell dimensions of the given <see cref="inventoryViewController"/> 
		/// </summary>
		private void SetSize(InventoryViewController inventoryViewController)
		{
			float cellSize = inventoryViewController.CellSize;
			float spacing = inventoryViewController.Spacing;
			float borderRadius = (cellSize - spacing) / 2;
			
			Vector2 viewSize = cellSize * (Vector2)ItemData.GetDimensions();
			viewSize -= spacing * Vector2.one;
			
			RectTransform.sizeDelta = viewSize;

			raycastTarget.raycastPadding = -spacing / 2 * Vector4.one;

			foreach (FreeModifier borderModifier in freeModifiers)
				borderModifier.Radius = Vector4.one * borderRadius;
		}

		public void OnInteractionStateChanged(InteractionState oldState, InteractionState newState)
		{
			if(newState == InteractionState.PointerOver)
				ItemTooltipsStaticManager.Show(this);
			
			if (oldState == InteractionState.PointerOver)
				ItemTooltipsStaticManager.Hide(this);
			
		}

		public void OnHoverPositionChanged(PointerEventData eventData)
		{
			ItemTooltipsStaticManager.UpdatePosition(this, eventData);
		}

		public void OnClick(PointerEventData eventData)
		{
			InventoryViewController target = _owner.TransferTarget;
			
			if (target.InventoryData.RequestAdd(ItemData))
			{
				_owner.InventoryData.Remove(ItemData);
				_owner.Refresh();
				target.Refresh();
			}
		}

		public void OnDragStart(PointerEventData eventData)
		{
			Debug.Log("on drag start");
			_isDragging = true;
		}
		
		public void OnDragEnd(PointerEventData eventData)
		{
			Debug.Log("on drag end");
			_isDragging = false;
			_owner.Refresh();
		}

		public void OnDragUpdate(PointerEventData eventData)
		{
			Vector2 raycastLocalPos = transform.parent.InverseTransformPoint(eventData.pointerCurrentRaycast.worldPosition);
			RectTransform.anchoredPosition = raycastLocalPos + RectTransform.sizeDelta.Multiply(-.5f, .5f);
		}

		public string Name => gameObject.name;
	}
}