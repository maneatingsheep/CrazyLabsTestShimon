using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowView : MonoBehaviour
{
    public GameObject Message;

    private void OnMouseDown()
    {
        Message.SetActive(true);
        LeanTween.delayedCall(3, () => { Message.SetActive(false); });
    }
}
