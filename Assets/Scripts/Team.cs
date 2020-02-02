using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    [SerializeField]
    private float health;

    public float Health
    {
        get { return health; }
        set
        {
            health = value;
            if (health > 100)
            {
                health = 100;
            }
            if (health < 0)
            {
                health = 0;
            }
        }
    }

    [SerializeField]
    private ProgressBar progressBar;

    [SerializeField]
    private GameSetting setting;
    private GameSetting.TeamSetting teamSetting;

    [SerializeField]
    private bool isCritter = false;

    public void Awake()
    {
        if(isCritter)
        {
            teamSetting = setting.Critter;
        }
        else
        {
            teamSetting = setting.Farmer;
        }

        Health = teamSetting.InitHealth;
        
    }

    public void Start()
    {
        progressBar.BarValue = Health;
    }

    public void Update()
    {
        if (setting.State == GameSetting.GameState.Ready)
        {
            Health = teamSetting.InitHealth;
            progressBar.BarValue = Health;
        }

        if (isCritter && setting.State == GameSetting.GameState.Play)
        {
            Health -= teamSetting.HealthLostAmount * teamSetting.HealthLostRate * Time.deltaTime;
            progressBar.BarValue = health;
        }

        if (isCritter)
        {
            setting.Critter.CurrentHealth = Health;
        }
        else
        {
            setting.Farmer.CurrentHealth = Health;
        }
    }

    public void HealthUp()
    {
        Health += teamSetting.HealthGainAmount;
        progressBar.BarValue = health;
    }

    public void HealthDown()
    {
        Health -= teamSetting.HealthLostAmount;
        progressBar.BarValue = health;
    }

}
