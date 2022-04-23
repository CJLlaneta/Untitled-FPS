using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightingBehavior : MonoBehaviour, IShooting
{


    [SerializeField] private Transform GunHand;
    [SerializeField] private bool IsAI = false;
    [SerializeField] private Animator Animator;

    [SerializeField] private Text txtBullet;
    // [SerializeField] private GameObject ShootingVFX;

    private Transform MuzzlePoint;
    private GameObject GunBarel;
    private Transform BulletEject;
    private GameObject GunPrefab;
    private GameObject DropPrefab;

    private string BulletHitTag = "BulletHitDefault";
    private string BulletHole_Default = "BulletHole_Default";
    private string BloodSplat = "BloodSplat";

    private string BulletCaseTag = "BulletCase";
    private string BulletTracer = "BulletTracer";
    private string BloodKick = "BloodKick";
    private string MissKick = "MissKick";


    private WeaponSettings Weapon;
    private float _SpreadValue = 0;

    private int ReserveBullet = 0;
    private List<GameObject> Kills = new List<GameObject>();

    private AudioSource AudioSource;
    public void OnShoot(Vector3 AimPosition, Vector3 Direction, float Range, float Damage, float Force, string MuzzleTag)
    {
        if (_CanFire && !_IsReloading)
        {
            if (ReserveBullet > 0)
            {
                RayCastShooting(AimPosition, Direction, Range, Damage, Force, MuzzleTag);
                ShootAnimation();
                OnReload();
                if (BulletEject != null && !IsAI)
                {
                    StartCoroutine(EjectionDelay());
                }
                if (GunBarel != null)
                {
                    GunBarel.SetActive(false);
                    GunBarel.SetActive(true);
                }
            }
            else
            {
                AudioManager.Instance.PlaySoundOneShot(AudioSource, Weapon.GunDrySound);
                StartCoroutine(FireRateCounter());
            }


            //ProjectileShooting(AimPosition, Direction, Range, Damage, Force, MuzzleTag);
        }

    }

    public bool CanShoot()
    {
        return _CanFire && ReserveBullet > 0;
    }


    public void OnDeath()
    {
        AnimationManager.Instance.StopAnimation(Animator);
        WeaponDrop();
    }
    public void OnMove()
    {
        AnimationManager.Instance.SetAnimationBoolean(Animator, "IsRunning", true);
    }
    public Transform GunPoint()
    {
        return MuzzlePoint;
    }
    public void OnIdle()
    {
        AnimationManager.Instance.SetAnimationBoolean(Animator, "IsRunning", false);
    }
    private void ShootAnimation()
    {
        if (!IsAI)
        {

            AnimationManager.Instance.PlayClip(Animator, "Fire");

            //GunPrefab.transform.position = GunHand.transform.position;
            // StartCoroutine(EjectionDelay());
        }
        // else
        // {
        //     // ShowVFX(BulletCaseTag, BulletEject.position, BulletEject.rotation);
        // }
        //AnimationManager.Instance.PlayClip(Animator, "Fire");
    }
    IEnumerator EjectionDelay()
    {
        yield return new WaitForSeconds(Weapon.EjectionDelay);
        // GameObject _g = BasicUtilities.Instance.ObjectPool(Weapon.BulletCaseTag, BulletEject.position, BulletEject.rotation);
        //ShowVFX(BulletCaseTag, BulletEject.position, BulletEject.rotation);
        BasicUtilities.Instance.ShowVFX(Weapon.BulletCaseTag, BulletEject.position, BulletEject.rotation);
        // PhysicsManager.Instance.PushObject(_g, BulletEject.forward, 5);
    }

    private void ProjectileShooting(Vector3 AimPosition, Vector3 Direction, float Range, float Damage, float Force, string MuzzleTag)
    {
        ShowVFX(MuzzleTag, MuzzlePoint.position, MuzzlePoint.rotation);
        // ShowVFX(BulletCaseTag, BulletEject.position, BulletEject.rotation);
        StartCoroutine(FireRateCounter());
        Vector3 _Direction = SpreadShooter(Direction);
        RaycastHit _v = PhysicsManager.Instance.RayPoint(AimPosition, _Direction, Range);
        if (_v.collider != null)
        {

            GameObject _g = BasicUtilities.Instance.ObjectPool("BulletProjectile", MuzzlePoint.position, MuzzlePoint.rotation);
            _g.GetComponent<Projectile>().SetBulletProperties(Force, Damage, _v.point);
        }
        else
        {

        }

    }

    void BulletUpdate()
    {
        ReserveBullet -= 1;
        if (ReserveBullet < 0)
        {
            ReserveBullet = 0;
        }
        txtBullet.text = ReserveBullet.ToString("000");
    }
    // IEnumerator ShootingVFXCast()
    // {
    //     ShootingVFX.SetActive(true);
    //     yield return new WaitForSeconds(0.1f);
    //     ShootingVFX.SetActive(false);

    // }
    private void FiringSound()
    {
        AudioManager.Instance.PlaySoundOneShot(AudioSource, Weapon.GunFireSound);
        if (Weapon.CockSound != null)
        {
            StartCoroutine(CockTrigger());
        }
    }
    IEnumerator CockTrigger()
    {
        yield return new WaitForSeconds(Weapon.CockTimePlayer);
        AudioManager.Instance.PlaySoundOneShot(AudioSource, Weapon.CockSound);
    }
    private void RayCastShooting(Vector3 AimPosition, Vector3 Direction, float Range, float Damage, float Force, string MuzzleTag)
    {
        // if (ShootingVFX != null)
        // {
        //     StartCoroutine(ShootingVFXCast());
        // }
        ShowVFX(Weapon.GunMuzzleTag, MuzzlePoint.position, MuzzlePoint.rotation);
        FiringSound();
        // if (IsAI)
        // {
        //     ShowVFX(BulletCaseTag, BulletEject.position, BulletEject.rotation);
        // }

        StartCoroutine(FireRateCounter());
        for (int a = 1; a <= Weapon.Shots; a++)
        {
            Vector3 _Direction = SpreadShooter(Direction);
            GameObject _g = PhysicsManager.Instance.RayInteraction(AimPosition, _Direction, Range); //Range
            Quaternion _rot = Quaternion.LookRotation(_Direction, Vector3.up);
            BasicUtilities.Instance.ShowVFX(BulletTracer, MuzzlePoint.position, _rot);
            //RayPointVFX(AimPosition, _Direction, Range, BulletTracer);
            _SpreadValue = Weapon.SpreadShot;
            if (_g != null)
            {

                if (IsTarget(_g.transform.root.tag))
                {

                    FiredEnemy(_g.transform.root.gameObject, Damage, Force, _g);
                    if (_g.transform.root.tag != "Player")
                    {
                        RayPointVFX(AimPosition, _Direction, Range, BloodSplat);
                        //   Debug.Log("Hit");
                    }

                    // RaycastHit _v = PhysicsManager.Instance.RayPoint(AimPosition, _Direction, Range);
                    // if (_v.collider != null)
                    // {
                    //     Quaternion _rot = Quaternion.LookRotation(transform.position, Vector3.up);
                    //     ShowVFX(BloodSplat, _v.point, _rot);
                    // }

                }
                else
                {
                    // Debug.Log(_g.transform.name);

                    // if (_g.tag != "Weapon")
                    // {
                    //     if (_g.tag != "HeavyObject")
                    //     {
                    //         PushObject(_g, Force);
                    //     }
                    //     RayPointVFX(AimPosition, _Direction, Range, BulletHitTag);
                    //     RayPointVFX(AimPosition, _Direction, Range, BulletHole_Default, _g.transform);
                    //     CheckIfExplosive(_g);
                    // }

                    if (_g.tag != "HeavyObject")
                    {
                        PushObject(_g, Force);
                    }
                    RayPointVFX(AimPosition, _Direction, Range, BulletHitTag);
                    RayPointVFX(AimPosition, _Direction, Range, BulletHole_Default, _g.transform);
                    CheckIfExplosive(_g);


                    // RaycastHit _v = PhysicsManager.Instance.RayPoint(AimPosition, _Direction, Range);
                    // if (_v.collider != null)
                    // {
                    //     Quaternion _rot = Quaternion.LookRotation(transform.position, Vector3.up);
                    //     ShowVFX(BulletHitTag, _v.point, _rot);
                    //     ShowVFX(BulletHole_Default, _v.point, Quaternion.LookRotation(_v.normal), _g.transform);
                    //     //BulletHole_Metal
                    // }

                }
            }
        }

    }
    private void CheckIfExplosive(GameObject Object)
    {
        if (Object.transform.tag == "Explosive")
        {

            Object.GetComponent<Explosive>().TriggerExplosion();
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
    private Vector3 SpreadShooter(Vector3 Direction)
    {
        Vector3 _ret = Direction;
        _ret.x += Random.Range(-_SpreadValue, _SpreadValue);
        _ret.y += Random.Range(-_SpreadValue, _SpreadValue);
        return _ret;
    }
    void WeaponDrop()
    {
        GunPrefab.SetActive(false);
        // Debug.Log("I Drop");
        DropPrefab.transform.position = GunPrefab.transform.position;
        DropPrefab.transform.rotation = GunPrefab.transform.rotation;
        StopAllCoroutines();
        _CanFire = true;
        _IsReloading = false;
        if (IsAI)
        {
            ReserveBullet = Weapon.Bullets;
        }
        DropPrefab.GetComponent<WeaponDropContainer>().MarkedWeapon(ReserveBullet, true);
        DropPrefab.SetActive(true);
    }
    public void OnUpdateAmmo(int Ammo)
    {
        ReserveBullet = Ammo;
        txtBullet.text = ReserveBullet.ToString("000");
    }
    public void SetWeaponsProperties(WeaponSettings WeaponSetting)
    {
        //float rateofFire, float reloadTime, GameObject gunPrefab
        if (GunPrefab != null)
        {
            WeaponDrop();
            // GunPrefab.SetActive(false);
        }

        Weapon = WeaponSetting;
        _CanFire = true;

        // if (ShootingVFX != null)
        // {
        //     ShootingVFX.SetActive(false);
        // }

        // if (DropPrefab != null)
        // {
        //     DropPrefab.SetActive(false);
        // }

        ReserveBullet = Weapon.Bullets;

        //DropPrefab
        DropPrefab = Instantiate(Weapon.WeaponDrop);
        DropPrefab.SetActive(false);
        if (IsAI)
        {
            GunPrefab = Instantiate(Weapon.TPSPrefab);
        }
        else
        {
            GunPrefab = Instantiate(Weapon.FPSPrefab);
        }

        GunPrefab.transform.position = GunHand.position;
        GunPrefab.transform.rotation = GunHand.rotation;
        GunPrefab.transform.SetParent(GunHand);
        if (AudioSource == null)
        {
            AudioSource = GunHand.GetComponent<AudioSource>();
        }


        foreach (Transform t in GunPrefab.GetComponentsInChildren<Transform>())
        {
            if (t.CompareTag("MuzzlePoint"))
            {
                MuzzlePoint = t;
                //break;
            }
            if (t.CompareTag("BulletEject"))
            {
                BulletEject = t;
                //break;
            }
            if (t.CompareTag("GunBarrel"))
            {
                GunBarel = t.gameObject;
                //                Debug.Log(GunBarel.name);
                if (GunBarel != null)
                {
                    GunBarel.SetActive(false);
                }
                //break;
            }

        }
        if (WeaponSetting.AnimatorController_FPS != null && !IsAI)
        {
            // Animator = GunPrefab.GetComponent<Animator>();
            // Animator.runtimeAnimatorController = WeaponSetting.AnimatorController_FPS;
            foreach (Transform t in GunPrefab.GetComponentsInChildren<Transform>())
            {
                if (t.CompareTag("FPSArms"))
                {
                    // MuzzlePoint = t;
                    Animator = t.gameObject.GetComponent<Animator>();
                    Animator.runtimeAnimatorController = WeaponSetting.AnimatorController_FPS;
                    break;
                }

            }
        }
        else
        {
            Animator.runtimeAnimatorController = Weapon.AnimatorController_TPS;
        }
        if (txtBullet != null)
        {
            txtBullet.text = ReserveBullet.ToString("000");
        }
    }


    public void OnReload()
    {
        if (IsAI)
        {
            //Debug.Log("test " + _ReserveBullet);
            // if (ReserveBullet <= 0 && !_IsReloading)
            // {
            //     //Debug.Log("test " + _ReserveBullet + " " + _CanFire);
            //     StartCoroutine(ReloadCounter());
            // }
            // else
            // {
            //     ReserveBullet -= 1;
            // }
        }
        else
        {
            BulletUpdate();
        }


    }

    IEnumerator ReloadCounter()
    {
        _IsReloading = true;

        yield return new WaitForSeconds(Weapon.ReloadTime);
        ReserveBullet = Weapon.Bullets;
        _IsReloading = false;
    }
    public void OnMelee()
    {

    }

    public void OnKick(Vector3 AimPosition, Vector3 Direction, float Range, float Damage, float KickForce)
    {
        GameObject _g = PhysicsManager.Instance.RayInteraction(AimPosition, Direction, Range);
        if (_g != null)
        {
            if (IsTarget(_g.transform.root.tag))
            {
                FiredEnemy(_g.transform.root.gameObject, Damage, KickForce, _g.transform.root.gameObject);
                RayPointVFX(AimPosition, Direction, Range, BloodKick);
            }
            else
            {
                PushObject(_g, KickForce);
                RayPointVFX(AimPosition, Direction, Range, MissKick);
                if (_g.tag == "HeavyObject")
                {
                    //                    Debug.Log(_g.transform.name);
                    _g.GetComponent<KickObject>().TriggerKicked();
                }
            }
        }
    }

    public void OnThrow()
    {

    }

    bool _CanFire = true;
    bool _IsReloading = false;
    IEnumerator FireRateCounter()
    {
        _CanFire = false;
        yield return new WaitForSeconds(Weapon.RateOfFire);
        _CanFire = true;
    }
    void ShowVFX(string MuzzleTag, Vector3 Position, Quaternion Direction, Transform Parent = null)
    {
        GameObject _g = ObjectPoolingManager.Instance.SpawnFromPool(MuzzleTag, Position, Direction);
        if (Parent != null)
        {
            _g.transform.SetParent(Parent);
        }
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
    void Update()
    {
        if (Weapon != null)
        {
            EaseRecoil();
        }
    }

    void EaseRecoil()
    {
        if (_SpreadValue > 0)
        {
            _SpreadValue -= Weapon.SpreadRecoverRate * Time.deltaTime;
            if (_SpreadValue <= 0)
            {
                _SpreadValue = 0;
            }
        }
    }
    void PushObject(GameObject Object, float Force = 10)
    {
        PhysicsManager.Instance.EnableRigid(Object);
        PhysicsManager.Instance.PushObject(Object, transform.forward, Force);
    }

    void FiredEnemy(GameObject Enemy, float Damage, float Force, GameObject PartHits)
    {
        Enemy.GetComponent<HealthSystem>().Shoot(Damage);
        CheckIfDead(Enemy, Force, PartHits);
    }
    IEnumerator PushThis(GameObject Enemy, float Force)
    {


        // if (Enemy.tag != "Player")
        // {
        //     float _timescale = GameSettingManager.Instance.GetTimeScale();
        //     GameSettingManager.Instance.SetGlobalTime(1);
        //     yield return new WaitForSeconds(0.01f);
        //     PhysicsManager.Instance.PushObject(Enemy, transform.forward, Force);
        //     GameSettingManager.Instance.SetGlobalTime(0.1f);
        // }
        // else
        // {
        //     yield return new WaitForSeconds(0.01f);
        //     PhysicsManager.Instance.PushObject(Enemy, transform.forward, Force);
        // }
        yield return new WaitForSeconds(0.01f);
        PhysicsManager.Instance.PushObject(Enemy, transform.forward, Force);

    }
    IEnumerator SlowMotionFlix()
    {
        // GameSettingManager.Instance.SetGlobalTime(PlayerAction.AlternateTime);
        // yield return new WaitForSecondsRealtime(0.2f);
        // GameSettingManager.Instance.SetGlobalTime(0);
        // yield return new WaitForSecondsRealtime(0.3f);
        // GameSettingManager.Instance.SetGlobalTime(PlayerAction.BulletTime);
        if (PlayerAction.isSlowMo)
        {
            GameSettingManager.Instance.SetGlobalTime(PlayerAction.AlternateTime);
        }

        yield return new WaitForSecondsRealtime(0.4f);
        if (PlayerAction.isSlowMo)
        {
            GameSettingManager.Instance.SetGlobalTime(PlayerAction.BulletTime);
        }
        else
        {
            GameSettingManager.Instance.SetGlobalTime(1);
        }

    }
    void SlowMoEffect()
    {
        if (gameObject.transform.root.tag == "Player")
        {
            // if (PlayerAction.isSlowMo)
            // {
            //     StopCoroutine(SlowMotionFlix());
            //     StartCoroutine(SlowMotionFlix());
            // }
            StopCoroutine(SlowMotionFlix());
            StartCoroutine(SlowMotionFlix());
        }
    }

    void CheckIfDead(GameObject Enemy, float Force, GameObject PartHits)
    {
        if (!Kills.Contains(Enemy))
        {
            if (!Enemy.GetComponent<HealthSystem>().enabled)
            {
                Kills.Add(Enemy);
                //PushThis(PartHits, Force);
                //  StartCoroutine(PushThis(PartHits, Force));
                PhysicsManager.Instance.PushObject(PartHits, transform.forward, Force);
                SlowMoEffect();
            }
        }
        else
        {
            PhysicsManager.Instance.PushObject(PartHits, transform.forward, Force);
            //StartCoroutine(PushThis(PartHits, Force));
        }


    }
}
