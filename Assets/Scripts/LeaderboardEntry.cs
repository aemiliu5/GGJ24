using TMPro;
using UnityEngine;
public class LeaderboardEntry : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI playerNameField;
    [SerializeField] private TextMeshProUGUI playerScoreField;
    [SerializeField] private TextMeshProUGUI playerRatingField;
    [SerializeField] private TextMeshProUGUI playerTimePlayedField;

    public void InitializeLeaderboardEntry(PlayerData playerData) {
        playerNameField.SetText(playerData.playerName);
        playerScoreField.SetText($"{playerData.playerScore}");
        FunFactor funFactor = (FunFactor)playerData.funRating;
        playerRatingField.SetText($"{funFactor}");
        float timePlayed = playerData.timePlayed / 60.0f;
        playerTimePlayedField.SetText($"{timePlayed:F2}");
    }
}
