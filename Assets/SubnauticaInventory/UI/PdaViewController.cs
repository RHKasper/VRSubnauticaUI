using SubnauticaInventory.DataModel;
using SubnauticaInventory.UI.Tooltips;
using SubnauticaInventory.UI.Tooltips.ItemTooltips;
using UnityEngine;
using UnityEngine.Serialization;

namespace SubnauticaInventory.UI
{
    public class PdaViewController : MonoBehaviour
    {
        [SerializeField] private Canvas baseCanvas;
        [SerializeField] private Canvas overlayCanvas;
        [FormerlySerializedAs("_itemTooltipsProvider")] [SerializeField] private ItemTooltipProvider itemTooltipProvider;


        public ItemTooltipProvider ItemTooltipProvider => itemTooltipProvider;
        public Canvas BaseCanvas => baseCanvas;
        public Canvas OverlayCanvas => overlayCanvas;
    }
}
