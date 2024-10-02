using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiMatchManager : MonoBehaviour
{
    MatchManager mg_matchManager;//match manager variable

    [SerializeField] Transform homeRosterTextAreaContent;
    [SerializeField] Transform awayRosterTextAreaContent;
    // Start is called before the first frame update
    void Start()
    {
        mg_matchManager = GameObject.Find("Match Manager").GetComponent<MatchManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.state == GameStates.Match)
        {
            /*
            // Get the away team player list
            Team awayTeamPlayers = mg_matchManager.GetAwayTeam().t_playersList;

            // Print the count for debugging
            print(awayTeamPlayers.Count);

            // Loop through players in the away team, but limit to the number of players available
            int playersToDisplay = Mathf.Min(awayTeamPlayers.Count, 8); // Display up to 8 players or less if not available
            
            for (int i = 0; i < playersToDisplay; i++)
            {
                awayRosterTextAreaContent.GetChild(i).GetComponent<TextMeshProUGUI>().text = awayTeamPlayers[i].GetOVR().ToString();
            }
            */
            // If there are less than 8 players, clear remaining text fields (optional)
            for (int i = 0; i < 8; i++)
            {
                awayRosterTextAreaContent.GetChild(i).GetComponent<TextMeshProUGUI>().text = ""; // Clear the text for unused slots
            }
            
        }

    }
}
