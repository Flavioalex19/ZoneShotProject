using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{

    [SerializeField] public  string t_teamName;
    [SerializeField] public int t_currentPlayerOnTheTeamIndex = 0;
    [SerializeField] public int t_maxNumberOfPlayerOnTeam = 8;
    [SerializeField] public List<Player> t_playersList = new List<Player>();

    public bool AddPlayerToTeam(Player newPlayer)
    {
        print(t_currentPlayerOnTheTeamIndex + " " + name);
        if (t_currentPlayerOnTheTeamIndex < t_maxNumberOfPlayerOnTeam)
        {
            t_playersList.Add(newPlayer);
            t_currentPlayerOnTheTeamIndex++;
            return true; // Player added successfully
        }
        else
        {
            Debug.Log("Team is full!");
            return false; // Team is full, player not added
        }
    }
    public void RestIndex()
    {
        t_currentPlayerOnTheTeamIndex = 0;
    }
    public void ResetList()
    {
        t_playersList.Clear();
    }
    public List<Player> GetPlayerList()
    {
        return t_playersList;
    }
}
