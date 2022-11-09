using UnityEngine;
using UnityEngine.UI;

namespace SubnauticaInventory.UI
{
    /// <summary>
    /// Generates a tile grid for the inventory screen
    /// </summary>
    [ExecuteAlways]
    public class GridBackgroundController : MonoBehaviour
    {
        public Vector2 cellDimensions = new(60, 60);
        public Vector2Int gridLayout = new(6, 8);
        [SerializeField] private RawImage gridImage;

        private Vector2 _lastSetCellDimensions;
        private Vector2Int _lastSetGridLayout;

        // Update is called once per frame
        void Update()
        {
            if (_lastSetCellDimensions != cellDimensions || _lastSetGridLayout != gridLayout)
            {
                Debug.Log("Resizing grid");
                Vector2 imageDimensions = cellDimensions;
                imageDimensions.Scale(gridLayout);

                gridImage.rectTransform.sizeDelta = imageDimensions;
                gridImage.uvRect = new Rect(0, 0, gridLayout.x, gridLayout.y);

                _lastSetCellDimensions = cellDimensions;
                _lastSetGridLayout = gridLayout;
            }
        }
    }
}
