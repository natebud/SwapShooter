using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectGround1 : MonoBehaviour {

	private bool isGrounded;
    private bool isCollide;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        Ray grounder = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(grounder, out hit, 2))
        {
            if (hit.distance > 1.3)
            {
                isGrounded = false;
                // print("airborne");
            }
            else
            {
                isGrounded = true;
                // print("grounded");
            }
        }


    }

    void FixedUpdate()
    {

    }


    void OnCollisionStay(Collision coll){
        isCollide = true;
        print("on");
	}
	void OnCollisionExit(Collision coll){
        isCollide = false;
        print("off");
	}

}