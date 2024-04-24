using System;
using System.Collections.Generic;
using Game_Manager.Mediator;
using Inputs;
using Nova;
using UnityEngine;

namespace UI.UI_Input
{
    public class UI_InputHandler : MonoBehaviour, IInputHandler , IAwakeable, IStartable
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
        private void UpdateInputStart()
        {
            // Touch touch = Input.touches[0];
            // Ray ray = _camera.ScreenPointToRay(touch.position);
            // Interaction.Update interaction = new Interaction.Update(ray, (uint)touch.fingerId);
            // Interaction.Point(interaction, true);
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
        private void Update()
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
    }
}
