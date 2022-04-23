using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObject : MonoBehaviour
{
    // Start is called before the first frame update
    public float Life = 3f;
    public bool RemoveParent = true;

    void Start()
    {

    }
    void OnEnable()
    {
        _cnt = 0;
    }
    // Update is called once per frame
    float _cnt = 0;
    void Update()
    {
        _cnt += 1 * Time.deltaTime;
        if (_cnt >= Life)
        {
            _cnt = 0;
            if (RemoveParent)
            {
                gameObject.transform.parent = null;
            }

            gameObject.SetActive(false);
        }
    }
}
