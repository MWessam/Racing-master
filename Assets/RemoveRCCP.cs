using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RemoveRCCP : MonoBehaviour
{
    [SerializeField] public Transform NewTransform;
    public void Remove()
    {
        foreach (Transform component in transform)
        {
            if (component.name.ToLower().Contains("RCCP".ToLower()))
            {
                component.gameObject.SetActive(false);
            }
            else
            {
                component.parent = transform.parent;
                component.localPosition = Vector3.zero;
            }
        }
        gameObject.SetActive(false);
    }
    
}
