using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int funFactor; // 3 - ecstatic, 2 - happy, 1 - neutral, 0 - disappointed - <0 dies
    
    public int funFactorCombo;
    public int totalCombo;
    public int loseCombo = 0;
    public float time;

    public PlayerController player;
    public BallSpawner ballSpawner;
    public GameObject namingPanel;
    public GameObject leaderboard;
    
    public static GameManager instance;

    public Score scoretext;
    public int scoreMultiplier;

    public TextMeshProUGUI funText;
    public TextMeshProUGUI startText;

    private InGameAudioMixer _audioMixer;
    public bool hasStarted;

    public Camera mainCam;
    private void Start()
    {
        instance = this;
        _audioMixer = InGameAudioMixer.instance;
        mainCam = Camera.main;
    }

    public void ReturnToMainMenu() {
        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R) && player.enabled)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (hasStarted) {
            time += Time.deltaTime;
            startText.enabled = false;
        }

        if (funFactor < 0)
        {
            Lose();
        }

        ManageCombo();

        funText.text = totalCombo.ToString();

        if (!hasStarted) {
            if (Input.GetKeyDown(KeyCode.Space) && player.enabled) {
                _audioMixer.EnableMusic();
                ballSpawner.timeSinceLastSpawn = 1.6875f;
                ballSpawner.enabled = true;
                hasStarted = true;
            }
        }
    }

    public void ManageCombo()
    {
        if (funFactorCombo > 10)
        {
            if (funFactor > 4)
                return;
            
            funFactor++;
            int enumIndex = funFactor;
            enumIndex = Mathf.Clamp(enumIndex, 0, Enum.GetValues(typeof(FunFactor)).Length);
            FunFactor fFactor = (FunFactor)enumIndex;
            _audioMixer.FunFactor = fFactor;
            funFactorCombo = 0;
        }
    }

    public void AddCombo()
    {
        totalCombo++;
        funFactorCombo++;
        loseCombo = 0;
    }

    public void LoseCombo()
    {
        totalCombo = 1;
        loseCombo++;
        if (loseCombo >= 3)
        {
            funFactor--;
            int enumIndex = funFactor;
            enumIndex = Mathf.Clamp(enumIndex, 0, Enum.GetValues(typeof(FunFactor)).Length);
            FunFactor fFactor = (FunFactor)enumIndex;
            _audioMixer.FunFactor = fFactor;
            loseCombo = 0;
        }
        
        mainCam.transform.DOShakePosition(0.1f, new Vector3(0.25f, 0, 0)).OnComplete(() => mainCam.transform.position = new (0,0,-10));
    }

    public void LowerMood() { funFactor--; }
    
    public void Lose() {
        player.StatsToPassToPlayerData(funFactor, scoretext.score);
        player.enabled = false;
        ballSpawner.enabled = false;
        namingPanel.SetActive(true);
    }

    public void EnableLeaderboard() {
        //leaderboard.GetComponent<LeaderboardManager>().AddToPlayerData(player.playerData);
        namingPanel.SetActive(false);
        leaderboard.SetActive(true);
    }
}
