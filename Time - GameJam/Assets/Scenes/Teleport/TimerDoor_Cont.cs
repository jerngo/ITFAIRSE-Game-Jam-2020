using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerDoor_Cont : MonoBehaviour
{
    public TimerDoorPlatform[] platforms;
    public AudioSource doorSfx;
    Animator anim;

    public int time=10;

    public bool isWanttoClose = false;

    bool isOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        timerText.enabled = false;

        if (isWanttoClose == false)
        {
            isOpen = false;
            CloseDoor();
        }
        else
        {
            isOpen = true;
            OpenDoor();
        }

    }

    void OpenDoor()
    {
        anim.SetBool("Open", true);
    }

    void CloseDoor()
    {
        anim.SetBool("Open", false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckAllPlatform();
    }
    int tempCount;
    void CheckAllPlatform()
    {
        tempCount = 0;
        foreach (TimerDoorPlatform platform in platforms)
        {
            if (platform.IsOn == true) tempCount += 1;
        }

        if (tempCount == platforms.Length)
        {

            if (isWanttoClose == false)
            {
                if (isOpen == false)
                {
                    OpenDoor();
                    doorSfx.Play();
                    isOpen = true;
                    StartCoroutine(Timer());
                }

            }
            else {
                if (isOpen == true)
                {
                    CloseDoor();
                    doorSfx.Play();
                    isOpen = false;
                    StartCoroutine(Timer());
                }

            }

               

        }

    }

    public Text timerText; 
    IEnumerator Timer() {

        timerText.enabled = true;
        count = time;
        StartCoroutine(countdown());

        yield return new WaitForSeconds(time);
        //StopCoroutine(countdown());
        StopAllCoroutines();
        timerText.enabled = false;

        if (isWanttoClose == false)
        {
            if (isOpen == true)
            {
                doorSfx.Play();
                CloseDoor();
                isOpen = false;

                foreach (TimerDoorPlatform platform in platforms)
                {
                    platform.resetPlatform();
                }
            }

        }
        else {

            if (isOpen == false)
            {
                doorSfx.Play();
                OpenDoor();
                isOpen = true;

                foreach (TimerDoorPlatform platform in platforms)
                {
                    platform.resetPlatform();
                }
            }
        }

            

    }

    float count = 0;
    IEnumerator countdown() {

        count -= 1;
        timerText.text = count.ToString();
        yield return new WaitForSeconds(1);
        StartCoroutine(countdown());
    }

    
}
