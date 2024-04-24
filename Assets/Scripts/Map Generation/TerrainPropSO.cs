using UI.Car_Selection;
using UnityEngine;
using UnityEngine.Serialization;

namespace Map_Generation
{
    [CreateAssetMenu(fileName = "Terrain Prop", menuName = "Scriptable Objects/Map Generation/Terrain Prop")]
    public class TerrainPropSO : DatabaseObjectSO
    {
        public GameObject Prefab;
        public Vector3 Offset;
        public float Chance;
        public bool IsFinish;
        public bool IsStart;

    }


}
