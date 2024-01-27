using System;
using UnityEngine;

public class SaveManager : MonoBehaviour {
    public static Action SaveOnDestroy;
    #region Singleton
    public static SaveManager instance;
    private  void Awake() {
        if (instance == null)
            instance = this;
        _saveHandler = new SaveJson(saveFileName);
    }

    #endregion
    [SerializeField] private string saveFileName;
    private SaveJson _saveHandler;

    public void SaveData(string key, object data) { _saveHandler.SaveKey(key, data); }
    public object GetData(string key) => _saveHandler.GetObject(key);
    public bool HasSavedKey(string key) => _saveHandler.HasSavedKey(key);
}
