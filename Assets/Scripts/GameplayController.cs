using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public GameplayView GameplayViewRef;

    public void Init()
    {
        GameplayViewRef.Init();
        GameplayViewRef.TransitionOut(false);
    }

    public void StartGame()
    {
        GameplayViewRef.TransitionIn(OnTransitionDone);
        GameplayViewRef.Reset();
    }

    private void OnTransitionDone()
    {
        GameplayViewRef.StartGame();
    }
}
