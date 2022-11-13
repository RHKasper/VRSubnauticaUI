using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace SubnauticaInventory.UI
{
	
	/// <summary>
	/// This script is responsible for determining the current interaction state of Item Views.
	/// It supports any number of pointers.
	/// </summary>
	public class InteractionStateManager : SimpleUiBehavior, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler, IDragHandler
	{
		[Tooltip("The distance in pixels required to begin a drag action")]
		public float dragThreshold = 20;
		public TextMeshProUGUI debugText;
		
		[Tooltip("Called whenever state change occurs. The first parameter is previous state, the second is new state")]
		public UnityEvent<InteractionState, InteractionState> OnInteractionStateChanged;
		
		[FormerlySerializedAs("OnPointerMoved")] public UnityEvent<PointerEventData> OnHoverPositionChanged;
		public UnityEvent<PointerEventData> OnClick;
		public UnityEvent<PointerEventData> OnDragEnd;
		public UnityEvent<PointerEventData> OnDragStart;
		public UnityEvent<PointerEventData> OnDragUpdate;

		public InteractionState CurrentInteractionState => _lastInteractionState;
		public IDropTarget CurrentDropTarget => _currentDropTarget;
		

		private HashSet<int> _pointersOver = new();
		private int _pointerDownId = -1;
		private Vector2 _dragOrigin;
		private bool _isDragging;
		private IDropTarget _currentDropTarget;

		private bool _isDirty;
		private InteractionState _lastInteractionState;
		private List<RaycastResult> _tempRaycastList = new(6);

		public void OnDisable()
		{
			OnInteractionStateChanged?.Invoke(_lastInteractionState, InteractionState.Default);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			_pointersOver.Add(eventData.pointerId);
			_isDirty = true;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			_pointersOver.Remove(eventData.pointerId);
			_isDirty = true;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			if (_pointerDownId == -1)
			{
				_pointerDownId = eventData.pointerId;
				_dragOrigin = eventData.position;
				_isDirty = true;
			}
		}
		
		public void OnPointerUp(PointerEventData eventData)
		{
			if (eventData.pointerId == _pointerDownId)
			{
				try
				{
					if(_isDragging)
						OnDragEnd?.Invoke(eventData);
					else
						OnClick?.Invoke(eventData);
				}
				catch (Exception e)
				{
					Debug.LogError(e);
				}

				_pointerDownId = -1;
				_dragOrigin = default;
				_isDragging = default;
				_currentDropTarget = default;
				_isDirty = true;
			}
		}
		

		public void OnPointerMove(PointerEventData eventData)
		{
			if (_lastInteractionState == InteractionState.PointerOver && eventData.pointerId == _pointersOver.First())
				OnHoverPositionChanged?.Invoke(eventData);
		}

		private void Update()
		{
			if (debugText)
			{
				string debug = "";
				debug += $"Pointers Over: {string.Join(",", _pointersOver)}\n";
				debug += $"Pointer Down Id: {_pointerDownId}\n";
				debug += $"IsDragging: {_isDragging}\n";
				debug += $"Last Interaction State: {_lastInteractionState}\n";
				debug += $"CurrentDropTarget: {(_currentDropTarget != null ? _currentDropTarget.Name : "-")}\n";
				debugText.text = debug;
			}

			if (_isDirty)
				UpdateInteractionState();
		}
		
		private IDropTarget CheckForDropTarget(PointerEventData dragEventData)
		{
			EventSystem.current.RaycastAll(dragEventData, _tempRaycastList);

			for (var i = 0; i < _tempRaycastList.Count; i++)
			{
				RaycastResult result = _tempRaycastList[i];
				if (result.gameObject != gameObject)
				{
					IDropTarget dropTarget = result.gameObject.GetComponent<IDropTarget>();
					if (dropTarget != null)
						return dropTarget;
				}
			}

			return null;
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

			if (_lastInteractionState != newInteractionState)
			{
				OnInteractionStateChanged?.Invoke(_lastInteractionState, newInteractionState);
				_lastInteractionState = newInteractionState;
			}
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (eventData.pointerId == _pointerDownId)
			{
				Vector2 dragOffset = eventData.position - _dragOrigin;

				if (_isDragging || dragOffset.sqrMagnitude >= dragThreshold * dragThreshold)
				{
					if (!_isDragging)
						OnDragStart?.Invoke(eventData);

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
			
			if(_isDragging)
				OnDragUpdate?.Invoke(eventData);
		}
	}
	public enum InteractionState { Default, PointerOver, PointerDown, DraggingNoTarget, DraggingWithTarget }
}