using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowRotate : MonoBehaviour {

  
    public Transform target;

    [SerializeField]
    private Vector3 offsetPosition;

    private Vector3 velocity = Vector3.zero;


    public PathCreation.PathCreator midPath;

    float distanceTravelled = 0f;

    public bool isFinish;

    public float smoothness;

    public bool isfly;

      public float moveHardness = 1;
    [SerializeField] private float lookHardness = 1;

    public Transform placeHolder;

    [SerializeField] private Vector2 deviationChangeTimeRange = new Vector2();
   

   
    private float _timer;
    private float _currentDeviationDuration;

    private float initoffsetz;

    private void Start()
    {
     
        initoffsetz = offsetPosition.z;
    }

    private void Update()
    {
        //if (_timer > _currentDeviationDuration)
        //{
        //    _currentDeviationDuration = Random.Range(deviationChangeTimeRange.x, deviationChangeTimeRange.y);
        //    _timer = 0;
        //    offsetPosition.z = initoffsetz + Random.Range(-1f, 1f);
        //}

        //_timer += Time.deltaTime;
    }

    private void LateUpdate()
    {
        
       Refresh();
         
       
    }


    public void Refresh()
    {
        distanceTravelled = midPath.path.GetClosestDistanceAlongPath(target.position);
        Quaternion rot = midPath.path.GetRotationAtDistance(distanceTravelled, PathCreation.EndOfPathInstruction.Loop);

        Vector3 currentRot = ApplyRotationToVector(offsetPosition, rot.eulerAngles.y);

        rot.eulerAngles = new Vector3(50, rot.eulerAngles.y, 0);
        transform.SetPositionAndRotation(target.position + currentRot, rot);


    }


    Vector3 ApplyRotationToVector(Vector3 vec, float angle)
    {
        return Quaternion.Euler(0, angle, 0) * vec;
    }


   

}
