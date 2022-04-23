using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform Cam;
    [SerializeField] private float shakeDuraction = 0f;

    [SerializeField] private float shakeAmount = 0.3f;
    [SerializeField] private float decreaseAmount = 1.0f;

    void Start()
    {

    }

    Vector3 originalPos;
    float _shakeDuraction = 0;
    bool _Shake = false;
    public void ShakeMe()
    {
        originalPos = Cam.localPosition;
        _shakeDuraction = shakeDuraction;
        _Shake = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (_Shake)
        {
            if (_shakeDuraction > 0)
            {
                Cam.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
                _shakeDuraction -= Time.deltaTime * decreaseAmount;
            }
            else
            {
                _shakeDuraction = 0;
                Cam.localPosition = originalPos;
                _Shake = false;
            }
        }
    }
}
