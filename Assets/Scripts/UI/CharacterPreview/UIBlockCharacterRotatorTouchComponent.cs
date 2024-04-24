using Nova;
using UI.UI_Input;
using UnityEngine;

namespace UI.CharacterPreview
{
    public class UIBlockCharacterRotatorTouchComponent : UIBlockTouchInputComponent
    {
        [SerializeField] Transform _characterTransform;
        Transform _cachedCameraTransform;
        Transform _currentTransformCached;
        private RCCP_ShowroomCamera _showroomCamera;
        [Range(1.0f, 10.0f)]
        [SerializeField]
        float _speed;
        [SerializeField] bool _isClockwise = false;
        public override void Awake()
        {
            base.Awake();

            _currentTransformCached = transform;
        }
        public override void Start()
        {
            _showroomCamera = FindObjectOfType<RCCP_ShowroomCamera>();
            base.Start();
            _cachedCameraTransform = Camera.main.transform;
        }
        protected override void HandleOnDrag(Gesture.OnDrag evt)
        {
            int directionIndicator = 1;
            if (_isClockwise)
            {
                directionIndicator = 1;
            }
            else
            {
                directionIndicator = -1;
            }
            _showroomCamera.OnDrag(evt.DragDeltaLocalSpace.x, evt.DragDeltaLocalSpace.y);
            // _cachedCameraTransform.RotateAround(_characterTransform.position, Vector3.up, Vector3.Dot(evt.DragDeltaLocalSpace, _cachedCameraTransform.right) * _speed * Time.deltaTime);
            //_cachedCameraTransform.Rotate(_characterTransform.up, Vector3.Dot(evt.DragDeltaLocalSpace , _currentTransformCached.right) * _speed * Time.deltaTime * directionIndicator, Space.Self);
        }
        protected override void HandleOnPress(Gesture.OnClick evt)
        {
        }

        protected override void HandleOnRelease(Gesture.OnRelease evt)
        {
        }
    }
}