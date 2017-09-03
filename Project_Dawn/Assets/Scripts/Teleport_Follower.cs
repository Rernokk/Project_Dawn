using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport_Follower : MonoBehaviour {
    Transform player;
    BoxCollider2D boxColl;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        boxColl = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Mouse1) && Mathf.Abs(transform.position.x - player.transform.position.x) <= 12f){
            RaycastHit2D[] hit = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down);
            if (hit.Length > 0)
            {
                foreach (RaycastHit2D info in hit)
                {
                    if (info.transform.tag == "Ground")
                    {
                        transform.position = new Vector3(info.point.x, info.point.y, transform.position.z);
                    }
                }
            } else
            {
                transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, transform.position.y, transform.position.z);
            }
        }
	}
}
