using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CubeController : MonoBehaviour
{
    public enum CubeColor { RED, GREEN, BLUE, YELLOW, PURPLE }
    public CubeColor color;
    public float height;
    
    // on enable set tag "Cube"
    private void OnEnable()
    {
        gameObject.tag = "Cube";
    }
}
