using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] float maxHealth;

    private void Start()
    {
        health = maxHealth;
    }
}
