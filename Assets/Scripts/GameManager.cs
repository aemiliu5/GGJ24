using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int funFactor; // 3 - ecstatic, 2 - happy, 1 - neutral, 0 - disappointed - <0 dies
    public int combo;
    private int loseCombo = 0;
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
    private void Start()
    {
        instance = this;
        _audioMixer = InGameAudioMixer.instance;
    }

    public void BeginPlay() {
        player.enabled = true;
        namingPanel.SetActive(false);
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

        funText.text = ballSpawner.timeSinceLastSpawn.ToString();

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
        if (combo > 10)
        {
            funFactor++;
            FunFactor fFactor = (FunFactor)funFactor;
            _audioMixer.FunFactor = fFactor;
        }
    }

    public void AddCombo()
    {
        combo++;
        loseCombo = 0;
    }

    public void LoseCombo()
    {
        combo = 1;
        loseCombo++;
        if (loseCombo >= 10)
        {
            funFactor--;
            FunFactor fFactor = (FunFactor)funFactor;
            _audioMixer.FunFactor = fFactor;
        }
    }

    public void Lose()
    {
        player.StatsToPassToPlayerData(funFactor, scoretext.score);
        player.enabled = false;
        ballSpawner.enabled = false;
        leaderboard.SetActive(true);
    }
}
