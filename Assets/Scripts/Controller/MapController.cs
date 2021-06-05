using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace Controller
{


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

        public void EndGame()
        {
            MapViewRef.SetInteractive(true);
            MapViewRef.TransitionIn(true);
        }
    }
}