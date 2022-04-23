using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform TriggerLocation;
    public float Range = 5f;
    List<GameObject> ListOfEnemies = new List<GameObject>();
    void CheckAndTrigger(List<GameObject> EnemyObject)
    {
        foreach (GameObject g in EnemyObject)
        {
            if (!ListOfEnemies.Contains(g))
            {
                ListOfEnemies.Add(g);
                g.GetComponent<AIController>().ImTrigger();
            }
        }
        gameObject.SetActive(false);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(TriggerLocation.position, Range);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.tag == "Player")
        {
            List<Collider> _col = PhysicsManager.Instance.ExplosionRange(TriggerLocation.position, Range);
            List<GameObject> _enemies = new List<GameObject>();
            foreach (Collider c in _col)
            {
                if (c.transform.root.tag == "Enemy")
                {
                    if (!_enemies.Contains(c.transform.root.gameObject))
                    {
                        _enemies.Add(c.transform.root.gameObject);
                    }
                }
            }
            CheckAndTrigger(_enemies);
        }
    }
}
