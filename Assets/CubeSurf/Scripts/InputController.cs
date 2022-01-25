using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

public class InputController : MonoBehaviour
{
    public static InputController instance;
    [ReadOnly]
    public bool interacting = false;
    private Camera cam;

    [ReadOnly]
    public static float lastInteraction = 0; 
    
    public Action OnInteractionStart;
    public Action OnInteractionStop;

    [ReadOnly,SerializeField]
    private bool inTab = true;

    internal Vector3 pre,delta;

    void Start()
    {
        instance = this;
    }

    void FixedUpdate()
    {
        // if(GameManager.instance!=null && GameManager.instance.status!=GameManager.GameStatus.PLAYING){
        //     return;
        // }

        if(interacting){
            if(Input.GetMouseButton(0)||Input.touchCount>0){
                InteractionStay();
                lastInteraction=Time.time;
            }else{
                InteractionStop();
            }
        }else{
            if(Input.GetMouseButton(0)||Input.touchCount>0){
                InteractionStart();
                lastInteraction=Time.time;
            }
        }

    }

    void InteractionStart(){
        if(EventSystem.current.IsPointerOverGameObject()){
            return;
        }
        cam = Camera.main;       
        inTab=true;
        interacting=true;
        pre = Input.mousePosition;
        delta = Vector3.zero;
        OnInteractionStart?.Invoke();
    }

    void InteractionStay(){
        delta = pre - Input.mousePosition;
        pre = Input.mousePosition;
        inTab=false;
    }

    public void InteractionStop(){
        interacting=false;
        inTab=false;
        OnInteractionStop?.Invoke();
        delta = Vector3.zero;
    }

}
