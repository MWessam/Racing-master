using Nova;
using UnityEngine;

namespace UI.UI_Animations
{
    [System.Serializable]
    public struct UIPopupAnimation : Nova.IAnimation
    {
        public UIBlock2D Block;
        public float TargetWidthPercentage;
        public float TargetHeightPercentage;
        private float _startWidthPercentage;
        private float _startHeightPercentage;
        public void Update(float percentDone)
        {
            if (percentDone == 0.0f)
            {
                _startWidthPercentage = Block.Size.Percent.x;
                _startHeightPercentage = Block.Size.Percent.y;
            }
            float sizeYPercentage = Mathf.Lerp(_startHeightPercentage, TargetHeightPercentage, percentDone);
            float sizeXPercentage = Mathf.Lerp(_startWidthPercentage, TargetWidthPercentage, percentDone);
            Block.Size.Percent = new Vector3(sizeXPercentage, sizeYPercentage, 0.0f); ;
        }
    }
}

