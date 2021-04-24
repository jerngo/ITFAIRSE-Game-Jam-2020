using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu_Cont : MonoBehaviour
{
    public void LoadSceneNext()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        PlayerPrefs.SetFloat("TimeSaved", 0);
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
