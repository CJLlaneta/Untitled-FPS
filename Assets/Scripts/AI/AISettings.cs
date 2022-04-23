using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable_Objects/AI/Settings")]
public class AISettings : ScriptableObject
{
    public float RotationSpeed { get { return _RotationSpeed; } private set { _RotationSpeed = value; } }
    [SerializeField] private float _RotationSpeed = 50f;

    public float AimSpeed { get { return _AimSpeed; } private set { _AimSpeed = value; } }
    [SerializeField] private float _AimSpeed = 10f;

    public float AimAdjustment { get { return _AimAdjustment; } private set { _AimAdjustment = value; } }
    [SerializeField] private float _AimAdjustment = 0.2f;

    public float FiringRange { get { return _FiringRange; } private set { _FiringRange = value; } }
    [SerializeField] private float _FiringRange = 10f;

    public float AccuracyArc { get { return _AccuracyArc; } private set { _AccuracyArc = value; } }
    [SerializeField] private float _AccuracyArc = 40f;

    public float SpreadMultiplier { get { return _SpreadMultiplier; } private set { _SpreadMultiplier = value; } }
    [SerializeField] private float _SpreadMultiplier = 0f;

    public float MovementSpeed { get { return _MovementSpeed; } private set { _MovementSpeed = value; } }
    [SerializeField] private float _MovementSpeed = 20;

    public float HoldTime { get { return _HoldTime; } private set { _HoldTime = value; } }
    [SerializeField] private float _HoldTime = 3f;

    public Vector2 ShootCounterHold { get { return _ShootCounterHold; } private set { _ShootCounterHold = value; } }
    [SerializeField] private Vector2 _ShootCounterHold = new Vector2(2, 4);

    public Vector2 ShootHoldTime { get { return _ShootHoldTime; } private set { _ShootHoldTime = value; } }
    [SerializeField] private Vector2 _ShootHoldTime = new Vector2(2, 4);

}
