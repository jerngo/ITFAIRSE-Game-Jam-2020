using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportEnd_Cont : MonoBehaviour
{
    public LevelManager levelManager;
    public int index;
    public AudioSource sfx;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            float timeSaved = PlayerPrefs.GetFloat("TimeSaved");
            float timeRemaining = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>().getRemainingTime();

            float currentScore = timeSaved + timeRemaining;

            PlayerPrefs.SetFloat("TimeSaved", currentScore);
            levelManager.LoadSceneNext();
            sfx.Play();
        }
    }
}
