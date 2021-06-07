using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewSettingsUtils : MonoBehaviour
{
    
    public static ViewSettingsUtils Instance;          

    private void Awake()
    {
        Instance = this;
    }


    public float StageHeight;
    public float StageWidth;

    public float TransitionTime;

}
