using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUtilities : MonoBehaviour
{
    private static BasicUtilities _instance;
    public static BasicUtilities Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("BasicUtilities");
                go.AddComponent<BasicUtilities>();
                _instance = go.GetComponent<BasicUtilities>();
            }
            return _instance;
        }
    }

    public void ShowVFX(string MuzzleTag, Vector3 Position, Quaternion Direction, Transform Parent = null)
    {
        GameObject _g = ObjectPoolingManager.Instance.SpawnFromPool(MuzzleTag, Position, Direction);
        if (Parent != null)
        {
            _g.transform.SetParent(Parent);
        }
    }

    public GameObject ObjectPool(string MuzzleTag, Vector3 Position, Quaternion Direction, Transform Parent = null)
    {
        GameObject _g = ObjectPoolingManager.Instance.SpawnFromPool(MuzzleTag, Position, Direction);
        if (Parent != null)
        {
            _g.transform.SetParent(Parent);
        }
        return _g;
    }

    public GameObject GetNearestGameObject(Vector3 CenterPoint, List<GameObject> Groups)
    {
        GameObject _ret = null;
        float _dis = 0;
        if (Groups.Count >= 1)
        {
            _ret = Groups[0];
            _dis = Vector3.Distance(CenterPoint, _ret.transform.position);
            foreach (GameObject g in Groups)
            {
                float _distance = Vector3.Distance(CenterPoint, g.transform.position);
                if (_distance < _dis)
                {
                    _ret = g;
                    _dis = _distance;
                }
            }

        }
        return _ret;
    }
}
