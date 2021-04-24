using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Cont : MonoBehaviour
{
    public Platform_Cont[] platforms;
    public AudioSource doorSfx;
    Animator anim;

    public bool isWanttoClose = false;

    bool isOpen=false;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        if (isWanttoClose == false)
        {
            isOpen = false;
            CloseDoor();
        }
        else {
            isOpen = true;
            OpenDoor();
        }
    }

    void OpenDoor() {
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
    void CheckAllPlatform() {
        tempCount = 0;
        foreach(Platform_Cont platform in platforms){
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
                }
            }
            else {
                if (isOpen == true)
                {
                    CloseDoor();
                    doorSfx.Play();
                    isOpen = false;
                }
            }
            
           
        }

        else {

            if (isWanttoClose == false)
            {
                if (isOpen == true)
                {
                    doorSfx.Play();
                    CloseDoor();
                    isOpen = false;
                }
            }

            else {

                if (isOpen == false)
                {
                    doorSfx.Play();
                    OpenDoor();
                    isOpen = true;
                }
            }

                
           
        }
            
    }
}
