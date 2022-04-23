using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTracer : MonoBehaviour
{
    // Start is called before the first frame update

    public float Speed = 300f;
    void MoveProjectile()
    {
        gameObject.transform.position += gameObject.transform.forward * Speed * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Bullet")
        {
            gameObject.SetActive(false);
        }

    }
    void Update()
    {
        MoveProjectile();
    }
}
