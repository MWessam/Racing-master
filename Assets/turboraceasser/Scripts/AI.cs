using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PathCreation.Examples
{

    public class AI : MonoBehaviour
    {
        [SerializeField] private Vector2 deviationChangeTimeRange = new Vector2();
        [SerializeField] private float dragControl = 1;

        [HideInInspector]
        public AIPathFollower _movements;
        private float _timer;
        private float _currentDeviationDuration;
        private float _targetDeviation;
        private float _targetMaxSpeed;
        [SerializeField] private float _marginFromEjection;
        private float _maxDeviation;
        public float initMaxSpeed;
        public Player player;

        public float finishDeviation;

        private void OnEnable()
        {
            _movements = GetComponent<AIPathFollower>();
        }

        void Start()
        {
            initMaxSpeed = _movements.MaxSpeed;
            _maxDeviation = _movements.ejectionThresold - _marginFromEjection;
            _targetDeviation = _movements.deviation;
        }
        // Update is called once per frame
        void Update()
        {
           // if (!Movements.Moving) return;
           if(!_movements.isfinish && !player._movements.isfinish)
            {
                if (_timer > _currentDeviationDuration && _movements.isStart && _movements.deviationModifAuthorization && !_movements.isfinish)
                {
                    _currentDeviationDuration = Random.Range(deviationChangeTimeRange.x, deviationChangeTimeRange.y);

                    _targetDeviation = Random.Range(-_maxDeviation, _maxDeviation);

                    _targetMaxSpeed = Random.Range(initMaxSpeed - 1f, initMaxSpeed + 1f);

                    if (_movements.onPath)
                    {
                        if ((player._movements.distanceTravelled - _movements.distanceTravelled) > 30 && !player._movements.isfinish)
                        {
                            _movements.distanceTravelled = player._movements.distanceTravelled - Random.Range(20, 30);
                            _targetMaxSpeed = initMaxSpeed + 10;
                        }

                        if ((_movements.distanceTravelled - player._movements.distanceTravelled) > 15 && !_movements.isfinish)
                        {

                            _targetMaxSpeed = initMaxSpeed - 3;
                        }
                    }

                    else
                    {
                        if (Mathf.Abs((player._movements.distanceTravelled - _movements.distanceTravelled)) > 80)
                        {
                            _movements.distanceTravelled = player._movements.distanceTravelled - Random.Range(20, 30);
                            _movements.distanceCovered = (_movements.distanceTravelled - _movements.startDistance);
                            _movements.Land();
                            _movements.fire.gameObject.SetActive(false);
                            _targetMaxSpeed = initMaxSpeed + 10;
                        }
                    }


                    _timer = 0;
                }

                if (_movements.deviationModifAuthorization && _movements.isStart && _movements.onPath && !_movements.isfinish && !player._movements.isfinish)
                    _movements.deviation = Mathf.Lerp(_movements.deviation, _targetDeviation, dragControl * Time.deltaTime);


                if (_movements.deviationModifAuthorization)
                {
                    _timer += Time.deltaTime;
                }


                else
                {
                    _timer = 0;
                }
            }


            if(player._movements.isfinish && !_movements.isfinish)
            {

                if (_timer > _currentDeviationDuration && _movements.isStart && _movements.deviationModifAuthorization )
                {

                    if (_movements.onPath)
                    {
                        _movements.distanceTravelled = player._movements.distanceTravelled - Random.Range(130, 140);
                    }

                    else
                    {

                        _movements.distanceTravelled = player._movements.distanceTravelled - Random.Range(130, 140);
                        _movements.distanceCovered = (_movements.distanceTravelled - _movements.startDistance);
                        _movements.Land();


                    }


                    _timer = 0;


                }
                if (_movements.deviationModifAuthorization && _movements.isStart && _movements.onPath && !_movements.isfinish && !player._movements.isfinish)
                    _movements.deviation = Mathf.Lerp(_movements.deviation, _targetDeviation, dragControl * Time.deltaTime);


                if (_movements.deviationModifAuthorization)
                {
                    _timer += Time.deltaTime;
                }


                else
                {
                    _timer = 0;
                }

            }


            if (_movements.isfinish)
            {

                _targetDeviation = Mathf.Lerp(_targetDeviation, finishDeviation, Time.deltaTime * 2.25f);
                _movements.deviation = _targetDeviation;

            
            }

           
        }
    }
}