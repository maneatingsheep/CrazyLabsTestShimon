using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using View;

namespace Controller
{
    public class MainFlowController : MonoBehaviour
    {

        private MapController _mapController;
        private GameplayController _gameplayController;
        
        
        void Start()
        {
            InitAllModels();
            InitAllControllers();
            
        }

        private void InitAllModels()
        {
            LevelManagerModel.Instance.Init();
        }

        private void InitAllControllers()
        {
            _mapController = GetComponent<MapController>();
            _gameplayController = GetComponent<GameplayController>();

            _mapController.Init();
            _gameplayController.Init();

            _mapController.OnGameStartClicked += StartGame;
            _gameplayController.OnGameOver += EndGame;
        }

        private void StartGame()
        {
            _mapController.StartGame();
            _gameplayController.StartGame();
        }

        private void EndGame()
        {
            _mapController.EndGame();
            _gameplayController.EndGame();
            LevelManagerModel.Instance.AdvanceLevel();
        }

    }
}