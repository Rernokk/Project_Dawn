using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Controller : MonoBehaviour {
    [SerializeField]
    KeyCode left = KeyCode.A, right = KeyCode.D, jump = KeyCode.Space, dash = KeyCode.LeftShift, flame = KeyCode.Mouse0, stealth = KeyCode.F, lightning = KeyCode.Mouse1;

    //Regular values
    [SerializeField]
    float playerSpeed = 1f, verticalJump = 1f, stealthDuration = .5f, explosionRange = 5f, lightningRange = 1f;
    public float teleportRange = 1f;

    //Damage
    [SerializeField]
    float flameDmg = 100f, explosionDmg = 100f, lightningDmg;

    //Cooldowns
    [SerializeField]
    float dashCD = 1f, stealthCD = 1f, lightningCD = 1f;
    float gravScale = 1f;

    [SerializeField]
    bool grounded = true, isStealth = false;
    bool canStealth = true, canTeleport = true, canDash = true, canBolt = true;

    [SerializeField]
    float TotalMana = 100, TotalHealth;
    float currentMana, currentHealth;

    [SerializeField]
    GameObject MyTelePrefab, MyDetonatePrefab;
    GameObject myTemporaryTeleport;

    Rigidbody2D rgd;

    [HideInInspector]
    public Vector3 direction;
    BoxCollider2D myColl;
    Transform FlameDirection;
    Transform myCam;

    [SerializeField]
    List<Monster> Aggro;
    Monster_List_Ref MonsterRef;

    void Start () {
        rgd = GetComponent<Rigidbody2D>();
        gravScale = rgd.gravityScale;
        direction = transform.right;
        myColl = GetComponent<BoxCollider2D>();
        currentMana = TotalMana;
        Aggro = new List<Monster>();
        FlameDirection = transform.Find("FlameAnchor").transform;
        FlameDirection.Find("FlameFX").GetComponent<ParticleSystem>().Stop();
        myCam = transform.Find("Main Camera").transform;
        MonsterRef = GameObject.Find("Monster_Ref").GetComponent<Monster_List_Ref>();
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
        #region Dash
        //TODO: Interact with walls correctly. Possibly act as a "taunt" to monsters aggro'd within range.
        if (Input.GetKeyDown(dash) && canDash)
        {
            myTemporaryTeleport = Instantiate(MyTelePrefab, transform.position + direction * teleportRange, Quaternion.identity);
        }

        if (Input.GetKeyUp(dash) && canDash && myTemporaryTeleport != null)
        {
            Destroy(Instantiate(MyDetonatePrefab, transform.position, Quaternion.identity), 3f);
            Vector3 myCamPos = new Vector3(myCam.localPosition.x, myCam.localPosition.y, myCam.localPosition.z);
            Vector3[] CamShake = new Vector3[5];
            for (int i = 0; i < 5; i++)
            {
                CamShake[i] = myCam.position + new Vector3(Random.Range(-.2f, .2f), Random.Range(-.2f, .2f), 0);
            }
            //StartCoroutine(CameraShake(CamShake, myCamPos));
            foreach (Monster m in MonsterRef.MonstersInRange(transform.position, explosionRange))
            {
                m.Damage(explosionDmg);
            }
            transform.position = myTemporaryTeleport.transform.position;
            Destroy(myTemporaryTeleport);
            StartCoroutine(StartDashCD());
        }
        #endregion
        #region Stealth
        if (Input.GetKeyDown(stealth) && canStealth)
        {
            StartCoroutine(StartStealthCD());
        }
        #endregion
        #region Flame
        if (Input.GetKeyDown(flame))
        {
            FlameDirection.Find("FlameFX").GetComponent<ParticleSystem>().Play(true);
        }
        if (Input.GetKey(flame))
        {
            //Tracking Mouse
            Vector3 lookPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lookPos.z = FlameDirection.transform.position.z;
            FlameDirection.LookAt(lookPos);

            //Finding Damageable Targets
            FlameDirection.Find("TriggerZone").GetComponent<Flame_Script>().DamageTargets(flameDmg);
        }
        if (Input.GetKeyUp(flame))
        {
            FlameDirection.Find("FlameFX").GetComponent<ParticleSystem>().Stop();
        }
        #endregion

        #region Lightning
        if (Input.GetKeyDown(lightning) && canBolt)
        {
            CastLightning();
        }
        #endregion
        #endregion
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            grounded = true;
        }
    }

    public void Damage(float Damage)
    {
        currentHealth -= Damage;
        if (currentHealth < 0)
        {
            SceneManager.LoadScene("Playground");
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

    IEnumerator StartLightningCD()
    {
        canBolt = false;
        yield return new WaitForSeconds(lightningCD);
        canBolt = true;
    }

    IEnumerator CameraShake(Vector3[] Positions, Vector3 Origin)
    {
        foreach (Vector3 pos in Positions)
        {
            float temp2 = 0;
            while (temp2 < 1)
            {
                Vector3 tempPos = Vector3.Lerp(new Vector3(myCam.localPosition.x, myCam.localPosition.y, pos.z), pos, temp2);
                tempPos.z = Origin.z;
                myCam.localPosition = tempPos;
                temp2 += Time.deltaTime*5;
                yield return null;
            }
            //yield return new WaitForSeconds(.5f);
        }
        float temp = 0;
        while (temp < 1)
        {
            temp += Time.deltaTime * 5;
            myCam.localPosition = Vector3.Lerp(myCam.localPosition, Origin, temp);
            yield return null;
        }
    }
    public void AddAggression(GameObject ctx)
    {
        Aggro.Add(ctx.GetComponent<Monster>());
    }

    void CastLightning()
    {
        StartCoroutine(StartLightningCD());
        Vector3 targetPos = Vector3.zero;
        RaycastHit2D info = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.down, 10f);
        targetPos = info.point;
        foreach(Monster m in MonsterRef.MonstersInRange(targetPos, lightningRange))
        {
            m.Damage(lightningDmg);
        }
    }
}
