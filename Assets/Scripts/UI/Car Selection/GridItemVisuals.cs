using Nova;
using UnityEngine;

namespace UI.Car_Selection
{
    [System.Serializable]
    public abstract class GridItemVisuals : ItemVisuals
    {
        public UIBlock2D ContentRoot;
        public UIBlock2D IconBlock;
        public UIBlock2D LockedIcon;
        public Color DefaultColor;
        public Color SelectedColor;
        public Color LockedColor;
        bool _isSelected = false;
        bool _isLocked = false;
        public bool IsLocked
        {
            get
            {
                return _isLocked;
            }
            set
            {
                if (value)
                {
                    ContentRoot.Color = LockedColor;
                }
                LockedIcon.gameObject.SetActive(IsLocked);
                _isLocked = value;
            }
        }
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                if (_isSelected)
                {
                    ContentRoot.Color = SelectedColor;
                }
                else
                {
                    ContentRoot.Color = _isLocked ? LockedColor : DefaultColor;
                }
            }
        }
        public abstract void Bind(GridData data);
    }
}