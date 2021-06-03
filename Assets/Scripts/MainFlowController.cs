using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainFlowController : MonoBehaviour
{

    private MapController _mapController;
    private GameplayController _gameplayController;
    
    void Start()
    {
        InitAllControllers();
    }

    private void InitAllControllers()
    {
        _mapController = GetComponent<MapController>();
        _gameplayController = GetComponent<GameplayController>();
        
        _mapController.Init();
        _gameplayController.Init();

        _mapController.OnGameStartClicked += StartGame;
    }

    private void StartGame()
    {
        _mapController.StartGame();
        _gameplayController.StartGame();
    }
    
}
