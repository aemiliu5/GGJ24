using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour {
    [SerializeField] private string mainSceneName;
    public void LoadMainScene() { SceneManager.LoadScene(mainSceneName); }
    public void QuitGame() { Application.Quit(); }
}
