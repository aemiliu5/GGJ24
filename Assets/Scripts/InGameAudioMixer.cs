using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class InGameAudioMixer : MonoBehaviour {
    #region Singleton
    public static InGameAudioMixer instance;
    public void Awake() {
        if (instance == null)
            instance = this;
    }
    #endregion
    [SerializeField] private AudioSource[] additionalTracks;
    [SerializeField] private float transitionTimeStep = 0.5f;

    public FunFactor funFactor;
    public bool testing;
    
    private float _currentFullVolume;
    private FunFactor _currentFunFactor;

    private SaveManager _saveManager;

    private void Start() { Initialize(); }

    private void Update() {
        if (!testing) return;
        CheckFunFactorTesting();
    }

    private void Initialize() {
        if(testing)
            _currentFunFactor = funFactor;
        _saveManager = SaveManager.instance;
    }

    private void ChangeFunFactor(FunFactor factor) {
        if (_currentFunFactor == factor) return;
        switch (factor) {
            case FunFactor.Disappointed:
                ChangeSourceVolume(additionalTracks[0], false);
                ChangeSourceVolume(additionalTracks[1], false);
                break;
            case FunFactor.Neutral:
                ChangeSourceVolume(additionalTracks[0], false);
                ChangeSourceVolume(additionalTracks[1], false);
                break;
            case FunFactor.Happy:
                ChangeSourceVolume(additionalTracks[0], true);
                ChangeSourceVolume(additionalTracks[1], false);
                break;
            case FunFactor.Ecstatic:
                ChangeSourceVolume(additionalTracks[0], true);
                ChangeSourceVolume(additionalTracks[1], true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        StartCoroutine(WaitAndSet(factor));
    }

    private void CheckFunFactorTesting() {
        if (funFactor == _currentFunFactor) {
            return;
        }
        switch (funFactor) {
            case FunFactor.Disappointed:
                ChangeSourceVolume(additionalTracks[0], false);
                ChangeSourceVolume(additionalTracks[1], false);
                break;
            case FunFactor.Neutral:
                ChangeSourceVolume(additionalTracks[0], false);
                ChangeSourceVolume(additionalTracks[1], false);
                break;
            case FunFactor.Happy:
                ChangeSourceVolume(additionalTracks[0], true);
                ChangeSourceVolume(additionalTracks[1], false);
                break;
            case FunFactor.Ecstatic:
                ChangeSourceVolume(additionalTracks[0], true);
                ChangeSourceVolume(additionalTracks[1], true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        StartCoroutine(WaitAndSet(funFactor));
    }

    private IEnumerator WaitAndSet(FunFactor factor) {
        yield return new WaitForSeconds(transitionTimeStep + 0.05f);
        _currentFunFactor = factor;
    }

    private void ChangeSourceVolume(AudioSource audioSource, bool fullVolume) {
        float vol = 0.0f;
        try { vol = (float)_saveManager.GetData(SaveKeywords.MasterVolume); }
        catch (InvalidCastException) {
            double castVol = (double)_saveManager.GetData(SaveKeywords.MasterVolume);
            vol = (float)castVol;
        }
        
        _currentFullVolume = vol;
        
        float targetVolume = fullVolume ? _currentFullVolume : 0;
        float originalVolume = audioSource.volume;
        DOVirtual.Float(originalVolume, targetVolume, transitionTimeStep, (x) => {
            audioSource.volume = x;
        });
    }
}
