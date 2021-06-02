using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameplayController : MonoBehaviour
{

    public Ball BallPF;

    private Ball[] _ballsPool;

    private Vector2 _offscreenPosition = new Vector2(20, 0);

    private Ball[,,] _grid = new Ball[20, 40, 2];

    private Ball _debugBall;
    
    // Start is called before the first frame update
    void Start()
    {
        
        
        _ballsPool = new Ball[300];
        for (int i = 0; i < _ballsPool.Length; i++)
        {
            _ballsPool[i] = Instantiate(BallPF, transform);
            _ballsPool[i].transform.position = _offscreenPosition;
            _ballsPool[i].OnClick = BallClicked;
            _ballsPool[i].Init();
        }

        StartCoroutine(TestOnPhone());
    }

    private IEnumerator TestOnPhone()
    {
        DropBalls();
        yield return new WaitForSeconds(1);
        DropBalls();
        yield return new WaitForSeconds(1);
        DropBalls();
        yield return new WaitForSeconds(1);
    }

    private void BallClicked(Ball ball)
    {
        _debugBall = ball;
        
        UpdateConnectionGrid();

        CheckNeighbours(_debugBall);
        for (int i = 0; i < _ballsPool.Length; i++)
        {
            if (!_ballsPool[i].IsOpen)
            {
                _ballsPool[i].Deactivate();      
                _ballsPool[i].transform.position = _offscreenPosition;
            }
            
        }
    }

    private void CheckNeighbours(Ball ball)
    {
        Vector3 pos = ball.transform.position;
        
        int gridx = Mathf.FloorToInt((pos.x + 8f) / 1.1f);
        int gridy = Mathf.FloorToInt((pos.y + 15f) / 1.1f);

        ball.IsOpen = false;

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                int tx = gridx + i;
                int ty = gridy + j;

                if (tx >= 0 && ty >= 0 && tx < _grid.GetLength(0) && ty < _grid.GetLength(1))
                {
                    for (int k = 0; k < 2; k++)
                    {
                        Ball neighbour = _grid[gridx + i, gridy + j, k];
                        if (neighbour != null && neighbour.Type == ball.Type && neighbour.IsOpen)
                        {
                            float dist = (neighbour.transform.position - ball.transform.position).magnitude;
                            if (dist < 1.1f)
                            {
                                /*Vector3 o = new Vector3(0, 0, -1);
                                Debug.DrawLine(ball.transform.position + o, neighbour.transform.position + o);*/
                                
                                CheckNeighbours(neighbour);
                            }
                        }
                    }
                }
                
                
            }            
        }

    }

    // Update is called once per frame
    public void DropBalls()
    {
        int ballCount = 50;
        int i = 0;
        while (i < _ballsPool.Length && ballCount > 0)
        {
            if (!_ballsPool[i].IsActivated)
            {
                _ballsPool[i].transform.position = new Vector2(Random.Range(-2f, 2f), Random.Range(3f, 5f));
                _ballsPool[i].SetType(Random.Range(0, 3));
                _ballsPool[i].Activate();
                ballCount--;
            }
            i++;
        }
    }

    public void UpdateConnectionGrid()
    {
        for (int i = 0; i < _grid.GetLength(0); i++)
        {
            for (int j = 0; j < _grid.GetLength(1); j++)
            {
                _grid[i, j, 0] = null;
                _grid[i, j, 1] = null;
            }            
        }

        for (int i = 0; i < _ballsPool.Length; i++)
        {
            if (_ballsPool[i].IsActivated)
            {
                int gridx = Mathf.FloorToInt((_ballsPool[i].transform.position.x + 8f) / 1.1f);
                int gridy = Mathf.FloorToInt((_ballsPool[i].transform.position.y + 15f) / 1.1f);
                _grid[gridx, gridy, (_grid[gridx, gridy, 0] == null) ? 0 : 1] = _ballsPool[i];
                _ballsPool[i].IsOpen = true;
            }
            
        }
        
    }
    
    
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DropBalls();
        }

        if (_debugBall != null)
        {
            /*UpdateConnectionGrid();

            CheckNeighbours(_debugBall);*/
        }
        
    }
}
