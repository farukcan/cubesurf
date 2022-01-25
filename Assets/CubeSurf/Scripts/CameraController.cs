﻿using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Active;
    public Transform target;
    public Transform lookAt;
    public bool autoOffset = true;
    [HideIf("autoOffset")]
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        Active = this;
        if(autoOffset){
            offset = target.position - transform.position;
        }    
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position,target.position - offset,Time.deltaTime);
        transform.LookAt(lookAt);
    }
}