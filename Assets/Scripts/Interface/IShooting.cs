using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal interface IShooting
{
    void OnShoot(Vector3 AimPosition, Vector3 Direction, float Range, float Damage, float Force, string MuzzleTag);
    void OnReload();
    void OnMelee();
    void OnThrow();
    void OnMove();
    void OnIdle();
    void OnDeath();

    bool CanShoot();

    void OnUpdateAmmo(int ReserveAmmo);

    Transform GunPoint();
    void OnKick(Vector3 AimPosition, Vector3 Direction, float Range, float Damage, float Force);
    void SetWeaponsProperties(WeaponSettings Weapon);

}
