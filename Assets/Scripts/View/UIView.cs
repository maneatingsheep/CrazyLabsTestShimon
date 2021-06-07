using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;

namespace View
{
    public class UIView : MonoBehaviour
    {
        public RequestView RequestPF;

        public RectTransform RequestsListView;
        public RectTransform StartButt;
        
        
        private LevelManagerModel _lmm;
        private GameLevelModel _currentlevel;

        private List<RequestView> _requests = new List<RequestView>();

        public Color[] BallColors;
        public float ButtInY;
        public float ButtOutY;
        public float RequestInY;
        public float RequestOutY;

        public void Init()
        {
            _lmm = LevelManagerModel.Instance;
        }

        public void SetLevel()
        {
            _currentlevel = _lmm.GetCurrentLevel();
            for (int i = 0; i < _currentlevel.TargetCounts.Length; i++)
            {
                if (_requests.Count <= i)
                {
                    _requests.Add(Instantiate(RequestPF, RequestsListView));
                }

                _requests[i].SetType(BallColors[_currentlevel.AllowedTypes[i]]);
                _requests[i].SetCount(_currentlevel.TargetCounts[i]);
                _requests[i].gameObject.SetActive(true);
            }

            for (int i = _currentlevel.TargetCounts.Length; i < _requests.Count; i++)
            {
                _requests[i].gameObject.SetActive(false);
            }
        }

        public void UpdateCount(int typeID, int currentCount)
        {
            _requests[typeID].SetCount(currentCount);
        }

        public void DoTransition(bool IsToGame, bool showAnimation)
        {


            float tt = 0;
            if (showAnimation)
            {
                tt = ViewSettingsUtils.Instance.TransitionTime;
            }
            
            //StartButt.anchoredPosition =toVector2.zero;

            float buttFrom = (IsToGame) ? ButtInY : ButtOutY;
            float butTo = (IsToGame) ? ButtOutY : ButtInY;
            
            float reqFrom = (IsToGame) ? RequestOutY : RequestInY;
            float reqTo = (IsToGame) ? RequestInY : RequestOutY;
            
            //LeanTween.value(0, -300, tt).setOnUpdate((float v) => StartButt.anchoredPosition = new Vector2(0, v));
            LeanTween.value(buttFrom, butTo, tt).setOnUpdate((float v) => StartButt.anchoredPosition = new Vector2(0, v));
            LeanTween.value(reqFrom, reqTo, tt).setOnUpdate((float v) => RequestsListView.anchoredPosition = new Vector2(0, v));


        }
    }
}