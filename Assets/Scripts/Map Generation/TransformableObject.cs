using Game_Manager.Mediator;
using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
namespace Map_Generation
{
    public class TransformableObject : Obstacle
    {
        public Vector3 RotationAxis;
        public Vector3 MovementAxis;
        public float MetersPerSeconds;
        public float AnglePerSeconds;
        private Vector3 _currRot;
        private Quaternion currentRotQuat;
        private void Start()
        {
            _currRot = transform.localRotation.eulerAngles;
            AnglePerSeconds = Random.Range(90, 270);
        }
        public void Update()
        {
            var transform1 = transform;
            _currRot += RotationAxis * (Time.deltaTime * AnglePerSeconds);
            transform1.localRotation = Quaternion.Euler(_currRot);
            transform1.position += MetersPerSeconds * Time.deltaTime * MovementAxis;
        }
    }
}
