using UI.UI_Input;

namespace UI.ButtonInput
{
    public class UIBackToMainMenuButton : UITransitionToMenuButton
    {
        public override void Awake()
        {
            base.Awake();
        }
        public override void Start()
        {
            base.Start();
            _targetMenu = UIHandler.Instance.MainMenu;

        }

    }
}
