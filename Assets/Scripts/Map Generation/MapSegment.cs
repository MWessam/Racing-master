using System.Linq;
using UnityEngine;
namespace Map_Generation
{
    public class MapSegment
    {
        public readonly GameObject RoadSeg;
        public readonly GameObject Obstacles;
        public readonly GameObject[] TerrainProps;
        public MapSegment(RoadSegmentsDatabaseSO roadSegments, ObstaclesDatabaseSO obstacles, TerrainPropsDatabaseSO terrains, Vector3 lastPosition)
        {
            float rng = Random.Range(0.0f, 1.0f);
            var road = roadSegments.Objects[0];
            var obstacle = obstacles.Objects[0];
            var terrain = terrains.Objects.ToArray();
            Vector3 offsetPosition = lastPosition + road.Offset;
            RoadSeg = GameObject.Instantiate(road.Prefab, offsetPosition, Quaternion.identity);
            var obstacleOffset = new Vector3(Random.Range(-obstacle.Offset.x, obstacle.Offset.x), obstacle.Offset.y, Random.Range(-obstacle.Offset.z, obstacle.Offset.z));
            Obstacles = GameObject.Instantiate(obstacle.Prefab, offsetPosition + obstacleOffset, Quaternion.identity);
            int range = Random.Range(0, terrain.Length - 1);
            TerrainProps = new GameObject[range + 1];
            for (int i = 0; i <= range; ++i)
            {
                var terrainOffset  = new Vector3(Random.Range(-terrain[i].Offset.x, terrain[i].Offset.x), terrain[i].Offset.y, Random.Range(-terrain[i].Offset.z, terrain[i].Offset.z));
                TerrainProps[i] = GameObject.Instantiate(terrain[i].Prefab, offsetPosition + terrainOffset, terrain[i].Prefab.transform.rotation);
            }
        }

    }
}