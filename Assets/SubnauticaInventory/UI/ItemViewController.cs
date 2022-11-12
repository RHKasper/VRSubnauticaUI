using System;
using System.Collections.Generic;
using System.Linq;
using SubnauticaInventory.DataModel;
using SubnauticaInventory.UI.Tooltips;
using UnityEngine;
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
			raycastTarget.raycastTarget = false;
		}
		
		public void OnDragEnd(PointerEventData eventData)
		{
			Debug.Log("on drag end");
			
			_owner.Pda.SwapTooltipProvider.Hide(this);
			transform.SetParent(_owner.ItemViewsParent);
			_owner.Refresh();
			raycastTarget.raycastTarget = true;

			EvaluateDragAndDrop(eventData);
		}
		
		public void OnDragUpdate(PointerEventData eventData)
		{
			Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
			Vector2 raycastLocalPos = _pdaOverlayCanvas.InverseTransformPoint(eventData.pointerCurrentRaycast.worldPosition);
			RectTransform.anchoredPosition = raycastLocalPos + RectTransform.sizeDelta.Multiply(-.5f, .5f);

			if (interactionStateManager.CurrentDropTarget is ItemViewController swapTarget && CanSwapWith(swapTarget))
				_owner.Pda.SwapTooltipProvider.Show(this);
			else
				_owner.Pda.SwapTooltipProvider.Hide(this);
		}

		/// <summary>
		/// This method is called when a drag ends to check if the pointer is over a valid <see cref="IDropTarget"/>
		/// and executes the drop if there is one.
		/// </summary>
		private void EvaluateDragAndDrop(PointerEventData eventData)
		{
			IDropTarget target = eventData.pointerCurrentRaycast.gameObject.GetComponent<IDropTarget>();

			if (target is InventoryViewController inventory && inventory != _owner)
			{
				TryMoveItemToAnotherInventory(inventory);
			}
		}
		
		private void TryMoveItemToAnotherInventory(InventoryViewController target)
		{
			if (target.InventoryData.RequestAdd(ItemData))
			{
				_owner.InventoryData.Remove(ItemData);
				_owner.Refresh();
				target.Refresh();
			}
		}

		
		/// <summary>
		/// Evaluates whether swapping with a given <see cref="ItemViewController"/> would be valid.
		/// </summary>
		/// <returns>true if <see cref="swapTarget"/> belongs to a different inventory, and its <see cref="ItemData"/> would fit in
		/// <see cref="_owner"/>'s if this object's <see cref="ItemData"/> wasn't in it</returns>
		private bool CanSwapWith(ItemViewController swapTarget)
		{
			if(swapTarget._owner.InventoryData == _owner.InventoryData)
				return false;

			List<ItemData> theseItems = _owner.InventoryData.Items.Where(i => i!= ItemData).ToList();
			theseItems.Add(swapTarget.ItemData);
			
			List<ItemData> thoseItems = swapTarget._owner.InventoryData.Items.Where(i => i!= swapTarget.ItemData).ToList();
			thoseItems.Add(ItemData);

			return BinPackingUtility.ItemsFitInBin(theseItems, _owner.InventoryData) &&
			       BinPackingUtility.ItemsFitInBin(thoseItems, swapTarget._owner.InventoryData);
		}

		public string Name => gameObject.name;
	}
}