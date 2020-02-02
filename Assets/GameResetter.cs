using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameResetter : MonoBehaviour
{
    float currentResetTime;
    public float resetTime = 10;
    public TextMeshProUGUI CountdownText;

    // Start is called before the first frame update
    void Start()
    {
        currentResetTime = resetTime;
    }

    // Update is called once per frame
    void Update()
    {
        currentResetTime -= Time.deltaTime;
        CountdownText.text = ((int)(currentResetTime)).ToString();

        if (currentResetTime <= 0)
        {
            SceneManager.LoadScene("Main Game");
        }
    }
}
