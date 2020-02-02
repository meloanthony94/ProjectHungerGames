using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameSetting setting;

    //[SerializeField]
    //private GameState State = GameState.Idle;

    [SerializeField]
    private float Timer = 30;

    [SerializeField]
    private float startTimer = 4;

    [SerializeField]
    private CountdownUI countDownUI;


    public GameObject MainMenuPanel;
    public GameObject PausePanel;
    public GameObject TitlePanel;
    public GameObject HowToPlayPanel;
    public GameObject CreditsPanel;
    public GameObject GameOverPanel;
    public GameObject inGameUIGroup;
    public GameObject CritterWinScreen;
    public GameObject FarmerWinScreen;

    [SerializeField]
    private CountdownTimerUI gameTimerUI;

    [SerializeField]
    private AudioSource menuEffectAudioSource;

    [SerializeField]
    private AudioSource backgroundAudioSource;

    [SerializeField]
    private AudioSource menuBoardAudioSource;

    [SerializeField]
    private AudioClip mainMenuClip;
    [SerializeField]
    private AudioClip gamePlayClip;
    [SerializeField]
    private AudioClip winScreenClip;

    [SerializeField]
    private AudioClip menuClickClip;
    [SerializeField]
    private AudioClip menuInClip;


    GameObject currentMenuPanel;

    bool isLeftPadActive = false;
    bool isRightPadActive = false;
    bool isUpPadActive = false;
    bool isDownPadActive = false;

    // Start is called before the first frame update
    void Start()
    {
        setting.State = GameSetting.GameState.Idle;
        inGameUIGroup.SetActive(false);
        currentMenuPanel = TitlePanel;
    }

    void MenuPanelActivator(GameObject panel)
    {
        currentMenuPanel?.SetActive(false);
        currentMenuPanel = panel;
        currentMenuPanel.SetActive(true);
        
        

    }

    // Update is called once per frame
    void Update()
    {
        // Handle Input
        if (Input.GetAxisRaw("DPAD X-Axis") == -1 && !isLeftPadActive)
        {
            if ((setting.State == GameSetting.GameState.Idle || setting.State == GameSetting.GameState.End) && (currentMenuPanel == TitlePanel || currentMenuPanel == GameOverPanel))
            {
                if (setting.State == GameSetting.GameState.End)
                    TitlePanel.SetActive(false);
                menuEffectAudioSource.PlayOneShot(menuClickClip);
                menuBoardAudioSource.PlayOneShot(menuInClip);
                MenuPanelActivator(CreditsPanel);

            }
            else if (currentMenuPanel == HowToPlayPanel)
            {
                if (setting.State == GameSetting.GameState.Idle)
                {
                    MenuPanelActivator(TitlePanel);
                    menuEffectAudioSource.PlayOneShot(menuClickClip);
                }
                else if (setting.State == GameSetting.GameState.End)
                {
                    TitlePanel.SetActive(true);
                    MenuPanelActivator(GameOverPanel);
                    menuEffectAudioSource.PlayOneShot(menuClickClip);
                }
            }

            isLeftPadActive = true;
        }
        else if (Input.GetAxisRaw("DPAD X-Axis") == 0)
        {
            isLeftPadActive = false;
        }

        if (Input.GetAxisRaw("DPAD X-Axis") == 1 && !isRightPadActive)
        {
            if (currentMenuPanel == CreditsPanel)
            {
                if (setting.State == GameSetting.GameState.Idle)
                {
                    MenuPanelActivator(TitlePanel);
                    menuEffectAudioSource.PlayOneShot(menuClickClip);
                }
                else if (setting.State == GameSetting.GameState.End)
                {
                    TitlePanel.SetActive(true);
                    MenuPanelActivator(GameOverPanel);
                    menuEffectAudioSource.PlayOneShot(menuClickClip);
                }
            }
            else if ((setting.State == GameSetting.GameState.Idle || setting.State == GameSetting.GameState.End) && (currentMenuPanel == TitlePanel || currentMenuPanel == GameOverPanel))
            {
                if (setting.State == GameSetting.GameState.End)
                    TitlePanel.SetActive(false);

                MenuPanelActivator(HowToPlayPanel);
                menuEffectAudioSource.PlayOneShot(menuClickClip);
            }

            isRightPadActive = true;
        }
        else if (Input.GetAxisRaw("DPAD X-Axis") == 0)
        {
            isRightPadActive = false;
        }

        if (Input.GetAxisRaw("DPAD Y-Axis") == 1 && !isUpPadActive)
        {
            isUpPadActive = true;
        }
        else if (Input.GetAxisRaw("DPAD Y-Axis") == 0)
        {
            isUpPadActive = false;
        }

        if (Input.GetAxisRaw("DPAD Y-Axis") == -1 && !isDownPadActive)
        {
            isDownPadActive = true;
        }
        else if (Input.GetAxisRaw("DPAD Y-Axis") == 0)
        {
            isDownPadActive = false;
        }

        if (Input.GetButtonDown("Touchpad"))
        {
            if (setting.State == GameSetting.GameState.Idle)
            {
                setting.State = GameSetting.GameState.Ready;
                menuEffectAudioSource.PlayOneShot(menuClickClip);
            }
        }

        switch (setting.State)
        {
            case GameSetting.GameState.Idle:
                startTimer = 4;
                countDownUI.gameObject.SetActive(false);
                PlayBackgroundMusic(mainMenuClip);
                break;
            case GameSetting.GameState.Ready:
                currentMenuPanel = MainMenuPanel;
                MenuPanelActivator(inGameUIGroup);
                Timer = setting.GameTime;
                startTimer -= Time.deltaTime;
                if (startTimer <= 0)
                {
                    setting.State = GameSetting.GameState.Play;
                }
                countDownUI.UpdateValue(startTimer);
                countDownUI.gameObject.SetActive(true);
                //PlayBackgroundMusic(gameStartCountDownClip, false);
                PlayBackgroundMusic(gamePlayClip);
                break;
            case GameSetting.GameState.Play:
                Timer -= Time.deltaTime;
                
                if (Timer <= 0)
                {
                    setting.State = GameSetting.GameState.End;

                    // Decide Game End State
                    if (setting.Critter.CurrentHealth > setting.Farmer.CurrentHealth)
                    {
                        FarmerWinScreen.SetActive(false);
                        CritterWinScreen.SetActive(true);
                    }

                    MainMenuPanel.SetActive(true);
                    TitlePanel.SetActive(false);
                    MenuPanelActivator(GameOverPanel);
                    //backgroundAudioSource.pitch = 1.0f;
                }
                //else if (Timer <= 5)
                //{
                //    backgroundAudioSource.pitch = 1.2f;
                //}

                gameTimerUI.UpdateTimer(Timer);
                countDownUI.gameObject.SetActive(false);
                break;

                case GameSetting.GameState.End:
                //menuEffectAudioSource.PlayOneShot(winCheerClip);
                PlayBackgroundMusic(winScreenClip);
                startTimer = 4;
                countDownUI.gameObject.SetActive(false);
                break;
        }
    }

    void PlayBackgroundMusic(AudioClip clip, bool loop = true)
    {
        if (backgroundAudioSource.clip != clip)
        {
            backgroundAudioSource.Stop();
            backgroundAudioSource.clip = clip;
            backgroundAudioSource.loop = loop;
            backgroundAudioSource.Play();
        }
    }
}
