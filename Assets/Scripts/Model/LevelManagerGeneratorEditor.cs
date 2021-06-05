using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR


[CustomEditor(typeof(LevelManagerModel))]
public class LevelManagerGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        LevelManagerModel lmm = (LevelManagerModel)target;

        GameLevelModel[] levels = new GameLevelModel[lmm.transform.childCount];
        
        if (GUILayout.Button("Generate LevelConfig file")){

            for (int i = 0; i < lmm.transform.childCount; i++)
            {
                LevelEditModel lem = lmm.transform.GetChild(i).GetComponent<LevelEditModel>();
                levels[i] = lem.GenerateLevel();
            }
            lmm.GetComponent<LevelParser>().WriteLevels(levels);            
        }

        
        

    }
}


#endif
