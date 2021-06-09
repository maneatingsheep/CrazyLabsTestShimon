using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

namespace View
{
    public class Ball : MonoBehaviour
    {
        internal bool IsActivated;
        internal int TypeID;

        private Rigidbody2D _rb;
        private SpriteRenderer _sr;

        public Action<Ball> OnClick;

        internal bool IsOpen;

        private Vector2 _offscreenPosition = new Vector2(1000, 1000);

        
        public void Init()
        {
            _rb = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
            transform.position = _offscreenPosition;
        }

        public void DeactivateGrace(float delay)
        {
            IsActivated = false;
            gameObject.SetActive(false);
            _rb.isKinematic = true;
            transform.position = _offscreenPosition;
            
            
        }

        
        public void Activate()
        {
            gameObject.SetActive(true);
            _rb.isKinematic = false;
            IsActivated = true;
            transform.localScale = Vector3.one;
        }

        public void SetType(int typeId, Color col)
        {
            TypeID = typeId;
            _sr.material.color = col;
        }

        public void OnMouseDown()
        {
            OnClick(this);
        }
    }
}