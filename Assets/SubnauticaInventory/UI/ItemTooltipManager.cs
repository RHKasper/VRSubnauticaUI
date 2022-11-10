using System;
using SubnauticaInventory.DataModel;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace SubnauticaInventory.UI
{
	public class ItemTooltipManager : SimpleUiBehavior
	{
		[SerializeField] private TextMeshProUGUI nameText;
		[SerializeField] private TextMeshProUGUI descriptionText;

		private static ItemTooltipManager _instance;
		private static string _pathFromResources = "ItemTooltip";

		#region Static Implementation
		
		public static void Show(ItemViewController itemView)
		{
			if(!_instance)
				Initialize();
			_instance.ShowInternal(itemView);
		}
		
		public static void Hide()
		{
			if(_instance)
				_instance.HideInternal();
		}

		private static void Initialize()
		{
			_instance = Instantiate(Resources.Load<ItemTooltipManager>(_pathFromResources));
			DontDestroyOnLoad(_instance);
		}
		
		#endregion
		
		private void ShowInternal(ItemViewController itemView)
		{
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

		private void HideInternal()
		{
			transform.SetParent(null);
			transform.localScale = Vector3.one;
			gameObject.SetActive(false);
		}
	}
}