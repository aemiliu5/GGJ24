using UnityEngine;

public class LeaderboardTester : MonoBehaviour {
    [SerializeField] private LeaderboardManager leaderboardManager;
    [SerializeField] private GameObject leaderboardCanvas;
    private string _name = "Player Name";
    private int _entryCount = 0;
    private int _playerScore = 0;
    private float _timePlayed = 0.0f;
    private bool _leaderboardEnabled = false;
    // Update is called once per frame
    private void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            CreateNewEntry();
        }

        if (Input.GetKeyDown(KeyCode.L)) {
            _leaderboardEnabled = !_leaderboardEnabled;
            leaderboardCanvas.SetActive(_leaderboardEnabled);
        }
    }

    private void CreateNewEntry() {
        PlayerData playerData = new PlayerData() {
            playerName = $"{_name}:{_entryCount}",
            playerScore = _playerScore,
            timePlayed = _timePlayed,
            funRating = Random.Range(0, 4)
        };
        
        Debug.Log($"LEADERBOARD MANAGER : {leaderboardManager == null}");
        
        leaderboardManager.AddToPlayerData(playerData);
        _entryCount++;
        _playerScore += 100;
        _timePlayed += 60.0f;
    }
}
