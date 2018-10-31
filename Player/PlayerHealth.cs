using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable
{
	public int initHealth = 20;
	public int minHealth = 10;
	public int currentHealth;
	public Slider health_slider;

	private CharacterController control;
	private int noHealth = 0;

	/*
	 *Initialize the current health 
	 *Initialize UI health slider
	 *Initialize object for movement 
	 */ 
	void Start () 
	{
		currentHealth = initHealth;
		health_slider.value = currentHealth;
		control = GetComponent <CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}


	/*
	 * Apply damage to health and adjust
	 * health UI.
	 * Player dies if there is no health
	 * Player swaps if min threshhold is met
	 */
	public void Damage(int damage, Vector3 hitPoint) {
		currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
		health_slider.value = currentHealth;

		if(currentHealth == noHealth) 
		{
			Dead();
		} 
		else if (currentHealth == minHealth)
		{
			swap();
		}
	}

    public void setHealth(int n)
    {
        currentHealth = n;
        health_slider.value = currentHealth;
    }

	private void swap() 
	{
		//TODO:
			//initiate swapping mechanics 
	}

	//Get current player health
	public int Health()
	{
		return currentHealth;
	}

	//get maximum allowable health
	public int MaxHealth()
	{
		return initHealth;
	}

	//Get player threshhold
	public int Threshold()
	{
		return minHealth;
	}

	//Set new threshhold
	public void SetNextThreshold(int i)
	{
		minHealth = i;
	}

    public int Increment()
    {
        return 0;
    }

	//End player control
	public void Dead() 
	{
		control.enabled = false;
	}//ded
}
	
//Sources used: 
//https://unity3d.com/learn/tutorials/projects/survival-shooter/player-health