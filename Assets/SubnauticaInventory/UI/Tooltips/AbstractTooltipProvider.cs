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
		protected readonly List<TPrefab> Pool = new();

		public void Show(TData itemView)
		{
			if (!ActiveTooltips.ContainsKey(itemView) || ActiveTooltips[itemView] == null)
			{
				ActiveTooltips[itemView] = GetTooltipFromPool();
				ActiveTooltips[itemView].SetData(itemView);
			}
		}

		public void Hide(TData itemView)
		{
			if (ActiveTooltips.ContainsKey(itemView))
			{
				TPrefab tooltip = ActiveTooltips[itemView]; 
				ActiveTooltips.Remove(itemView);
				ReturnToPool(tooltip);
			}
		}

		private TPrefab GetTooltipFromPool()
		{
			TPrefab tooltip;

			if (Pool.Any())
			{
				tooltip = Pool[^1];
				Pool.RemoveAt(Pool.Count-1);
			}
			else
			{
				tooltip = Instantiate(tooltipPrefab);
			}

			return tooltip;
		}

		private void ReturnToPool(TPrefab tooltip)
		{
			Transform tooltipTransform = tooltip.transform;
			tooltipTransform.SetParent(transform);
			tooltipTransform.localScale = Vector3.one;
			tooltip.gameObject.SetActive(false);
			Pool.Add(tooltip);
		}
	}
}