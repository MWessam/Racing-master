using Nova;
using System;
using UnityEngine;

namespace UI.UI_Input
{
    public abstract class UIBlockTouchInputComponent : UIBlockInputComponent
    {
        Camera _camera;
        public override void Awake()
        {
            base.Awake();
            _camera = Camera.main;
        }
        private void OnEnable()
        {
            _block.AddGestureHandler<Gesture.OnDrag>(HandleOnDrag);
            _block.AddGestureHandler<Gesture.OnClick>(HandleOnPress);
            _block.AddGestureHandler<Gesture.OnRelease>(HandleOnRelease);
        }
        private void OnDisable()
        {
            _block.RemoveGestureHandler<Gesture.OnDrag>(HandleOnDrag);
            _block.RemoveGestureHandler<Gesture.OnClick>(HandleOnPress);
            _block.RemoveGestureHandler<Gesture.OnRelease>(HandleOnRelease);
        }
        public override void Start()
        {
            base.Start();

        }
        protected abstract void HandleOnDrag(Gesture.OnDrag evt);
        protected abstract void HandleOnPress(Gesture.OnClick evt);
        protected abstract void HandleOnRelease(Gesture.OnRelease evt);
    }
}
