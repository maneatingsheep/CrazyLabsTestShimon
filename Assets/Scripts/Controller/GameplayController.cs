using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using View;

namespace Controller
{
    public class GameplayController : MonoBehaviour
    {
        public GameplayView GameplayViewRef;
        public GameplayUIView UIViewRef;
        private GameLevelModel _currentLevel;

        public Action OnGameOver;

        private int[] _currentCounts;

        public void Init()
        {
            UIViewRef.Init();
            GameplayViewRef.Init();
            GameplayViewRef.OnBallsEliminated += BallsEliminaßted;

            GameplayViewRef.TransitionOut(false);
        }

        private void BallsEliminaßted(int typeID, int count)
        {
            if (typeID >= _currentCounts.Length)
            {
                GameplayViewRef.DropBalls();
                return;
            }

            ;


            _currentCounts[typeID] += count;
            UIViewRef.UpdateView(typeID, Mathf.Max(0, _currentLevel.TargetCounts[typeID] - _currentCounts[typeID]));

            bool isGameOver = true;
            for (int i = 0; i < _currentCounts.Length; i++)
            {
                isGameOver &= _currentLevel.TargetCounts[i] - _currentCounts[i] <= 0;
            }

            if (isGameOver)
            {
                OnGameOver();
            }
            else
            {
                GameplayViewRef.DropBalls();
            }

        }

        public void StartGame()
        {
            _currentLevel = LevelManagerModel.Instance.GetCurrentLevel();

            _currentCounts = new int[_currentLevel.TargetCounts.Length];

            UIViewRef.Reset();
            GameplayViewRef.Reset();
            GameplayViewRef.TransitionIn(OnTransitionDone);
        }

        private void OnTransitionDone()
        {
            GameplayViewRef.StartGame();
        }

        public void EndGame()
        {
            GameplayViewRef.TransitionOut(true);
        }
    }
}