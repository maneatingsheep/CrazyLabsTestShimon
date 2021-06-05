using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class RequestView : MonoBehaviour
    {
        public Image ColorImage;
        public Text CountText;

        public void SetType(Color color)
        {
            ColorImage.color = color;
        }

        public void SetCount(int count)
        {
            CountText.text = count.ToString();
        }
    }
}