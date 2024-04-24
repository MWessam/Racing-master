using UI.Car_Selection;
using UnityEngine;

namespace Map_Generation
{
    [CreateAssetMenu(fileName ="Road Segment", menuName = "Scriptable Objects/Map Generation/Road Segment")]
    public class RoadSegmentSO : DatabaseObjectSO
    {
        public GameObject Prefab;
        public Vector3 Offset;
        public float Chance;
    }
}
