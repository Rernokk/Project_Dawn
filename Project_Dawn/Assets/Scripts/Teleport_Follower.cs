using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport_Follower : MonoBehaviour {
    Player_Controller player;
    BoxCollider2D boxColl;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Controller>();
        boxColl = GetComponent<BoxCollider2D>();
	}

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + player.teleportRange * player.direction;
    }
}
