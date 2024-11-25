using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTwo : MonoBehaviour
{
    [Header("Health")]
    public GameObject EnemyBody;
    [SerializeField] float health;
    [SerializeField] float maxHealth;

    [Header("Weopon")]
    public GameObject BulletPrefeb;
    public Transform BulletSpawn1;
    public Transform BulletSpawn2;
    [SerializeField] Transform gun;
    [SerializeField] float turnSpeed = 0.05f;
    [SerializeField] Transform target;
    [SerializeField] float Timer;
    [SerializeField] float FireSpeed = 1000;
    public bool CanSeePlayer;
    float bulletTimer;

    [Header("Destroy")]
    [SerializeField] float timeDestroy;
    bool destroy;

    Rigidbody rb;
    EnemyState enemyState;

    Quaternion rotGoal;
    Vector3 direction;

    void Start()
    {
        health = maxHealth;
        bulletTimer = Timer;

        rb = GetComponent<Rigidbody>();
        
        enemyState = EnemyState.Idle;

    }

    private void Update()
    {
        switch (enemyState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Destroy:
                DestroyEnemy();
                break;
        }

    }

    private void DestroyEnemy()
    {
        Debug.Log("Is Destroy");
        rb.isKinematic = false;
        Destroy(gameObject, timeDestroy);
    }

    private void Idle()
    {
        Debug.Log("Is Idle");
        if (CanSeePlayer)
        {
            enemyState = EnemyState.Attack;
        }
        if (destroy)
        {
            enemyState = EnemyState.Destroy;
        }
    }

    private void Attack()
    {
        Debug.Log("Is Attack");
        if (!CanSeePlayer)
        {
            enemyState = EnemyState.Idle;
        }
        if (destroy)
        {
            enemyState = EnemyState.Destroy;
        }
        WeaponControl();
    }

    private void WeaponControl()
    {
        if (CanSeePlayer)
        {
            direction = (target.position - gun.position).normalized;
            rotGoal = Quaternion.LookRotation(direction);

            gun.rotation = Quaternion.Slerp(gun.rotation, rotGoal, turnSpeed);

            SpawnBullet();
           
        }
        else
        {
            gun.transform.localEulerAngles = Vector3.zero;
        }
    }

    private void SpawnBullet()
    {

            bulletTimer -= Time.deltaTime;
            if (bulletTimer > 0) return;

            bulletTimer = Timer;
            GameObject bullet1 = Instantiate(BulletPrefeb, BulletSpawn1.position, BulletSpawn1.rotation);

            bullet1.GetComponent<Rigidbody>().AddForce(bullet1.transform.forward.normalized * FireSpeed);
        
            GameObject bullet2 = Instantiate(BulletPrefeb, BulletSpawn2.position, BulletSpawn2.rotation);

            bullet2.GetComponent<Rigidbody>().AddForce(bullet2.transform.forward.normalized * FireSpeed);
        
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            destroy = true;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent<PlayerController>(out PlayerController playerController)) 
        { 
            EnemyBody.gameObject.SetActive(true);
            CanSeePlayer = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.TryGetComponent<PlayerController>(out PlayerController playerController)) 
        { 
            CanSeePlayer = false;
        }
    }

    public enum EnemyState
    {
        Attack,
        Destroy,
        Idle
    }
}
