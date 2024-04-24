using Nova;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skdks : MonoBehaviour
{
    [SerializeField] Interactable _button;
    private UIBlock2D _uiBlock;
    // Start is called before the first frame update
    void Start()
    {
        _button = GetComponent<Interactable>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void Button()
    {
        Debug.Log("Hi");
    }
}
