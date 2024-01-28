using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour {
    [SerializeField] private string mainSceneName;
    [SerializeField] private CanvasGroup creditsCanvasGroup;
    [SerializeField] private float creditCanvasGroupFadeDuration = 1.0f;
    [SerializeField] private GameObject mainMenuHolder;
    [SerializeField] private LeaderboardManager leaderboardManager;
    [SerializeField] private Transform rightCurtain;
    [SerializeField] private Transform rightCurtainPosition;
    [SerializeField] private Transform leftCurtain;
    [SerializeField] private Transform leftCurtainPosition;
    [SerializeField] private float sceneTransitionTime;

    [SerializeField] private MainMenuThemeHandler mainAudioSource;

    private void Start() {
        FindObjectOfType<MainMenuThemeHandler>().FadeAudioSource(false);
    }

    public void LoadMainScene() {
        rightCurtain.DOMove(rightCurtainPosition.position, 2.0f);
        leftCurtain.DOMove(leftCurtainPosition.position, 2.0f).OnComplete(delegate { StartCoroutine(WaitAndLoadScene()); });
        mainAudioSource.StartFading();
    }

    private IEnumerator WaitAndLoadScene() {
        yield return new WaitForSeconds(sceneTransitionTime);
        SceneManager.LoadScene(mainSceneName);
    }
    
    public void QuitGame() { Application.Quit(); }
    public void FadeCanvasGroup(bool fade) {
        float target = fade ? 0.0f : 1.0f;
        creditsCanvasGroup.interactable = !fade;
        creditsCanvasGroup.blocksRaycasts = !fade;
        creditsCanvasGroup.DOFade(target, creditCanvasGroupFadeDuration);
    }

    // public void EnableLeaderboardManager() {
    //     leaderboardManager.gameObject.SetActive(true);
    //     if (!leaderboardManager.HasEntries()) {
    //         leaderboardManager.gameObject.SetActive(false);
    //         return;
    //     }
    //     mainMenuHolder.SetActive(false);
    // }
}
