using TMPro;
using UnityEngine;

namespace SubnauticaInventory.UI
{
	public class ItemTooltipManager : SimpleUiBehavior
	{
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
			
			RectTransform.anchorMax = itemView.RectTransform.anchorMax;
			RectTransform.anchorMin= itemView.RectTransform.anchorMin;
			Vector2 itemViewSizeOffset = itemView.RectTransform.sizeDelta.Multiply(.5f, -1f);
			Vector2 position = itemView.RectTransform.anchoredPosition + itemViewSizeOffset;
			RectTransform.anchoredPosition = position;
			
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
	}
}