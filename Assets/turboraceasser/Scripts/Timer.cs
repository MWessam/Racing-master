using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Timer : MonoBehaviour
{

    public TextMeshProUGUI counter;
    public GameObject bg;

    private float time=4;

    private bool isStart;

    private bool isFinish;

    void Start()
    {
        isStart = false;
        counter.gameObject.SetActive(false);
        bg.SetActive(false);
    }

   
    void Update()
    {
        if(Input.GetMouseButtonDown(0)&& !isStart && !isFinish)
        {
            isStart = true;
            counter.gameObject.SetActive(true);
            bg.SetActive(true);
        }


        if(isStart)
        {
            if (time > 1)
            {
                time -= Time.deltaTime;
                counter.text = Mathf.FloorToInt(time % 60).ToString();
            }
            else
            {
                time = 1;
                isStart = false;
                counter.text = "GO !!";
                isFinish = true;

                DOVirtual.DelayedCall(1f, () =>
                {
                    counter.gameObject.SetActive(false);
                    bg.SetActive(false);
                });

            }
        }
       
    }
}
