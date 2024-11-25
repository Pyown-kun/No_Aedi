using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] float maxHealth;
    public Image healthBar;
    bool takeDamage;

    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        //Heal(3);
    }

    public void TakeDamage(float damage)
    {
        health -=damage;
        healthBar.fillAmount = health / maxHealth;
        takeDamage = true;
    }

    public void Heal(float healingAmount)
    {
        health += healingAmount;
        healingAmount = Mathf.Clamp(health, 0, 100);

        healthBar.fillAmount = healingAmount / maxHealth;
    }


}
