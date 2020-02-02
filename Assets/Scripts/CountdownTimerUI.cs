using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CountdownTimerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI uiText;



    public void UpdateTimer(float value)
    {
        uiText.text = string.Format("{0}", (int)value);
    }

}
