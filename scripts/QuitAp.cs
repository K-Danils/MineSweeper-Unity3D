using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitAp : MonoBehaviour
{
    /*
     * Menu Button Behaviours
    */
    public void QuitTheGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void SwitchToHard()
    {
        SceneManager.LoadScene("Hard");
    }

    public void SwitchToMedium()
    {
        SceneManager.LoadScene("Intermediate");
    }

    public void SwitchToEasy()
    {
        SceneManager.LoadScene("Easy");
    }
    public void SwitchToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
