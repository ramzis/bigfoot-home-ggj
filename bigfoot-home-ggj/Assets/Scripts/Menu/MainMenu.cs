using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject credits;

    private void OnValidate()
    {
        Debug.Assert(credits != null, "Assign Credits reference.");
    }

    private void Start()
    {
        credits.SetActive(false);
    }

    public void StartButton()
    {
        SceneManager.LoadScene(1);
    }

    public void CreditsButton()
    {
        credits.SetActive(!credits.activeSelf);
    }

	public void ExitButton()
    {
        Application.Quit();
    }
}
