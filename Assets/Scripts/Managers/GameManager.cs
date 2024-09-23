using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] DraftManager dm_DraftManager; 
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
    private void Update()
    {
        
        //testing
        if(state == GameStates.Match)
        {
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
        }
        SaveSystem.ClearSave();
        SaveSystem.ClearTeamData(dm_DraftManager.t_Teams);
        // Clear the players list for each team in memory
        foreach (Team team in dm_DraftManager.t_Teams)
        {
            team.GetPlayerList().Clear();
            team.RestIndex();
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
