﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

    //Weapon script, currently all weapons use same W_Makarov script with altered public variables
    //Weapons are saved as prefabs after creation.
public class W_Makarov : MonoBehaviour {

    public bool equipped = false;
    public bool reloading = false;

    public bool automatic = false;

    public bool swap = false;

    public float fireRate = .25f;
    public float range = 50;
    public float min_spr;           //inaccuracy min
    public float cur_spr;           //inaccuracy current
    public float spread = .05f;     //inaccuracy max

    public float spr_in = .05f;            //increment
    public float spr_de = .0025f;            //decrement

    public float spr_x = 0;                 //spread offset 
    public float spr_y = 0;                 //spread offset

    public float off_x;             //shot deviation x
    public float off_y;             //shot deviation y
    public int damage = 1;
    public Transform gunEnd;        
    public float perfectFireTime = .35f;    //unused
    public float equipDelay = .35f;         //unused

        //coordinates coresponding to where weapon should appear on screen in relation to camera when equipped.
    public static float x = .325f;      
    public static float y = -.264f;
    public static float z = .735f; 

    public Vector3 cam_pos = new Vector3(x, y, z);

    public int max_reserve = 80;    
    public int cur_reserve = 80;
    public int cur_clip = 8;

    public int clip = 8;            //weapon clip size
    public float t_reload = 1.5f;   //reload time

    private Camera fpsCam = null;
    private GameObject player = null;
    public LineRenderer lineRenderer;
    private WaitForSeconds shotLength = new WaitForSeconds(.07f);
    public float nextFireTime;

    private SwapManager sm;         //handles possession

    private AudioSource[] source; //Added ArrayList of AudioSources, make sure to add two Audio Sources into a weapon + first one has to be the fire sound, second is reload
    private ShootScript tracer;

    private TextMeshProUGUI ammoCount;
    private GameObject pMenu, oMenu, goScreen, winScreen;

    //Called when script is enabled
    void Start()
    {
        cur_spr = min_spr;

        lineRenderer = GetComponent<LineRenderer>();
        fpsCam = GetComponentInParent<Camera>();
        player = GameObject.Find("Player");
        sm = player.GetComponent<SwapManager>();
        source = GetComponents<AudioSource>();          //Added line here
        tracer = GetComponent<ShootScript>();

        pMenu = GameObject.Find("UICanvas").transform.Find("PauseMenu").gameObject;
        oMenu = GameObject.Find("UICanvas").transform.Find("OptionsMenu").gameObject;
        goScreen = GameObject.Find("UICanvas").transform.Find("GameOverScreen").gameObject;
        winScreen = GameObject.Find("UICanvas").transform.Find("WinScreen").gameObject;
        ammoCount = GameObject.Find("HUD").transform.Find("Ammo Counter").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cur_spr > min_spr)
        {
            cur_spr = cur_spr - spr_de;
            if(cur_spr < 0)
            {
                cur_spr = 0;
            }
        }
        

        if (equipped && pMenu.activeSelf == false && oMenu.activeSelf == false && goScreen.activeSelf == false &&  winScreen.activeSelf == false)
        {
            //draws line from gunEnd to center of screen
            RaycastHit hit;
            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(.5f, .5f, 0));
  
