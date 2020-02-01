using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                break;
            case GameState.Ready:
                Timer = setting.GameTime;
                startTimer -= Time.deltaTime;
                if (startTimer <= 0)
                {
                    State = GameState.Play;
                }
                break;
            case GameState.Play:
                Timer -= Time.deltaTime;
                if (Timer <= 0)
                {
                    State = GameState.End;

                    // Decide Game End State
                }
                break;
            case GameState.End:
                startTimer = 3;
                break;
        }
    }
}
