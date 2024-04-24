using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;
using DG.Tweening;

public class ObstacleControl : MonoBehaviour
{

    Rigidbody rigidbody;
    bool collided = false;
    float minForce = 10f;
    float currentSpeed;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PathFollower pathFollower = collision.gameObject.GetComponentInParent<PathFollower>();


            if (!collided)
            {
                collided = true;
                //if(!pathFollower.landBoost)
                //{
                     currentSpeed = pathFollower.speed / 1.25f;
                //}
                
                Vector3 force = pathFollower.transform.forward * currentSpeed;
                rigidbody.AddForce(force, ForceMode.Impulse);
                pathFollower.speed /= 1.5f;
                

                
            }

            else
            {
               
                //if (!pathFollower.landBoost)
                //{
                     currentSpeed = pathFollower.speed / 2f;
                //}
                
                if (currentSpeed < minForce) currentSpeed = minForce;
                Vector3 force = pathFollower.transform.forward * currentSpeed;
                rigidbody.AddForce(force, ForceMode.Impulse);
            }

            HapticManager.Instance.PlayHaptic(HapticType.MediumImpact);

        }

        if (collision.gameObject.CompareTag("Ai"))
        {
            AIPathFollower pathFollower = collision.gameObject.GetComponentInParent<AIPathFollower>();


            if (!collided)
            {
                collided = true;
                float currentSpeed = pathFollower.speed / 1.25f;
                Vector3 force = pathFollower.transform.forward * currentSpeed;
                rigidbody.AddForce(force, ForceMode.Impulse);
                pathFollower.speed /= 1.5f;

            }

            else
            {
                float currentSpeed = pathFollower.speed / 2f;
                if (currentSpeed < minForce) currentSpeed = minForce;
                Vector3 force = pathFollower.transform.forward * currentSpeed;
                rigidbody.AddForce(force, ForceMode.Impulse);
            }

        }
    }
}
