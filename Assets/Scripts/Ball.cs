using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public bool IsActivated;
    public int Type;
    
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;

    public  Action<Ball> OnClick;

    public bool IsOpen;
    public void Init()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
    }
    
    public void Deactivate()
    {
        _rb.isKinematic = true;
        IsActivated = false;
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        _rb.isKinematic = false;
        IsActivated = true;
    }

    public void SetType(int type)
    {
        Type = type;
        _sr.color = (type == 0) ? Color.red : ((type == 1) ? Color.green : ((type == 2) ? Color.blue : (Color.yellow)));
    }

    public void OnMouseDown()
    {
        OnClick(this);
    }
}
