using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutor_Cont : MonoBehaviour
{
    public LevelManager levelManager;
    public int index;
    public AudioSource sfx;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            levelManager.LoadScene(0);
            sfx.Play();
        }
    }
}
