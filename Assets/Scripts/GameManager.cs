using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum GameState
{
    Idle,
    Ready,
    Play,
    End
}

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameSetting setting;

    [SerializeField]
    private GameState State = GameState.Idle;

    [SerializeField]
    private float Timer = 30;

    [SerializeField]
    private float startTimer = 3;

    [SerializeField]
    private CountdownUI countDownUI;

    [SerializeField]
    private CountdownTimerUI gameTimerUI;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Handle Input
        //if (Input.GetButtonDown(""))
        //{
        //    //if (GameState.Idle)
        //}

        switch (State)
        {
            case GameState.Idle:
                startTimer = 3;
                countDownUI.gameObject.SetActive(false);
                break;
            case GameState.Ready:
                Timer = setting.GameTime;
                startTimer -= Time.deltaTime;
                if (startTimer <= 0)
                {
                    State = GameState.Play;
                }
                countDownUI.UpdateValue(startTimer);
                countDownUI.gameObject.SetActive(true);
                break;
            case GameState.Play:
                Timer -= Time.deltaTime;
                if (Timer <= 0)
                {
                    State = GameState.End;
                    Timer = 0;
                    // Decide Game End State
                }
                gameTimerUI.UpdateTimer(Timer);
                
                countDownUI.gameObject.SetActive(false);
                break;
            case GameState.End:
                startTimer = 3;
                countDownUI.gameObject.SetActive(false);
                break;
        }
    }
}