            if (reloading && Time.time > nextFireTime)
            {
                transform.Translate(0, 0, -2);
                reloading = !reloading;
            }
            //shoot function, automatic weapons, nearly identical to semi-auto, see below
            else if (automatic && Input.GetButton("Fire1") && (Time.time > nextFireTime))
            {
                if (cur_clip > 0)
                {
                    if ((Time.time - nextFireTime) < perfectFireTime)   //remember to place in automatic
                    {
                        cur_spr = cur_spr + spr_in;
                        if (cur_spr > spread)
                        {
                            cur_spr = spread;
                        }
                        print(cur_spr);
                    }
                  /*  else
                    {
                        cur_spr = min_spr;
                    }*/

                    nextFireTime = Time.time + fireRate;
                    cur_clip = cur_clip - 1;
                    Vector2 off_spr = Random.insideUnitCircle.normalized * cur_spr;
                    off_x = off_spr.x;
                    off_y = off_spr.y;
                    Vector3 spr = Quaternion.AngleAxis(off_x, Vector3.right) * Quaternion.AngleAxis(off_y, Vector3.up) * fpsCam.transform.forward;

                    //Debug.DrawRay(fpsCam.transform.position, spr * 100, Color.red, 5f);

                    if (Physics.Raycast(rayOrigin, spr, out hit, range))
                    {
                        GameObject n_enemy = hit.collider.gameObject;
                        IDamageable dmgScript = hit.collider.gameObject.GetComponent<EnemyHealth>();

                        if (dmgScript != null)
                        {
                            n_enemy.GetComponent<EnemyPOV>().hit();
                            dmgScript.Damage(damage, hit.point);
                            
                            if (dmgScript.Health() <= (dmgScript.Threshold())) {
                                //print("hit");
                                dmgScript.SetNextThreshold(dmgScript.Health() - dmgScript.Increment());
                                sm.Swap(n_enemy);
                            }
                        }

                        if (hit.rigidbody != null)
                        {
                            hit.rigidbody.AddForce(-hit.normal * 100f);
                        }
                            //debug, draws line from gun end to center of screen
                        lineRenderer.SetPosition(0, gunEnd.position); 
                        lineRenderer.SetPosition(1, hit.point);
                    }
                    StartCoroutine(ShotEffect(off_x,off_y));
                }
                else if(Time.time > nextFireTime)
                {
                    //empty gun sound
                }
            }
                //shoot function semi-auto
            else if (Input.GetButtonDown("Fire1") && (Time.time > nextFireTime))
            {
                if (cur_clip > 0)
                {

                    if ((Time.time - nextFireTime) < perfectFireTime)   //remember to place in automatic
                    {
                        cur_spr = cur_spr + spr_in;
                        if (cur_spr > spread)
                        {
                            cur_spr = spread;
                        }
                        print(cur_spr);
                    }
                    //sets next time on clock when player can fire again
                    nextFireTime = Time.time + fireRate;
                    cur_clip = cur_clip - 1;

                        //calculates spread on weapon, randomly picks point within circle with a radius of 'spread'
                    Vector2 off_spr = Random.insideUnitCircle.normalized * cur_spr;
                    off_x = off_spr.x;
                    off_y = off_spr.y;
                    Vector3 spr = Quaternion.AngleAxis(off_x, Vector3.right) * Quaternion.AngleAxis(off_y, Vector3.up) * fpsCam.transform.forward;

                    if (Physics.Raycast(rayOrigin, spr, out hit, range))
                    {
                        GameObject n_enemy = hit.collider.gameObject;
                        IDamageable dmgScript = hit.collider.gameObject.GetComponent<EnemyHealth>();


                        //checks if shot object is damageable
                        if (dmgScript != null)
                        {   
                                //hurt enemy for 'damage'
                            dmgScript.Damage(damage, hit.point);
                            n_enemy.GetComponent<EnemyPOV>().hit();
                            //If enemy's health is <= threshold, possess, set new threshold for possessed enemy for later
                            if (dmgScript.Health() <= (dmgScript.Threshold()))
                            { 
                                dmgScript.SetNextThreshold(dmgScript.Health() - dmgScript.Increment());
                                sm.Swap(n_enemy);

                            }
                        }
                            //shot effects physics based object
                        if (hit.rigidbody != null)
                        {
                            hit.rigidbody.AddForce(-hit.normal * 100f);
                        }

                        lineRenderer.SetPosition(0, gunEnd.position);
                        lineRenderer.SetPosition(1, hit.point);
                    }

                    StartCoroutine(ShotEffect(off_x, off_y));
                }
                else if (Time.time > nextFireTime)
                {
                    //empty gun sound
                }
            }
                //reloads if 'r' is pressed
            else if (Input.GetKeyDown("r") && cur_clip != clip)
            {
                Reload();
            }
                //sets next available firetime after reload is called
            else if (reloading && Time.time > nextFireTime)
            {
                transform.Translate(0, 0, -2);
                reloading = !reloading;
            }
        }
        ammoCount.SetText(cur_clip + "/" + cur_reserve);
    }

    public bool Swap()  //unused
    {
        return swap;
    }

        //Semi-automatic and full automatic functions are near identical, method will be created to save space later.
    public void Fire()                          
    {

    }

    public void Reload()    //reload function
    {
        nextFireTime = Time.time + t_reload;
        if (cur_reserve > 0)
        {
            reloading = true;
            transform.Translate(0, 0, 2);   //temporary, hides weapon while reloading

            source[1].Play();                   //Tells the second Audio Source to play, which is the reload sound
            if (clip > cur_reserve)
            {                   
                cur_clip = cur_reserve;
                cur_reserve = 0;
            }
            else
            {
                cur_reserve = cur_reserve - clip + cur_clip;
                cur_clip = clip;
            }
        }
    }


    private IEnumerator ShotEffect(float x, float y)    //coroutine to handle lineRenderer, audio, visual effects
    {
        lineRenderer.enabled = true;
        yield return shotLength;
        lineRenderer.enabled = false;
        //tracer.fire(x,y);
        source[0].Play();                   //Tells the first Audio Source to play, which is the fire sound
    }


}
