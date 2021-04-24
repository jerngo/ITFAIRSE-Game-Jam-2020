using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Score_Cont : MonoBehaviour
{
    public Text score;

    private void Awake()
    {
        score.text = "<"+PlayerPrefs.GetFloat("TimeSaved").ToString("0.00")+" Seconds>";
    }
}
