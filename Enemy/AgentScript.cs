using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

    /*Navigation script for enemies, allows for basic movement within pre-made navigation map*/
public class AgentScript : MonoBehaviour {

    public float timer;
    public int newTargetTimer;
    public float speed;
    public Vector3 target;
    public NavMeshAgent agent;
   // public NavMesh nm;

    public bool sentry;
    public bool shot;

    // Use this for initialization
    void Start ()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
     //   nm = agent.
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;
        agent.speed = speed;
        agent.isStopped = false;

        if (shot && timer >= newTargetTimer)
        {
            shot = false;
        }
        else if (timer >= newTargetTimer)
        {
            reTarget();
            timer = 0;
        }
	}
        /*Sets a new destination for the character to move to, at the moment the
         destination is a randomly selected point within the premade navigation map*/
    void reTarget()
    {
        //agent.SamplePathPosition()

        float myX = gameObject.transform.position.x;
        float myZ = gameObject.transform.position.z;

        float newX = Random.Range(myX - 50, myX + 50);
        float newZ = Random.Range(myZ - 50, myZ + 50);

        target = new Vector3(newX, gameObject.transform.position.y, newZ);

        if (!sentry) {
            agent.SetDestination(target);
        }
    }
/*
    public void reTarget(Vector3 pos)
    {
        shot = true;
        if (!sentry)
        {
            float myX = gameObject.transform.position.x;
            float myZ = gameObject.transform.position.z;

            target = new Vector3(pos.x, gameObject.transform.position.y, pos.z);

            agent.SetDestination(target);
            timer = 0;
        }
    }
    */
}
