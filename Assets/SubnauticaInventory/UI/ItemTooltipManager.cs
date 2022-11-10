using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SubnauticaInventory.UI
{
	public class ItemTooltipManager : SimpleUiBehavior
	{
		[Header("Settings")]
		[SerializeField] private Vector2 offsetFromPointer = new(10,10);
		
		
		[Header("Internal References")]
		[SerializeField] private TextMeshProUGUI nameText;
		[SerializeField] private TextMeshProUGUI descriptionText;

		private static ItemTooltipManager _instance;
		private static ItemViewController _currentItemView;
		private static string _pathFromResources = "ItemTooltip";

		#region Static Implementation
		
		public static void Show(ItemViewController itemView)
		{
			if(!_instance)
				Initialize();
			_instance.ShowInternal(itemView);
		}
		
		public static void Hide(ItemViewController itemView)
		{
			if(_instance)
				_instance.HideInternal(itemView);
		}

		public static void UpdatePosition(ItemViewController itemView, PointerEventData eventData)
		{
			if(itemView == _currentItemView)
				_instance.UpdatePositionInternal(eventData);
		}

		private static void Initialize()
		{
			_instance = Instantiate(Resources.Load<ItemTooltipManager>(_pathFromResources));
			DontDestroyOnLoad(_instance);
		}
		
		#endregion
		
		private void ShowInternal(ItemViewController itemView)
		{
			_currentItemView = itemView;
			
			nameText.text = itemView.ItemData.Name;
			descriptionText.text = itemView.ItemData.Description;
			transform.SetParent(itemView.RectTransform.parent);
			transform.localScale = Vector3.one;

			gameObject.SetActive(true);
		}

		private void HideInternal(ItemViewController itemView)
		{
			if (itemView == _currentItemView)
			{
				transform.SetParent(null);
				transform.localScale = Vector3.one;
				gameObject.SetActive(false);
			}
		}

		private void UpdatePositionInternal(PointerEventData eventData)
		{
			var raycastHit = eventData.pointerCurrentRaycast;
			RectTransform.anchoredPosition = offsetFromPointer + (Vector2) transform.parent.InverseTransformPoint(raycastHit.worldPosition);
		}
	}
}