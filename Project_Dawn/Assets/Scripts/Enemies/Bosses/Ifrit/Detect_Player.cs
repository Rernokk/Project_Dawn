using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detect_Player : MonoBehaviour {
    Transform parent;
	// Use this for initialization
	void Start () {
        parent = transform.parent;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            parent.GetComponent<Boss_Init>().TriggerFight();
        }
    }
}
