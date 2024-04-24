using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, 1.0f);
    }
}
