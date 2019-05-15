﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool paused = false;
    [SerializeField]
    private GameObject pauseMenu;

    void Start()
    {
        pauseMenu.SetActive(false);
    }

	void Update ()
    {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = TogglePause();
        }
	}

    bool TogglePause()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
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

    public void ResumeButton()
    {
        paused = TogglePause();
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void ReturnToTitleButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void ExitButton()
    {
        Application.Quit();
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
