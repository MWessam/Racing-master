using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SubscribeOverlayCam : MonoBehaviour
{
    private Camera mainCam;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        var camData = mainCam.GetUniversalAdditionalCameraData();
        var overlay = GetComponent<Camera>();
        if (!camData.cameraStack.Contains(overlay))
        {
            camData.cameraStack.Add(overlay);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
