using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbLadder : MonoBehaviour {

    private CharacterController chController;
    private Rigidbody playerBody;
    public Transform chPos;
    public bool canClimb = false;
    private float speed = .1f;

    void Start()
    {
        chController = gameObject.GetComponent<CharacterController>();
        playerBody = gameObject.GetComponent<Rigidbody>();
        chPos = playerBody.transform;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ladder")
        {
            chController.enabled = false;
            playerBody.isKinematic = true;
            canClimb = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ladder")
        {
            chController.enabled = true;
            playerBody.isKinematic = false;
            canClimb = false;
        }
    }

    void Update()
    {
        if (canClimb)
        {
            if (Input.GetKey("w"))
            {
                chPos.transform.Translate(Vector3.up * speed);
            }
            if (Input.GetKey("s"))
            {
                chPos.transform.Translate(Vector3.down * speed);
            }
        }
    }
}
