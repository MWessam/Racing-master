using System;
using System.Collections.Generic;
using Game_Manager.Mediator;
using Inputs;
using UnityEngine;

namespace UI.UI_Input
{
    public class TouchInputHandler : MonoBehaviour, IInputHandler, IUpdateInput, IAwakeable, IStartable, IUpdateable, IOnEnabled, IOnDisabled
    {
        Dictionary<string, IInput> _inputs = new Dictionary<string, IInput>();
        bool _isEnabled = true;
        Camera _camera;
        public void Awake()
        {
            AddInput("Touch", new TouchInput());
            AssignInputEvent("Touch", UpdateInputStart, InputState.Pressed);
        }
        public void Start()
        {
            _camera = Camera.main;
        }
        public void UpdateInputStart()
        {
            Touch touch = Input.touches[0];
            Ray ray = _camera.ScreenPointToRay(touch.position);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.transform.parent.TryGetComponent(out ObjectTouchInputComponent touchObject))
                {
                    touchObject.HandlePressed();
                }
            }
        }
        public void TriggerInput(string key)
        {
            if (_inputs.TryGetValue(key, out IInput input))
            {
                input.InvokeInputState();
            }
        }
        public void AssignInputEvent(string key, Action inputEvent, InputState state)
        {
            if (!_inputs.TryGetValue(key, out IInput input))
            {
                return;
            }
            input.AddEvent(inputEvent, state);
        }
        public void AddInput(string key, IInput input)
        {
            if (!_inputs.ContainsKey(key))
            {
                _inputs.Add(key, input);
            }
        }
        public void Update()
        {
            UpdateInputs();
        }
        public void UpdateInputs()
        {
            if (!_isEnabled) return;
            foreach (IInput input in _inputs.Values)
            {
                input.UpdateInputState();
            }
        }

        public void SetInputState(bool state)
        {
            _isEnabled = state;
        }

        public void OnEnable()
        {
            UIHandler.Instance.OnCameraChange += (cam) => _camera = cam;

        }

        public void OnDisable()
        {
            UIHandler.Instance.OnCameraChange -= (cam) => _camera = cam;

        }
    }
}
