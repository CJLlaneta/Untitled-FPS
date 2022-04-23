using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EjectorTrigger : MonoBehaviour
{
    public string PoolTag = "BulletEject";

    void OnEnable()
    {
        ShowVFX(PoolTag, transform.position, transform.rotation);
    }

    void ShowVFX(string MuzzleTag, Vector3 Position, Quaternion Direction)
    {
        GameObject _g = ObjectPoolingManager.Instance.SpawnFromPool(MuzzleTag, Position, Direction);

    }
}
