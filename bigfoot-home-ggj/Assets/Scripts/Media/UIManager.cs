using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour {

    public TextMeshProUGUI pollutionDisplay;

    public void OnValidate()
    {
        Debug.Assert(pollutionDisplay != null, "Missing Pollution Display text");
    }
}
