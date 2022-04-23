using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;




public class AIController : MonoBehaviour
{
    //AI settings
    [SerializeField] private AISettings AISetting;
    //AI settings

    //Settings Guns

    [SerializeField] private WeaponSettings Weapon;

    [SerializeField] private List<WeaponSettings> Weapons;
    [SerializeField] private GameObject[] ContrainControllers;

    //Settings Guns

    public float Health = 1;

    [SerializeField] private NavMeshAgent NavMeshAgent;
    [SerializeField] private GameObject CharacterBody;

    [SerializeField] private Transform Aimer;
    [SerializeField] private Transform HimLookAt;
    [SerializeField] private HealthSystem HealthSystem;
    [SerializeField] private Rig Rig;
    [SerializeField] private RigBuilder RigBuilder;

    [SerializeField] private bool IsAFK = false;
    [SerializeField] private State StateAI = State.Idle;

    private IShooting IShooting;
    private GameObject Target;

    // private Vector3 TargetProxy;
    private Transform CenterMass;
    private Transform MuzzlePoint;




    void Start()
    {
        Initialize();
    }

    void Initialize()
    {


        NavMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        HealthSystem = gameObject.GetComponent<HealthSystem>();
        HealthSystem.Health = Health;
        // PhysicsManager.Instance.DisableRigid(CharacterBody);
        //Animator = CharacterBody.GetComponent<Animator>();
        //Target = GameObject.FindGameObjectWithTag("Player");
        Target = AIManager.Instance.GetTarget("Player");
        CenterMass = FindCenterMass(Target);

        NavigationManager.Instance.SetNavAgentSpeed(NavMeshAgent, AISetting.MovementSpeed);
        IShooting = gameObject.GetComponent<IShooting>();
        if (Weapons.Count > 0)
        {
            int _rnd = Random.Range(0, Weapons.Count);
            Weapon = Weapons[_rnd];
        }
        //Weapon.SpreadShot += AISetting.SpreadMultiplier;
        IShooting.SetWeaponsProperties(Weapon);
        MuzzlePoint = IShooting.GunPoint();
        // int _i = Weapon.ConstraintType;
        // Debug.Log(_i);
        _ShootCounterHold = (int)Random.Range(AISetting.ShootCounterHold.x, AISetting.ShootCounterHold.y);
        _ShootHoldTime = Random.Range(AISetting.ShootHoldTime.x, AISetting.ShootHoldTime.y);
        ContrainControllers[Weapon.ConstraintType].SetActive(true);
        RigBuilder.Build();

        // Debug.Log(_i);

        //Animator.runtimeAnimatorController = Weapon.AnimatorController_TPS;

    }

    private enum State
    {
        Idle,
        Hunt
    }
    // Update is called once per frame
    private Transform FindCenterMass(GameObject target)
    {
        Transform _ret = null;
        foreach (Transform t in target.GetComponentsInChildren<Transform>())
        {
            if (t.CompareTag("CenterMass"))
            {
                _ret = t;
                break;
            }

        }
        return _ret;
    }
    void FixedUpdate()
    {

    }

