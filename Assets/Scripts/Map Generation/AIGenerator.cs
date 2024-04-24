using Cars;
using Game_Manager.Mediator;
using System;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using Random = UnityEngine.Random;
namespace Map_Generation
{
    public class AIGenerator : MonoBehaviour, IStartable
    {
        [SerializeField] 
        private Transform _player;
        public RankingView RankPrefab;
        [SerializeField]
        private NavMeshSurface _navMeshSurface;
        [SerializeField]
        private RCCP_AIWaypointsContainer _waypointsContainer;
        [SerializeField]
        private int _aiCount;
        private AISpawnPoint[] _spawnPoints;
        private PlayerSpawnPoint _playerSpawnPoint;
        private CarData[] _carDatabase;
        public void Start()
        {
            _carDatabase = PersistantPlayerDataBase.Instance.Cars;
            _playerSpawnPoint = FindObjectOfType<PlayerSpawnPoint>();
            _spawnPoints = FindObjectsOfType<AISpawnPoint>().Shuffle().Take(_aiCount).ToArray();
            _carDatabase.Initialize();
            _navMeshSurface.BuildNavMesh();
            _waypointsContainer.PopulateWaypoints();
            for (int i = 0; i < Math.Clamp(_aiCount, 0, _spawnPoints.Length); ++i)
            {
                int carIndex = Random.Range(0, _carDatabase.Length);
                var aiCar = RCCP.SpawnRCC(_carDatabase[carIndex].CarDataStatics.CarPrefab, _spawnPoints[i].transform.position, _player.rotation, false, true, false);
                aiCar.name = "AI Car";
                var ai = aiCar.gameObject.AddComponent<RCCP_AI>();
                ai.obstacleLayers = LayerMask.GetMask(LayerMask.LayerToName(6));
                ai.raycastLength = 30.0f;
                ai.raycastAngle = 10.0f;
                var aiController = aiCar.gameObject.AddComponent<CarController>();
                ai.waypointsContainer = _waypointsContainer;
                ai.maximumSpeed = _carDatabase[carIndex].TopSpeed.CurrentStat;
                aiCar.Engine.maximumTorqueAsNM = _carDatabase[carIndex].Acceleration.CurrentStat * 25;
                var rank = Instantiate(RankPrefab, ai.transform.position, ai.transform.rotation, ai.transform);
                aiController.RankView = rank;
                ai.nextWaypointPassDistance = 0;
            }
            _player.position = _playerSpawnPoint.transform.position + Vector3.up;
            _player.rotation = Quaternion.identity;
        }
    }
}
