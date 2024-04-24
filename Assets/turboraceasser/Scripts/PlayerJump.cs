using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;

public class PlayerJump : MonoBehaviour
{

    public bool canPlayerJump;
    public bool canAiJump;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canPlayerJump)
        {
            PathFollower pathFollower = other.GetComponentInParent<PathFollower>();
            pathFollower.deviation = -pathFollower.deviation;
            // pathFollower.StartFlyRamp();
        }

        if (other.CompareTag("Ai") && canAiJump)
        {
            AIPathFollower pathFollower = other.GetComponentInParent<AIPathFollower>();
            pathFollower.deviation = -pathFollower.deviation;
            // pathFollower.StartFlyRamp();
        }
    }


}
