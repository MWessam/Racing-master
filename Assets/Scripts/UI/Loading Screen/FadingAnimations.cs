using Nova;
using UnityEngine;

namespace UI.Loading_Screen
{
    public struct FadingAnimations : Nova.IAnimation
    {
        public UIBlock Block;
        public bool IsFadeIn;
        private Color _baseColor;
        private Color _targetColor;
        public void Update(float percentDone)
        {
            if (percentDone <= 0)
            {
                _baseColor = Block.Color;
                _baseColor = IsFadeIn ? ChangeAlpha(_baseColor , 0.0f) : ChangeAlpha(_baseColor , 1.0f);
                _targetColor = IsFadeIn ? ChangeAlpha(_baseColor, 1.0f) : ChangeAlpha(_baseColor, 0.0f);
            }
            Block.Color = Color.Lerp(_baseColor, _targetColor, percentDone);
        }

        private Color ChangeAlpha(Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }
    }
}