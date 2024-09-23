using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // All Stats
    //[SerializeField] private Team ps_myTeam;
    [SerializeField] GameObject bt_PlayerInfo;
    // Age
    [Tooltip("21 - 35")]
    [SerializeField] private int ps_age;
    [SerializeField] public int ps_OVR;//CHANGE!!!!!!!!!!!!!!!!!
    [SerializeField] private int ps_PotencialOVR;
    private int ps_MaxAge = 35;
    private int ps_EarlyAge = 21;

    // All of those stats are from 10 to 99
    #region Base Stats
    [SerializeField] private int ps_consitancy;
    [SerializeField] private int ps_MaxPotencialConsitancy;
    [SerializeField] private int ps_awareness;
    [SerializeField] private int ps_MaxPotencialAwareness;
    #endregion

    #region Offense
    [SerializeField] private int ps_Junking;
    [SerializeField] private int ps_MaxPotencialJunking;
    [SerializeField] private int ps_Control;
    [SerializeField] private int ps_MaxPotencialControl;
    [SerializeField] private int ps_Shooting;
    [SerializeField] private int ps_MaxPotencialShooting;
    #endregion

    #region Defense
    [SerializeField] private int ps_Positioning;
    [SerializeField] private int ps_MaxPotencialPositioning;
    [SerializeField] private int ps_Stealing;
    [SerializeField] private int ps_MaxPotencialStealing;
    [SerializeField] private int ps_Guarding;
    [SerializeField] private int ps_MaxPotencialGuarding;
    [SerializeField] private int ps_Pressure;
    [SerializeField] private int ps_MaxPotencialPressure;
    #endregion

    #region Zone Stats
    [SerializeField] private int ps_InsideScoring;
    [SerializeField] private int ps_MaxPotencialInsideScoring;
    [SerializeField] private int ps_MidScoring;
    [SerializeField] private int ps_MaxPotencialMidScoring;
    [SerializeField] private int ps_OutsideScoring;
    [SerializeField] private int ps_MaxPotencialOutsideScoring;
    #endregion

    // Those stats will never change
    #region Personal Stats
    [SerializeField] private int player_Learning;
    [Tooltip("How often a player prefers certain zones to score")]
    [SerializeField] private int player_ZonePreferences;
    [SerializeField] private int player_longevity;
    // Personality
    [Tooltip("Play style for the player, does not interfere with the contract options. 1 to 5")]
    [SerializeField] private int player_personality;
    #endregion

    // Set Potencial/Max Stats
    public void SetMaxPotencial()
    {
        ps_MaxPotencialConsitancy = Random.Range(55, 99);
        ps_MaxPotencialAwareness = Random.Range(55, 99);
        ps_MaxPotencialJunking = Random.Range(55, 99);
        ps_MaxPotencialControl = Random.Range(55, 99);
        ps_MaxPotencialShooting = Random.Range(55, 99);
        ps_MaxPotencialPositioning = Random.Range(55, 99);
        ps_MaxPotencialStealing = Random.Range(55, 99);
        ps_MaxPotencialGuarding = Random.Range(55, 99);
        ps_MaxPotencialPressure = Random.Range(55, 99);
        ps_MaxPotencialInsideScoring = Random.Range(55, 99);
        ps_MaxPotencialMidScoring = Random.Range(55, 99);
        ps_MaxPotencialOutsideScoring = Random.Range(55, 99);

        ps_PotencialOVR = (ps_MaxPotencialConsitancy + ps_MaxPotencialAwareness + ps_MaxPotencialJunking + ps_MaxPotencialControl + ps_MaxPotencialShooting + ps_MaxPotencialPositioning + ps_MaxPotencialStealing +
            ps_MaxPotencialGuarding + ps_MaxPotencialPressure + ps_MaxPotencialInsideScoring + ps_MaxPotencialMidScoring + ps_MaxPotencialOutsideScoring) / 12;
    }

    public void SetOVRAndStats()
    {
        ps_age = Random.Range(ps_EarlyAge, ps_MaxAge);

        ps_consitancy = Random.Range(55, ps_MaxPotencialConsitancy);
        ps_awareness = Random.Range(55, ps_MaxPotencialAwareness);
        ps_Junking = Random.Range(55, ps_MaxPotencialJunking);
        ps_Control = Random.Range(55, ps_MaxPotencialControl);
        ps_Shooting = Random.Range(55, ps_MaxPotencialShooting);
        ps_Positioning = Random.Range(55, ps_MaxPotencialPositioning);
        ps_Stealing = Random.Range(55, ps_MaxPotencialStealing);
        ps_Guarding = Random.Range(55, ps_MaxPotencialGuarding);
        ps_Pressure = Random.Range(55, ps_MaxPotencialPressure);
        ps_InsideScoring = Random.Range(55, ps_MaxPotencialInsideScoring);
        ps_MidScoring = Random.Range(55, ps_MaxPotencialMidScoring);
        ps_OutsideScoring = Random.Range(55, ps_MaxPotencialOutsideScoring);

        ps_OVR = (ps_consitancy + ps_awareness + ps_Junking + ps_Control + ps_Shooting + ps_Positioning + ps_Stealing + ps_Guarding + ps_Pressure +
            ps_InsideScoring + ps_MidScoring + ps_OutsideScoring) / 12;
    }
    //Gets
    public GameObject GetPlayerInfoButton()
    {
        return bt_PlayerInfo;
    }
    public int GetOVR()
    {
        return ps_OVR;
    }
    public void SetOVR(int newOVR)
    {
        ps_OVR=newOVR;
    }
}
