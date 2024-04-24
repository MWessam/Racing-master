using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

namespace PathCreation.Examples
{

    public class UiManager : MonoBehaviour
    {
        public DistanceView Distance;
        public SpeedView Speed;
        public RankingView Rank;

        [HideInInspector]
        public DistanceCounter playerDistanceCounter;
        private TextMeshPro playerText;
        [HideInInspector]
        private Finish finish;
        private PathFollower playerPath;
        private Player _player;
        private HelpScreen _help;
        
        public bool finishOnce;

        private void Awake()
        {
            _player = GameObject.FindFirstObjectByType<Player>();
            playerDistanceCounter = _player.GetComponent<DistanceCounter>();
            finish = GameObject.FindGameObjectWithTag("Finish").GetComponent<Finish>();
            playerText = playerDistanceCounter.text;
            playerPath = playerDistanceCounter.GetComponent<PathFollower>();
            finishOnce = false;
            _help = FindObjectOfType<HelpScreen>(true);
        }

        void Start()
        {
            _nitroButton.gameObject.SetActive(false);
        }
        void Update()
        {
            if (playerPath.isStart)
            {
                if (_help.gameObject.activeSelf)
                {
                    _help.gameObject.SetActive(false);
                }

                if (!_nitroButton.activeSelf)
                {
                    _nitroButton.SetActive(true);
                }
            }
            Rank.UpdateRank(playerDistanceCounter.rank);
            Speed.UpdateSpeed(playerPath.speed);
            float finishDistance = playerPath.pathCreator.path.GetClosestDistanceAlongPath(finish.transform.position);
            Distance.UpdateDistance(playerPath.distanceTravelled / finishDistance, 1);
            if(playerPath.isfinish && !finishOnce)
            {
                finishOnce = true;
                _nitroButton.gameObject.SetActive(false);
            }

            if (_nitroCount <= 0 && _nitroButton.activeSelf)
            {
                _nitroButton.SetActive(false);
            }

        }


        List<T> GetUniqueRandomElements<T>(List<T> inputList, int count)
        {
            List<T> inputListClone = new List<T>(inputList);
            Shuffle(inputListClone);
            return inputListClone.GetRange(0, count);
        }

        void Shuffle<T>(List<T> inputList)
        {
            for (int i = 0; i < inputList.Count - 1; i++)
            {
                T temp = inputList[i];
                int rand = Random.Range(i, inputList.Count);
                inputList[i] = inputList[rand];
                inputList[rand] = temp;
            }
        }

        private int _nitroCount = 2;
        private bool _isNitro;
        public GameObject _nitroButton;

        public void UseNitro()
        {
            if (_nitroCount <= 0 || _isNitro) return;
            _nitroCount--;
            if (_nitroCount == 0)
            {
                _nitroButton.gameObject.SetActive(false);
            }
            playerPath.ApplyTempMultiplicator(1 + PersistantPlayerDataBase.Instance.CurrentCar.Nitro.CurrentStat / 100, 4);
            StartCoroutine(UsedNitro());
        }

        private IEnumerator UsedNitro()
        {
            _isNitro = true;
            var oldMaxSpeed = playerPath.MaxSpeed;
            playerPath.MaxSpeed *= 1 + PersistantPlayerDataBase.Instance.CurrentCar.Nitro.CurrentStat / 100;
            yield return new WaitForSeconds(4.0f);
            playerPath.MaxSpeed = oldMaxSpeed;
            _isNitro = false;

        }
    }
}