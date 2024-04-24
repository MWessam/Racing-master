using Nova;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingView : MonoBehaviour
{
    public TextBlock RankText;
    public void UpdateRank(int rank)
    {
        string orderth = rank switch
        {
            1 => "st",
            2 => "nd",
            3 => "rd",
            _ => "th"
        };
        RankText.Text = $"{rank}{orderth}";
    }
}
