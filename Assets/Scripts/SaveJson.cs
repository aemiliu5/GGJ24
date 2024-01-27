using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class SaveJson
{
    private static readonly string SavePath = Application.persistentDataPath + "/" + "PlayerSave";
    private readonly string _combinedPath;
    private Dictionary<string, object> _data = new Dictionary<string, object>();
    public SaveJson(string saveFileName) {
        _combinedPath = SavePath + "/" +saveFileName;
        CheckSaveFile();
    }

    private void CheckSaveFile() {
        bool fileAndDirectoryExist = Directory.Exists(SavePath) && File.Exists(_combinedPath);
        if (!Directory.Exists(SavePath)) {
            Directory.CreateDirectory(SavePath);
        }
        if (!File.Exists(_combinedPath)) {
            File.Create(_combinedPath);
        }

        if (!fileAndDirectoryExist) return;
        
        //Load data at initialization
        using StreamReader sr = new StreamReader(_combinedPath);
        string saveFileText = sr.ReadToEnd();

        if (string.IsNullOrEmpty(saveFileText)) {
            return;
        }
        
        _data = JsonConvert.DeserializeObject<Dictionary<string, object>>(saveFileText, new JsonSerializerSettings() {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Auto    
        });
        sr.Dispose();
    }

    public void SaveKey(string key, object data) {
        _data[key] = data;
        Debug.Log($"SAVED TYPE : {data.GetType()}");
        SaveToFile();
    }

    public object GetObject(string key) {
        if (_data.ContainsKey(key)) return _data[key];
        Debug.LogWarning($"Save file does not contain key : {key}");
        return null;
    }

    private void SaveToFile() {
        string jsonConversion = JsonConvert.SerializeObject(_data, new JsonSerializerSettings() {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Auto 
        });
        using StreamWriter sw = new StreamWriter(_combinedPath);
        sw.Write(jsonConversion);
        sw.Dispose();
    }
    
    public bool HasSavedKey(string key) { return _data.ContainsKey(key); }
}
