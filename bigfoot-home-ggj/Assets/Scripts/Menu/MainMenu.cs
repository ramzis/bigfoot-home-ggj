using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    GameObject credits;

    void Start()
    {
        credits = GameObject.Find("Credits");
        credits.SetActive(false);
    }

    public void StartButton()
    {
        SceneManager.LoadScene(1);
    }

    public void CreditsButton()
    {
        credits.SetActive(true);
    }

	public void ExitButton()
    {
        Application.Quit();
    }
}