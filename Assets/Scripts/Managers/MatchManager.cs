using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{

    [SerializeField] Team homeTeam;
    [SerializeField] Team awayTeam;

    [SerializeField] GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;

        // Ensure DraftManager and teams are loaded correctly
        //LoadTeams();
        gameManager.dm_DraftManager.PrintPlayer();
    }

   

    // Update is called once per frame
    void Update()
    {
        /*
        // Iterate through the list backwards to avoid index shifting when removing items
        for (int i = awayTeam.t_playersList.Count - 1; i >= 0; i--)
        {
            print(awayTeam.t_playersList[i].PlayerFirstName + " TEAM PLAYER");
            
            
        */
    
    }
    private void LoadTeams()
    {
        // Check if data has been loaded from the save file before accessing teams
        if (gameManager.dm_DraftManager != null && gameManager.dm_DraftManager.t_Teams.Count > 1)
        {
            homeTeam = gameManager.dm_DraftManager.t_Teams[0];
            awayTeam = gameManager.dm_DraftManager.t_Teams[1];
        }
        else
        {
            Debug.LogError("Teams are not loaded correctly from save file.");
        }
    }
    public Team GetHomeTeam()
    {
        return homeTeam;
    }
    public Team GetAwayTeam()
    {
        return awayTeam;
    }
}
