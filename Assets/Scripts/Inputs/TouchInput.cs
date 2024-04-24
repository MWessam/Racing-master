using System;
using UnityEngine;

namespace Inputs
{
    public class TouchInput : IInput
    {
        private InputState _state = InputState.None;
        public InputState State => _state;
        public event Action PressedInput;
        public event Action HeldInput;
        public event Action ReleasedInput;

        public void InvokeInputState()
        {
            switch (_state)
            {
                case InputState.Pressed:
                    PressedInput?.Invoke(); break;
                case InputState.Held:
                    HeldInput?.Invoke(); 
                    break;
                case InputState.Released:
                    ReleasedInput?.Invoke(); break;
            }
        }


        public void UpdateInputState()
        {
            if(Input.touchCount == 0) return;
            switch (Input.touches[0].phase)
            {
                case TouchPhase.Began:
                    _state = InputState.Pressed; break;
                case TouchPhase.Moved:
                    _state = InputState.Held; break;
                case TouchPhase.Ended:
                    _state = InputState.Released; break;
                //case TouchPhase.Stationary:
                //    _state = InputState.Held; break;
                default:
                    _state = InputState.None; return;
            }
            InvokeInputState();
        }

        public void AddEvent(Action inputEvent, InputState state)
        {
            switch (state)
            {
                case InputState.Pressed:
                    PressedInput += inputEvent; break;
                case InputState.Released:
                    ReleasedInput += inputEvent; break;
                case InputState.Held:
                    HeldInput += inputEvent; break;
                default:
                    throw new Exception("This state is not valid");
            }
        }
    }
}