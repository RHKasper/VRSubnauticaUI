﻿using System;
using System.Collections.Generic;
using System.Linq;
using SubnauticaInventory.DataModel;
using SubnauticaInventory.UI.Tooltips;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SubnauticaInventory.UI
{
	/// <summary>
	/// This class is responsible for controlling the visuals and behavior of Item Views. It relies heavily on the
	/// <see cref="InteractionStateManager"/> on the same object.
	/// </summary>
	public partial class ItemViewController : SimpleUiBehavior, IDropTarget
	{
		[Header("Settings")] 
		[SerializeField] private float dragSpeed = 6;
		[SerializeField] private float snapToSwapSpeed = 10;
		
		[Header("Debug")]
		public Vector2 desiredAnchoredPosition;
		
		[Header("Internal References")]
		public Image highlightBorder;
		[SerializeField] private Image raycastTarget;
		[SerializeField] private FreeModifier[] freeModifiers;
		[SerializeField] private Image itemImage;
		[SerializeField] private InteractionStateManager interactionStateManager;

		private InventoryViewController _owner;
		private Transform _pdaOverlayCanvas;
		


		public Transform PdaOverlayCanvas => _pdaOverlayCanvas;
		public ItemData ItemData { get; private set; }
		
		public void SetData(ItemData itemData, InventoryViewController inventoryViewController)
		{
			ItemData = itemData;
			_owner = inventoryViewController;
			_pdaOverlayCanvas = inventoryViewController.PdaOverlayCanvas.transform;
			itemImage.sprite = itemData.sprite;
			highlightBorder.gameObject.SetActive(false);
			SetSize(inventoryViewController);

			gameObject.name = $"ItemView - {itemData.name} ({itemData.width}x{itemData.height})";
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
				_owner.Pda.ItemTooltipProvider.Show(this);

			if (oldState == InteractionState.PointerOver)
				_owner.Pda.ItemTooltipProvider.Hide(this);
		}

		public void OnHoverPositionChanged(PointerEventData eventData)
		{
			_owner.Pda.ItemTooltipProvider.UpdatePosition(this, eventData);
		}

		public void OnClick(PointerEventData eventData)
		{
			InventoryViewController target = _owner.TransferTarget;
			TryMoveItemToAnotherInventory(target);
		}

		public void OnDragStart(PointerEventData eventData)
		{
			Debug.Log("on drag start");
			transform.SetParent(_owner.PdaOverlayCanvas.transform);
			//raycastTarget.raycastTarget = false;
		}
		
		public void OnDragEnd(PointerEventData eventData)
		{
			Debug.Log("on drag end");
			
			_owner.Pda.SwapTooltipProvider.Hide(this);
			transform.SetParent(_owner.ItemViewsParent);
			_owner.Refresh();
			//raycastTarget.raycastTarget = true;

			EvaluateDragAndDrop(eventData);
		}
		
		public void OnDragUpdate(PointerEventData eventData)
		{
			Debug.Log($"Current Drop Target: {interactionStateManager.CurrentDropTarget}");
			Vector2 raycastLocalPos = _pdaOverlayCanvas.InverseTransformPoint(eventData.pointerCurrentRaycast.worldPosition);
			desiredAnchoredPosition = raycastLocalPos + RectTransform.sizeDelta.Multiply(-.5f, .5f);

			if (interactionStateManager.CurrentDropTarget is ItemViewController swapTarget && CanSwapWith(swapTarget))
			{
				_owner.Pda.SwapTooltipProvider.Show(this);
				Vector3 targetWorldPosition = swapTarget.RectTransform.position;
				desiredAnchoredPosition = swapTarget.RectTransform.sizeDelta.WithNegativeY()/2 + (Vector2) RectTransform.parent.InverseTransformPoint(targetWorldPosition);
			}
			else
				_owner.Pda.SwapTooltipProvider.Hide(this);
		}

		private void Update()
		{
			switch (interactionStateManager.CurrentInteractionState)
			{
				case InteractionState.DraggingNoTarget:
					LerpTowardDesiredPosition(dragSpeed);
					break;
				
				case InteractionState.DraggingWithTarget:
					if(interactionStateManager.CurrentDropTarget is ItemViewController itemViewController)
						_owner.Pda.ItemTooltipProvider.Hide(itemViewController);
					
					if(_owner.Pda.SwapTooltipProvider.HasActive(this))
						LerpTowardDesiredPosition(snapToSwapSpeed);
					else
						LerpTowardDesiredPosition(dragSpeed);
					break;
				
				default:
					LerpTowardDesiredPosition(snapToSwapSpeed);	
					break;
			}
		}

		/// <summary>
		/// This method is called when a drag ends to check if the pointer is over a valid <see cref="IDropTarget"/>
		/// and executes the drop if there is one.
		/// </summary>
		private void EvaluateDragAndDrop(PointerEventData eventData)
		{
			if(!eventData.pointerCurrentRaycast.gameObject)
				return;

			IDropTarget target = interactionStateManager.CurrentDropTarget;

			if (target is InventoryViewController inventory && inventory != _owner)
				TryMoveItemToAnotherInventory(inventory);
			else if (target is ItemViewController item) 
				SwapItem(item);
		}
		
		private void TryMoveItemToAnotherInventory(InventoryViewController target)
		{
			if (target.InventoryData.RequestAdd(ItemData))
			{
				_owner.InventoryData.Remove(ItemData);
				_owner.Refresh();
				target.Refresh();
				_owner.Pda.ItemTooltipProvider.Hide(this);
			}
		}
		
		private void SwapItem(ItemViewController swapTarget)
		{
			if (_owner.InventoryData.RequestSwap(ItemData, swapTarget._owner.InventoryData, swapTarget.ItemData))
			{
				_owner.Pda.ItemTooltipProvider.Hide(this);
				swapTarget._owner.Pda.ItemTooltipProvider.Hide(swapTarget);
				swapTarget._owner.Refresh();
				_owner.Refresh();
			}
		}

		
		/// <summary>
		/// Evaluates whether swapping with a given <see cref="ItemViewController"/> would be valid.
		/// </summary>
		/// <returns>true if <see cref="swapTarget"/> belongs to a different inventory, and its <see cref="ItemData"/> would fit in
		/// <see cref="_owner"/>'s if this object's <see cref="ItemData"/> wasn't in it</returns>
		private bool CanSwapWith(ItemViewController swapTarget)
		{
			
			return !swapTarget.interactionStateManager.IsDragging && swapTarget._owner != _owner && 
			       BinPackingUtility.ItemsCanSwap(_owner.InventoryData, ItemData, swapTarget._owner.InventoryData, swapTarget.ItemData);
		}

		private void LerpTowardDesiredPosition(float speed)
		{
			RectTransform.anchoredPosition = Vector2.Lerp(RectTransform.anchoredPosition, desiredAnchoredPosition, Time.deltaTime * speed);
		}
		
		public string Name => gameObject.name;
	}
}