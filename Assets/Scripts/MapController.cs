using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{

    public Transform ParalaxFront;
    public Transform ParalaxBack;

    
    private Vector2 _paralaxOffset = Vector2.zero;
    private float _scale = 1;

    private const float WorldUnitHeigth = 24f;

    private const float ScrollRange = 6f;
    
    void Start()
    {
        
        TouchDetector td = GetComponent<TouchDetector>();
        td.Init();
        td.OnGestureDetected = DoGesture;
        

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
}
