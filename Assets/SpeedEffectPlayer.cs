using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedEffectPlayer : MonoBehaviour
{
    private ParticleSystem _ps;
    private RCCP_CarController _player;
    private bool _activeEffect;
    // Start is called before the first frame update
    void Start()
    {
        _ps = GetComponent<ParticleSystem>();
        _player = RCCP_SceneManager.Instance.activePlayerVehicle;
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.speed >= PersistantPlayerDataBase.Instance.CurrentCar.TopSpeed.CurrentStat * 0.7f)
        {
            if (!_activeEffect)
            {
                _ps.Play();
                _activeEffect = true;
            }
        }
        else
        {
            _ps.Stop();
            _activeEffect = false;
        }
    }
}
