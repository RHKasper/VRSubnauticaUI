using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SubnauticaInventory.UI.Tooltips
{
	public class ItemTooltipController : SimpleUiBehavior
	{
		[Header("Settings")]
		[SerializeField] private Vector2 offsetFromPointer = new(24,-30);
		
		[Header("Internal References")]
		[SerializeField] private TextMeshProUGUI nameText;
		[SerializeField] private TextMeshProUGUI descriptionText;

		public void ShowInternal(ItemViewController itemView)
		{
			nameText.text = itemView.ItemData.name;
			descriptionText.text = itemView.ItemData.description;
			
			transform.SetParent(itemView.RectTransform.parent);
			transform.localScale = Vector3.one;

			gameObject.SetActive(true);
		}

		public void UpdatePositionInternal(PointerEventData eventData)
		{
			var raycastHit = eventData.pointerCurrentRaycast;
			RectTransform.anchoredPosition = offsetFromPointer + (Vector2) transform.parent.InverseTransformPoint(raycastHit.worldPosition);
		}
	}
}