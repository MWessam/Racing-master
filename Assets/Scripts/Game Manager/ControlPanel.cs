using Game_Manager.Mediator;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Game_Manager
{
    /// <summary>
    /// Class that handles initialization and update of monobehaviours.
    /// Increases performance by up to 10x and honestly makes working with unity callback much easier
    /// </summary>
    public class ControlPanel : Singleton<ControlPanel>, IAwakeable
    {
        private List<Mediator.Mediator> _mediators = new();
        private LinkedList<Mediator.Mediator> _dirtyMediators = new();
        private void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            AwakeMediators();
        }
        public void AddMediator(Mediator.Mediator mediator)
        {
            if (_dirtyMediators.Contains(mediator) || _mediators.Contains(mediator)) return;
            _dirtyMediators.AddLast(mediator);
        }
        public void AwakeMediators()
        {
            InitializeMediatorsArray();
            foreach (IMediator mediator in _mediators)
            {
                if (mediator.IsActive)
                {
                    mediator.AwakeAll();
                }
            }
            foreach (IMediator mediator in _mediators)
            {
                if (mediator.IsActive)
                {
                    mediator.OnEnableAll();
                }
            }
            foreach (IMediator mediator in _mediators)
            {
                if (mediator.IsActive)
                {
                    mediator.StartAll();
                }
            }
        }
        private void InitializeMediatorsArray()
        {
            _mediators = FindObjectsOfType<Mediator.Mediator>(true).ToList();
            foreach(IMediator mediator in _mediators)
            {
                mediator.InitializeReferences();
            }
        }

        private void Update()
        {
            if (_dirtyMediators.First != null)
            {
                _mediators.AddRange(_dirtyMediators);
                _dirtyMediators.Clear();
            }
            foreach (IMediator mediator in _mediators)
            {
                if (mediator.IsActive)
                {
                    mediator.UpdateAll(Time.deltaTime);
                }
            }
        }
        private void FixedUpdate()
        {
            foreach (IMediator mediator in _mediators)
            {
                if (mediator.IsActive)
                {
                    mediator.FixedUpdateAll(Time.fixedDeltaTime);
                }
            }
        }
    }
}