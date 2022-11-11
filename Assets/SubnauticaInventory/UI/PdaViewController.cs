using UnityEngine;

namespace SubnauticaInventory.UI
{
    public class PdaViewController : MonoBehaviour
    {
        [SerializeField] private Canvas baseCanvas;
        [SerializeField] private Canvas overlayCanvas;

        public Canvas BaseCanvas => baseCanvas;
        public Canvas OverlayCanvas => overlayCanvas;
    }
}
