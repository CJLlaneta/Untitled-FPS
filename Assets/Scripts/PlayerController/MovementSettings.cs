using UnityEngine;
[CreateAssetMenu(menuName = "Scriptable_Objects/Movement/Settings")]
public class MovementSettings : ScriptableObject
{
    public float speed { get { return _speed; } private set { _speed = value; } }
    [SerializeField] private float _speed = 5.0f;

    public float SlopeSpeed { get { return _SlopeSpeed; } private set { _SlopeSpeed = value; } }
    [SerializeField] private float _SlopeSpeed = 8f;


    public float rollspeed { get { return _rollspeed; } private set { _rollspeed = value; } }
    [SerializeField] private float _rollspeed = 10.0f;

    public float rollduration { get { return _rollduration; } private set { _rollduration = value; } }
    [SerializeField] private float _rollduration = 1.5f;

    public float dashspeed { get { return _dashspeed; } private set { _dashspeed = value; } }
    [SerializeField] private float _dashspeed = 10.0f;

    public float dashduration { get { return _dashduration; } private set { _dashduration = value; } }
    [SerializeField] private float _dashduration = 1.5f;

    public float jumpForce { get { return _jumpForce; } private set { _jumpForce = value; } }
    [SerializeField] private float _jumpForce = 13.0f;

    public float antiBump { get { return _antiBump; } private set { _antiBump = value; } }
    [SerializeField] private float _antiBump = 4.5f;

    public float gravity { get { return _gravity; } private set { _gravity = value; } }
    [SerializeField] private float _gravity = 30.0f;

    public bool WillSlideOnSlopes { get { return _WillSlideOnSlopes; } private set { _WillSlideOnSlopes = value; } }
    [SerializeField] private bool _WillSlideOnSlopes = true;

}
