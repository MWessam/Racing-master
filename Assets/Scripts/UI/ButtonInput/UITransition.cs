using Game_Manager.Mediator;
using UI.UI_Input;
using UnityEngine;
namespace UI.ButtonInput
{
    public class UITransition : MonoBehaviour, IAwakeable
    {
        [SerializeField] protected MenuTransitionDirection _from;
        [SerializeField] protected MenuTransitionDirection _to;
        [SerializeField] protected Menu _targetMenu;
        public void Transition()
        {
            UIHandler.Instance.TransitionToMenu(_targetMenu, _from, _to);
            gameObject.SetActive(false);
        }

        public void Awake()
        {
        }
    }
}
