using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[CustomEditor(typeof(PlayerInitializer))]
public class ObstacleGenerator : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        PlayerInitializer initializer = (PlayerInitializer)target;
        if (GUILayout.Button("Initialize Random Obstacles"))
        {
            initializer.GameManager.ConstructLine();
            if (initializer.ObstacleObjects.Count >= 0)
            {
                initializer.ObstacleObjects.ForEach(x => DestroyImmediate(x));
                initializer.ObstacleObjects.Clear();
            }
            float start = 0.1f;
            int count = 10;
            float[] cumulative = GenerateCumulativeRandomNumbers(count, start, 0.8f);
            for (int i = 0; i < count; ++i)
            {
                var position = initializer.PathCreator.path.GetPointAtTime(cumulative[i]);
                var rotation = initializer.PathCreator.path.GetRotation(cumulative[i]);
                rotation.eulerAngles = new Vector3(0.0f, rotation.eulerAngles.y, 0.0f);
                var randomObstacle = initializer.Obstacles[Random.Range(0, initializer.Obstacles.Count)];
                var obstacleHolder = new GameObject();
                obstacleHolder.transform.parent = initializer.ObstacleHolder;
                obstacleHolder.transform.rotation = rotation;
                var obstacle = (GameObject)PrefabUtility.InstantiatePrefab(randomObstacle.Obstacle, obstacleHolder.transform);
                obstacle.transform.localPosition = Vector3.zero;
                obstacleHolder.transform.position =
                    position + Vector3.up * Random.Range(randomObstacle.YRange.x, randomObstacle.YRange.y);
                initializer.ObstacleObjects.Add(obstacleHolder);
            }
        }

        if (GUILayout.Button("Delete Obstacles"))
        {
            if (initializer.ObstacleObjects.Count >= 0)
            {
                initializer.ObstacleObjects.ForEach(x => DestroyImmediate(x));
                initializer.ObstacleObjects.Clear();
            }
        }
    }
    private static float[] GenerateCumulativeRandomNumbers(int n, double x, double y)
    {
        System.Random rand = new System.Random();
        float[] randomNumbers = new float[n];

        for (int i = 0; i < n; i++)
        {
            // Generate a random number between x and y
            randomNumbers[i] = (float)(x + (rand.NextDouble() * (y - x)));
        }

        // Sort the array to make the numbers cumulative
        Array.Sort(randomNumbers);
        return randomNumbers;
    }
}
