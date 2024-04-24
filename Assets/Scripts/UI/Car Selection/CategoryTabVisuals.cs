using Nova;
using UnityEngine;

namespace UI.Car_Selection
{
    [System.Serializable]
    public class CategoryTabVisuals : ItemVisuals
    {
        public UIBlock2D MainTabBlock = null;
        public Color DefaultColor;
        public UIBlock2D TabIcon;
        [SerializeField] Color _selectedColor;
        bool _isSelected = false;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                MainTabBlock.Color = _isSelected ? _selectedColor : DefaultColor;
            }
        }
    }
}
