using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDropContainer : MonoBehaviour
{
    public WeaponSettings WeaponHold;
    private int ReserveAmmo = 0;
    private bool IsHold = false;

    public void MarkedWeapon(int reserveammo, bool isHold)
    {
        ReserveAmmo = reserveammo;
        IsHold = isHold;
    }
    public void WeaponEquip(GameObject Target)
    {
        _TargetPickUp = Target;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        //  gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<MeshCollider>().enabled = false;
    }

    GameObject _TargetPickUp = null;
    void Update()
    {
        if (_TargetPickUp != null)
        {
            gameObject.transform.LookAt(_TargetPickUp.transform);
            gameObject.transform.position += gameObject.transform.forward * 10 * Time.unscaledDeltaTime;
            if (IsWithinRange())
            {
                EquipWeapon();
            }
        }
    }
    void EquipWeapon()
    {
        IShooting IShooting = _TargetPickUp.GetComponent<IShooting>();
        IShooting.SetWeaponsProperties(WeaponHold);
        if (IsHold)
        {
            IShooting.OnUpdateAmmo(ReserveAmmo);
        }

        gameObject.SetActive(false);
    }
    bool IsWithinRange()
    {
        return AIManager.Instance.IsWithinRange(gameObject.transform.position, _TargetPickUp.transform.position, 1);
    }
}
