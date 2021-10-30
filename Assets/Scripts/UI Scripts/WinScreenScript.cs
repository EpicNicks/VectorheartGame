using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenScript : MonoBehaviour
{
    public GameObject WinScreen;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(WinScreen.activeSelf == true)
            {
                WinScreen.SetActive(false);
                Time.timeScale = 1f;
                Cursor.visible = false;
            }
            else
            {
                WinScreen.SetActive(true);
                Cursor.visible = true;
                Time.timeScale = 0f;
            }
        }
    }


    public void GoBackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
}
