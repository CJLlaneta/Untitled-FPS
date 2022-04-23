using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Speed;
    private float BulletForce = 10;
    private float Damage = 1;


    private string BulletHitTag = "BulletHitDefault";
    private string BulletHole_Default = "BulletHole_Default";
    private string BloodSplat = "BloodSplat";
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MoveProjectile();
    }
    public void SetBulletProperties(float bulletforce, float damage, Vector3 TargetLocation)
    {
        BulletForce = bulletforce;
        Damage = damage;
        transform.LookAt(TargetLocation);
    }
    void MoveProjectile()
    {
        gameObject.transform.position += gameObject.transform.forward * Speed * Time.deltaTime;
    }


    void OnTriggerEnter(Collider other)
    {
        if (IsTarget(other.transform.root.tag))
        {

            FiredEnemy(other.transform.root.gameObject, Damage, BulletForce);
            Quaternion _rot = Quaternion.LookRotation(transform.position, Vector3.up);
            BasicUtilities.Instance.ShowVFX(BloodSplat, transform.position, _rot);

        }
        else
        {
            PushObject(other.gameObject, BulletForce);
            Quaternion _rot = Quaternion.LookRotation(transform.position, Vector3.up);
            BasicUtilities.Instance.ShowVFX(BulletHitTag, transform.position, _rot);
            RaycastHit _v = PhysicsManager.Instance.RayPoint(transform.position, transform.forward, 10);
            if (_v.collider != null)
            {
                BasicUtilities.Instance.ShowVFX(BulletHole_Default, transform.position, Quaternion.LookRotation(_v.normal), other.gameObject.transform);
            }
        }
        gameObject.SetActive(false);
    }

    void FiredEnemy(GameObject Enemy, float Damage, float Force)
    {
        Enemy.GetComponent<HealthSystem>().Shoot(Damage);
        CheckIfDead(Enemy, Force);
    }

    void CheckIfDead(GameObject Enemy, float Force)
    {
        if (!Enemy.GetComponent<HealthSystem>().enabled)
        {
            PhysicsManager.Instance.PushObject(Enemy, transform.forward, Force);
        }
    }
    void PushObject(GameObject Object, float Force = 10)
    {
        PhysicsManager.Instance.PushObject(Object, transform.forward, Force);
    }

    bool IsTarget(string Tag)
    {
        bool _ret = false;
        if (Tag == "Player" || Tag == "Enemy")
        {
            return true;
        }
        return _ret;
    }
}
