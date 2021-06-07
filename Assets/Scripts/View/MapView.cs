using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    public class MapView : MonoBehaviour
    {
        
        public ParticleSystem[] Particles;

        private ParalaxView _paralaxView;

        public float ScrollRange;
        public float ScaleMin;
        public float ScaleMax;

        public float OffsetOut;
        
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

        private void DoGesture(Vector2 endPointNormalized, Vector2 posDeltaNormalized, float scaleFactor)
        {
            _scale *= scaleFactor;
            if (_scale < ScaleMin)
            {
                _scale = ScaleMin;
            }
            else if (_scale > ScaleMax)
            {
                _scale = ScaleMax;
            }

            _paralaxOffset += posDeltaNormalized * ViewSettingsUtils.Instance.StageHeight;

            if (_paralaxOffset.magnitude > ScrollRange * _scale)
            {
                _paralaxOffset.Normalize();
                _paralaxOffset *= ScrollRange * _scale;
            }
            
            Vector2 endPoint = endPointNormalized * ViewSettingsUtils.Instance.StageHeight;
            Vector2 diff = _paralaxOffset - endPoint;
            diff *= scaleFactor;

            _paralaxOffset = endPoint + diff;

            _paralaxView.SetParalax(_paralaxOffset, _scale);
        }

        public void TransitionOut()
        {
            Vector2 outOffset = new Vector2(0, OffsetOut);

            float tt = ViewSettingsUtils.Instance.TransitionTime;
            

            LeanTween.value(gameObject, _paralaxOffset, outOffset, tt)
                .setOnUpdate((Vector2 v) => { _paralaxOffset = v; });
            LeanTween.value(gameObject, _scale, 1, tt).setOnUpdate((float v) =>
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
                float tt = ViewSettingsUtils.Instance.TransitionTime;


                _paralaxOffset = new Vector2(0, OffsetOut);
                ;
                _scale = 1;

                LeanTween.value(gameObject, _paralaxOffset, Vector2.zero, tt).setOnUpdate((Vector2 v) =>
                {
                    _paralaxOffset = v;
                    _paralaxView.SetParalax(_paralaxOffset, _scale);
                });

            }
            else
            {
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
}