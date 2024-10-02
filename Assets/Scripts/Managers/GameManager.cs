using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public enum GameStates
{
    None,
    Draft,
    Match
}
public class GameManager : MonoBehaviour
{
    public GameStates state;

    [SerializeField] public DraftManager dm_DraftManager; 
    public static GameManager instance;

    //Teams
    public List<Team> teams = new List<Team>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        if (state == GameStates.None)
            state = GameStates.Draft;
    }

    void Start()
    {
        if (dm_DraftManager == null)
        {
            dm_DraftManager = FindObjectOfType<DraftManager>();
            //print(dm_DraftManager.name);
        }
        print(state);
    }
    private void FixedUpdate()
    {
        
        //testing
        if(state == GameStates.Match)
        {
            dm_DraftManager.LoadPlayerData();
            //SaveSystem.ClearSave();
            //SaveSystem.ClearTeamData(t_Teams);
            if (Input.GetKeyDown(KeyCode.Escape) && SaveSystem.LoadPlayers() != null)
            {
                print("Clear save");
                //SaveSystem.ClearSave();
                //SaveSystem.ClearTeamData(DraftManager.instance,t);
                #region Clear save tst region
                
                SaveSystem.ClearSave();
                SaveSystem.ClearTeamData(dm_DraftManager.t_Teams);
                // Clear the players list for each team in memory
                foreach (Team team in dm_DraftManager.t_Teams)
                {
                    team.GetPlayerList().Clear();
                    team.RestIndex();
                }
                #endregion

            }
            foreach (Team team in dm_DraftManager.t_Teams)
            {
                //print(team.t_playersList.Count + " " + "Player on each team viewd by the gama manager");
                //print(team.t_playersList.Count + " - " + team.name + " viewd by GAME MANAGER");
                for (int i = 0; i < team.t_playersList.Count; i++)
                {
                    print(team.name + " " + team.t_playersList[i].GetOVR() + " GAME MANAGER PRINT OVR");
                }
            }
            
            //check if is saved
            if (Input.GetKeyDown(KeyCode.E))
            {
                foreach (Team team in dm_DraftManager.t_Teams)
                {
                    for (int i = 0; i < team.t_playersList.Count; i++)
                    {
                        print(team.name + " " + team.t_playersList[i].GetOVR());
                    }
                }
            }
            
        }
        
    }
   

    public void StartDraft()
    {
        if (dm_DraftManager != null)
        {
            dm_DraftManager.BeginDraft();
        }
    }
}
