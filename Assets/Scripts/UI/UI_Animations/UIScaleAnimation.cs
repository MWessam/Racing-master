using Nova;
using UnityEngine;

namespace UI.UI_Animations
{
    [System.Serializable]
    public struct UIScaleAnimation : Nova.IAnimation
    {
        public UIBlock Block;
        public Vector2 TargetSizePercentage;
        private Vector2 _cachedSizePercentage;
   

        public void Update(float percentDone)
        {
            if (percentDone <= 0)
            {
                _cachedSizePercentage = Block.Size.Percent;
            }
            Block.Size.Percent = Vector2.Lerp(_cachedSizePercentage, TargetSizePercentage, percentDone);
            if (percentDone >= 1)
            {
                Block.Size.Percent = TargetSizePercentage;
            }
        }
    }
}