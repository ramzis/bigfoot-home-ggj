using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroScene : MonoBehaviour
{
    [SerializeField]
    private Image image;
    private RectTransform rect;

    private int state;
    private int animationIdx;
    private bool inputAllowed;

    private Vector2[] statePositions = new[]
    {
        new Vector2(0, -285),
        new Vector2(0, 35),
        new Vector2(0, 281)
    };

    private void Start()
    {
        state = 0;
        animationIdx = state;
        rect = image.GetComponent<RectTransform>();
        rect.anchoredPosition = statePositions[animationIdx];
        StartCoroutine(DisableInputTimed(1f));
    }

    void Update()
    {
        if (Input.anyKey && inputAllowed)
        {
            StartCoroutine(DisableInputTimed(1.5f));
            ManageInput();
        }

        if(Vector2.Distance(rect.anchoredPosition,statePositions[animationIdx]) > 0.1f)
            rect.anchoredPosition = Vector2.Lerp(
                rect.anchoredPosition,
                statePositions[animationIdx],
                Time.deltaTime);
    }

    private void ManageInput()
    {
        if (state < 3)
        {
            state++;

            if(state == 3)
                SceneManager.LoadScene(2);

            if (state < statePositions.Length)
                animationIdx = state;
        }
	}

    private IEnumerator DisableInputTimed(float t)
    {
        inputAllowed = false;
        yield return new WaitForSeconds(t);
        inputAllowed = true;
    }
}
