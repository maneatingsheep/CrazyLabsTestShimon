using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


namespace Model
{
    public class LevelParser : MonoBehaviour
    {
        private string _path = "Assets/Resources/levels.txt";

#if UNITY_EDITOR
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

#endif

        public GameLevelModel[] ReadLevels()
        {

            TextAsset levelsTxt = (TextAsset) Resources.Load("levels", typeof(TextAsset));

            LevelManagerModel.LevelJsonWrapper levelsWrapper =
                JsonUtility.FromJson<LevelManagerModel.LevelJsonWrapper>(levelsTxt.text);

            return levelsWrapper.levels;
        }
    }
}
