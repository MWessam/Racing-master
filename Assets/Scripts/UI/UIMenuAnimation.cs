using Nova;
using UnityEngine;

namespace UI
{
    public struct UIMenuAnimation : IAnimation
    {
        public UIBlock2D CurrentBlock;
        public Vector2 TargetPositionPercentage;
        public void Update(float percentDone)
        {
            Vector2 cachedPosition = CurrentBlock.Position.Raw;
            CurrentBlock.Position.Value = Vector2.Lerp(cachedPosition, TargetPositionPercentage, percentDone);
            if (percentDone >= 1)
            {
                CurrentBlock.Position.Value = TargetPositionPercentage;
            }
        }
    }
}
