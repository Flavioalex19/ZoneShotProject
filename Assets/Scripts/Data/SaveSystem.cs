using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private static string saveFilePath = Application.persistentDataPath + "/gameSave.json";

    public static void SavePlayers(List<Team> teams)
    {
        List<PlayerData> allPlayerData = new List<PlayerData>();

        foreach (Team team in teams)
        {
            foreach (Player player in team.GetPlayerList())
            {
                allPlayerData.Add(new PlayerData(player));
            }
        }

        string json = JsonUtility.ToJson(new PlayerDataListWrapper { players = allPlayerData }, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Game saved to " + saveFilePath);
    }

    public static List<Player> LoadPlayers()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            PlayerDataListWrapper wrapper = JsonUtility.FromJson<PlayerDataListWrapper>(json);
            List<Player> loadedPlayers = new List<Player>();

            foreach (PlayerData playerData in wrapper.players)
            {
                Player player = DraftManager.CreatePlayerFromData(playerData); // Implement this method
                loadedPlayers.Add(player);
            }

            return loadedPlayers;
        }
        else
        {
            Debug.LogWarning("No save file found.");
            return null;
        }
    }
    public static List<PlayerData> LoadPlayerData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            PlayerDataListWrapper playerListWrapper = JsonUtility.FromJson<PlayerDataListWrapper>(json);
            return playerListWrapper.players;
        }
        else
        {
            Debug.LogWarning("No save file found.");
            return null;
        }
    }

    public static void ClearSave()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Save file deleted.");
        }
    }
    public static void ClearTeamData(List<Team> teams)
    {
        foreach (Team team in teams)
        {
            team.GetPlayerList().Clear(); // Clear the player list for each team
            team.ResetList();
        }

        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Save file deleted.");
        }
    }
}
[System.Serializable]
public class PlayerDataListWrapper
{
    public List<PlayerData> players;
}
