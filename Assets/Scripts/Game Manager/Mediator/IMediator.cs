namespace Game_Manager.Mediator
{
    public interface IMediator
    {
        void AwakeAll();
        void StartAll();
        void UpdateAll(float deltaTime);
        void FixedUpdateAll(float fixedDeltaTime);
        void OnDestroyAll();
        void OnEnableAll();
        void OnDisableAll();
        void InitializeReferences();
        public bool IsActive { get; }
        public bool IsNotFirstFrame { get; set; }
        public bool HasBeenEnabled { get; }

    }
}