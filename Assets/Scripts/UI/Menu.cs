using Game_Manager.Mediator;
using Nova;
using UI.ButtonInput;
using UI.UI_Input;
using UnityEngine;

namespace UI
{
    public class Menu : MonoBehaviour, IAwakeable, IStartable
    {
        public UIBackToMainMenuButton BackButton;
        protected UIBlock2D _menuBlock;
        protected AnimationHandle _animationHandle;
        public virtual void Awake()
        {
            _menuBlock = GetComponent<UIBlock2D>();
        }
        public void Start()
        {
            _menuBlock.transform.position = Vector3.zero;
            if (UIHandler.Instance.CurrentMenu == null)
            {
                UIHandler.Instance.CurrentMenu = this;
                UIHandler.Instance.StartMenu = this;
                UIHandler.Instance.MainMenu = this;
            }
            MenuInitialize();
        }
        protected virtual void MenuInitialize()
        {
        }
        /// <summary>
        /// Seamlessly transition to position. 
        /// START POSITION: WILL TELEPORT MENU TO THAT POSITION! (It should be inactive eitherway)
        /// END POSITION: WILL TWEEN TO THAT POSITION
        /// </summary>
        /// <param name="startPositionPercentage"></param>
        /// <param name="endPositionPercentage"></param>
        public virtual UIMenuAnimation Transition(Vector2 startPositionPercentage, Vector2 endPositionPercentage)
        {
            if (BackButton != null)
            {
                BackButton.gameObject.SetActive(this != UIHandler.Instance.MainMenu);
            }
            _menuBlock.Position.Raw = startPositionPercentage;
            return new UIMenuAnimation() { CurrentBlock = _menuBlock , TargetPositionPercentage = endPositionPercentage};
        }
        public Vector2 CurrentBlockPositionPercent => _menuBlock.Position.Raw;
    }
}
