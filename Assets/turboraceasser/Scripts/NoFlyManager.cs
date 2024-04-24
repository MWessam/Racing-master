using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathCreation.Examples
{
    public class NoFlyManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
                PathFollower movement = other.GetComponentInParent<PathFollower>();
                movement.canFly = false;
            }

            if (other.transform.CompareTag("Ai"))
            {
                AIPathFollower movement = other.GetComponentInParent<AIPathFollower>();
                movement.canFly = false;
            }
        }


        private void OnTriggerExit(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
                PathFollower movement = other.GetComponentInParent<PathFollower>();
                movement.canFly = true;
            }

            if (other.transform.CompareTag("Ai"))
            {
                AIPathFollower movement = other.GetComponentInParent<AIPathFollower>();
                movement.canFly = true;
            }
        }



    }
}