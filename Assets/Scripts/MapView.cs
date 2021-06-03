using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapView : MonoBehaviour
{

    public Transform ParalaxFront;
    public Transform ParalaxBack;

    public RectTransform StartButt;
    
    private Vector2 _paralaxOffset = Vector2.zero;
    private float _scale = 1;
    private TouchDetector _td;

    private const float WorldUnitHeigth = 24f;

    private const float ScrollRange = 6f;
    
    public void Init()
    {
        
        _td = GetComponent<TouchDetector>();
        _td.Init();
        _td.OnGestureDetected = DoGesture;
        

    }

    private void DoGesture(Vector2 endPointNorm, Vector2 posDeltaNorm, float normScaleDelta)
    {
        
        
        _scale *= normScaleDelta;
        if (_scale < 0.5f)
        {
            _scale = 0.5f;
        }
        else if (_scale > 2f)
        {
            _scale = 2f;
        }

        _paralaxOffset += posDeltaNorm * WorldUnitHeigth;

        if (_paralaxOffset.magnitude > ScrollRange * _scale)
        {
            _paralaxOffset.Normalize();
            _paralaxOffset *= ScrollRange * _scale;
        }
        
        
        Vector2 endPoint = endPointNorm * WorldUnitHeigth;
        Vector2 diff = _paralaxOffset - endPoint;
        diff *= normScaleDelta;

        _paralaxOffset = endPoint + diff;
        
        SetParalax();
    }


    private void SetParalax()
    {
        
        ParalaxFront.position = _paralaxOffset;
        ParalaxBack.position = _paralaxOffset * 0.2f;

        ParalaxFront.localScale = new Vector3(_scale, _scale, 1) ;

        float backScale = (_scale - 1) * 0.2f + 1;
        ParalaxBack.localScale = new Vector3(backScale, backScale, 1) ;
        
    }


    public void TransitionOut()
    {
        Vector2 outOffset = new Vector2(0, 25f);

        LeanTween.value(0, -300, 0.5f).setOnUpdate((float v)=>StartButt.anchoredPosition = new Vector2(0, v));
        
        LeanTween.value(gameObject, _paralaxOffset, outOffset, 0.5f).setOnUpdate((Vector2 v) => { _paralaxOffset = v; });
        LeanTween.value(gameObject, _scale, 1, 0.5f).setOnUpdate((float v) =>
        {
            _scale = v;
            SetParalax();
        });
        
    }

    public void TransitionIn(bool showAnimation)
    {
        if (showAnimation)
        {
            LeanTween.move(StartButt.gameObject, Vector2.zero, 0.5f);
            
            _paralaxOffset = new Vector2(0, 25f);;
            _scale = 1;
            
            LeanTween.value(gameObject, _paralaxOffset, Vector2.zero, 0.5f).setOnUpdate((Vector2 v) =>
            {
                _paralaxOffset = v;
                SetParalax();
            });
            
        }
        else
        {
            StartButt.anchoredPosition = Vector2.zero;
            
            _paralaxOffset = Vector2.zero;
            _scale = 1;
            SetParalax();
            
            
        }
        
    }

    public void SetInteractive(bool isInteractive)
    {
        _td.IsInteractive = isInteractive;
    }
}
