using System.IO;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public static SkinManager Instance { get; private set; }

    public Material[] skins;
    public int CurrentIndex { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (Instance != null)
        { 
            Destroy(gameObject); 
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadData();
    }

    public Material GetCurrentMaterial()
    {
        return skins[Mathf.Clamp(CurrentIndex, 0, skins.Length - 1)];
    }

    public void SelectSkin(int index)
    {
        CurrentIndex = index;
        SaveData();
    }

    [System.Serializable]
    public class Data
    {
        public int skinIndex;
    }

    public void SaveData()
    {
        Data data = new Data();
        data.skinIndex = CurrentIndex;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Data data = JsonUtility.FromJson<Data>(json);

            CurrentIndex = data.skinIndex;
        }
    }
}
