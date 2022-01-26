using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class InfiniteLinePlacer : MonoBehaviour
{
    public float spacegap = 1;
    public float randomizationScale = 0;
    public bool randomizeX = true;
    public bool randomizeY = true;
    public bool randomizeZ = true;
    [Range(0,1)]
    public float probility = 1;
    public string poolName = "pool_name";
    public Transform origin;
    public float viewRange = 10;
    public int seed=13;
    public float dotRadius = 0.01f;

    [ReadOnly]
    public List<int> activeIndexList = new List<int>();
    [ReadOnly]
    public Dictionary<int,GameObject> activeObjects = new Dictionary<int,GameObject>();

    private float originMovementAmount = 0;
    private Vector3 preOriginPosition;
    private Pool ThePool => Pool.GetPool(poolName);

    // draw line gizmos to forward and back direction
    private void OnDrawGizmos()
    {
        // if scapegap is zero or less, return
        if (spacegap <= 0)
        {
            return;
        }
        Vector3 closestPoint = ClosestPointOnInfiniteLine();
        // draw the line
        Gizmos.color = Color.green;
        Gizmos.DrawLine(closestPoint, closestPoint + transform.forward * viewRange);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(closestPoint, closestPoint + transform.forward * -viewRange);
        // closest index        
        int index = GetClosestIndex(closestPoint);
        // draw closest index spheres
        for (int i = Mathf.FloorToInt(viewRange / spacegap * -1); i <= Mathf.CeilToInt(viewRange / spacegap); i++)
        {
            Gizmos.color = RandomizerBool(index+i) ? Color.blue : Color.grey;
            Gizmos.DrawSphere(GetPositionOfIndex(index+i), dotRadius);
        }
        // draw purple sphere for ClosestPointOnInfiniteLine
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(closestPoint, dotRadius);
        // draw line to origin
        Gizmos.color = Color.white;
        Gizmos.DrawLine(closestPoint, origin.position);
    }

    private int GetClosestIndex(Vector3 closestPoint){
        // find distance between closestPoint and position
        float distance = Vector3.Distance(closestPoint, transform.position);
        Vector3 f = transform.position + transform.forward;;
        Vector3 b = transform.position + transform.forward * -1;
        bool isOnBack = Vector3.Distance(closestPoint, b) < Vector3.Distance(closestPoint, f);
        // calculate index with spacegap of distance
        return (int)(distance / spacegap * (isOnBack ? -1 : 1));
    }

    private void Place(int index){
        GameObject go = ThePool.GetPooledObject();
        go.transform.position = GetPositionOfIndex(index);
        go.SetActive(true);
        activeObjects.Add(index, go);
    }

    private void Displace(int index){
        if(activeObjects.ContainsKey(index)){
            try
            {
                GameObject go = activeObjects[index];
                if(!go.CompareTag("DontDisplace")){
                    go.SetActive(false);
                    #if UNITY_EDITOR
                        if(UnityEditor.EditorApplication.isPlaying == false){
                            DestroyImmediate(go);
                        }
                    #endif
                }
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }
            activeObjects.Remove(index);
        }
    }

    [Button("Reflesh")]
    public void RefleshIndexList(){
        var currentList = new List<int>();
        int index = GetClosestIndex(ClosestPointOnInfiniteLine());
        for (int i = Mathf.FloorToInt(viewRange / spacegap * -1); i <= Mathf.CeilToInt(viewRange / spacegap); i++)
        {
            if (RandomizerBool(index + i))
            {
                currentList.Add(index + i);
            }
        }
        foreach (var item in activeIndexList.ToArray())
        {
            if (!currentList.Contains(item))
            {
                activeIndexList.Remove(item);
                Displace(item);
            }else{
                currentList.Remove(item);
            }
        }
        foreach (var item in currentList){
            activeIndexList.Add(item);
            Place(item);
        }
    }
    [Button("Clear Active Index List")]
    public void ClearActiveIndexList(){
        foreach (var item in activeIndexList)
        {
            Displace(item);
        }
        activeIndexList.Clear();
    }

    private Vector3 GetPositionOfIndex(int index){
        if(randomizationScale == 0){
            return transform.position + transform.forward * index * spacegap;
        }
        float noise = RandomizerFloat(index);
        Vector3 offset = noise * RandomizerOnSphere(index) * randomizationScale;
        return transform.position + transform.forward * spacegap * index + offset;
    }

    private Vector3 RandomizerOnSphere(int index){
        float x = randomizeX ? RandomizerFloat(index*2)*2f-1f : 0;
        float y = randomizeY ? RandomizerFloat(index*3)*2f-1f : 0;
        float z = randomizeZ ? RandomizerFloat(index*5)*2f-1f : 0;
        return (new Vector3(x, y, z)).normalized;
    }

    private float RandomizerFloat(int x)
    {
        Random.InitState(seed*x);
        return Random.value;
    }

    private bool RandomizerBool(int x)
    {
        if (probility == 1f)
        {
            return true;
        }
        float noise = RandomizerFloat(x);
        return noise < probility;
    }

    // calculate closest point on infinite line of forward direction for the position of the origin
    private Vector3 ClosestPointOnInfiniteLine()
    {
        // if origin is null return zero vector
        if (origin == null)
        {
            return Vector3.zero;
        }
        Vector3 lsh = origin.position - transform.position;
        float dotP = Vector3.Dot(lsh, transform.forward);
        return transform.position + transform.forward * dotP;
    }

    // Start is called before the first frame update
    void Start()
    {
        // if origin is not null then
        if (origin != null)
        {
            ClearActiveIndexList();
            // set preOriginPosition to origin position
            preOriginPosition = origin.position;
            // set originMovementAmount to origin position
            originMovementAmount = Vector3.Distance(origin.position, transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // when origin preoamount spacegap reflesh
        if (origin != null)
        {
            // set originMovementAmount to origin position
            originMovementAmount += Vector3.Distance(origin.position, preOriginPosition);
            // set preOriginPosition to origin position
            preOriginPosition = origin.position;
            // if originMovementAmount is greater than spacegap then
            if (originMovementAmount >= spacegap)
            {
                // reflesh index list
                RefleshIndexList();
                // set originMovementAmount to zero
                originMovementAmount = 0;
            }
        }
    }
}
