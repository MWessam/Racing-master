using Game_Manager.Mediator;
using Nova;
using UI.UI_Animations;
using UnityEngine;

namespace UI.Loading_Screen
{
    public class LoadingIcon : MonoBehaviour, IAwakeable, IOnEnabled
    {
        private UIBlock2D _block;
        [SerializeField] Texture2D _icon;
        [SerializeField] Vector3 _rotationAxis;
        [Tooltip("IN DEGREESSSS! THE ANGLEEEE TOOOO ROTATEEEE TOOOOOO INNNNNN DEGREEEEEEEEEEEEEEEEEEEEESSSSSSSSSSSSSSSSSSSSS!!!!!")]
        [SerializeField] float _angle;
        AnimationHandle _animationHandle;
        AnimationHandle _loopAnimationHandle;
        [SerializeField] float _animationTime;
        private Vector2 _baseSize;
        public void Awake()
        {
            _block = GetComponent<UIBlock2D>();
            _block.SetImage(_icon);
            _baseSize = _block.Size.Percent;
        }
        public void OnEnable()
        {
            StartAnimation();
            _block.Size.Percent = Vector2.zero;
        }
        public void StartAnimation()
        {
            FadingAnimations fadeAnimation = new FadingAnimations() { Block = _block, IsFadeIn = true };
            RotationAnimation rotationAnimation = new RotationAnimation() { Block = _block, Angle = _angle, Axis = _rotationAxis };
            _animationHandle = fadeAnimation.Run(_animationTime / 2);
            _animationHandle = _animationHandle.Chain(new UIScaleAnimation() { Block = _block , TargetSizePercentage = _baseSize}, _animationTime);
            _loopAnimationHandle = rotationAnimation.Loop(_animationTime);
        }
        public void CancelAnimation()
        {
            _loopAnimationHandle.Cancel();
            _animationHandle = new UIScaleAnimation() { Block = _block, TargetSizePercentage = Vector2.zero }.Run(_animationTime);
        }

    }
}
