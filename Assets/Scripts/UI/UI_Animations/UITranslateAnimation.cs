using Nova;
using UnityEngine;

namespace UI.UI_Animations
{
    [System.Serializable]
    public struct UITranslateAnimation : Nova.IAnimation
    {
        public UIBlock Block;
        public Vector2 TargetPositionPercentage;
        private Vector2 _cachedPositionPercentage;

        public void Update(float percentDone)
        {
            if (percentDone <= 0)
            {
                _cachedPositionPercentage = Block.Position.Percent;
            }
            Block.Position.Percent = Vector2.Lerp(_cachedPositionPercentage, TargetPositionPercentage, percentDone);
            if (percentDone >= 1)
            {
                Block.Position.Percent = TargetPositionPercentage;
            }
        }
    }
}