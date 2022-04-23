using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickObject : MonoBehaviour
{
    // Start is called before the first frame update
    private bool _isKicked = false;
    private Rigidbody rgBody;
    void Start()
    {
        PhysicsManager.Instance.DisableRigid(gameObject);
    }
    public void TriggerKicked()
    {
        if (rgBody == null)
        {
            rgBody = gameObject.GetComponent<Rigidbody>();
        }
        _cntmomentum = 0;
        _isKicked = true;

    }
    // Update is called once per frame
    Vector3 _limit = new Vector3(0, 0, 0.5f);
    float _cntmomentum = 0;
    void Update()
    {
        if (_isKicked)
        {
            if (rgBody.velocity.magnitude < 1.5f)
            {
                _cntmomentum += 1 * Time.deltaTime;
                if (_cntmomentum >= 0.5f)
                {
                    _isKicked = false;
                }
                // GameSettingManager.Instance.SetGlobalTime(0);
            }
            // Debug.Log("Stop " + rgBody.velocity.magnitude);
        }

    }

    private void RayPointVFX(Vector3 AimPosition, Vector3 Direction, float Range, string Tags, Transform Parent = null)
    {
        RaycastHit _v = PhysicsManager.Instance.RayPoint(AimPosition, Direction, Range);

        if (_v.collider != null)
        {
            Quaternion _rot = Quaternion.LookRotation(transform.position, Vector3.up);
            if (Parent != null)
            {
                GameObject _g = BasicUtilities.Instance.ObjectPool(Tags, _v.point, Quaternion.LookRotation(_v.normal));
                _g.transform.SetParent(Parent);
            }
            else
            {
                BasicUtilities.Instance.ShowVFX(Tags, _v.point, Quaternion.LookRotation(_v.normal));
            }

        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (_isKicked)
        {
            if (col.gameObject.transform.root.tag == "Enemy")
            {
                col.gameObject.transform.root.GetComponent<HealthSystem>().Shoot(1000);

                foreach (ContactPoint contact in col.contacts)
                {
                    BasicUtilities.Instance.ShowVFX("BloodSplat", contact.point, Quaternion.LookRotation(contact.normal));
                    // Debug.DrawRay(contact.point, contact.normal, Color.white);
                    PhysicsManager.Instance.PushObject(col.gameObject.transform.root.gameObject, rgBody.velocity, 10);
                }
            }
        }
    }

}
