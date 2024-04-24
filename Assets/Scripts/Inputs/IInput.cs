using System;

namespace Inputs
{
    /// <summary>
    /// Interface that should be implemented by one input type.
    /// Depending on state, will fire off an event that return the Input value as an object (make sure to cast it)
    /// </summary>
    public interface IInput
    {
        InputState State { get; }
        void InvokeInputState();
        void AddEvent(Action inputEvent, InputState state);
        void UpdateInputState();
        event Action PressedInput;
        event Action HeldInput;
        event Action ReleasedInput;
    }
}