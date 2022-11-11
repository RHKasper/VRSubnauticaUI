using System.Collections.Generic;
using System.Linq;
using SubnauticaInventory.DataModel;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SubnauticaInventory.UI.Tooltips
{
	public static class ItemTooltipsStaticManager
	{
		private static string _pathFromResources = "ItemTooltip";
		private static List<ItemTooltipController> _pool = new();
		private static Dictionary<ItemViewController, ItemTooltipController> _activeTooltips = new();
		private static Transform _poolParent;

		public static void Show(ItemViewController itemView)
		{
			if (!_activeTooltips.ContainsKey(itemView) || _activeTooltips[itemView] == null)
			{
				_activeTooltips[itemView] = GetTooltipFromPool();
				_activeTooltips[itemView].ShowInternal(itemView);
			}
		}

		public static void Hide(ItemViewController itemView)
		{
			if (_activeTooltips.ContainsKey(itemView))
			{
				ItemTooltipController tooltip = _activeTooltips[itemView]; 
				_activeTooltips.Remove(itemView);
				ReturnToPool(tooltip);
			}
		}

		public static void UpdatePosition(ItemViewController itemView, PointerEventData eventData)
		{
			if(_activeTooltips.ContainsKey(itemView))
				_activeTooltips[itemView].UpdatePositionInternal(eventData);
		}

		private static ItemTooltipController GetTooltipFromPool()
		{
			ItemTooltipController tooltip;

			if (_pool.Any())
			{
				tooltip = _pool[^1];
				_pool.RemoveAt(_pool.Count-1);
			}
			else
			{
				tooltip = Object.Instantiate(Resources.Load<ItemTooltipController>(_pathFromResources), _poolParent);
			}

			return tooltip;
		}

		private static void ReturnToPool(ItemTooltipController tooltip)
		{
			if (!_poolParent)
			{
				_poolParent = new GameObject("Item Tooltip Pool").transform;
				Object.DontDestroyOnLoad(_poolParent);
			}
			
			tooltip.transform.SetParent(_poolParent);
			tooltip.transform.localScale = Vector3.one;
			tooltip.gameObject.SetActive(false);
			_pool.Add(tooltip);
		}
	}
}