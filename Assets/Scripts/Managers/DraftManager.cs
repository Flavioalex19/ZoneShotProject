using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DraftManager : MonoBehaviour
{
    public static DraftManager instance;

    [SerializeField] GameObject p_leaguePlayer; // Player prefab
    [SerializeField] int numberOfPlayerToGenerate;//Numbers of player to create- this number will change if this is being called at the new season o a continiation of one
    [SerializeField] List<Player> p_DraftPlayerList = new List<Player>();

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
        GeneratePlayers();
    }

    private void GeneratePlayers()
    {
        for (int i = 0; i < numberOfPlayerToGenerate; i++)
        {
            GameObject newPlayer = Instantiate(p_leaguePlayer, Vector3.zero/*ui_viewport.transform.position*/, Quaternion.identity);
            Player playerComponent = newPlayer.GetComponent<Player>();

            if (playerComponent != null)
            {
                playerComponent.SetMaxPotencial();
                playerComponent.SetOVRAndStats();
                p_DraftPlayerList.Add(playerComponent);
                CreatePlayerButton(playerComponent);


            }
        }
    }
    public void AddPlayerToDraftList(GameObject player)
    {

    }
    //later remove this from draft manager and put on the game manager sincve that you need this to see avalible player after the draft and the team rooster
    void CreatePlayerButton(Player player)
    {
        // Instantiate the player button prefab
        GameObject newButton = Instantiate(player.GetPlayerInfoButton());
        // Set the button's parent to the grid layout group
        newButton.transform.SetParent(ui_viewport.transform, false);
        // Find the Text component that will display the OVR
        TextMeshProUGUI ovrText = newButton.transform.Find("Text_Draft_PlayerOVR").GetComponent<TextMeshProUGUI>();
        if (ovrText != null)
        {
            ovrText.text = player.GetOVR().ToString();
        }
    }
    public void BeginDraft()
    {
        // Draft logic
    }
}
