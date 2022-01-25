using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IProgress : MonoBehaviour
{
    internal float progress=0f;

    public float GetProgress(LerpingType type)
    {
        if( type == LerpingType.SINUS)
        {
            return Mathf.Sin(Mathf.PI / 2 * progress);
        }
        if( type == LerpingType.SQUARE)
        {
            return progress * progress;
        }
        return progress;
    }
}

public enum LerpingType
{
    LINEAR,
    SINUS,
    SQUARE,
}
