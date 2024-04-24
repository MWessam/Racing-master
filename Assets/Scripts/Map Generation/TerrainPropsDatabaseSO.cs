using System.Linq;
using UI.Car_Selection;
using UnityEngine;
using UnityEngine.Rendering;

namespace Map_Generation
{
    [CreateAssetMenu(fileName = "Terrain Props Database", menuName = "Scriptable Objects/Map Generation/Terrain Props Database")]
    public class TerrainPropsDatabaseSO : ObjectDatabaseSO<TerrainPropSO>
    {
        public Color SkyBoxColor;
        public Material SkyboxMaterial;
        public VolumeProfile PostProcessVolume;
        public GameObject GetRandomProp()
        {
            var randomProp = Objects[Random.Range(0, Objects.Length)];
            while (randomProp.IsFinish || randomProp.IsStart)
            {
                randomProp = Objects[Random.Range(0, Objects.Length)];
            }
            return randomProp.Prefab;
        }
        public GameObject GetLastProp() => Objects.First(i => i.IsFinish).Prefab;
        public GameObject GetFirstProp() => Objects.First(i => i.IsStart).Prefab;
    }
}
