using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;


namespace PathCreation.Examples
{

    public class RaceManager : MonoBehaviour
    {

        public DistanceCounter[] allDistance;

        public bool canSort;

       

        void Start()
        {
           
            canSort = true;
        }
        // Update is called once per frame
        void Update()
        {
            if(canSort)
            {
                BubbleSort(allDistance);
            }
        }

        public void BubbleSort(DistanceCounter[] dis)
        {
            // for (int i = dis.Length - 2; i >= 0; i--)
            // {
            //     for (int j = 0; j <= i; j++)
            //     {
            //         if (dis[j].distance < dis[j + 1].distance)
            //         {
            //             DistanceCounter temp = dis[j + 1];
            //             dis[j + 1] = dis[j];
            //             dis[j] = temp;
            //         }
            //     }
            // }
            allDistance = allDistance.OrderBy(x => x.distance).Reverse().ToArray();

            allDistance[0].text.text = "1st";
            allDistance[1].text.text = "2nd";
            allDistance[2].text.text = "3rd";
            allDistance[3].text.text = "4th";
            allDistance[4].text.text = "5th";
            allDistance[5].text.text = "6th";

            allDistance[0].rank = 1;
            allDistance[1].rank = 2;
            allDistance[2].rank = 3;
            allDistance[3].rank = 4;
            allDistance[4].rank = 5;
            allDistance[5].rank = 6;
        }
    }
}