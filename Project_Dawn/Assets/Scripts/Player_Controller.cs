﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {
    [SerializeField]
    KeyCode left = KeyCode.A, right = KeyCode.D, jump = KeyCode.Space, dash = KeyCode.LeftShift, teleport = KeyCode.Mouse1, stealth = KeyCode.F;

    //Regular values
    [SerializeField]
    float playerSpeed = 1f, verticalJump = 1f, teleportRange = 1f, stealthDuration = .5f;

    //Cooldowns
    [SerializeField]
    float dashCD = 1f, stealthCD = 1f, teleportCD = 1f;

    float gravScale = 1f;

    [SerializeField]
    bool grounded = true, isStealth = false;
    bool canStealth = true, canTeleport = true, canDash = true;

    [SerializeField]
    int TotalMana = 100;
    int currentMana;

    [SerializeField]
    GameObject MyTelePrefab;
    GameObject myTemporaryTeleport;

    Rigidbody2D rgd;
    Vector3 direction;
    BoxCollider2D myColl;

    void Start () {
        rgd = GetComponent<Rigidbody2D>();
        gravScale = rgd.gravityScale;
        direction = transform.right;
        myColl = GetComponent<BoxCollider2D>();
        currentMana = TotalMana;
	}
	
	// Update is called once per frame
	void Update () {
        #region Movement
        #region Left/Right
        if (Input.GetKey(left))
        {
            direction = -transform.right;
            rgd.AddForce(direction * Time.deltaTime * playerSpeed, ForceMode2D.Impulse);
        }
        if (Input.GetKey(right))
        {
            direction = transform.right;
            rgd.AddForce(direction * Time.deltaTime * playerSpeed, ForceMode2D.Impulse);
        }
        #endregion
        #region Jumping
        if (Input.GetKey(KeyCode.Space) && grounded)
        {
            rgd.velocity += new Vector2(0,1) * verticalJump;
            grounded = false;
        }

        if (rgd.velocity.y < 0 && rgd.gravityScale == gravScale)
        {
            rgd.gravityScale *= 1.5f;
        }

        if (rgd.velocity.y >= 0 && rgd.gravityScale != gravScale)
        {
            rgd.gravityScale = gravScale;
        }
        #endregion
        #endregion

        #region Skills
        //Teleport
        //Casting Teleport Location
        if (Input.GetKeyDown(teleport) && canTeleport)
        {
            Ray myRay = new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down);
            RaycastHit2D hit = Physics2D.Raycast(myRay.origin, myRay.direction, 3f);
            Debug.DrawRay(myRay.origin, (Vector3)hit.point - myRay.origin, Color.red, 5f);
            if (hit.transform != null && hit.transform.tag == "Ground")
            {
                Ray nextRay = new Ray(myRay.origin, transform.position - myRay.origin);
                RaycastHit2D nextHit = Physics2D.Raycast(nextRay.origin, nextRay.direction);
                if ((nextHit.transform == null) || (nextHit.transform != null && nextHit.transform.tag != "Unpassable"))
                {
                    myTemporaryTeleport = Instantiate(MyTelePrefab, (Vector3)hit.point - new Vector3(0, MyTelePrefab.GetComponent<BoxCollider2D>().bounds.extents.y, 0), Quaternion.identity);
                }
            }
        }

        //Teleporting
        if (Input.GetKeyUp(teleport) && canTeleport)
        {
            if (myTemporaryTeleport != null)
            {
                Destroy(myTemporaryTeleport);
            }
            Ray myRay = new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down);
            RaycastHit2D hit = Physics2D.Raycast(myRay.origin, myRay.direction, 3f);
            Debug.DrawRay(myRay.origin, (Vector3)hit.point - myRay.origin, Color.red, 5f);
            if (hit.transform != null && hit.transform.tag == "Ground")
            {
                Ray nextRay = new Ray(myRay.origin, transform.position - myRay.origin);
                RaycastHit2D nextHit = Physics2D.Raycast(nextRay.origin, nextRay.direction);
                if ((nextHit.transform == null) || (nextHit.transform != null && nextHit.transform.tag != "Unpassable"))
                {
                    transform.position = (Vector3)hit.point + new Vector3(0, myColl.bounds.extents.y, 0);
                    StartCoroutine(StartTeleportCD());
                }
            }
        }

        //Dash
        if (Input.GetKeyDown(dash) && canDash)
        {
            Ray dir = new Ray(transform.position + new Vector3(Mathf.Sign(direction.x) * (myColl.bounds.extents.x+.05f), 0, 0), direction * teleportRange);
            RaycastHit2D hit = Physics2D.Raycast(dir.origin, dir.direction, teleportRange);
            if (hit.transform == null || hit.transform.tag != "Unpassable")
            {
                transform.position += teleportRange * direction;
            } else
            {
                transform.position = hit.point + new Vector2((-Mathf.Sign(direction.x) * myColl.bounds.extents.x), 0);
            }
        }

        //Stealth
        if (Input.GetKeyDown(stealth) && canStealth)
        {
            StartCoroutine(StartStealthCD());
        }
        #endregion
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            grounded = true;
        }
    }

    IEnumerator StartStealthCD()
    {
        canStealth = false;
        isStealth = true;
        yield return new WaitForSeconds(stealthDuration);
        isStealth = false;
        yield return new WaitForSeconds(stealthCD - stealthDuration);
        canStealth = true;
    }

    IEnumerator StartDashCD()
    {
        canDash = false;
        yield return new WaitForSeconds(dashCD);
        canDash = true;
    }

    IEnumerator StartTeleportCD()
    {
        canTeleport = false;
        yield return new WaitForSeconds(teleportCD);
        canTeleport = true;
    }
}
