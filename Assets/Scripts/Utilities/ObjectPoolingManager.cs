using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{


    private static ObjectPoolingManager _instance;


    public Dictionary<string, Queue<GameObject>> poolDictionary;

    // private List<PoolProperties> Pools;
    public static ObjectPoolingManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("ObjectPoolingManager");
                go.AddComponent<ObjectPoolingManager>();
                _instance = go.GetComponent<ObjectPoolingManager>();
            }
            return _instance;
        }
    }

    public void InitilializePool(List<PoolProperties> PoolObject)
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (PoolProperties pool in PoolObject)
        {
            Queue<GameObject> _obj = new Queue<GameObject>();
            for (int i = 0; i < pool.Size; i++)
            {
                GameObject _g = Instantiate(pool.Prefab);
                _g.SetActive(false);
                _obj.Enqueue(_g);
            }
            poolDictionary.Add(pool.Tag, _obj);
        }
    }

    public GameObject SpawnFromPool(string Tag, Vector3 Position, Quaternion rotation)
    {
        if (poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("No pool for " + tag);
            return null;
        }

        GameObject ObjectToSpawn = null;
        // do
        // {
        //     ObjectToSpawn = poolDictionary[Tag].Dequeue();
        //     poolDictionary[Tag].Enqueue(ObjectToSpawn);
        //     if (ObjectToSpawn.activeInHierarchy)
        //     {
        //         ObjectToSpawn = null;
        //     }
        // } while (ObjectToSpawn == null);
        ObjectToSpawn = poolDictionary[Tag].Dequeue();
        ObjectToSpawn.transform.position = Position;
        ObjectToSpawn.transform.rotation = rotation;
        poolDictionary[Tag].Enqueue(ObjectToSpawn);
        ObjectToSpawn.SetActive(true);
        return ObjectToSpawn;
    }

}

[System.Serializable]
public class PoolProperties
{
    public string Tag;
    public GameObject Prefab;
    public int Size;
}
