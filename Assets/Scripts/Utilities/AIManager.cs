using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIManager : MonoBehaviour
{
    private static AIManager _instance;
    public Dictionary<string, Queue<GameObject>> TagetLibrary;
    public static AIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("AIManager");
                go.AddComponent<AIManager>();
                _instance = go.GetComponent<AIManager>();
            }
            return _instance;
        }
    }


    public GameObject GetTarget(string TargetTag)
    {
        GameObject _ret = null;
        _ret = GameObject.FindGameObjectWithTag(TargetTag);
        return _ret;
    }

    public bool IsWithinRange(Vector3 ObjectA, Vector3 ObjectB, float Distance)
    {
        bool _ret = false;
        float _dis = Vector3.Distance(ObjectA, ObjectB);
        if (_dis <= Distance)
        {
            return true;
        }
        return _ret;
    }

    public bool IsWithinSpotRange(GameObject Target, Vector3 ObserverPosition, float SpottingRange)
    {
        bool _ret = false;
        Vector3 TargetPosition = Target.transform.position;
        float _dis = Vector3.Distance(TargetPosition, ObserverPosition);
        if (_dis <= SpottingRange)
        {
            // bool blocked = false;
            //Debug.DrawLine(ObserverPosition, TargetPosition, blocked ? Color.red : Color.green);
            RaycastHit hit;
            if (Physics.Raycast(ObserverPosition, (TargetPosition - ObserverPosition), out hit, 100))
            {
                //Debug.Log(hit.transform.root.gameObject.name);
                if (hit.transform.root.gameObject == Target)
                {
                    _ret = true;
                }

            }
        }
        return _ret;
    }

    public bool IsWithinArkRange(Vector3 Direction, Vector3 ObserverPosition, Vector3 TargetPosition, float ArcRange)
    {
        bool _ret = false;

        float angel = Vector3.Angle(Direction, TargetPosition - ObserverPosition);
        if (Mathf.Abs(angel) > 1 && Mathf.Abs(angel) < ArcRange)
        {
            return true;
        }
        return _ret;
    }

    public Vector3 PredictedPosition(Vector3 TargetPosition, Vector3 ShooterPosition, Vector3 targetVelocity, float projectileSpeed)
    {
        Vector3 displacement = TargetPosition - ShooterPosition;
        float targetMoveAngle = Vector3.Angle(-displacement, targetVelocity) * Mathf.Deg2Rad;

        if (targetVelocity.magnitude == 0 || targetVelocity.magnitude > projectileSpeed && Mathf.Sin(targetMoveAngle) / projectileSpeed > Mathf.Cos(targetMoveAngle) / targetVelocity.magnitude)
        {
            return TargetPosition;
        }

        float shootAngle = Mathf.Asin(Mathf.Sin(targetMoveAngle) * targetVelocity.magnitude / projectileSpeed);
        return TargetPosition + targetVelocity * displacement.magnitude / Mathf.Sin(Mathf.PI - targetMoveAngle - shootAngle) * Mathf.Sin(shootAngle) / targetVelocity.magnitude;
    }
    public void DrawGizmos(Vector3 Position, float Range, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(Position, Range);
    }
    public void LookAtTheTarget(Vector3 TargetPosition, GameObject Observer, float RotationSpeed = -1)
    {
        Quaternion LookRotation;
        Vector3 Direction;
        Vector3 ObserverPosition = Observer.transform.position;
        Direction = (TargetPosition - ObserverPosition).normalized;
        LookRotation = Quaternion.LookRotation(Direction);
        LookRotation.x = 0;
        LookRotation.z = 0;

        if (RotationSpeed == -1)
        {
            Observer.transform.rotation = Quaternion.Slerp(Observer.transform.rotation, LookRotation, 1);
        }
        else
        {
            Observer.transform.rotation = Quaternion.Slerp(Observer.transform.rotation, LookRotation, Time.deltaTime * RotationSpeed);
        }
    }

    public void AimingLook(Vector3 TargetPosition, GameObject Observer, float RotationSpeed = -1)
    {
        Quaternion LookRotation;
        Vector3 Direction;
        Vector3 ObserverPosition = Observer.transform.position;
        Direction = (TargetPosition - ObserverPosition).normalized;
        LookRotation = Quaternion.LookRotation(Direction);

        if (RotationSpeed == -1)
        {
            Observer.transform.rotation = Quaternion.Slerp(Observer.transform.rotation, LookRotation, 1);
        }
        else
        {
            Observer.transform.rotation = Quaternion.Slerp(Observer.transform.rotation, LookRotation, Time.deltaTime * RotationSpeed);
        }
    }
}


