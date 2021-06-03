﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public MapView MapViewRef;

    public Action OnGameStartClicked;
    
    public void Init()
    {
        MapViewRef.Init();
        MapViewRef.TransitionIn(false);
        MapViewRef.SetInteractive(true);
    }

    public void StartGame()
    {
        MapViewRef.SetInteractive(false);
        MapViewRef.TransitionOut();
    }

    public void GameStartClicked()
    {
        OnGameStartClicked();
    }
}
