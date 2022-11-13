using System.Collections.Generic;
using System.Linq;
using SubnauticaInventory.UI.Tooltips.ItemTooltips;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SubnauticaInventory.UI.Tooltips
{
	/// <summary>
	/// This is the base class for other tooltip providers like <see cref="ItemTooltipProvider"/>. It
	/// contains generic pooling and instantiation methods, along with public <see cref="Show"/> and <see cref="Hide"/>
	/// methods that should take care of most tooltip functionality
	/// </summary>
	/// <typeparam name="TPrefab"></typeparam>
	/// <typeparam name="TData"></typeparam>
	public abstract class AbstractTooltipsProvider<TPrefab,TData> : MonoBehaviour where TPrefab : AbstractTooltipController<TData>
	{
		[SerializeField] protected TPrefab tooltipPrefab;

		protected readonly Dictionary<TData, TPrefab> ActiveTooltips = new();
		protected readonly Dictionary<TData, TPrefab> InactiveTooltips = new();

		public void Show(TData data)
		{
			if (!HasActive(data) || ActiveTooltips[data] == null)
			{
				ActiveTooltips[data] = GetTooltipFromPool(data);
				ActiveTooltips[data].SetData(data);
			}
		}

		public void Hide(TData data)
		{
			if (HasActive(data))
			{
				TPrefab tooltip = ActiveTooltips[data]; 
				ActiveTooltips.Remove(data);
				ReturnToPool(tooltip, data);
			}
		}

		public bool HasActive(TData data) => ActiveTooltips.ContainsKey(data);

		private TPrefab GetTooltipFromPool(TData data)
		{
			TPrefab tooltip;

			if (InactiveTooltips.ContainsKey(data))
			{
				tooltip = InactiveTooltips[data];
				InactiveTooltips.Remove(data);
			}
			else
			{
				tooltip = Instantiate(tooltipPrefab);
			}

			return tooltip;
		}

		private void ReturnToPool(TPrefab tooltip, TData data)
		{
			Transform tooltipTransform = tooltip.transform;
			tooltipTransform.SetParent(transform);
			tooltipTransform.localScale = Vector3.one;
			tooltip.gameObject.SetActive(false);
			
			InactiveTooltips[data] = tooltip;
		}
	}
}