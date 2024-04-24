using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenRoot : Singleton<MonoBehaviour>
{
    public override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}
