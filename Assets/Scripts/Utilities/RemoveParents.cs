using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveParents : MonoBehaviour
{

    void Start()
    {

        Transform[] t = gameObject.transform.GetComponentsInChildren<Transform>();
        foreach (Transform child in t)
        {
            child.parent = null;
        }

    }

    void Update()
    {



    }

}
