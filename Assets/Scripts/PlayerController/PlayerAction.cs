using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerAction : MonoBehaviour
{
    // Start is called before the first frame update

    public WeaponSettings WeaponSetting;
    [SerializeField] private CameraShake CameraShake;

    private IShooting IShooting;

    [SerializeField] private float KickRange = 2f;
    [SerializeField] private float PickRange = 2f;
    [SerializeField] private float KickForce = 2f;
    [SerializeField] private float KickDamage = 100f;
    [SerializeField] private float KickDelay = 0.05f;

    [SerializeField] private float NormalTime = 1f;
    [SerializeField] private float SlowTime = 1f;
    [SerializeField] private float HoldTime = 0.1f;
    [SerializeField] private Animator Animator;

    [SerializeField] private float SlomotionRate = 100f;
    private float _SlomotionRate = 100f;
    [SerializeField] private float SlomotionReduction = 0.5f;
    [SerializeField] private float SlomotionIncrease = 0.5f;

    [SerializeField] private bool UnliSlowno = false;
    [SerializeField] private Text SlowMotiontxt;
    [SerializeField] private GameObject SlowmoVFX;
    [SerializeField] private AudioReverbFilter AudioRevertFilter;


    void Start()
    {
        IShooting = gameObject.GetComponent<IShooting>();
        GameSettingManager.Instance.HideCursor();
        IShooting.SetWeaponsProperties(WeaponSetting);
        GameSettingManager.Instance.SetGlobalTime(NormalTime);
        AlternateTime = HoldTime;
        BulletTime = SlowTime;
        _SlomotionRate = SlomotionRate;
        AudioRevertFilter = GameObject.FindGameObjectWithTag("BackgroundMusic").GetComponent<AudioReverbFilter>();

        SetSlowMoEffect(false);
    }

    void SetSlowMoEffect(bool Status)
    {
        if (AudioRevertFilter != null)
        {
            AudioRevertFilter.enabled = Status;
        }
        SlowmoVFX.SetActive(Status);
    }

    // Update is called once per frame
    public static bool isSlowMo = false;
    public static float AlternateTime = 0.1f;
    public static float BulletTime = 0.3f;
    void KeyAction()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (input.x != 0 || input.y != 0)
        {
            //Debug.Log("walking");
            IShooting.OnMove();
        }
        else
        {
            IShooting.OnIdle();
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            //WeaponSetting.WeaponRange
            if (IShooting.CanShoot())
            {

                CameraShake.ShakeMe();
            }
            IShooting.OnShoot(transform.position, transform.forward, 100, WeaponSetting.Damage, WeaponSetting.GunForce, WeaponSetting.GunMuzzleTag);
        }


        if (Input.GetKeyDown(KeyCode.V))
        {
            Kick();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            PickUpChecker();
        }
        // if (Input.GetKeyDown(KeyCode.LeftShift) && SlomotionRate > 0)
        // {
        //     if (!isSlowMo)
        //     {

        //         GameSettingManager.Instance.SetGlobalTime(SlowTime);
        //         // isSlowMo = true;

        //     }
        //     else
        //     {
        //         GameSettingManager.Instance.SetGlobalTime(NormalTime);
        //         // isSlowMo = false;
        //         _cntSlowmo = 0;
        //     }
        //     isSlowMo = !isSlowMo;
        // }
        // if (isSlowMo && SlomotionRate <= 0)
        // {
        //     isSlowMo = false;
        //     GameSettingManager.Instance.SetGlobalTime(NormalTime);
        // }

        if (Input.GetKeyDown(KeyCode.Mouse1) && SlomotionRate > 0)
        {
            GameSettingManager.Instance.SetGlobalTime(SlowTime);
            isSlowMo = true;
            SetSlowMoEffect(isSlowMo);

        }
        else if (Input.GetKeyUp(KeyCode.Mouse1) || SlomotionRate <= 0)
        {
            if (isSlowMo)
            {
                GameSettingManager.Instance.SetGlobalTime(NormalTime);
                isSlowMo = false;
                SetSlowMoEffect(isSlowMo);
            }
        }

    }

    void Kick()
    {
        if (!AnimationManager.Instance.IsAnimationClipPlaying(Animator, "Kick"))
        {
            AnimationManager.Instance.PlayClip(Animator, "Kick");
            StartCoroutine(KickPush());
        }
    }

    IEnumerator KickPush()
    {
        yield return new WaitForSeconds(KickDelay);
        IShooting.OnKick(transform.position, transform.forward, KickRange, KickDamage, KickForce);
    }
    float _cntSlowmo = 0;
    void Update()
    {
        KeyAction();
        if (!UnliSlowno)
        {
            SlowMotionUpdate();
        }

        //SlowMoEffect();

    }
    private void PickUpChecker()
    {
        // GameObject _g = PhysicsManager.Instance.RayInteraction(gameObject.transform.position, gameObject.transform.forward, PickRange);
        // if (_g != null && _g.tag == "Weapon")
        // {
        //     GetThisWeapon(_g);
        // }
        RaycastHit _ray = PhysicsManager.Instance.RayPoint(gameObject.transform.position, gameObject.transform.forward, PickRange);
        if (_ray.collider != null)
        {
            List<Collider> _cols = PhysicsManager.Instance.ExplosionRange(_ray.point, 3);
            List<GameObject> _g = new List<GameObject>();
            foreach (Collider c in _cols)
            {
                //Debug.Log(c.gameObject.tag);
                if (c.gameObject.tag == "Weapon")
                {
                    _g.Add(c.gameObject);
                    //  return;
                }
            }
            GameObject _wpn = BasicUtilities.Instance.GetNearestGameObject(_ray.point, _g);
            GetThisWeapon(_wpn);
            //GetThisWeapon(c.gameObject);

        }
    }

    void GetThisWeapon(GameObject Weapon)
    {
        if (Weapon != null)
        {
            Weapon.GetComponent<WeaponDropContainer>().WeaponEquip(gameObject);
        }

    }
    float _cntSlowmotion = 0;
    void SlowMotionUpdate()
    {
        if (isSlowMo)
        {
            if (SlomotionRate < 0)
            {
                SlomotionRate = 0;
            }
            _cntSlowmotion += 1 * Time.unscaledDeltaTime;
            if (_cntSlowmotion >= 0.2f)
            {
                SlomotionRate -= SlomotionReduction;
                _cntSlowmotion = 0;
            }

        }
        else if (!isSlowMo)
        {
            if (SlomotionRate < _SlomotionRate)
            {
                _cntSlowmotion += 1 * Time.unscaledDeltaTime;
                if (_cntSlowmotion >= 0.2f)
                {
                    SlomotionRate += SlomotionIncrease;
                    _cntSlowmotion = 0;
                }
            }

            if (SlomotionRate > _SlomotionRate)
            {
                SlomotionRate = _SlomotionRate;
            }
        }
        SlowMotiontxt.text = SlomotionRate.ToString("000");
    }
    IEnumerator SlowFlick()
    {
        GameSettingManager.Instance.SetGlobalTime(0.7f);
        yield return new WaitForSecondsRealtime(0.8f);
        GameSettingManager.Instance.SetGlobalTime(SlowTime);
        _cntSlowmo = 0;
    }
    void SlowMoEffect()
    {
        if (isSlowMo)
        {
            _cntSlowmo += 1 * Time.unscaledDeltaTime;
            if (_cntSlowmo >= 4f)
            {
                StartCoroutine(SlowFlick());
            }
        }
    }




}
