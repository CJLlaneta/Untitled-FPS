using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioChecker : MonoBehaviour
{
    // Start is called before the first frame update

    private AudioSource AudioSource;
    void OnEnable()
    {
        if (AudioSource == null)
        {
            AudioSource = gameObject.GetComponent<AudioSource>();
        }
        AudioSource.pitch = Time.timeScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (AudioSource != null)
        {
            AudioSource.pitch = Time.timeScale;
        }
    }
}
