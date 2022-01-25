using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SurfingCharacterController : MonoBehaviour
{
    // TODO: Modify!
    public static SurfingCharacterController Instance; 
    public float speed = .01f;

    public enum CharacterAnimation {
        Walking,Falling
    }

    public CharacterAnimation currentAnimation = CharacterAnimation.Walking;

    public Animator animator;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentAnimation==CharacterAnimation.Walking){
            transform.Translate(Vector3.forward * speed);
            transform.Translate(Vector3.right * speed * InputController.instance.delta.x / 10);
        }
    }

    private void SetAnimation(CharacterAnimation animation){
        if(animation!=currentAnimation){
            currentAnimation = animation;
            animator.Play(animation.ToString());
            // if(
            //         animation==CharacterAnimation.Falling
            //     ){
            //     // do something if needed
            // }
        }
    }
}
