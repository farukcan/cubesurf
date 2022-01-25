using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    // TODO: Modify!
    public static CharacterController Instance; 
    [ReadOnly]
    public float speed = .01f;

    public enum CharacterAnimation {
        Walking,Falling
    }

    public CharacterAnimation currentAnimation = CharacterAnimation.Walking;
    private UnityEngine.CharacterController controller;

    public Animator animator;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        controller = GetComponent<UnityEngine.CharacterController>();
    }


    private void OnControllerColliderHit(ControllerColliderHit hit) {
        // SetAnimation(CharacterAnimation.Hit);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentAnimation==CharacterAnimation.Walking){
            controller.Move(Vector3.forward * speed);
            controller.Move(Vector3.right * speed * InputController.instance.delta.x / 10);
        }
        controller.Move(Physics.gravity * Time.deltaTime);
        if(!controller.isGrounded){
            SetAnimation(CharacterAnimation.Falling);
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