    public void ImTrigger()
    {
        StateAI = State.Hunt;
    }
    void Update()
    {
        if (StateAI == State.Idle)
        {
            if (HealthSystem.ImHit)
            {
                StateAI = State.Hunt;
            }
            if (IsTargetSpotted())
            {
                StateAI = State.Hunt;
            }
        }
        else
        {

            MoveToTarget();
            AimToTarget();
            PositionTheTargetPoint();
            if (IsTargetSpotted())
            {
                FireWeapon();
            }

        }
        CheckHealth();
        LookAtTarget();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AISetting.FiringRange);
    }
    void CheckHealth()
    {
        if (!HealthSystem.enabled)
        {
            Death();
        }
    }
    void PositionTheTargetPoint()
    {
        HimLookAt.position = Target.transform.position;
        //HimLookAt.position = TargetProxy;
    }
    bool IsTargetInRange()
    {
        return AIManager.Instance.IsWithinRange(transform.position, Target.transform.position, AISetting.FiringRange);

    }

    bool IsTargetSpotted()
    {
        return AIManager.Instance.IsWithinSpotRange(Target, Aimer.position, AISetting.FiringRange);
        //return NavigationManager.Instance.NavRaycast(NavMeshAgent, Target.transform.position, Aimer.position);
    }
    float _cntHold = 0;
    bool _CanMove = false;

    void MoveCharacter()
    {
        NavigationManager.Instance.SetNavigation(Target.transform.position, NavMeshAgent);
        //SetWalkingAnimation(true);
        IShooting.OnMove();
        Rig.weight = 0.4f;
    }

    void StopCharacter()
    {
        NavigationManager.Instance.StopMovement(NavMeshAgent);
        //SetWalkingAnimation(false); //false
        IShooting.OnIdle();
        Rig.weight = 1f;
        _cntHold = 0;
        _CanMove = false;
    }
    void MoveToTarget()
    {
        //Debug.Log(IsTargetSpotted());
        if (IsTargetInRange() && IsTargetSpotted())
        {
            StopCharacter();
        }
        else
        {
            if (_CanMove)
            {
                if (NavigationManager.Instance.DestinationReachable(Target.transform.position, NavMeshAgent))
                {
                    MoveCharacter();
                }
                else
                {
                    StopCharacter();
                }

            }
            else
            {
                _cntHold += 1 * Time.deltaTime;
                if (_cntHold >= AISetting.HoldTime)
                {
                    _cntHold = 0;
                    _CanMove = true;
                }
            }

        }
    }

    // void SetWalkingAnimation(bool Status)
    // {


    //     AnimationManager.Instance.SetAnimationBoolean(Animator, "IsRunning", Status);
    // }

    // void SetFiringAnimation()
    // {
    //     if (!AnimationManager.Instance.IsAnimationClipPlaying(Animator, "Fire"))
    //     {
    //         // AnimationManager.Instance.PlayClip(Animator, "Shoot");
    //     }


    // }
    float _cntShootHoldTime = 0;
    int _cntShootCounter = 0;

    int _ShootCounterHold = 0;
    float _ShootHoldTime = 0;
    void FireWeapon()
    {
        if (!IsAFK)
        {
            if (_cntShootCounter > _ShootCounterHold)
            {
                _cntShootHoldTime += 1 * Time.deltaTime;
                if (_cntShootHoldTime >= _ShootHoldTime)
                {
                    _cntShootHoldTime = 0;
                    _cntShootCounter = 0;
                }
            }
            else
            {
                if (IsWithinSightArc() && IShooting.CanShoot())
                {
                    //SetFiringAnimation();
                    Vector3 _direction = AddSpreader(MuzzlePoint.forward);
                    //Vector3 _direction = AddSpreader(Aimer.forward);
                    _cntShootCounter += 1;
                    // Debug.Log(_cntShootCounter);
                    //IShooting.OnShoot(Aimer.position, _direction, Weapon.WeaponRange, Weapon.Damage, Weapon.GunForce, Weapon.GunMuzzleTag);
                    IShooting.OnShoot(MuzzlePoint.position, _direction, Weapon.WeaponRange, Weapon.Damage, Weapon.GunForce, Weapon.GunMuzzleTag);
                }
            }

            // if (IsWithinSightArc())
            // {
            //     //SetFiringAnimation();
            //     Vector3 _direction = AddSpreader(MuzzlePoint.forward);
            //     //Vector3 _direction = AddSpreader(Aimer.forward);
            //     _cntShootCounter += 1;
            //     //IShooting.OnShoot(Aimer.position, _direction, Weapon.WeaponRange, Weapon.Damage, Weapon.GunForce, Weapon.GunMuzzleTag);
            //     IShooting.OnShoot(MuzzlePoint.position, _direction, Weapon.WeaponRange, Weapon.Damage, Weapon.GunForce, Weapon.GunMuzzleTag);
            // }

        }


    }

    private Vector3 AddSpreader(Vector3 vector)
    {

        Vector3 _ret = vector;
        _ret.x += Random.Range(-AISetting.SpreadMultiplier, AISetting.SpreadMultiplier);
        _ret.y += Random.Range(-AISetting.SpreadMultiplier, AISetting.SpreadMultiplier);
        return _ret;
    }

    bool IsWithinSightArc()
    {

        return AIManager.Instance.IsWithinArkRange(Aimer.forward, transform.position, Target.transform.position, AISetting.AccuracyArc);
    }

    void AimToTarget()
    {
        //Target.transform.position

        AIManager.Instance.AimingLook(Target.transform.position, Aimer.gameObject, AISetting.AimSpeed);
    }

    void LookAtTarget()
    {
        AIManager.Instance.LookAtTheTarget(Target.transform.position, gameObject, AISetting.RotationSpeed);
    }
    void Death()
    {
        CharacterBody.GetComponent<RigBuilder>().enabled = false;
        // AnimationManager.Instance.StopAnimation(Animator);
        IShooting.OnDeath();
        NavMeshAgent.enabled = false;
        PhysicsManager.Instance.EnableRigid(CharacterBody);
        this.enabled = false;

    }
}
