using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SubnauticaInventory.UI.Tooltips
{
	/// <summary>
	/// Base class for Tooltip controllers like <see cref="ItemTooltips.ItemTooltipController"/>.
	/// </summary>
	/// <typeparam name="TData">The type of data this tooltip needs</typeparam>
	public abstract class AbstractTooltipController<TData> : SimpleUiBehavior
	{
		public abstract void SetData(TData itemView);
	}
}