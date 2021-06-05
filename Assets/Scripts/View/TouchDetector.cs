using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{

    public class TouchDetector : MonoBehaviour
    {

        public delegate void Gesture(Vector2 startPointNorm, Vector2 deltaMoveNorm, float scaleFactor);

        public Gesture OnGestureDetected;

        private float _screenHeight;
        private float _screenRat;

        private bool _isPaning = false;
        private bool _isScaling = false;

        private Vector2 _initialPanPosition;
        private float _inititlaScaleDist;
        public bool IsInteractive = false;


        public void Init()
        {
            _screenHeight = Screen.height;
            _screenRat = Screen.width / _screenHeight;
        }

        void Update()
        {
            if (!IsInteractive)
            {
                _isPaning = false;
                _isScaling = false;
                return;
            }

            Touch[] touches = Input.touches;

            if (touches.Length > 0 || Input.GetMouseButton(0))
            {

                float scaleDelta = 1;

                //mouse compatibility
                Vector2 pos0 = NormlizeScreenPosition(Input.mousePosition);

                if (touches.Length > 0)
                {
                    pos0 = NormlizeScreenPosition(touches[0].position);
                }

                if (touches.Length > 1)
                {
                    Vector2 pos1 = NormlizeScreenPosition(touches[1].position);

                    float dist = (pos0 - pos1).magnitude;

                    if (_isScaling)
                    {
                        scaleDelta = (Mathf.Abs(dist / _inititlaScaleDist));
                    }

                    _inititlaScaleDist = dist;

                    _isScaling = true;
                }
                else
                {
                    if (_isScaling)
                    {
                        _initialPanPosition = pos0;
                    }

                    _isScaling = false;
                }

                if (_isPaning)
                {
                    OnGestureDetected(pos0, pos0 - _initialPanPosition, scaleDelta);
                }

                _initialPanPosition = pos0;

                _isPaning = true;
            }
            else
            {
                _isPaning = false;
                _isScaling = false;
            }

        }

        private Vector2 NormlizeScreenPosition(Vector2 pixelPosition)
        {

            Vector2 relativePos = pixelPosition / _screenHeight;
            Vector2 normPos = new Vector2(relativePos.x - 0.5f * _screenRat, relativePos.y - 0.5f);
            return normPos;
        }
    }
}