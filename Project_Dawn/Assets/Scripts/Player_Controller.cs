using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {
    [SerializeField]
    KeyCode left = KeyCode.A, right = KeyCode.D, jump = KeyCode.Space;

    [SerializeField]
    float playerSpeed = 1f, verticalJump = 1f;
    float gravScale = 1f;
    [SerializeField]
    bool grounded = true;
    Rigidbody2D rgd;
    void Start () {
        rgd = GetComponent<Rigidbody2D>();
        gravScale = rgd.gravityScale;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(left))
        {
            rgd.AddForce(-transform.right * Time.deltaTime * playerSpeed, ForceMode2D.Impulse);
        }
        if (Input.GetKey(right))
        {
            rgd.AddForce(transform.right * Time.deltaTime * playerSpeed, ForceMode2D.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rgd.velocity += new Vector2(0,1) * verticalJump;
            grounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
}
