using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{

    [SerializeField] string t_teamName;
    [SerializeField] int t_currentPlayerOnTheTeamIndex = 0;
    [SerializeField] int t_maxNumberOfPlayerOnTeam = 12;
    [SerializeField] List<Player> t_players = new List<Player>();

    public bool AddPlayerToTeam(Player newPlayer)
    {
        if (t_currentPlayerOnTheTeamIndex < t_maxNumberOfPlayerOnTeam)
        {
            t_players.Add(newPlayer);
            t_currentPlayerOnTheTeamIndex++;
            return true; // Player added successfully
        }
        else
        {
            Debug.Log("Team is full!");
            return false; // Team is full, player not added
        }
    }
}
