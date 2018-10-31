using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable {

    public EnemyAttack es;
    public int startingHealth;
    public int threshold;
    public int increment;

    public int currentHealth;

    // Use this for initialization
    void Start()
    {
        increment = startingHealth - threshold;
        if (gameObject.GetComponent<EnemyAttack>().armed)
        {
            es = GetComponent<EnemyAttack>();
        }
            
        currentHealth = startingHealth;
    }

    public void Damage(int damage, Vector3 hitPoint)
    {
        
        if (es!=null) {
            es.nextFireTime = es.nextFireTime + Random.Range(.5f,1);
        }
    
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Dead();
        }
    }

    public int Health()
    {
        return currentHealth;
    }

    public int Threshold()
    {
        return threshold;
    }

    public int MaxHealth()
    {
        return startingHealth;
    }

    public int Increment()
    {
        return increment;
    }

    public void SetNextThreshold(int i)
    {
        threshold = i;
    }
	
    public void Dead()
    {
        gameObject.SetActive(false);
    }
}
