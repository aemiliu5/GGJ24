using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour {
    [SerializeField] private string mainSceneName;
    [SerializeField] private CanvasGroup creditsCanvasGroup;
    [SerializeField] private float creditCanvasGroupFadeDuration = 1.0f;
    public void LoadMainScene() { SceneManager.LoadScene(mainSceneName); }
    public void QuitGame() { Application.Quit(); }
    public void FadeCanvasGroup(bool fade) {
        float target = fade ? 0.0f : 1.0f;
        creditsCanvasGroup.interactable = !fade;
        creditsCanvasGroup.blocksRaycasts = !fade;
        creditsCanvasGroup.DOFade(target, creditCanvasGroupFadeDuration);
    }
}
