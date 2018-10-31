using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SwapManager : MonoBehaviour
{

    public GameObject initial;
    private bool reorient;

    private GameObject c_enemy;
    private Rigidbody c_rb;
    private GameObject player;
    private Camera fpsCam;
    private WeaponManager wm;

    private GameObject n_weapon;

    private IDamageable dmgScript;
    private PlayerHealth ph;
    private CharacterController ch;
    private EnemyAttack att;

    private AudioSource[] source;

    // Use this for initialization
    void Start()
    {
        c_enemy = initial;
        dmgScript = c_enemy.GetComponent<IDamageable>();
        player = this.gameObject;
        fpsCam = player.GetComponentInChildren<Camera>();
        wm = player.GetComponent<WeaponManager>();
        reorient = false;
        ph = GetComponent<PlayerHealth>();
        ch = GetComponent<CharacterController>();
        source = GetComponents<AudioSource>();
    }

 /*   public void Update()
    {
        todo set delay after swap so enemies don't instantly shoot you
    } 
*/

    public void Swap(GameObject n_enemy)
    {
        if (n_enemy.tag != "Corpse")
        {
                //enable current enemy
            c_enemy.transform.parent = null;

            CapsuleCollider col = c_enemy.GetComponent<CapsuleCollider>();
            col.enabled = true;

            c_rb = c_enemy.GetComponent<Rigidbody>();
            c_rb.isKinematic = false;
            c_rb.useGravity = true;

            //        c_enemy.GetComponent<MeshRenderer>().enabled = true;

            for (int i = 0; i < 2; i++)
            {
                c_enemy.transform.GetChild(i).GetComponent<SkinnedMeshRenderer>().enabled = true; //
            }

            GameObject w_enemy = wm.EnemyUnequip(n_enemy);
            GameObject w_player = wm.PlayerUnequip();

            if (!ch.isGrounded && ch.up >30)
            {
                print("bye-bye");
                col.radius = .2f;
                //c_enemy.transform.Rotate(player.transform.position * Time.deltaTime * 5);
                c_enemy.tag = "Corpse";

                wm.Discard(w_enemy);
                w_enemy.GetComponent<W_Makarov>().cur_clip = n_enemy.GetComponent<EnemyAttack>().cur_clip;
            }
            else if (dmgScript.Threshold() > 0)
            {

                c_enemy.GetComponent<Animator>().enabled = true;                                      //
                c_enemy.GetComponent<AgentScript>().enabled = true;
                c_enemy.GetComponent<EnemyPOV>().enabled = true;
                c_enemy.GetComponent<NavMeshAgent>().enabled = true;
                c_enemy.transform.Rotate(Vector3.up, 180);

                att = c_enemy.GetComponent<EnemyAttack>();

                att.weapon.transform.parent = null;

                att.enemyEquip(w_player);
                att.enabled = true;
                att.cur_clip = w_player.GetComponent<W_Makarov>().cur_clip;
                att.nextFireTime = att.nextFireTime + 3;
            }
            else
            {
                col.radius = .2f;
                c_enemy.transform.Rotate(-2, 0, 0);
                c_enemy.tag = "Corpse";

                wm.Discard(w_enemy);
                w_enemy.GetComponent<W_Makarov>().cur_clip = n_enemy.GetComponent<EnemyAttack>().cur_clip;
            }

            wm.Equip(w_enemy);

            //

            //disable new enemy

            n_enemy.GetComponent<Collider>().enabled = false;
            att = n_enemy.GetComponent<EnemyAttack>();

            //     n_enemy.GetComponent<MeshRenderer>().enabled = false;

            for (int i = 0; i < 2; i++)
            {
                n_enemy.transform.GetChild(i).GetComponent<SkinnedMeshRenderer>().enabled = false;
            }

            c_rb = n_enemy.GetComponent<Rigidbody>();
            c_rb.isKinematic = true;
            c_rb.useGravity = false;

            var destination = n_enemy.transform.position;
            var originalPos = player.transform.position;
            player.transform.position = destination;

            n_enemy.GetComponent<Animator>().enabled = false;
            n_enemy.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            n_enemy.GetComponent<NavMeshAgent>().enabled = false;
            n_enemy.GetComponent<EnemyPOV>().enabled = false;
            n_enemy.GetComponent<AgentScript>().enabled = false;
            att.enabled = false;
            ch.peak = 0;

            if (reorient == true)
            {
                //print(n_enemy.transform.rotation.eulerAngles.y);
                player.transform.localEulerAngles = new Vector3(0, n_enemy.transform.rotation.eulerAngles.y, 0);
            }

            n_enemy.transform.parent = player.transform;

            if (dmgScript.Threshold() <= 0)
            {

                //    dmgScript.Dead();
            }

            c_enemy = n_enemy;
            dmgScript = c_enemy.GetComponent<IDamageable>();

            //gethealth

            ph.setHealth(n_enemy.GetComponent<EnemyHealth>().currentHealth);            //

            source[0].Play();
            System.Random rnd = new System.Random();
            source[rnd.Next(1,3)].Play();

        }
   
    }
}
