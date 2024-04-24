using System.Collections;
using Game_Manager.Mediator;
using Nova;
using UI.UI_Input;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Loading_Screen
{
    public class LoadingMenu : Menu, IOnEnabled, IOnDisabled, IOnDestroy
    {
        [SerializeField] LoadingIcon _loadingIcon;
        [SerializeField] float _animationTime;
        public int SceneName;
        [SerializeField] GameObject _root;
        public bool IsLoadingAnimationDone => _animationHandle.IsComplete();
        public override void Awake()
        {
            base.Awake();
            UIHandler.Instance.OnCameraChange += ReadyLoadingUICamera;
        }
        void ReadyLoadingUICamera(Camera cam)
        {
            if (_root.TryGetComponent(out ScreenSpace screen))
            {
                // screen.TargetCamera = cam;
            }

        }
        public void OnEnable()
        {
            if(_root.TryGetComponent<ScreenSpace>(out var space))
            {
                space.TargetCamera = Camera.main;
                Camera.main.transform.rotation = Quaternion.Euler(Vector3.zero);
            }
            _animationHandle = new FadingAnimations() { Block = _menuBlock, IsFadeIn = true }.Run(_animationTime);
            StartCoroutine(StartLoadingIcon());
        }

        public void OnDisable()
        {
            // _loadingIcon.CancelAnimation();
            // _loadingIcon.gameObject.SetActive(false);
            _root.gameObject.SetActive(false);
            StartCoroutine(DisableTimer());
        }
        IEnumerator DisableTimer()
        {
            _animationHandle = new FadingAnimations() { Block = _menuBlock, IsFadeIn = false }.Run(_animationTime);
            yield return new WaitUntil( () => _animationHandle.IsComplete());
            gameObject.SetActive(false);
            // _loadingIcon.gameObject.SetActive(false);

        }
        IEnumerator StartLoadingIcon()
        {
            yield return new WaitUntil(() => _animationHandle.IsComplete());
            // _loadingIcon.gameObject.SetActive(true);
        }

        public void OnDestroy()
        {
            UIHandler.Instance.OnCameraChange -= ReadyLoadingUICamera;

        }
        public void InitializeLoadMenu()
        {
            _root.gameObject.SetActive(true);
        }
    }
}
