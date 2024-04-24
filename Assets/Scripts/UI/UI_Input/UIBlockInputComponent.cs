using Game_Manager.Mediator;
using Nova;
using UnityEngine;

namespace UI.UI_Input
{
    public abstract class UIBlockInputComponent : MonoBehaviour, IAwakeable, IStartable
    {
        protected UIBlock2D _block;
        public virtual void Awake()
        {
            _block = GetComponent<UIBlock2D>();
        }
        public virtual void Start() { }
    }
}
