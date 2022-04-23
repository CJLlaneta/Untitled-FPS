using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float Health = 1;

    void Start()
    {

    }


    public void Shoot(float Damage)
    {
        Health -= Damage;
        if (Health <= 0)
        {
            this.enabled = false;
        }
        ImHit = true;
    }

    public bool ImHit = false;
}
