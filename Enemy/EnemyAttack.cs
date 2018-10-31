using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

    public GameObject weapon;

    private GameObject player;
    private Vector3 plPos;
    private Vector3 spr;
    private PlayerHealth ph;
    private EnemyPOV ep;
    private ShootScript sh;

    private W_Makarov w;
    private Transform gunEnd;

    private float range;
    private int damage;
    private float fireRate;
    private float spread;

    public int cur_clip;
    private int clip;
    private float reload;

    public bool armed = false;

    public float off_z;
    public float off_y;

    public float anglex;
    public float angley;

    public float z;
    public float y;

    public float burst = 0;

    public float nextFireTime;
    private bool reloading = false;
    private AudioSource[] source;

	// Use this for initialization
	void Awake () {
        if (weapon !=null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            ep = GetComponent<EnemyPOV>();
            enemyEquip(weapon);
            armed = true;
        }
        else
        {
            cur_clip = 0;
            armed = false;
        }
	}

    // Update is called once per frame
    void Update() {

        if (ep.found && armed)
        {
            plPos = player.transform.position;
            RaycastHit hit;

            if (reloading && Time.time > nextFireTime)
            {
                cur_clip = clip;
                reloading = false;
            }

            else if (Time.time > nextFireTime)
            {
                if (cur_clip > 0) {

                    nextFireTime = Time.time + fireRate;
                    cur_clip = cur_clip - 1;
                    source[0].Play();

                    if (w.automatic)
                    {
                        if (burst == 0)
                        {
                            nextFireTime = nextFireTime + 1;
                            burst = Random.Range(3, 6);
                        }
                        else
                        {
                            burst--;
                        }
                    }
                    else
                    {
                        if (burst == 0)
                        {
                            nextFireTime = nextFireTime + .5f;
                            burst = Random.Range(2, 4);
                        }
                        else
                        {
                            burst--;
                        }
                    }

                    Vector2 off_spr = Random.insideUnitCircle.normalized * spread * 1.5f;
                    y = plPos.y - gunEnd.transform.position.y + 1f;
                    z = Mathf.Sqrt(Mathf.Pow((plPos.z - gunEnd.transform.position.z), 2) + Mathf.Pow((plPos.x - gunEnd.transform.position.x), 2));

                    off_z = off_spr.x;
                    off_y = Mathf.Abs(Mathf.Rad2Deg * Mathf.Atan2(y, z)) + off_spr.y;

                    sh.fire(off_z, off_y-.1f);

                    if (y < 0)
                    {
                        spr = Quaternion.AngleAxis(off_z, Vector3.forward)*  Quaternion.AngleAxis(-off_y, -transform.right) *  transform.forward;
                    }
                    else {
                        spr = Quaternion.AngleAxis(off_z, Vector3.forward)*  Quaternion.AngleAxis(off_y, -transform.right)  *  transform.forward;
                    }

                    //Debug.DrawRay(gunEnd.transform.position, spr * 100, Color.red, 4f);

                    if (Physics.Raycast(gunEnd.transform.position, spr, out hit, range))
                    {

                        if (hit.transform.tag == "Player")
                        {
                            ph = hit.transform.GetComponent<PlayerHealth>();
                            ph.Damage(damage, hit.transform.position);
                            //    print("hit");
                        }
                    }
                    else
                    {
                     //   print("miss");
                    }
                }
                else
                {
                    nextFireTime = Time.time + reload;
                    burst = 0;
                    reloading = true;
                 //   print("reload");
                    source[1].Play();
                }
            }
        }
	}

    public void enemyEquip(GameObject weapon)
    {
        w = weapon.GetComponent<W_Makarov>();

        gunEnd = w.gunEnd;
        range = w.range;
        damage = w.damage;
        spread = w.spread + .6f;
        fireRate = w.fireRate + .1f;
        clip = w.clip;
        cur_clip = w.cur_clip;
        reload = w.t_reload + .4f;

        source = weapon.GetComponents<AudioSource>();

        w.enabled = false;
        w.equipped = false;

        Rigidbody w_rb = weapon.GetComponent<Rigidbody>();

        if (w_rb.useGravity)
        {
            w_rb.isKinematic = true;
            w_rb.useGravity = false;

            Collider[] c_off = weapon.GetComponents<Collider>();
            foreach (Collider c in c_off)
            {
                c.enabled = false;
            }
        }
        else
        {
            weapon.GetComponent<LineRenderer>().enabled = false;
        }

        sh = weapon.GetComponent<ShootScript>();

        weapon.transform.parent = transform;
        weapon.transform.position = transform.position;
        Vector3 pos = w.cam_pos + Vector3.up * 2 - Vector3.forward * .4f;
        weapon.transform.Translate(pos, transform);
        weapon.transform.localEulerAngles = new Vector3(transform.rotation.y, transform.rotation.x - 180, 0);
    }
}
