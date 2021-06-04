using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapView : MonoBehaviour
{

    public RectTransform StartButt;

    public ParticleSystem[] Particles;
    
    private ParalaxView _paralaxView;
    
    private const float WorldUnitHeigth = 24f;
    
    private const float ScrollRange = 6f;
    
    private TouchDetector _td;

    private Vector2 _paralaxOffset = Vector2.zero;
    private float _scale = 1;
    
    public void Init()
    {
        
        _td = GetComponent<TouchDetector>();
        _paralaxView = GetComponent<ParalaxView>();
        
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
        
        _paralaxView.SetParalax(_paralaxOffset, _scale);
    }


    


    public void TransitionOut()
    {
        Vector2 outOffset = new Vector2(0, 25f);

        LeanTween.value(0, -300, 0.5f).setOnUpdate((float v)=>StartButt.anchoredPosition = new Vector2(0, v));
        
        LeanTween.value(gameObject, _paralaxOffset, outOffset, 0.5f).setOnUpdate((Vector2 v) => { _paralaxOffset = v; });
        LeanTween.value(gameObject, _scale, 1, 0.5f).setOnUpdate((float v) =>
        {
            _scale = v;
            _paralaxView.SetParalax(_paralaxOffset, _scale);
        });

        SetPArticlesActivation(false);
    }

    

    public void TransitionIn(bool showAnimation)
    {
        if (showAnimation)
        {
            LeanTween.value(-300, 0, 0.5f).setOnUpdate((float v)=>StartButt.anchoredPosition = new Vector2(0, v));
            
            _paralaxOffset = new Vector2(0, 25f);;
            _scale = 1;
            
            LeanTween.value(gameObject, _paralaxOffset, Vector2.zero, 0.5f).setOnUpdate((Vector2 v) =>
            {
                _paralaxOffset = v;
                _paralaxView.SetParalax(_paralaxOffset, _scale);
            });
            
        }
        else
        {
            StartButt.anchoredPosition = Vector2.zero;
            
            _paralaxOffset = Vector2.zero;
            _scale = 1;
            _paralaxView.SetParalax(_paralaxOffset, _scale);
            
            
        }
        
        SetPArticlesActivation(true);
    }

    private void SetPArticlesActivation(bool isActive)
    {
        for (int i = 0; i < Particles.Length; i++)
        {
            if (isActive)
            {
                Particles[i].Play();
            }
            else
            {
                Particles[i].Pause();
            }
                
        }
        
    }
    
    public void SetInteractive(bool isInteractive)
    {
        _td.IsInteractive = isInteractive;
    }
}
