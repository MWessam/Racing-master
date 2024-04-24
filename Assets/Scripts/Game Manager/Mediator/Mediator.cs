using System;
using UnityEngine;
using MonoBehaviour = UnityEngine.MonoBehaviour;

namespace Game_Manager.Mediator
{
    public class Mediator : MonoBehaviour, IMediator
    {
        private IAwakeable[] _awakeables = Array.Empty<IAwakeable>();
        private IUpdateable[] _updateables = Array.Empty<IUpdateable>();
        private IFixedUpdateable[] _fixedUpdateables = Array.Empty<IFixedUpdateable>();
        private IStartable[] _startables = Array.Empty<IStartable>();
        private IOnEnabled[] _onEnables = Array.Empty<IOnEnabled>();
        private IOnDisabled[] _onDisables = Array.Empty<IOnDisabled>();
        private IOnDestroy[] _onDestroys = Array.Empty<IOnDestroy>();
        public bool IsActive { get; set; }
        public bool IsNotFirstFrame { get; set; } = true;
        public bool HasBeenEnabled { get; private set; }
        public bool CanAwakeAgainOnSceneLoad;
        public bool CanStartAgainOnSceneLoad;
        private bool _hasAwoken;
        private bool _hasStarted;
        private void Awake()
        {
            // AwakeAll();
        }
        private void Start()
        {
            // StartAll();
        }
        public void AwakeAll()
        {
            if (_hasAwoken && !CanAwakeAgainOnSceneLoad) return;
            for (int i = 0; i < _awakeables.Length; ++i)
            {
                _awakeables[i].Awake();
            }
            _hasAwoken = true;
        }

        public void FixedUpdateAll(float fixedDeltaTime)
        {
            for (int i = 0; i < _fixedUpdateables.Length; ++i)
            {
                _fixedUpdateables[i].FixedUpdateComponent(fixedDeltaTime);
            }
        }

        public void InitializeReferences()
        {
            _awakeables = GetComponents<IAwakeable>();
            _updateables = GetComponents<IUpdateable>();
            _startables = GetComponents<IStartable>();
            _fixedUpdateables = GetComponents<IFixedUpdateable>();
            _onEnables = GetComponents<IOnEnabled>();
            _onDisables = GetComponents<IOnDisabled>();
            _onDestroys = GetComponents<IOnDestroy>();
            IsActive = gameObject.activeInHierarchy;
        }

        public void OnDestroyAll()
        {
            for (int i = 0; i < _onDestroys.Length; ++i)
            {
                _onDestroys[i].OnDestroy();
            }
        }

        public void OnDisableAll()
        {
            for (int i = 0; i < _onDisables.Length; ++i)
            {
                _onDisables[i].OnDisable();
            }
            IsActive = false;
            HasBeenEnabled = false;
        }
        private void OnEnable()
        {
            // if (!IsNotFirstFrame)
            //     return;
            // if (!_hasAwoken)
            // {
            //     InitializeReferences();
            //     ControlPanel.S_Instance.AddMediator(this);
            //     AwakeAll();
            //     _hasAwoken = true;
            // }
            // OnEnableAll();
            // if (!_hasStarted)
            // {
            //     StartAll();
            //     _hasStarted = true;
            // }
        }
        public void OnEnableAll()
        {
            if (IsActive && HasBeenEnabled) return;
            for (int i = 0; i < _onEnables.Length; ++i)
            {
                _onEnables[i].OnEnable();
            }
            IsActive = true;
            HasBeenEnabled = true;

        }
        public void StartAll()
        {
            if (_hasStarted && !CanStartAgainOnSceneLoad) return;
            for (int i = 0; i < _startables.Length; ++i)
            {
                _startables[i].Start();
            }
            _hasStarted = true;

        }
        private void Update()
        {
            // UpdateAll(Time.deltaTime);
        }
        public void UpdateAll(float deltaTime)
        {
            for (int i = 0; i < _updateables.Length; ++i)
            {
                _updateables[i].Update();
            }
        }
        private void OnDestroy()
        {
            // for (int i = 0; i < _onDestroys.Length; ++i)
            // {
            //     _onDestroys[i].Destroy();
            // }
        }
        private void OnDisable()
        {
            // OnDisableAll();
        }
    }
}