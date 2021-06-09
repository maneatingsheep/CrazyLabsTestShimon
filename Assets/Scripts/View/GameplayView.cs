using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace View
{

    public class GameplayView : MonoBehaviour
    {

        public Ball BallPF;

        public Color[] BallColors;

        public Action<int, int> OnBallsEliminated;

        public float StartDelay;

        public float DropRange;

        public float NeighbourRange;
        public float GridCellSize;

        public int MinChainLen;

        public int BallIsntanceCount; //max ammount of balls
        
        private Ball[] _ballsPool;  
        
        private Ball[,,] _grid;

        private Vector2 OutPos;

        private GameLevelModel _levelModel;



        // Start is called before the first frame update
        public void Init()
        {
            _ballsPool = new Ball[BallIsntanceCount];
            for (int i = 0; i < _ballsPool.Length; i++)
            {
                _ballsPool[i] = Instantiate(BallPF, transform);
                _ballsPool[i].OnClick = BallClicked;
                _ballsPool[i].Init();
                _ballsPool[i].DeactivateGrace(0);
            }

            OutPos = new Vector2(0, -ViewSettingsUtils.Instance.StageHeight);
            
            _grid = new Ball[Mathf.CeilToInt(ViewSettingsUtils.Instance.StageWidth / GridCellSize), 
                Mathf.CeilToInt(ViewSettingsUtils.Instance.StageHeight / GridCellSize), 
                2];
        }

        public void Reset()
        {
            _levelModel = LevelManagerModel.Instance.GetCurrentLevel();

            for (int i = 0; i < _ballsPool.Length; i++)
            {
                _ballsPool[i].DeactivateGrace(0);
            }
        }

        public void StartGame()
        {
            DropBalls();
        }


        private void BallClicked(Ball ball)
        {

            UpdateConnectionGrid();

            int usedBallsCount = CheckNeighbours(ball);


            for (int i = 0; i < _levelModel.BallCount; i++)
            {
                if (!_ballsPool[i].IsOpen)
                {
                    if (usedBallsCount < MinChainLen)
                    {
                        _ballsPool[i].IsOpen = true;
                    }
                    else
                    {
                        _ballsPool[i].DeactivateGrace((ball.transform.position - _ballsPool[i].transform.position).magnitude * 0.03f);
                        
                    }
                }

            }

            if (usedBallsCount >= MinChainLen)
            {
                OnBallsEliminated(ball.TypeID, usedBallsCount);
            }

        }

        private int CheckNeighbours(Ball ball)
        {
            int count = 1;
            
            Vector2Int gridPos = BallGridCoords(ball.transform.position);

            ball.IsOpen = false;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int tx = gridPos.x + i;
                    int ty = gridPos.y + j;

                    if (tx >= 0 && ty >= 0 && tx < _grid.GetLength(0) && ty < _grid.GetLength(1))
                    {
                        for (int k = 0; k < 2; k++)
                        {
                            Ball neighbour = _grid[gridPos.x + i, gridPos.y + j, k];
                            if (neighbour != null && neighbour.TypeID == ball.TypeID && neighbour.IsOpen)
                            {
                                float dist = (neighbour.transform.position - ball.transform.position).magnitude;
                                if (dist < NeighbourRange)
                                {
                                    /*Vector3 o = new Vector3(0, 0, -1);
                                    Debug.DrawLine(ball.transform.position + o, neighbour.transform.position + o);*/

                                    count += CheckNeighbours(neighbour);
                                }
                            }
                        }
                    }


                }
            }

            return count;
        }

        // Update is called once per frame
        public void DropBalls()
        {
            int i = 0;
            while (i < _levelModel.BallCount)
            {
                if (!_ballsPool[i].IsActivated)
                {
                    float sh = ViewSettingsUtils.Instance.StageHeight;

                    //vertical random just to add some time drop variation
                    _ballsPool[i].transform.position = new Vector2(Random.Range(-DropRange/ 2f, DropRange / 2f),  sh / 2f + Random.Range(-1f, -2f));
                    int typeIndex = Random.Range(0, _levelModel.AllowedTypes.Length);
                    _ballsPool[i].SetType(typeIndex, BallColors[_levelModel.AllowedTypes[typeIndex]]);
                    _ballsPool[i].Activate();
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

            for (int i = 0; i < _levelModel.BallCount; i++)
            {
                if (_ballsPool[i].IsActivated)
                {
                    Vector2Int gridPos = BallGridCoords(_ballsPool[i].transform.position);

                    //ignore out of bound sballs
                    if (gridPos.x > 0 && gridPos.x < _grid.GetLength(0) && gridPos.y > 0 &&
                        gridPos.y < _grid.GetLength(1))
                    {
                        _grid[gridPos.x, gridPos.y, (_grid[gridPos.x, gridPos.y, 0] == null) ? 0 : 1] = _ballsPool[i];
                        _ballsPool[i].IsOpen = true;
                    }
                    
                }

            }

        }

        private Vector2Int BallGridCoords(Vector2 pos)
        {
            int gridx = Mathf.FloorToInt((pos.x + ViewSettingsUtils.Instance.StageWidth / 2f) / GridCellSize);
            int gridy = Mathf.FloorToInt((pos.y + ViewSettingsUtils.Instance.StageHeight / 2f) / GridCellSize);
            return new Vector2Int(gridx, gridy);
        }
        
        public void TransitionIn(Action doneCallback)
        {
            float tt = ViewSettingsUtils.Instance.TransitionTime;

            LeanTween.move(gameObject, Vector2.zero, tt);
            LeanTween.delayedCall(tt + StartDelay, doneCallback);
        }


        public void TransitionOut(bool showAnimation)
        {
            if (showAnimation)
            {
                LeanTween.move(gameObject, OutPos, ViewSettingsUtils.Instance.TransitionTime);
            }
            else
            {
                transform.position = OutPos;
            }

        }
    }
}