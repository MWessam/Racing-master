using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathCreation.Examples
{
    public class Ground : MonoBehaviour
    {
        //private void OnTriggerEnter(Collider other)
        //{
        //    if (other.transform.CompareTag("Player"))
        //    {
        //        PathFollower movement = other.GetComponentInParent<PathFollower>();
        //        movement.enabled = false;
        //        movement.GetComponent<Player>().enabled = false;
        //        movement.fallSpeed = 0;
        //        movement.flyingSpeed = 0;
        //        movement.nos.gameObject.SetActive(false);
        //        movement.ShakeCamera(2.5f, 0.5f);
        //        movement.dead.Play();
        //        movement.solidcar.SetActive(false);
        //        movement.brokencar.SetActive(true);
        //        GameCanvasControl.Instance.gameOverPanelControl.EnablePanel(GameState.LevelFailed, 1.5f);
        //    }
        //}

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<BoxCollider>().enabled = false;
                PathFollower movement = collision.transform.GetComponentInParent<PathFollower>();
                movement.enabled = false;
                movement.GetComponent<Player>().enabled = false;
                movement.fallSpeed = 0;
                movement.flyingSpeed = 0;
                movement.nos.gameObject.SetActive(false);
                movement.ShakeCamera(2.5f, 0.5f);
                movement.dead.Play();
                GameManager.Instance.Lose();
                HapticManager.Instance.PlayHaptic(HapticType.Failure);

            }
        }

    }
}
