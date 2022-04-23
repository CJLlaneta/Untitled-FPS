using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovements : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private MovementSettings _settings = null;
    [SerializeField] private float resizeCrouch = 1;
    [SerializeField] private float resizeDock = 1;

    [SerializeField] private HeadbobController HeadbobController;


    private Vector3 _moveDirection;
    // private Vector3 _dashDirection;

    private CharacterController _controller;

    [SerializeField] private GameObject DamageEffect;

    [SerializeField] private HealthSystem HealthSystem;

    [SerializeField] private Text HPtxt;

    //Slideing Parameters
    private Vector3 hitPointNormal;
    RaycastHit slopeHit;
    private bool IsSliding
    {
        get
        {
            Debug.DrawRay(transform.position, Vector3.down, Color.red);
            if (_controller.isGrounded)
            {
                slopeHit = PhysicsManager.Instance.RayPoint(transform.position, Vector3.down, 3f);
                if (slopeHit.collider != null && slopeHit.transform.root.tag == "Area")
                {
                    hitPointNormal = slopeHit.normal;
                    return Vector3.Angle(hitPointNormal, Vector3.up) > _controller.slopeLimit;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }
    }
    //Slideing Parameters
    private float _originalSize = 2;
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _originalSize = _controller.height;

    }

    // Update is called once per frame
    void Update()
    {
        HealthChecker();
        DefaultMovement();
        UpdateDirection();

    }
    void FixedUpdate()
    {
        //UpdateDirection();
    }

    void LateUpdate()
    {
        HPUpdate();
    }

    void HPUpdate()
    {
        HPtxt.text = HealthSystem.Health.ToString("000");
    }
    void HealthChecker()
    {
        if (HealthSystem.ImHit)
        {
            HealthSystem.ImHit = false;
            DamageEffect.SetActive(true);
        }
    }


    private KeyCode DashKey()
    {
        return KeyCode.LeftControl;
        //return KeyCode.Mouse1;
    }
    private void UpdateDirection()
    {
        if (_settings.WillSlideOnSlopes && IsSliding)
        {
            _moveDirection += new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z) * _settings.SlopeSpeed;
        }
        _controller.Move((_moveDirection * Time.timeScale) * Time.unscaledDeltaTime);
        HeadbobController.HandeHeadBob(_moveDirection);
    }
    bool _isDash = false;
    private void DefaultMovement()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (_controller.isGrounded && !_isDash)
        {

            if (input.x != 0 && input.y != 0)
            {
                input *= 0.777f;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            else
            {
                _moveDirection.y = -_settings.antiBump;
            }
            if (Input.GetKeyDown(DashKey()) && (_settings.WillSlideOnSlopes && !IsSliding))
            {
                StartCoroutine(DashCounter());
                _controller.height = resizeCrouch;
                //_dashDirection = _moveDirection;
                _moveDirection.x = input.x * _settings.dashspeed;
                _moveDirection.z = input.y * _settings.dashspeed;
                _moveDirection = transform.TransformDirection(_moveDirection);
            }
            else
            {
                _moveDirection.x = input.x * _settings.speed;
                _moveDirection.z = input.y * _settings.speed;
                _moveDirection = transform.TransformDirection(_moveDirection);
            }

        }
        else if (_isDash && _controller.isGrounded)
        {

            if (Input.GetKeyUp(DashKey()) || IsSliding)
            {
                // _isDash = false;
                // _controller.height = _originalSize;
                ResetCrouchDock();
                StopCoroutine(DashCounter());
            }
        }
        else if (_isDash && !_controller.isGrounded)
        {

            _moveDirection.y -= _settings.gravity * Time.deltaTime;
            if (Input.GetKeyUp(DashKey()) || IsSliding)
            {

                ResetCrouchDock();
                StopCoroutine(DiveCounter());
            }
        }
        else
        {
            _moveDirection.y -= _settings.gravity * Time.deltaTime;
            if (_settings.WillSlideOnSlopes && !IsSliding)
            {
                if (Input.GetKeyDown(DashKey()))
                {
                    StartCoroutine(DiveCounter());
                    _controller.height = resizeDock;
                    Vector3 _center = _controller.center;
                    _center.y = 0.5f;
                    _controller.center = _center;
                    //Debug.Log("Slider");
                    //_dashDirection = _moveDirection;
                    _moveDirection.x = input.x * _settings.rollspeed;
                    _moveDirection.z = input.y * _settings.rollspeed;
                    _moveDirection = transform.TransformDirection(_moveDirection);
                }
            }


        }
    }

    private void ResetCrouchDock()
    {
        //_moveDirection = new Vector3(_moveDirection.x, _moveDirection.y - 1.8f, _moveDirection.z);

        Vector3 _center = _controller.center;
        if (_center.y > 0)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.5f, gameObject.transform.position.z);
        }
        else
        {
            // gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + resizeCrouch, gameObject.transform.position.z);
        }
        _center.y = 0f;
        _controller.center = _center;
        _isDash = false;
        _controller.height = _originalSize;
        //        Debug.Log("check");

    }
    IEnumerator DashCounter()
    {
        _isDash = true;
        yield return new WaitForSeconds(_settings.dashduration);
        ResetCrouchDock();
    }

    IEnumerator DiveCounter()
    {
        _isDash = true;
        yield return new WaitForSeconds(_settings.rollduration);
        _moveDirection = Vector3.zero;
    }
    private void Jump()
    {
        _moveDirection.y += _settings.jumpForce;
    }


}
