using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public static float MouseX;

    private float lastTouchX;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            lastTouchX = Input.mousePosition.x;
        }
        else if(Input.GetMouseButton(0))
        {
            MouseX = Input.mousePosition.x - lastTouchX;
            //MouseX = Mathf.Clamp(Input.mousePosition.x - lastTouchX, -50f, 50f);
            lastTouchX = Input.mousePosition.x;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            MouseX = 0;
        }

    }



}
