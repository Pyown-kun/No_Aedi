using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] float health;
    [SerializeField] float maxHealth;

    [Header("Weopon")]
    public GameObject BulletPrefeb;
    public Transform BulletSpawn1;
    public Transform BulletSpawn2;
    [SerializeField] Transform topGun1;
    [SerializeField] Transform botGun1;
    [SerializeField] Transform topGun2;
    [SerializeField] Transform botGun2;
    [SerializeField] Transform target;
    [SerializeField] float turnSpeed;
    [SerializeField] float Timer;
    [SerializeField] float FireSpeed = 1000;
    float bulletTimer;

    [Header("Destroy")]
    [SerializeField] float timeDestroy;
    bool destroy;

    Rigidbody rb;
    private FieldOfView fieldOfView;
    EnemyState enemyState;

    Quaternion rotGoal;
    Vector3 direction;

    void Start()
    {
        health = maxHealth;
        
        fieldOfView = GetComponent<FieldOfView>();

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
        if (fieldOfView.CanSeePlayer)
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
        if (!fieldOfView.CanSeePlayer)
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
        if (fieldOfView.CanSeePlayer)
        {
            //Gun1
            botGun1.transform.LookAt(new Vector3(target.position.x, botGun1.position.y, target.position.z));

            topGun1.transform.LookAt(new Vector3(target.position.x, topGun1.position.y, target.position.z));


            direction = (target.position - botGun1.position).normalized;
            rotGoal = Quaternion.LookRotation(direction);

            topGun1.rotation = Quaternion.Slerp(botGun1.rotation, rotGoal, turnSpeed);
            
            //Gun2
            botGun2.transform.LookAt(new Vector3(target.position.x, botGun2.position.y, target.position.z));

            topGun2.transform.LookAt(new Vector3(target.position.x, topGun2.position.y, target.position.z));


            direction = (target.position - botGun2.position).normalized;
            rotGoal = Quaternion.LookRotation(direction);

            topGun2.rotation = Quaternion.Slerp(botGun2.rotation, rotGoal, turnSpeed);

            SpawnBullet();
           
        }
        else
        {
            botGun1.transform.localEulerAngles = Vector3.zero;
        }
    }

    private void SpawnBullet()
    {
        bulletTimer -= Time.deltaTime;
        if (bulletTimer > 0) return;

        bulletTimer = Timer;
        
        //Gun1
        GameObject bullet1 = Instantiate(BulletPrefeb, BulletSpawn1.position, BulletSpawn1.rotation);
        bullet1.GetComponent<Rigidbody>().AddForce(bullet1.transform.forward.normalized * FireSpeed);

        //Gun2
        GameObject bullet2 = Instantiate(BulletPrefeb, BulletSpawn2.position, BulletSpawn1.rotation);
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

    public enum EnemyState
    {
        Attack,
        Destroy,
        Idle
    }
}
