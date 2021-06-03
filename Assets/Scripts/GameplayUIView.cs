using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUIView : MonoBehaviour
{
    public RequestView RequestPF;

    private LevelManagerModel _lmm;
    private GameLevelModel _currentlevel;

    private List<RequestView> _requests = new List<RequestView>();
    
    public Color[] BallColors;
    
    public void Init()
    {
        _lmm = LevelManagerModel.Instance;
    }

    public void Reset()
    {
        _currentlevel = _lmm.GetCurrentLevel();
        for (int i = 0; i < _currentlevel.TargetCounts.Length; i++)
        {
            if (_requests.Count <= i)
            {
                _requests.Add(Instantiate(RequestPF, transform)) ;
            }

            _requests[i].SetType(BallColors[_currentlevel.AllowedTypes[i]]);
            _requests[i].SetCount(_currentlevel.TargetCounts[i]);
            _requests[i].gameObject.SetActive(true);
        }

        for (int i = _currentlevel.TargetCounts.Length ; i < _requests.Count; i++)
        {
            _requests[i].gameObject.SetActive(false);
        }
    }

    public void UpdateView(int typeID, int currentCount)
    {
        _requests[typeID].SetCount(currentCount);
    }
}
