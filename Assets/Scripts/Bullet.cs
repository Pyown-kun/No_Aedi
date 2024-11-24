using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletLife = 3f;
    [SerializeField] float bulletDamage = 3f;


    private void Awake()
    {
        Destroy(gameObject, bulletLife);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemyComponent))
        {
            enemyComponent.TakeDamage(bulletDamage);
        }
        
        if (collision.gameObject.TryGetComponent<EnemyTwo>(out EnemyTwo enemyComponentTwo))
        {
            enemyComponentTwo.TakeDamage(bulletDamage);
        }

        Destroy(gameObject);
    }
}
