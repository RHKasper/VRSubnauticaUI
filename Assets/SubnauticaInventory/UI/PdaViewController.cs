using SubnauticaInventory.DataModel;
using SubnauticaInventory.UI.Tooltips;
using UnityEngine;

namespace SubnauticaInventory.UI
{
    public class PdaViewController : MonoBehaviour
    {
        [SerializeField] private Canvas baseCanvas;
        [SerializeField] private Canvas overlayCanvas;
        [SerializeField] private ItemTooltipsProvider _itemTooltipsProvider;


        public ItemTooltipsProvider ItemTooltipsProvider => _itemTooltipsProvider;
        public Canvas BaseCanvas => baseCanvas;
        public Canvas OverlayCanvas => overlayCanvas;
    }
}
