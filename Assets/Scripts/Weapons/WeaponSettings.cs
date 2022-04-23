using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptable_Objects/Weapons/Settings")]
public class WeaponSettings : ScriptableObject
{
    // public GameObject Prefab { get { return _Prefab; } private set { _Prefab = value; } }
    // [SerializeField] private GameObject _Prefab;


    public GameObject FPSPrefab { get { return _FPSPrefab; } private set { _FPSPrefab = value; } }
    [SerializeField] private GameObject _FPSPrefab;
    public GameObject TPSPrefab { get { return _TPSPrefab; } private set { _TPSPrefab = value; } }
    [SerializeField] private GameObject _TPSPrefab;

    public GameObject WeaponDrop { get { return _WeaponDropb; } private set { _WeaponDropb = value; } }
    [SerializeField] private GameObject _WeaponDropb;

    public float WeaponRange { get { return _WeaponRange; } private set { _WeaponRange = value; } }
    [SerializeField] private float _WeaponRange = 50f;

    public float Damage { get { return _Damage; } private set { _Damage = value; } }
    [SerializeField] private float _Damage = 1f;

    public int Bullets { get { return _Bullets; } private set { _Bullets = value; } }
    [SerializeField] private int _Bullets = 15;
    public int Shots { get { return _Shots; } private set { _Shots = value; } }
    [SerializeField] private int _Shots = 1;

    public float RateOfFire { get { return _RateOfFire; } private set { _RateOfFire = value; } }
    [SerializeField] private float _RateOfFire = 0.5f;

    public float SpreadShot { get { return _SpreadShot; } set { _SpreadShot = value; } }
    [SerializeField] private float _SpreadShot = 0f;

    public float SpreadRecoverRate { get { return _SpreadRecoverRate; } private set { _SpreadRecoverRate = value; } }
    [SerializeField] private float _SpreadRecoverRate = 0.24f;

    public float GunForce { get { return _GunForce; } private set { _GunForce = value; } }
    [SerializeField] private float _GunForce = 5f;

    public float ReloadTime { get { return _ReloadTime; } private set { _ReloadTime = value; } }
    [SerializeField] private float _ReloadTime = 0.5f;

    public float EjectionDelay { get { return _EjectionDelay; } private set { _EjectionDelay = value; } }
    [SerializeField] private float _EjectionDelay = 0.1f;

    public string GunMuzzleTag { get { return _GunMuzzleTag; } private set { _GunMuzzleTag = value; } }
    [SerializeField] private string _GunMuzzleTag = "MuzzleDefault";

    public string BulletCaseTag { get { return _BulletCaseTag; } private set { _BulletCaseTag = value; } }
    [SerializeField] private string _BulletCaseTag = "SmallBulletCase";


    public AudioClip GunFireSound { get { return _GunFireSound; } private set { _GunFireSound = value; } }
    [SerializeField] private AudioClip _GunFireSound;

    public AudioClip GunDrySound { get { return _GunDrySound; } private set { _GunDrySound = value; } }
    [SerializeField] private AudioClip _GunDrySound;
    public AudioClip CockSound { get { return _CockSound; } private set { _CockSound = value; } }
    [SerializeField] private AudioClip _CockSound;
    public float CockTimePlayer { get { return _CockTimePlayer; } private set { _CockTimePlayer = value; } }
    [SerializeField] private float _CockTimePlayer = 0.3f;


    public RuntimeAnimatorController AnimatorController_FPS { get { return _AnimatorController_FPS; } private set { _AnimatorController_FPS = value; } }
    [SerializeField] private RuntimeAnimatorController _AnimatorController_FPS;

    public RuntimeAnimatorController AnimatorController_TPS { get { return _AnimatorController_TPS; } private set { _AnimatorController_TPS = value; } }
    [SerializeField] private RuntimeAnimatorController _AnimatorController_TPS;


    //0 pistol 1 rifle
    public int ConstraintType { get { return _ConstraintType; } private set { _ConstraintType = value; } }
    [SerializeField] private int _ConstraintType = 0;



}
