using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SubnauticaInventory.UI
{
	public class InteractionStateManager : SimpleUiBehavior, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
	{
		public enum InteractionState { Default, PointerOver, PointerDown, DraggingNoTarget, DraggingWithTarget }

		[Tooltip("The distance in pixels required to begin a drag action")]
		public float dragThreshold = 20;
		public TextMeshProUGUI debugText;
		
		[Tooltip("Called whenever state change occurs. The first parameter is previous state, the second is new state")]
		public Action<InteractionState, InteractionState> OnInteractionStateChanged;

		private HashSet<int> _pointersOver = new();
		private int _pointerDownId = -1;
		private Vector2 _dragOrigin;
		private bool _isDragging;
		private IDropTarget _currentDropTarget;

		private bool _isDirty;
		private InteractionState _lastInteractionState;

		public void OnPointerEnter(PointerEventData eventData)
		{
			Debug.Log($"Pointer Enter: {eventData.pointerId}");
			_pointersOver.Add(eventData.pointerId);
			_isDirty = true;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			Debug.Log($"Pointer Exit: {eventData.pointerId}");
			_pointersOver.Remove(eventData.pointerId);
			_isDirty = true;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			if (_pointerDownId == -1)
			{
				Debug.Log($"Pointer Down: {eventData.pointerId}");
				_pointerDownId = eventData.pointerId;
				_dragOrigin = eventData.position;
				_isDirty = true;
			}
		}
		
		public void OnPointerUp(PointerEventData eventData)
		{
			if (eventData.pointerId == _pointerDownId)
			{
				Debug.Log($"Pointer Up: {eventData.pointerId}");
				_pointerDownId = -1;
				_dragOrigin = default;
				_isDragging = default;
				_currentDropTarget = default;
				_isDirty = true;
			}
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (eventData.pointerId == _pointerDownId)
			{
				Vector2 dragOffset = eventData.position - _dragOrigin;

				if (dragOffset.sqrMagnitude >= dragThreshold * dragThreshold)
				{
					Debug.Log($"Dragging: {eventData.pointerId}");
					_isDragging = true;
					_currentDropTarget = CheckForDropTarget(eventData);
					_isDirty = true;
				}
				else
				{
					_isDragging = default;
					_currentDropTarget = default;
					_isDirty = true;
				}
			}
		}

		private void Update()
		{
			string debug = "";
			debug += $"Pointers Over: {string.Join(",", _pointersOver)}\n";
			debug += $"Pointer Down Id: {_pointerDownId}\n";
			debug += $"IsDragging: {_isDragging}";
			debug += $"Last Interaction State: {_lastInteractionState}";
			debugText.text = debug;

			if (_isDirty)
				UpdateInteractionState();
		}
		
		private IDropTarget CheckForDropTarget(PointerEventData dragEventData)
		{
			return null;
			// raycast against UI
		}

		private void UpdateInteractionState()
		{
			InteractionState newInteractionState;

			if (_currentDropTarget != null)
				newInteractionState = InteractionState.DraggingWithTarget;
			else if (_isDragging)
				newInteractionState = InteractionState.DraggingNoTarget;
			else if (_pointerDownId != -1)
				newInteractionState = InteractionState.PointerDown;
			else if (_pointersOver.Any())
				newInteractionState = InteractionState.PointerOver;
			else
				newInteractionState = InteractionState.Default;

			OnInteractionStateChanged?.Invoke(_lastInteractionState, newInteractionState);
			_lastInteractionState = newInteractionState;
		}
	}
}