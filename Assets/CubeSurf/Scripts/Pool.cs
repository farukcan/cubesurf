using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    // statics
    public static Dictionary<string, Pool> pools = new Dictionary<string, Pool>();

    // publics
    public string poolName;
    public GameObject[] prefabs;
    public int amount = 10;
    public bool expandable = true;
    public bool defaultActive = false;

    // internals
    internal GameObject[] objects;
    internal int lastIndex = 0;

    private void Awake()
    {
        RegisterPool();
        CreateInstances();
    }

    private void OnDestroy()
    {
        UnregisterPool();
    }

    private void CreateInstances()
    {
        amount = amount < 1 ? 0 : amount;
        objects = new GameObject[amount];
        if (amount == 0 )
        {
            Debug.Log("Pool is empty : " + poolName);
        }
        for (int i = 0; i < amount; i++)
        {
            objects[i] = Instantiate(prefabs[i%prefabs.Length], transform);
            objects[i].SetActive(defaultActive);
        }
    }

    private void RegisterPool()
    {
        if (pools.ContainsKey(poolName))
        {
            Debug.Log("Overriding pool : " + poolName);
        }
        pools[poolName] = this;
    }

    private void UnregisterPool()
    {
        pools.Remove(poolName);
    }

    public GameObject GetPooledObject()
    {
        int i = lastIndex;
        if(expandable && objects[i].activeSelf){
            int j = amount;
            do {
                j--;
                lastIndex = (lastIndex + 1) % amount;
            }while(objects[lastIndex].activeSelf && j>0);
            if(j==0){
                lastIndex = amount;
                List<GameObject> list = new List<GameObject>();
                list.AddRange(objects);
                GameObject additional = Instantiate(prefabs[amount%prefabs.Length], transform);
                additional.SetActive(false);
                list.Add(additional);
                objects = list.ToArray();
                amount++;
            }
            i=lastIndex;
        }else{
            lastIndex = (i + 1) % amount;
        }
        return objects[i];
    }

    public static Pool GetPool(string name)
    {
        return pools[name];
    }
}
