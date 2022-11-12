using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SubnauticaInventory.UI.Tooltips
{
	public class ItemTooltipsProvider : MonoBehaviour
	{
		[SerializeField] private ItemTooltipController tooltipPrefab;

		private readonly Dictionary<ItemViewController, ItemTooltipController> _activeTooltips = new();
		private readonly List<ItemTooltipController> _pool = new();

		public void Show(ItemViewController itemView)
		{
			if (!_activeTooltips.ContainsKey(itemView) || _activeTooltips[itemView] == null)
			{
				_activeTooltips[itemView] = GetTooltipFromPool();
				_activeTooltips[itemView].ShowInternal(itemView);
			}
		}

		public void Hide(ItemViewController itemView)
		{
			if (_activeTooltips.ContainsKey(itemView))
			{
				ItemTooltipController tooltip = _activeTooltips[itemView]; 
				_activeTooltips.Remove(itemView);
				ReturnToPool(tooltip);
			}
		}

		public void UpdatePosition(ItemViewController itemView, PointerEventData eventData)
		{
			if(_activeTooltips.ContainsKey(itemView))
				_activeTooltips[itemView].UpdatePositionInternal(eventData);
		}

		private ItemTooltipController GetTooltipFromPool()
		{
			ItemTooltipController tooltip;

			if (_pool.Any())
			{
				tooltip = _pool[^1];
				_pool.RemoveAt(_pool.Count-1);
			}
			else
			{
				tooltip = Instantiate(tooltipPrefab);
			}

			return tooltip;
		}

		private void ReturnToPool(ItemTooltipController tooltip)
		{
			Transform tooltipTransform = tooltip.transform;
			tooltipTransform.SetParent(transform);
			tooltipTransform.localScale = Vector3.one;
			tooltip.gameObject.SetActive(false);
			_pool.Add(tooltip);
		}
	}
}