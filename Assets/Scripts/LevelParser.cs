using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LevelParser : MonoBehaviour
{

    private string _path = "Assets/Resources/levels.txt";
    public void WriteLevels(GameLevelModel[] levels)
    {

        LevelManagerModel.LevelJsonWrapper wrapper = new LevelManagerModel.LevelJsonWrapper();
        wrapper.levels = levels;
        
        string levelConfigStr = JsonUtility.ToJson(wrapper);
        
        StreamWriter sw = new StreamWriter(_path, false);
        sw.Write(levelConfigStr);
        sw.Close();
        
        AssetDatabase.ImportAsset(_path); 
        
        
        
    }

    public GameLevelModel[] ReadLevels()
    {
        StreamReader sr = new StreamReader(_path); 
        string json = sr.ReadToEnd();
        sr.Close();

        LevelManagerModel.LevelJsonWrapper levelsWrapper = JsonUtility.FromJson<LevelManagerModel.LevelJsonWrapper>(json);

        return levelsWrapper.levels;
    }
}
