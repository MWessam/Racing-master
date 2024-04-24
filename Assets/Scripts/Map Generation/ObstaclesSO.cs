using UI.Car_Selection;
using UnityEngine;
namespace Map_Generation
{
    [CreateAssetMenu(menuName = "Create ObstaclesSO", fileName = "ObstaclesSO", order = 0)]
    public class ObstaclesSO : DatabaseObjectSO
    {
        public GameObject Prefab;
        public float Chance;
        public Vector3 Offset;

    }
}