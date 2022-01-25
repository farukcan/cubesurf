using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Note : MonoBehaviour {
    #if UNITY_EDITOR
            [Multiline]
            public string DeveloperDescription = "";
    #endif
}