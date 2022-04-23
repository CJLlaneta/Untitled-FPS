using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationManager : MonoBehaviour
{
    private static NavigationManager _instance;

    public static NavigationManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("NavigationManager");
                go.AddComponent<NavigationManager>();
                _instance = go.GetComponent<NavigationManager>();

            }
            return _instance;
        }
    }

    public void StopMovement(NavMeshAgent NavAgent)
    {
        NavAgent.enabled = false;
    }

    public bool DestinationReachable(Vector3 Destination, NavMeshAgent NavAgent)
    {
        if (!NavAgent.enabled)
        {
            NavAgent.enabled = true;
        }
        NavMeshPath navMeshPath = new NavMeshPath();
        if (NavAgent.CalculatePath(Destination, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
        {
            return true;
        }
        return false;
    }
    public bool NavRaycast(NavMeshAgent NavAgent, Vector3 TargetPosition, Vector3 ObserverPosition)
    {
        bool _ret = false;
        NavMeshHit hit;
        _ret = !NavMesh.Raycast(ObserverPosition, TargetPosition, out hit, NavMesh.AllAreas); // Return true if obstructed
        //Debug.DrawLine(transform.position, target.position, blocked ? Color.red : Color.green);
        return _ret;
    }
    public void SetNavAgentSpeed(NavMeshAgent NavAgent, float Speed)
    {
        NavAgent.speed = Speed;
    }


    public void SetNavigation(Vector3 Targetpostion, NavMeshAgent NavAgent)
    {
        if (!NavAgent.enabled)
        {
            NavAgent.enabled = true;
        }
        NavAgent.SetDestination(Targetpostion);
    }

}

