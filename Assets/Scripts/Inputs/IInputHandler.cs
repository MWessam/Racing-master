using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inputs
{
    /// <summary>
    /// Interface that handles all sorts of inputs and assigns events to them
    /// </summary>
    public interface IInputHandler
    {
        void TriggerInput(string key);
        void AddInput(string key, IInput input);
        void AssignInputEvent(string key, Action inputEvent, InputState state);
        void UpdateInputs();
        void SetInputState(bool state);
    }
}