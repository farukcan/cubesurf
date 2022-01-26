using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Run blinker;
    private void Start()
    {
        // set tag "Wall"
        gameObject.tag = "Wall";
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = new Color(
            meshRenderer.material.color.r,
            meshRenderer.material.color.g,
            meshRenderer.material.color.b
            ,0f
        );
    }

    public void Blink(){
        if(blinker == null){
            blinker = Run.Lerp(1,t=>{
                // set material color alpha to t
                meshRenderer.material.color = new Color(
                    meshRenderer.material.color.r,
                    meshRenderer.material.color.g,
                    meshRenderer.material.color.b
                    ,1f-t
                );
            });
        }
    } 
}
