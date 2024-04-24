using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;
using PathCreation;
using DG.Tweening;


public class Finish : MonoBehaviour
{

    public Transform[] finishLineWayPoints;
    public Transform[] finishLineFinishPositions;

    public ParticleSystem finishParticle;
    public int playerCount=0;

    public RaceManager raceManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            other.GetComponent<BoxCollider>().enabled = false;
            playerCount++;
            raceManager.canSort = false;
            GameManager.Instance.cameraManagement.RaceFinish(new Vector3(5f, 9f, -10f), 0f);


            PathFollower movement = other.GetComponentInParent<PathFollower>();
            movement.playerControl.finishDeviation = playerCount % 2 == 0 ? -.3f : .3f;

            movement.warpEffect.gameObject.SetActive(false);

            Vector3 playerFinishPoint = finishLineFinishPositions[playerCount - 1].position;
            float finishDistanceTravelled = movement.pathCreator.path.GetClosestDistanceAlongPath(playerFinishPoint);
            movement.finishDistanceTravelled = finishDistanceTravelled;

            movement.isfinish = true;

            if(playerCount <= 1) finishParticle.Play();

            movement.MaxSpeed = 40f - 3f * playerCount;
            movement.speed = movement.MaxSpeed;
            GameManager.Instance.Win();

            HapticManager.Instance.PlayHaptic(HapticType.Success);

        }

        if (other.transform.CompareTag("Ai"))
        {
            other.GetComponent<BoxCollider>().enabled = false;
            playerCount++;
            if (playerCount <= 1) finishParticle.Play();

            AIPathFollower movement = other.GetComponentInParent<AIPathFollower>();
            AI ai = movement.GetComponent<AI>();
            ai.finishDeviation = playerCount % 2 == 0 ? -.3f : .3f;

            Vector3 playerFinishPoint = finishLineFinishPositions[playerCount - 1].position;
            float finishDistanceTravelled = movement.pathCreator.path.GetClosestDistanceAlongPath(playerFinishPoint);
            movement.finishDistanceTravelled = finishDistanceTravelled;

            movement.MaxSpeed = 40f - 3f * playerCount;
            movement.speed = movement.MaxSpeed;

            movement.isfinish = true;
        }

    }
}