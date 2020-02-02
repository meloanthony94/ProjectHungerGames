using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameSetting", menuName = "GameSetting", order = 1)]
public class GameSetting : ScriptableObject
{
    public enum GameState
    {
        Idle,
        Ready,
        Play,
        End
    }

    [Header("Player Setting")]
    public TeamSetting Farmer;
    public TeamSetting Critter;

    [Header("Global Setting")]
    public float GameTime = 20f;
    public float PlayerRaycastLength = 5;
    public GameState State;

    [System.Serializable]
    public class TeamSetting
    {
        [Header("Health")]
        public float InitHealth;
        public float HealthGainAmount;
        public float HealthLostAmount;
        public float HealthLostRate;
        public float CurrentHealth;

        [Header("Action Speed")]
        public float DigSpeedFast;
        public float DigSpeedSlow;
        public float PlantSpeedFast;
        public float PlantSpeedSlow;
        public float HarvestSpeedFast;
        public float HarvestSpeedSlow;
        public float EatSpeedFast;
        public float EatSpeedSlow;
    }
}
