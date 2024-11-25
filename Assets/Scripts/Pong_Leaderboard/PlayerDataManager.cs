using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class PlayerDataManager : MonoBehaviour
{
    [System.Serializable]
    public class Player
    {
        public string name;
        public int score;
    }

    [System.Serializable]
    public class PlayerList
    {
        public List<Player> players = new List<Player>();
    }

    private PlayerList playerList = new PlayerList();
    private string filePath;

    private void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "players.json");
        LoadPlayerData();
    }

    public void SavePlayerData(string name, int score)
    {
        Player newPlayer = new Player { name = name, score = score };
        playerList.players.Add(newPlayer);
        playerList.players.Sort((x, y) => y.score.CompareTo(x.score));

        string json = JsonUtility.ToJson(playerList, true);
        File.WriteAllText(filePath, json);
    }

    public PlayerList GetLeaderboard()
    {
        LoadPlayerData();
        return playerList;
    }

    private void LoadPlayerData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            playerList = JsonUtility.FromJson<PlayerList>(json);
            playerList.players.Sort((x, y) => y.score.CompareTo(x.score));
        }
    }
}