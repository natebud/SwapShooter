using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPOV : MonoBehaviour {

    public Transform player;
    public bool found = false;

    public int forgetAfter;
    private float forget;
    public bool shot;
   // public bool stun;

    public int range = 75;

    private GameObject pMenu, oMenu, goScreen, winScreen;

    // Use this for initialization
    void Start () {
        pMenu = GameObject.Find("UICanvas").transform.Find("PauseMenu").gameObject;
        oMenu = GameObject.Find("UICanvas").transform.Find("OptionsMenu").gameObject;
        goScreen = GameObject.Find("UICanvas").transform.Find("GameOverScreen").gameObject;
        winScreen = GameObject.Find("UICanvas").transform.Find("WinScreen").gameObject;
        shot = false;
        forgetAfter = 2;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 direction = player.position - this.transform.position;
        float angle = Vector3.Angle(direction, this.transform.forward);

        if (pMenu.activeSelf == false && oMenu.activeSelf == false && goScreen.activeSelf == false && winScreen.activeSelf == false)
        {
            if ((Vector3.Distance(player.position, this.transform.position) < range && angle < 45) || direction.magnitude < 3 || shot == true)
            {
                found = true;
                direction.y = 0;

                if (shot)
                {
                    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), .035f);
                }
               /* else if (stun)
                {

                }
*/                else if (direction.magnitude > 5)
                {
                    this.transform.Translate(0, 0, .025f);
                    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), .5f);
                }
                else if (direction.magnitude < 3) //enemy will automatically face player if player gets to close.
                {
                    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), .055f);
                }
                else
                {
                    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), .9f);
                }

                if (Time.time > forget)
                {
                    shot = false;
                }
            }
            else
            {
                found = false;
                forget = Time.time + forgetAfter;
            }
        }
	}

    public void hit()
    {
        shot = true;
    }

    public void hid()
    {
        shot = false;
    }
}
