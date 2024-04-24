using System;
using System.Collections;
using System.Collections.Generic;
using Game_Manager.Mediator;
using Inputs;
using Nova;
using UI.Loading_Screen;
using UI.UI_Animations;
using UnityEngine;

namespace UI.UI_Input
{
    public class UIHandler : Singleton<UIHandler>, IAwakeable, IStartable, IOnDisabled, IOnEnabled
    {
        IInputHandler _inputHandler = null;
        public IInputHandler InputHandler => _inputHandler;

        [SerializeField] Menu _mainMenu;
        [SerializeField] Menu _startMenu;
        [SerializeField] Menu _currentMenu;
        LoadingMenu _currentLoadingMenu;
        public static readonly Dictionary<MenuTransitionDirection, Vector2> DirectionVectorDictionary = new() { { MenuTransitionDirection.Left, Vector2.left }, { MenuTransitionDirection.Right, Vector2.right }, { MenuTransitionDirection.Top, Vector2.up }, { MenuTransitionDirection.Bottom, Vector2.down } };
        public Menu MainMenu
        {
            get => _mainMenu;
            set => _mainMenu = value;
        }
        public Menu StartMenu
        {
            get => _startMenu;
            set
            {
                _startMenu = value;
            }
        }
        public Menu CurrentMenu
        {
            get => _currentMenu;
            set { _currentMenu = value; }
        }
        public float TransitionAnimationTime = 0.5f;
        public event Action<Camera> OnCameraChange;
        Camera _currentCamera;
        public Camera SetCurrentCamera 
        { 
            set
            {
                _currentCamera = value;
                OnCameraChange?.Invoke(value);
            }
        }

    
        private AnimationHandle _animationHandle = new();
        public override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            _currentMenu = _startMenu;
            _inputHandler = GetComponent<IInputHandler>();
            _inputHandler.AddInput("Touch", new TouchInput());
        }
        public void Start()
        {
            SetCurrentCamera = Camera.current;
        }
        public void TransitionToMenu(Menu menu, MenuTransitionDirection directionStart, MenuTransitionDirection directionEnd)
        {
            if (_currentMenu == menu || !_animationHandle.IsComplete()) return;
            Vector2 startPosition = DirectionVectorDictionary[directionStart] * 500;
            Vector2 endPosition = DirectionVectorDictionary[directionEnd] * 500;
            if (_currentMenu != null)
            {
                Menu previousMenu = _currentMenu;
                previousMenu.gameObject.SetActive(true);
                _currentMenu = menu;
                _animationHandle = previousMenu.Transition(previousMenu.CurrentBlockPositionPercent, endPosition * 2).Run(TransitionAnimationTime);
                _currentMenu.gameObject.SetActive(true);
                StartCoroutine(DisableMenu(previousMenu));
            }
            else
            {
                _currentMenu = menu;
                _currentMenu.gameObject.SetActive(true);
                _animationHandle = menu.Transition(endPosition * 2, menu.CurrentBlockPositionPercent).Run(TransitionAnimationTime);
                StartCoroutine(DisableMenu(_currentMenu));
            }
            _animationHandle = _animationHandle.Include(_currentMenu.Transition(startPosition, Vector2.zero), TransitionAnimationTime);
        }
        
        public void TransitionToLoadingMenu(LoadingMenu loadingMenu)
        {
            _currentLoadingMenu = loadingMenu;
            _currentLoadingMenu.gameObject.SetActive(true);
            StartCoroutine(LoadingMenuTransitionCoroutine());
        }
        public void LoadedMenu(Menu target)
        {
            StartCoroutine(FromLoadingMenuTransition(target));
        }
        public void LoadedScene()
        {
            SetCurrentCamera = GameObject.FindWithTag("Overlay Camera").GetComponent<Camera>();
            _currentLoadingMenu.OnDisable();
            

        }
        private IEnumerator FromLoadingMenuTransition(Menu targetMenu)
        {
            _currentLoadingMenu.OnDisable();
            targetMenu.gameObject.SetActive(true);
            yield return new WaitUntil(() => _currentLoadingMenu.IsLoadingAnimationDone);
            _currentLoadingMenu.gameObject.SetActive(false);
        }
        IEnumerator LoadingMenuTransitionCoroutine()
        {
            yield return new WaitUntil(() => _currentLoadingMenu.IsLoadingAnimationDone);
            _currentMenu.gameObject.SetActive(false);
        }
        IEnumerator DisableMenu(Menu menu)
        {
            yield return new WaitUntil(() => _animationHandle.IsComplete());
            menu.gameObject.SetActive(false); 
        }

        public void OnDisable()
        {
            LoadingSceneManager.OnLoadedScene -= LoadedScene;
        }

        public void OnEnable()
        {
            LoadingSceneManager.OnLoadedScene += LoadedScene;

        }
    }
}
