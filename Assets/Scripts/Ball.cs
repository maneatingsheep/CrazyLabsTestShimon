using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Ball : MonoBehaviour
{
    public bool IsActivated;
    public int TypeID;
    
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

    public void SetType(int typeId, Color col)
    {
        TypeID = typeId;
        //_sr.color = col;
        _sr.material.color = col;
    }

    public void OnMouseDown()
    {
        OnClick(this);
    }
}
