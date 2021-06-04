using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxView : MonoBehaviour
{

    public Transform[] ParalaxElements;
    public float[] ParalaxScale;
    
    
    public void SetParalax(Vector2 offset, float scale)
    {

        for (int i = 0; i < ParalaxElements.Length; i++)
        {
            ParalaxElements[i].position = offset * ParalaxScale[i];
            float layerScale = (scale - 1) * ParalaxScale[i] + 1;
            ParalaxElements[i].localScale = new Vector3(layerScale, layerScale, 1) ;
        }
        
    }
}
