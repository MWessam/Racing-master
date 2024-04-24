using AmazingAssets.CurvedWorld;
using DefaultNamespace;
using Game_Manager.Mediator;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
namespace Map_Generation
{
    public class Map : Singleton<Map>, IAwakeable, IStartable
    {
        [SerializeField]
        private TerrainPropsDatabaseSO _themePropsSegment;
        [SerializeField]
        private Volume _volume;
        private List<GameObject> _objects = new();
        public List<CurvePoint> Curves = new();
        private int _currentCurveIndex = 0;
        public Vector3 NextObjectPosition
        {
            get
            {
                if (_currentCurveIndex + 1 > _objects.Count - 2) return _objects[^1].transform.position;
                return _objects[_currentCurveIndex + 1].transform.position;
            }
        }
        public float DistanceBetweenCurrent2Segments
        {
            get
            {
                if (_currentCurveIndex >= _objects.Count - 1) return 0.0f;
                return (_objects[_currentCurveIndex].transform.position - _objects[_currentCurveIndex + 1].transform.position).magnitude;
            }
        }
        public CurvePoint CurrentCurve => Curves[Mathf.Clamp(_currentCurveIndex, 0, Curves.Count - 1)];
        public CurvePoint GetNextCurve 
        {
            get
            {
                if (_currentCurveIndex + 1 >= Curves.Count)
                {
                    return Curves[^1];
                }
                return Curves[_currentCurveIndex + 1];
            }
        }
        public CurvedWorldController CurveController;
        [SerializeField] 
        private int _segmentCount;
        public Transform LastSegment => _objects[^1].GetComponentInChildren<FinishLine>().transform;
        public Transform LastCheckpoint => _objects[^1].GetComponentsInChildren<CheckPoint>().Last().transform;
        public override void Awake()
        {
            base.Awake();
            _themePropsSegment = PersistantPlayerDataBase.Instance.GetRandomTheme();
            _themePropsSegment.Initialize();
            _volume.profile = _themePropsSegment.PostProcessVolume;
            Vector3 lastPosition = Vector3.zero - Vector3.forward * 3.0f;
            CurvePoint curvePoint = new CurvePoint(Vector3.zero, Vector2.zero);
            Curves.Add(curvePoint);
            Transform firstProp = _themePropsSegment.GetFirstProp().transform;
            _objects.Add(Instantiate(firstProp, lastPosition, firstProp.rotation, transform).gameObject);
            lastPosition = _objects[0].GetComponentInChildren<SegmentConnector>().transform.position;
            for (int i = 1; i < _segmentCount - 1; ++i)
            {
                Transform randomProp = _themePropsSegment.GetRandomProp().transform;
                _objects.Add(Instantiate(randomProp.gameObject, lastPosition, randomProp.rotation, transform));
                lastPosition = _objects[i].GetComponentInChildren<SegmentConnector>().transform.position;
                Vector2 RandomDirection = new Vector2(Random.Range(-0.5f,0.5f), Random.Range(-0.1f,0.1f));
                Vector2 bendStrength = Vector2.Lerp(Curves[^1].BendStrength, RandomDirection, Random.Range(0.5f,0.9f));
                curvePoint = new CurvePoint(lastPosition, bendStrength);
                Curves.Add(curvePoint);
            }
            foreach (var obstacle in FindObjectsOfType<Obstacle>())
            {
                obstacle.gameObject.layer = LayerMask.NameToLayer("Obstacle");
            }
            // var car = RCCP.SpawnRCC(PersistantPlayerDataBase.S_Instance.Cars[0].CarDataStatics.CarPrefab, transform.position, Quaternion.identity, false, false, false);
            Transform lastProp = _themePropsSegment.GetLastProp().transform;
            _objects.Add(Instantiate(lastProp.gameObject, lastPosition, lastProp.rotation, transform));
            _objects[^1].gameObject.AddComponent<EndSegment>();
            LastCheckpoint.gameObject.AddComponent<RCCP_Waypoint>();
            _objects[^1].transform.position = _objects[^2].GetComponentInChildren<SegmentConnector>().transform.position;
            RenderSettings.skybox = _themePropsSegment.SkyboxMaterial;
            // Camera.main.backgroundColor = _themePropsSegment.SkyBoxColor;
        }
        public void OnChunkTriggerHandler(GameObject chunkToCheck)
        {
            for (int i = 0; i < _objects.Count; i++)
            {
                GameObject chunk = _objects[i];
                if (chunk != chunkToCheck)
                    continue;
                // for (int j = 0; j < i; j++)
                // {
                //     _objects[j].SetActive(false);
                // }
                _currentCurveIndex = i + 1;
                break;
            }
        }
        public void Start()
        {
            
        }
    }
}