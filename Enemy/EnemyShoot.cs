using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
//    private const int SLEEP = 1;
    public int ammoLeft = 0, maxAmmo = 0, damage = 0, clipAmmo = 0, clipCapacity = 0;
    private float fireRate = 0f, reloadRate = 0f, range = 0f, spread = 0f;
    private float reloadTimeLeft = 0f, fireRateWait = 0f;

 //   public GameObject Wp;
    private W_Makarov Weapon;
    public EnemyPOV EnemyPov;
    private GameObject Player;
    public float playerDistance;
    public PlayerHealth PlayerHealth;
    private AudioSource[] source;
    private LineRenderer lineRenderer;
    private Transform gunEnd;
    private Vector3 gunRay;

    void Start()
    {
    //    Debug.Log("Init");
        Weapon = GetComponent<W_Makarov>();
        //GetComponent< GetComponent<W_Makarov>();
        //Weapon = this.GetComponent<W_Makarov>();
        //maxAmmo = Weapon.max_reserve;
        //ammoLeft = Weapon.cur_reserve;
        EnemyPov = GetComponentInParent<EnemyPOV>();
        clipAmmo = Weapon.cur_clip;
        clipCapacity = Weapon.clip;
        fireRate = Weapon.fireRate;
        reloadRate = Weapon.t_reload;
        range = Weapon.range;
        spread = Weapon.spread;
        damage = Weapon.damage;
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerHealth = Player.GetComponent<PlayerHealth>();
 //       Debug.Log("Init End");
        source = GetComponents<AudioSource>();
        lineRenderer = GetComponent<LineRenderer>();
        gunEnd = Weapon.gunEnd;
        gunRay = transform.position;

    }
 
    void Update()
    {
        //updateWeapon();
     //   Debug.Log("Updating");
        if (!IsReloading() && !IsCoolingOff())
        {
            Debug.Log("Checking range...");
            if (EnemyPov.found)
            {
                Debug.Log("Taking shot.");
                takeShot();
            }//Attempt shot if player is in range of enemy
        }//Check if player is not reloading or under a fire rate constraint
        fireRateWait -= Time.deltaTime * fireRate;
    }
 
 /*   private bool PlayerInRange()
    {
 
        RaycastHit hit;
        return Physics.Raycast(this.transform.position, Player.transform.position, out hit, range*3);
    }//True if player is within range of enemy's scope
 */
    private void takeShot()
    {    //TODO: incorporate randomness and inaccuracy in shot
        
        RaycastHit hit;
        if (Physics.Raycast(gunRay, transform.forward, out hit, range*4)) {
        //    print("hit");
        //    Debug.DrawRay(gunRay, transform.forward, Color.red, 5f, false);
            GameObject ht = hit.collider.gameObject;
        //    print(ht.tag);
            if (ht.tag.Equals("Player"))
            {
                Debug.Log("Just shot:");
                StartCoroutine(ShotEffect());
                fireRateWait = fireRate;
                Reload();
                
                if (ShotSuccess())
                {
                    PlayerHealth.Damage(damage, Vector3.forward);
                }
            }
        }

        
    }//Take a shot at player            
 
    private bool ShotSuccess()
    {//implmenent this with collision and contact pnt
        return true;
    }//Checks if player was hit
 
    private void Reload()
    {
        clipAmmo -= 1;
        if (clipAmmo < 1)
        {
            source[1].Play();
            Console.WriteLine("Reloading:");
            reloadTimeLeft = Time.time + reloadRate;
            clipAmmo = clipCapacity;//or bullets left?
            ammoLeft -= clipCapacity;
 
        }
    }//Decrement clip count and reload(maybe)
 
    private bool IsReloading()
    {//Disabled rn
        if (reloadTimeLeft > 0f)
        {
            return true;
        }
        else if (reloadTimeLeft <= 0f)
        {
            reloadTimeLeft = 0f;
            return false;
        }
        return false;
    }//Check if enemy is reloading
 
    private bool IsCoolingOff()
    {
        if (fireRateWait > 0f)
        {
            return true;
        }
        else if (fireRateWait <= 0f)
        {
   
            return false;
        }
        return false;
    }//Return true if player is currently limited by mechanical fire rate
 
/*    private bool hasAmmo()
    {
        if (ammoLeft < clipCapacity)
        {
            clipCapacity = ammoLeft;
        }
        return ammoLeft > 0;
    }//Check if any ammo is left. Check and
    //set if ammo left is less than clip's capacity
*/ 
    private void updateWeapon()
    {
    //    Weapon.cur_reserve = ammoLeft;
        Weapon.clip = clipAmmo;
    }

    private IEnumerator ShotEffect()
    {
        lineRenderer.enabled = true;
        source[0].Play();
        lineRenderer.enabled = false;
        yield return 0;
    }
}
