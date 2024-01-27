using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour {
    [SerializeField] private GameObject leaderboardEntry;
    [SerializeField] private Transform leaderboardEntryHolder;
    private List<PlayerData> _playerData = new List<PlayerData>();

    private SaveManager _saveManager;
    private bool _initialized;
    private void OnEnable() {
        if (!_initialized) {
            _saveManager = SaveManager.instance;
            var savedData = SaveManager.instance.GetData(SaveKeywords.PlayerDataKey);
            if (savedData != null)
                _playerData = (List<PlayerData>)savedData;
            _initialized = true;
        }
        
        if (_playerData == null) return;
        //Display leaderboard entries
        DisplayEntries();
    }
    
    private void OnDisable() {
        //Destroy previous entries
        DestroyEntries();
    }
    
    private void DisplayEntries() {
    	var orderedList = _playerData.OrderByDescending(data => data.playerScore);
        foreach (var playerDataEntry in orderedList) {
            GameObject go = Instantiate(leaderboardEntry.gameObject, leaderboardEntryHolder);
            LeaderboardEntry entry = go.GetComponent<LeaderboardEntry>();
            entry.InitializeLeaderboardEntry(playerDataEntry);
        }
    }

    private void DestroyEntries() {
        for (int i = 0; i < leaderboardEntryHolder.childCount; i++) {
            Destroy(leaderboardEntryHolder.GetChild(i).gameObject);
        }
    }

    public void AddToPlayerData(PlayerData leaderboardEntry) {
        _playerData.Add(leaderboardEntry);
        SaveManager.instance.SaveData(SaveKeywords.PlayerDataKey, _playerData);
    }
}
