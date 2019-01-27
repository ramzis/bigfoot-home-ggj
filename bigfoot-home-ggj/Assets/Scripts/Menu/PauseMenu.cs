using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MainMenu {

    private bool paused = false;
    GameObject pauseMenu;

    void Start()
    {
        pauseMenu = GameObject.Find("Menu");
        pauseMenu.SetActive(false);
        ToggleCursorLock();
    }

	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = TogglePause();
        }
	}

    bool TogglePause()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        ToggleCursorLock();
        if (!paused)
        {
            Time.timeScale = 0f;
            return true;
        }
        else
        {
            Time.timeScale = 1f;
            return false;
        }
    }

    public void ResumeButton(){
        paused = TogglePause();
    }

    public void ReturnToTitleButton(){
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    void ToggleCursorLock()
    {
        Cursor.visible = !Cursor.visible;
        if (Cursor.lockState == CursorLockMode.Locked)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }
}
