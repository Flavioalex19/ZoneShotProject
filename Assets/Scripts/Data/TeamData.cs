using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TeamData 
{
    public string teamName;
    public List<PlayerData> playersData;

    public TeamData(Team team)
    {
        playersData = new List<PlayerData>();

        foreach (Player player in team.GetPlayerList())
        {
            playersData.Add(new PlayerData(player));
        }
    }
}
[System.Serializable]
public class PlayerData
{
    public string playerName;
    public int playerOVR;
    public int playerPotential;

    public PlayerData(Player player)
    {
        playerName = player.PlayerFirstName;  // Ensure these methods exist
        playerOVR = player.GetOVR();
        //playerPotential = player.GetPotential();
    }
}
