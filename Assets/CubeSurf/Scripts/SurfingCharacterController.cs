using System.Linq;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SurfingCharacterController : MonoBehaviour
{
    public static SurfingCharacterController Instance; 
    public float speed = .01f;
    public float controlSpeed = .005f;
    public float maxControlMovementAmount = .01f;

    public Camera cam;

    public float maxFOV = 90f;
    private float defaultFOV;

    [ReadOnly]
    public List<CubeController> cubes = new List<CubeController>();

    public enum CharacterAnimation {
        Walking,Falling
    }

    public CharacterAnimation currentAnimation = CharacterAnimation.Walking;

    public Animator animator;

    private float defaultY;
    private Vector3 lastPosition;
    private Rigidbody rigidBody;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        Instance = this;
        rigidBody =  GetComponent<Rigidbody>();
        defaultY = transform.position.y;
        defaultFOV = cam.fieldOfView;
    }
    private void FixedUpdate()
    {
        if(currentAnimation==CharacterAnimation.Walking){
            transform.Translate(Vector3.forward * speed);
            float delta = InputController.instance.delta.x;
            var controlVector = Vector3.right * controlSpeed * delta;
            if(controlVector.sqrMagnitude > maxControlMovementAmount*maxControlMovementAmount){
                controlVector = controlVector.normalized * maxControlMovementAmount;
                Debug.Log("Control Vector Mag:" + controlVector.sqrMagnitude);
            }
            transform.Translate(controlVector);
        }
    }

    private void Update(){
        Movement();
        Rule();
    }

    private void LateUpdate(){
        // Lerp camera fieldOfView
        float target = Mathf.Lerp(defaultFOV, maxFOV , 1f/30f*cubes.Count);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, target, Time.deltaTime);
    }

    private void Movement(){
        // if there is no cube set position.y to defaultY
        if(cubes.Count==0){
            transform.position = new Vector3(transform.position.x,defaultY,transform.position.z);
        }else{
            // if there is a cube set position.y to the cube's height
            // calculate the total height of cubes with Linq
            float totalHeight = cubes.Sum(cube => cube.height);
            float variableHeight = defaultY+totalHeight;
            transform.position = new Vector3(transform.position.x,variableHeight,transform.position.z);
            // set cube heights upper to lower
            foreach(var cube in cubes){
                variableHeight-=cube.height/2;
                cube.transform.position = new Vector3(transform.position.x,variableHeight,transform.position.z);
                variableHeight-=cube.height/2;
            }
        }
        lastPosition = transform.position;
    }

    private void Rule(){
        if(cubes.Count>=5){
            var colors = new CubeController.CubeColor[] {
                 CubeController.CubeColor.RED
                ,CubeController.CubeColor.GREEN
                ,CubeController.CubeColor.BLUE
                ,CubeController.CubeColor.YELLOW
                ,CubeController.CubeColor.PURPLE
            };
            if(colors.All(
                    color => cubes.Any(cube => cube.color == color)
                )){
                // remove from cubes all colors one by one
                foreach (var color in colors){
                    var cubeToRemove = cubes.Find(cube => cube.color == color);
                    cubes.Remove(cubeToRemove);
                    Run.Lerp(.9f,t=>{
                        cubeToRemove.transform.localScale = Vector3.Lerp(cubeToRemove.transform.localScale,Vector3.zero,t);
                    });
                    Run.After(1f,()=>{
                        cubeToRemove.gameObject.SetActive(false);
                    });
                }
                Haptic.MediumTaptic();
            }
        }
    }

    // OnTriggerEnter and tag is "Cube"
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Cube"){
            // set other tag DontDisplace
            other.gameObject.tag = "DontDisplace";
            // add cube to cubes list
            cubes.Add(other.gameObject.GetComponent<CubeController>());
            Haptic.LightTaptic();
        }
        // when collider with Wall blink the wall
        if(other.gameObject.tag=="Wall"){
            other.gameObject.GetComponent<WallController>().Blink();
        }
    }
    private void OnTriggerStay(Collider other) {
        // if other is "Wall" reverse InputController delta
        if(other.gameObject.tag=="Wall"){
            transform.position = lastPosition;
            InputController.instance.delta = Vector3.zero;
        }
    }
}
