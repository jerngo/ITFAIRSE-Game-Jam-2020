using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public float timerCount;
    float remainingTime;
    public bool isRealtime = true;
    public Text timerText;

    bool isError = false;
    // Start is called before the first frame update
    void Start()
    {
        remainingTime = timerCount;
    }

    public void LoadScene(int index) {
        SceneManager.LoadScene(index);
    }

    public float getRemainingTime() {
        return remainingTime;
    }

    public void LoadSceneNext()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public AudioSource enginefailsfx;

    void countDown() {
        if (isRealtime)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
            }
        }
        else {
            if (remainingTime < timerCount)
            {
                remainingTime += Time.deltaTime;
            }
        }

        if (remainingTime < 20)
        {
            GameObject.FindGameObjectWithTag("MonitorControl").GetComponent<Monitor_Cont>().UpdateCondition("Warning");
        }
        else {
            GameObject.FindGameObjectWithTag("MonitorControl").GetComponent<Monitor_Cont>().UpdateCondition("Good");
        }

        if (remainingTime <= 0)
        {
            if (isError == false) {
                timerText.text = "ERROR";
                enginefailsfx.Play();
                GameObject thePlayer = GameObject.Find("MainPlayer");
                thePlayer.GetComponent<Robot_Cont>().setCantMove(true);
                thePlayer.GetComponent<Robot_Cont>().stopSFX();
                thePlayer.GetComponent<Robot_Cont>().showSmoke();

                thePlayer.GetComponent<TimeBody>().enabled = false;
                GameObject.FindGameObjectWithTag("VirtualCam").GetComponent<Cam_Cont>().Shake();
                isError = true;
            }
         

        }
        else {
            timerText.text = remainingTime.ToString("0.00");
        }
      
    }

    // Update is called once per frame
    void Update()
    {
        countDown();
    }
}
