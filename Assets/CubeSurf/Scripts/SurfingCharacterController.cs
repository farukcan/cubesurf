using System.Linq;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SurfingCharacterController : MonoBehaviour
{
    // TODO: Modify!
    public static SurfingCharacterController Instance; 
    public float speed = .01f;
    public float controlSpeed = .005f;

    [ReadOnly]
    public List<CubeController> cubes = new List<CubeController>();

    public enum CharacterAnimation {
        Walking,Falling
    }

    public CharacterAnimation currentAnimation = CharacterAnimation.Walking;

    public Animator animator;

    private float defaultY;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        Instance = this;
        defaultY = transform.position.y;
    }

    private void FixedUpdate()
    {
        if(currentAnimation==CharacterAnimation.Walking){
            transform.Translate(Vector3.forward * speed);
            transform.Translate(Vector3.right * controlSpeed * InputController.instance.delta.x / 10);
        }
    }

    private void Update(){
        Movement();
        Rule();
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
                    cubeToRemove.gameObject.SetActive(false);
                }
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
        }
    }
}
