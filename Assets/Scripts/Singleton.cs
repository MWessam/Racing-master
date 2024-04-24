using Game_Manager.Mediator;
using UnityEngine;

public class Singleton<T> : MonoBehaviour, IAwakeable where T : MonoBehaviour
{
    public static T Instance
    {
        get; protected set;
    }

    public virtual void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        ExtraSingletonLogic();
        Instance = GetComponent<T>();
    }
    protected virtual void ExtraSingletonLogic()
    {
    }
}

public class SingletonNonPersistent<T> : Singleton<T> where T : MonoBehaviour
{
    public override void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        ExtraSingletonLogic();
        Instance = GetComponent<T>();
    }
}