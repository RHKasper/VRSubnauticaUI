using SubnauticaInventory.DataModel;
using SubnauticaInventory.UI.Tooltips;
using SubnauticaInventory.UI.Tooltips.ItemTooltips;
using SubnauticaInventory.UI.Tooltips.SwapTooltips;
using UnityEngine;
using UnityEngine.Serialization;

namespace SubnauticaInventory.UI
{
    public class PdaViewController : MonoBehaviour
    {
        [SerializeField] private Canvas baseCanvas;
        [SerializeField] private Canvas overlayCanvas;
        [FormerlySerializedAs("_itemTooltipsProvider")] [SerializeField] private ItemTooltipProvider itemTooltipProvider;
        [SerializeField] private SwapTooltipProvider swapTooltipProvider;


        public ItemTooltipProvider ItemTooltipProvider => itemTooltipProvider;
        public SwapTooltipProvider SwapTooltipProvider => swapTooltipProvider;
        public Canvas BaseCanvas => baseCanvas;
        public Canvas OverlayCanvas => overlayCanvas;
    }
}
