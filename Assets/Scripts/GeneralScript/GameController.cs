using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    public List<PoolProperties> PoolObjects;

    [SerializeField] private GameObject Character;
    [SerializeField] List<GameObject> Enemies;

    void Awake()
    {
        ObjectPoolingManager.Instance.InitilializePool(PoolObjects);
    }

    void IgnoreCollisionCharacter()
    {
        // Character = GameObject.FindGameObjectWithTag("Player");
        // Enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        // foreach (GameObject g in Enemies)
        // {
        //     PhysicsManager.Instance.CharacterIgnoreCollision(Character, g);
        // }
        PhysicsManager.Instance.IgnoreLayerCollision(8, 9);
    }
    void Start()
    {
        IgnoreCollisionCharacter();
    }
}
