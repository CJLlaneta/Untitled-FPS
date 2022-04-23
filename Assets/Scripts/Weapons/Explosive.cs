using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    [SerializeField] private GameObject ExplosiveVFX;
    [SerializeField] private float ExplosionRange = 5;
    [SerializeField] private float ExplosionForce = 25;
    [SerializeField] private Vector3 ExplosionPositionOffset = Vector3.zero;
    private string BodyFire = "BodyFire";
    void Start()
    {
        ExplosiveVFX.SetActive(false);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 _pos = transform.position;
        _pos.x += ExplosionPositionOffset.x;
        _pos.y += ExplosionPositionOffset.y;
        _pos.z += ExplosionPositionOffset.z;
        Gizmos.DrawWireSphere(_pos, ExplosionRange);

    }
    bool IsTargetSpotted(GameObject Target)
    {
        return AIManager.Instance.IsWithinSpotRange(Target, transform.position, ExplosionRange);
    }

    bool HasBlocked(GameObject Target)
    {
        Vector3 _pos = transform.position;
        _pos.x += ExplosionPositionOffset.x;
        _pos.y += ExplosionPositionOffset.y;
        _pos.z += ExplosionPositionOffset.z;
        Vector3 _dir = (Target.transform.position - _pos).normalized;
        RaycastHit _ray = PhysicsManager.Instance.RayPoint(_pos, _dir, ExplosionRange);




        if (_ray.collider != null)
        {
            // Debug.Log(_ray.transform.root.tag);
            //Debug.DrawRay(_pos, _dir, Color.red, 1000);
            if (_ray.transform.root.tag == "Area")
            {
                return true;
            }
        }
        return false;
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
    List<GameObject> _TargetHit = new List<GameObject>();
    public void TriggerExplosion(GameObject Exception = null)
    {
        List<Collider> _cols = PhysicsManager.Instance.ExplosionRange(transform.position, ExplosionRange);
        foreach (Collider _c in _cols)
        {

            // if (IsTargetSpotted(_c.gameObject) || IsTargetSpotted(_c.gameObject.transform.root.gameObject))
            // {
            //     //Debug.Log(_c.gameObject.transform.root.tag);
            //     if (IsTarget(_c.gameObject.transform.root.tag))
            //     {
            //         FiredEnemy(_c.gameObject.transform.root.gameObject);
            //     }
            //     else
            //     {
            //         AddExplosionForce(_c.gameObject);
            //     }
            // }
            // if (IsTargetSpotted(_c.gameObject) || IsTargetSpotted(_c.gameObject.transform.root.gameObject))
            // {


            if (IsTarget(_c.gameObject.transform.root.tag))
            {
                if (!_TargetHit.Contains(_c.gameObject.transform.root.gameObject) &&
                !HasBlocked(_c.gameObject))
                {
                    FiredEnemy(_c.gameObject.transform.root.gameObject);
                    _TargetHit.Add(_c.gameObject.transform.root.gameObject);
                    if (_c.gameObject.transform.root.tag != "Player")
                    {
                        BasicUtilities.Instance.ShowVFX(BodyFire, _c.gameObject.transform.position, _c.gameObject.transform.rotation, _c.gameObject.transform);
                    }
                }
                //Debug.Log("test");
                //FiredEnemy(_c.gameObject.transform.root.gameObject);
            }
            else
            {
                //  Debug.Log(_c.gameObject.name);
                if (!_TargetHit.Contains(_c.gameObject))
                {
                    if (Exception != _c.gameObject)
                    {
                        ExplosionChain(_c.gameObject);
                    }
                    else
                    {
                        AddExplosionForce(_c.gameObject);
                        _TargetHit.Add(_c.gameObject);
                    }

                }

            }

            //}

        }
        ExplosiveVFX.transform.parent = null;
        ExplosiveVFX.SetActive(true);
        gameObject.SetActive(false);
        // StartCoroutine(DisableMe());
    }
    void ExplosionChain(GameObject Object)
    {
        if (Object != gameObject && Object.transform.tag == "Explosive")
        {

            if (Object.GetComponent<Explosive>())
            {
                // Debug.Log(gameObject);
                Object.GetComponent<Explosive>().TriggerExplosion(gameObject);
            }
        }
    }
    void FiredEnemy(GameObject Character)
    {

        Character.GetComponent<HealthSystem>().Shoot(10000);
        BasicUtilities.Instance.ShowVFX("BloodSplat", Character.transform.position, Character.transform.rotation);
        // CheckIfDead(Character);
        //StartCoroutine(PushThis(Character));
        AddExplosionForce(Character);

    }
    IEnumerator DisableMe()
    {
        // do
        // {
        //     yield return null;
        // } while (_TriggerFirst);
        yield return new WaitForSeconds(0.05f);

        gameObject.SetActive(false);
    }
    //bool _TriggerFirst = false;
    IEnumerator PushThis(GameObject Enemy)
    {
        //_TriggerFirst = true;
        yield return new WaitForSeconds(0.01f);

        AddExplosionForce(Enemy);
        // _TriggerFirst = false;
    }
    void CheckIfDead(GameObject Enemy)
    {
        if (!Enemy.GetComponent<HealthSystem>().enabled)
        {
            StartCoroutine(PushThis(Enemy));
        }

    }


    void AddExplosionForce(GameObject Object)
    {
        // PhysicsManager.Instance.PushObject(Object, Object.transform.position - transform.position, ExplosionForce);
        if (Object != gameObject)
        {
            PhysicsManager.Instance.AddForceExplosion(Object, transform.position, ExplosionForce, ExplosionRange);
        }


    }
}
