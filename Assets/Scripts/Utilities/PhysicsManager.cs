using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PhysicsManager : MonoBehaviour
{
    private static PhysicsManager _instance;

    public static PhysicsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("PhysicsManager");
                go.AddComponent<PhysicsManager>();
                _instance = go.GetComponent<PhysicsManager>();

            }
            return _instance;
        }

    }

    public GameObject RayInteraction(Vector3 StartingPoint, Vector3 Direction, float Range)
    {
        GameObject _ret = null;
        RaycastHit Hit;
        if (Physics.Raycast(StartingPoint, Direction, out Hit, Range))
        {
            _ret = Hit.transform.gameObject;
        }
        return _ret;
    }

    public RaycastHit RayPoint(Vector3 StartingPoint, Vector3 Direction, float Range)
    {
        // Vector3 _ret = Vector3.zero;
        RaycastHit Hit;
        if (Physics.Raycast(StartingPoint, Direction, out Hit, Range))
        {
            //_ret = Hit.point;
            return Hit;
        }
        return Hit;
    }

    public void IgnoreLayerCollision(int Layer1, int Layer2)
    {
        Physics.IgnoreLayerCollision(Layer1, Layer2);
        // Debug.Log(LayerMask.LayerToName(8));
    }
    public void CharacterIgnoreCollision(GameObject CharacterObject, GameObject ObjectIgnore)
    {
        Collider _Charcol = CharacterObject.GetComponent<CharacterController>();
        Component[] _colBs = ObjectIgnore.transform.GetComponentsInChildren<BoxCollider>();
        Component[] _colSp = ObjectIgnore.transform.GetComponentsInChildren<SphereCollider>();
        foreach (BoxCollider _bc in _colBs)
        {
            // Debug.Log(_bc.name);
            Physics.IgnoreCollision(_Charcol, _bc, true);
        }
        foreach (SphereCollider _sp in _colSp)
        {
            //  Debug.Log(_sp.name);
            Physics.IgnoreCollision(_Charcol, _sp, true);
        }
    }
    public void DisableRigid(GameObject CharacterObject)
    {
        Component[] _rgs = CharacterObject.transform.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody _rg in _rgs)
        {
            _rg.isKinematic = true;
            _rg.velocity = Vector3.zero;
        }
    }

    public void EnableRigid(GameObject CharacterObject, float Force = 0)
    {
        Component[] _rgs = CharacterObject.transform.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody _rg in _rgs)
        {
            _rg.interpolation = RigidbodyInterpolation.Interpolate;
            //_rg.interpolation = RigidbodyInterpolation.Extrapolate;
            _rg.isKinematic = false;
            _rg.velocity = Vector3.zero;
        }

    }
    public bool IsMovingObject(GameObject CollidedObject)
    {
        bool _ret = false;
        if (CollidedObject.GetComponent<Rigidbody>())
        {
            Rigidbody _rig = CollidedObject.GetComponent<Rigidbody>();

            if (_rig.velocity.magnitude > 0.05)
            {
                // Debug.Log(_rig.velocity.magnitude);
                return true;
            };
        }
        return _ret;
    }


    public void AddForceExplosion(GameObject Target, Vector3 ExplosionSource, float Force, float Range)
    {
        // Target.GetComponent<Rigidbody>().AddExplosionForce(Force, ExplosionSource, Range);
        Rigidbody _rg = Target.GetComponent<Rigidbody>();
        if (_rg != null)
        {
            // _rg.velocity = DirectionForce * PushForce;
            _rg.AddExplosionForce(Force, ExplosionSource, Range, 0, ForceMode.Impulse);
        }

        RigidChildExplode(Target, ExplosionSource, Force, Range);
    }
    private void RigidChildExplode(GameObject Target, Vector3 ExplosionSource, float Force, float Range)
    {
        for (int i = 0; i < Target.transform.childCount; i++)
        {
            Rigidbody _rg = Target.transform.GetChild(i).GetComponent<Rigidbody>();
            if (_rg != null)
            {
                _rg.AddExplosionForce(Force, ExplosionSource, Range, 0, ForceMode.Impulse);
            }
            if (Target.transform.GetChild(i).transform.childCount >= 1)
            {
                RigidChildExplode(Target.transform.GetChild(i).gameObject, ExplosionSource, Force, Range);
            }
        }
    }

    private void AddForceCalculation(Rigidbody RigidBody, Vector3 Direction, float PushForce)
    {
        float _finalForce = PushForce * Time.timeScale;
        RigidBody.AddForce(Direction * PushForce, ForceMode.Impulse);
    }
    public void PushObject(GameObject CollisionRigid, Vector3 DirectionForce, float PushForce)
    {
        Rigidbody _rg = CollisionRigid.GetComponent<Rigidbody>();
        if (_rg != null)
        {
            // _rg.velocity = DirectionForce * PushForce;
            // float _finalForce = PushForce * Time.timeScale;
            // _rg.AddForce(DirectionForce * _finalForce, ForceMode.Impulse);
            AddForceCalculation(_rg, DirectionForce, PushForce);
        }

        RigidChildPush(CollisionRigid, DirectionForce, PushForce);
    }
    private void RigidChildPush(GameObject CollisionRigid, Vector3 DirectionForce, float PushForce)
    {
        for (int i = 0; i < CollisionRigid.transform.childCount; i++)
        {
            Rigidbody _rg = CollisionRigid.transform.GetChild(i).GetComponent<Rigidbody>();
            if (_rg != null)
            {
                //_rg.velocity = DirectionForce * PushForce;
                // float _finalForce = PushForce * Time.timeScale;
                // _rg.AddForce(DirectionForce * _finalForce, ForceMode.Impulse);
                AddForceCalculation(_rg, DirectionForce, PushForce);
                // _rg.AddExplosionForce(PushForce, DirectionForce, 100);
            }
            if (CollisionRigid.transform.GetChild(i).transform.childCount >= 1)
            {
                RigidChildPush(CollisionRigid.transform.GetChild(i).gameObject, DirectionForce, PushForce);
            }
        }
    }
    public List<Collider> ExplosionRange(Vector3 center, float radius)
    {
        List<Collider> _ret = new List<Collider>();
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (Collider hitCollider in hitColliders)
        {
            _ret.Add(hitCollider);
        }
        return _ret;
    }





}
