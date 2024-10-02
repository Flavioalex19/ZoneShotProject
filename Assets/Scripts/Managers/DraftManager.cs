using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DraftManager : MonoBehaviour
{
    public static DraftManager instance;

    [SerializeField] GameObject p_leaguePlayer; // Player prefab
    [SerializeField] int numberOfPlayerToGenerate;//Numbers of player to create- this number will change if this is being called at the new season o a continiation of one
    [SerializeField] List<Player> p_DraftPlayerList = new List<Player>();//Avaliable for the draft

    // Step 1: Declare the dictionary to store player-button associations
    Dictionary<Player, GameObject> playerButtonMap = new Dictionary<Player, GameObject>();

    [SerializeField] public List<Team> t_Teams = new List<Team>();
    int t_currentTeamIndex = 0;

    [SerializeField] GridLayoutGroup ui_viewport;

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
        
    }

    void Start()
    {
          /*
          #region Clear save tst region
          SaveSystem.ClearSave();
          SaveSystem.ClearTeamData(t_Teams);
          // Clear the players list for each team in memory
          foreach (Team team in t_Teams)
          {
              ClearSaveData();
              team.GetPlayerList().Clear();
              team.RestIndex();
              team.ResetList();
          }
          #endregion
          */

        for (int i = 0; i < t_Teams.Count; i++)
        {
            print(t_Teams[i].t_playersList.Count + " " + t_Teams[i].t_teamName);
        }
        if (SaveSystem.LoadPlayers() != null)
        {
            
            // Load the saved team data
            //TeamData data = SaveSystem.LoadTeams();
            LoadPlayerData();
            for (int i = 0; i < t_Teams.Count; i++)
            {
                for (int j = 0; j < t_Teams[i].GetPlayerList().Count; j++)
                {
                    //print(t_Teams[i].t_teamName + " " + t_Teams[i].GetPlayerList()[j].GetOVR() + " " + "This my OVR");
                }
                //SceneManager.LoadScene(1);
            }
            
        }
        else
        {
            print("No Save");
            //print(p_DraftPlayerList.Count + " Avaliable Players");
            //Generate the PLayers
            GeneratePlayers();
        }
       
    }
    private void Update()
    {
        if (GameManager.instance.state == GameStates.Draft)
        {
            //verify if there is no more players to draft
            if (ui_viewport.transform.childCount == 0)
            {
                Debug.Log("No more players in the UI viewport.");
                //End The Draft
                EndDraft();
                
            }
            /*
            if (Input.GetKeyDown(KeyCode.Escape) && SaveSystem.LoadPlayers() != null)
            {
                print("Clear save");
                SaveSystem.ClearSave();
                SaveSystem.ClearTeamData(t_Teams);

            }
            */
        }
        //print(t_Teams[0].t_teamName + " " + t_Teams[0].GetPlayerList().Count);
        for (int i = 0; i < t_Teams.Count; i++)
        {
            //print(t_Teams[i].t_playersList.Count + " " + t_Teams[i].t_teamName + " " + "This is print in the draft manager");
        }

    }
    private void GeneratePlayers()
    {
        for (int i = 0; i < numberOfPlayerToGenerate; i++)
        {
            GameObject newPlayer = Instantiate(p_leaguePlayer, Vector3.zero/*ui_viewport.transform.position*/, Quaternion.identity);
            Player playerComponent = newPlayer.GetComponent<Player>();

            if (playerComponent != null && ui_viewport!=null)
            {
                playerComponent.SetPlayerName();
                playerComponent.SetMaxPotencial();
                playerComponent.SetOVRAndStats();
                p_DraftPlayerList.Add(playerComponent);
                CreatePlayerButton(playerComponent);
                //Debug.Log("Generated player with OVR: " + playerComponent.GetOVR());
                Debug.Log("Name" + " " + playerComponent.PlayerFirstName);

            }
        }
        //print(p_DraftPlayerList.Count + " Avaliable Players");
    }
    //later remove this from draft manager and put on the game manager sincve that you need this to see avalible player after the draft and the team rooster
    void CreatePlayerButton(Player player)
    {
        // Instantiate the player button prefab
        GameObject newButton = Instantiate(player.GetPlayerInfoButton());
        // Set the button's parent to the grid layout group
        newButton.transform.SetParent(ui_viewport.transform, false);

        // Step 2: Track the player-button association
        playerButtonMap[player] = newButton;

        // Find the Text component that will display the OVR
        TextMeshProUGUI ovrText = newButton.transform.Find("Text_Draft_PlayerOVR").GetComponent<TextMeshProUGUI>();
        if (ovrText != null)
        {
            ovrText.text = player.GetOVR().ToString();
            newButton.GetComponentInChildren<Button>().onClick.AddListener(() => AddPlayerToTeam(player));
        }
        
    }
    public void AddPlayerToTeam(Player player)
    {
        //print("added");
        // Add player to the current team and update UI
        Team currentTeam = t_Teams[t_currentTeamIndex];
        

        if (currentTeam.AddPlayerToTeam(player))
        {
            p_DraftPlayerList.Remove(player);// Remove player from draft pool

            // Remove the player's button from the UI and dictionary
            if (playerButtonMap.ContainsKey(player))
            {
                GameObject playerButton = playerButtonMap[player];
                Destroy(playerButton); // Remove the button from the UI
                playerButtonMap.Remove(player); // Remove from dictionary
            }
            //print(currentTeam.name + " " + currentTeam.t_playersList.Count);
            // Logic for advancing to the next team and any other updates
            AdvanceToNextTeam();
            
        }
        else
        {
            Debug.Log("This team is full, cannot add more players." + " " + currentTeam.name);
        }
    }
    void AdvanceToNextTeam()
    {
        t_currentTeamIndex++;

        if (t_currentTeamIndex >= t_Teams.Count)
        {
            t_currentTeamIndex = 0; // Loop back to the first team
        }
    }
    #region Draft
    public void BeginDraft()
    {
        // Draft logic
    }
    public void EndDraft()
    {
        //print("Draft is Over");
        if (AreAllTeamsFull())
        {
            print("Full");
            if (GameManager.instance.state == GameStates.Draft) GameManager.instance.state = GameStates.Match;
            SceneManager.LoadScene(1);
        }
        else
        {
            print("NOT FULL");
            SaveSystem.SavePlayers(t_Teams);
            if (GameManager.instance.state == GameStates.Draft) GameManager.instance.state = GameStates.Match;
            SceneManager.LoadScene(1);
        }
        /*
        SaveSystem.SavePlayers(t_Teams);
        if (GameManager.instance.state == GameStates.Draft) GameManager.instance.state = GameStates.Match;
        SceneManager.LoadScene(1);
        */
        //print("Saved");
        /*
        for (int i = 0; i < t_Teams.Count; i++)
        {
            print(t_Teams[i].t_playersList.Count + " " + t_Teams[i].t_teamName);
        }
        */
    }
    #endregion
    public bool AreAllTeamsFull()
    {
        foreach (Team team in t_Teams)
        {
            if (team.GetPlayerList().Count < team.t_maxNumberOfPlayerOnTeam)
            {
                return false; // Found a team that is not full
            }
        }
        return true; // All teams are full
    }
    #region Save/Load
    //Save/Load
    public void LoadPlayerData()
    {
        List<PlayerData> playerDataList = SaveSystem.LoadPlayerData(); // Adjusted method to match new SaveSystem function

        if (playerDataList != null)
        {
            //print(t_Teams[0].GetPlayerList().Count + " " + t_Teams[0].name + "DRAFT MANAGER CHECK!!!!");
            
            // Only load player data if the teams' player lists are empty
            for (int i = 0; i < t_Teams.Count; i++)
            {
                if (t_Teams[i].GetPlayerList().Count != 0) // Check if the team already has players
                {
                    
                    print("Load´Player to teams!!!!!!!!!!!!!!");
                    LoadPlayersToTeams(playerDataList);
                    
                    
                }
                else
                {
                    Debug.Log(t_Teams[i].t_teamName + " already has players. Skipping load.");
                    //LoadPlayersToTeams(playerDataList);
                    
                }
            }
            
            

        }
        else
        {
            Debug.LogWarning("No player data was loaded.");
            
        }
    }

    public void LoadPlayersToTeams(List<PlayerData> playerDataList)
    {
        /*
        // Assuming t_Teams is a list of teams in the same order as the playerDataList
        int currentTeamIndex = 0;

        foreach (PlayerData playerData in playerDataList)
        {
            // Get the team from the list
            Team team = t_Teams[currentTeamIndex];

            // Create player from data
            Player player = CreatePlayerFromData(playerData);

            // Set the OVR for the player
            player.SetOVR(playerData.playerOVR); // Add this line to set OVR
            //print(playerData.playerOVR + "OVR OF PLAYER");
            // Add player to the team
            team.GetPlayerList().Add(player);

            // Move to the next team if needed
            currentTeamIndex = (currentTeamIndex + 1) % t_Teams.Count;
        }
        */
        /*
        // Assuming t_Teams is a list of teams in the same order as the playerDataList
        int currentTeamIndex = 0;

        foreach (PlayerData playerData in playerDataList)
        {
            // Get the team from the list
            Team team = t_Teams[currentTeamIndex];

            // Check if the team is full before adding a player
            if (team.GetPlayerList().Count < team.t_maxNumberOfPlayerOnTeam)
            {
                // Create player from data
                Player player = CreatePlayerFromData(playerData);

                // Set the OVR for the player
                player.SetOVR(playerData.playerOVR); // Set OVR from PlayerData

                // Check if the player already exists in the team
                if (!team.GetPlayerList().Contains(player))
                {
                    // Add player to the team
                    team.GetPlayerList().Add(player);
                }
                // Add player to the team
                //team.GetPlayerList().Add(player);
            }

            // Move to the next team if needed
            currentTeamIndex = (currentTeamIndex + 1) % t_Teams.Count;
        }
        */
        // Clear existing players in the teams to avoid duplication
        ClearTeams();

        // Assuming t_Teams is a list of teams in the same order as the playerDataList
        int currentTeamIndex = 0;

        foreach (PlayerData playerData in playerDataList)
        {
            // Get the team from the list
            Team team = t_Teams[currentTeamIndex];

            // Check if the team is full before adding a player
            if (team.GetPlayerList().Count < team.t_maxNumberOfPlayerOnTeam)
            {
                // Create player from data
                Player player = CreatePlayerFromData(playerData);

                // Set the OVR for the player
                player.SetOVR(playerData.playerOVR); // Set OVR from PlayerData

                // Check if the player already exists in the team using PlayerID
                if (!team.GetPlayerList().Any(existingPlayer => existingPlayer.PlayerFirstName == player.PlayerFirstName))
                {
                    // Add player to the team
                    team.GetPlayerList().Add(player);
                }
            }

            // Move to the next team if needed
            currentTeamIndex = (currentTeamIndex + 1) % t_Teams.Count;
        }
    }
    // Clear existing players in the teams
    private void ClearTeams()
    {
        foreach (Team team in t_Teams)
        {
            team.GetPlayerList().Clear();
        }
    }
    public static Player CreatePlayerFromData(PlayerData data)
    {
        /*
        // Implement this method to create a Player instance from PlayerData
        Player player = new Player
        {
            // Example values, replace with actual methods/properties
            //playerName = data.playerName,
            ps_OVR = data.playerOVR,
            //playerPotential = data.playerPotential
        };
        return player;
        */
        // Instantiate the player prefab and get the Player component
        GameObject newPlayer = Instantiate(DraftManager.instance.p_leaguePlayer);
        Player player = newPlayer.GetComponent<Player>();

        // Now set the data from PlayerData
        player.SetOVR(data.playerOVR);
        // Add other data fields as needed

        return player;
    }
    #endregion
    public void ClearSaveData()
    {
        /*
        // Clear the player lists for all teams
        for (int i = 0; i < t_Teams.Count; i++)
        {
            t_Teams[i].GetPlayerList().Clear(); // Clear the player list for each team
        }

        // Now call the SaveSystem to clear the saved data
        SaveSystem.ClearSave();
        SaveSystem.ClearTeamData(t_Teams);

        Debug.Log("All teams' player lists have been cleared, and save data has been wiped.");
        */
        // Ensure the player lists are cleared for all teams in memory
        foreach (Team team in t_Teams)
        {
            team.GetPlayerList().Clear();  // Clear player list in memory
        }

        // Now delete the saved data
        SaveSystem.ClearSave();
        Debug.Log("All teams' player lists have been cleared, and save data has been wiped.");
    }
    public void PrintPlayer()
    {
        for (int i = 0; i < t_Teams.Count; i++)
        {
            for (int j = 0; j < t_Teams[i].t_playersList.Count; j++)
            {
                print(t_Teams[i].t_playersList[j].PlayerFirstName + " " + t_Teams[i].t_playersList[j].GetOVR() + " This is FROM DRAF MANAGER");
            }
        }
    }
}
