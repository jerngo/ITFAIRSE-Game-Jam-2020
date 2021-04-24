using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerDoorPlatform : MonoBehaviour
{
    public Color OnColor;
    public Color OffColor;

    public SpriteRenderer LightIndicator;
    public bool IsOn = false;

    public AudioSource audioOn;
    public AudioSource audioOff;

    bool alreadyDetected = false;
    int objectOn = 0;

    private void Start()
    {
        LightIndicator.color = OffColor;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            objectOn += 1;
            if (alreadyDetected == false)
            {
                LightIndicator.color = OnColor;
                IsOn = true;
                audioOn.Play();
                alreadyDetected = true;

            }

        }
    }

    public void resetPlatform() {
        LightIndicator.color = OffColor;
        IsOn = false;
        audioOff.Play();
        alreadyDetected = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            objectOn -= 1;
            if (objectOn == 0)
            {
                
            }


        }
    }
}
