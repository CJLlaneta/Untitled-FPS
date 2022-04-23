using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRandomizer : MonoBehaviour
{
    // Start is called before the first frame update

    public List<GameObject> GameObjects;
    void Start()
    {

    }

    void InitializeRandom()
    {
        int _index = 0;
        foreach (GameObject g in GameObjects)
        {
            g.SetActive(false);
        }
        _index = Random.Range(0, GameObjects.Count);
        GameObjects[_index].SetActive(true);
        //Debug.Log("Is running " + GameObjects[_index].name);
    }

    void OnEnable()
    {

        InitializeRandom();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
