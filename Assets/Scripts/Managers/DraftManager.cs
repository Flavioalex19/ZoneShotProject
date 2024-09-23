using System.Collections;
using System.Collections.Generic;
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
        #region Clear save tst region
        
        SaveSystem.ClearSave();
        SaveSystem.ClearTeamData(t_Teams);
        // Clear the players list for each team in memory
        foreach (Team team in t_Teams)
        {
            team.GetPlayerList().Clear();
            team.RestIndex();
        }
        
        #endregion
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
                    print(t_Teams[i].t_teamName + " " + t_Teams[i].GetPlayerList()[j].GetOVR());
                }
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
            if (ui_viewport != null)
            {
                //verify if there is no more players to draft
                if (ui_viewport.transform.childCount == 0)
                {
                    Debug.Log("No more players in the UI viewport.");
                    //End The Draft
                    EndDraft();
                }
                if (Input.GetKeyDown(KeyCode.Escape) && SaveSystem.LoadPlayers() != null)
                {
                    print("Clear save");
                    SaveSystem.ClearSave();
                    SaveSystem.ClearTeamData(t_Teams);

                }
            }
            
        }
        //print(t_Teams[0].t_teamName + " " + t_Teams[0].GetPlayerList().Count);
        for (int i = 0; i < t_Teams.Count; i++)
        {
            print(t_Teams[i].t_playersList.Count + " " + t_Teams[i].t_teamName);
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
                playerComponent.SetMaxPotencial();
                playerComponent.SetOVRAndStats();
                p_DraftPlayerList.Add(playerComponent);
                CreatePlayerButton(playerComponent);


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
        print("added");
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
        print("Draft is Over");
        if(ui_viewport.transform.childCount == 0) SceneManager.LoadScene(1);
        else
        {
            SaveSystem.SavePlayers(t_Teams);
            if (GameManager.instance.state == GameStates.Draft) GameManager.instance.state = GameStates.Match;
            SceneManager.LoadScene(1);
        }
        
        /*
        for (int i = 0; i < t_Teams.Count; i++)
        {
            print(t_Teams[i].t_playersList.Count + " " + t_Teams[i].t_teamName);
        }
        */
    }
    #endregion
    #region Save/Load
    //Save/Load
    public void LoadPlayerData()
    {
        List<PlayerData> playerDataList = SaveSystem.LoadPlayerData(); // Adjusted method to match new SaveSystem function

        if (playerDataList != null)
        {
            LoadPlayersToTeams(playerDataList);
        }
        else
        {
            Debug.LogWarning("No player data was loaded.");
        }
    }

    public void LoadPlayersToTeams(List<PlayerData> playerDataList)
    {
        // Assuming t_Teams is a list of teams in the same order as the playerDataList
        int currentTeamIndex = 0;

        foreach (PlayerData playerData in playerDataList)
        {
            // Get the team from the list
            Team team = t_Teams[currentTeamIndex];

            // Create player from data
            Player player = CreatePlayerFromData(playerData);

            // Add player to the team
            team.GetPlayerList().Add(player);

            // Move to the next team if needed
            currentTeamIndex = (currentTeamIndex + 1) % t_Teams.Count;
        }
    }
    public static Player CreatePlayerFromData(PlayerData data)
    {
        // Implement this method to create a Player instance from PlayerData
        Player player = new Player
        {
            // Example values, replace with actual methods/properties
            //playerName = data.playerName,
            ps_OVR = data.playerOVR,
            //playerPotential = data.playerPotential
        };
        return player;
    }
    #endregion
}
