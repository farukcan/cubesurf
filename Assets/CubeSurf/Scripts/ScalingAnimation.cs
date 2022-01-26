using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingAnimation : MonoBehaviour
{
    public float delay = 1f;
    public float delayForTZero = 0.1f;
    private Vector3 defaultScale;

    private Run lerper;

    void Awake()
    {
        defaultScale = transform.localScale;
    }

    void OnEnable()
    {
        lerper = Run.Lerp(Time.time<1f?delayForTZero:delay,t=>{
            transform.localScale = Vector3.Lerp(Vector3.zero,defaultScale,t);
        });
    }

    void OnDisable(){
        lerper.Abort();
    }
}
