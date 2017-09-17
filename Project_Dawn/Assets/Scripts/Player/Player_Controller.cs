﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player_Controller : MonoBehaviour
{
    #region KeyCodes
    KeyCode left = KeyCode.A, right = KeyCode.D, jump = KeyCode.Space,
        shift = KeyCode.LeftShift, lmb = KeyCode.Mouse0, stealth = KeyCode.F, rmb = KeyCode.Mouse1,
        one = KeyCode.Alpha1, two = KeyCode.Alpha2, three = KeyCode.Alpha3, four = KeyCode.Alpha4;
    #endregion
    #region Floats
    public int Power, Defense;
    //Regular values
    [SerializeField]
    float playerSpeed = 1f, verticalJump = 1f, stealthDuration = .5f, explosionRange = 5f, lightningRange = 1f;
    float teleportRange = 1f;

    //Damage Ratios
    [SerializeField]
    float flameRatio = 8.28f, explosionRatio = 12f, lightningRatio = 15f;

    //Cooldowns
    float dashCD = 1f, stealthCD = 1f, lightningCD = 1f;
    float gravScale = 1f;

    [SerializeField]
    float TotalMana = 100, TotalHealth = 100;

    [SerializeField]
    float currentMana, currentHealth;

    public float HealthPercent
    {
        get
        {
            return currentHealth / TotalHealth;
        }
    }

    public float ManaPercent
    {
        get
        {
            return currentMana / TotalMana;
        }
    }


    #endregion
    #region Bools
    [SerializeField]
    bool grounded = true, isStealth = false;
    bool canStealth = true, canTeleport = true, canDash = true, canBolt = true;
    [SerializeField]
    bool isInUI = false;
    #endregion
    #region GameObjects
    [SerializeField]
    GameObject MyTelePrefab, MyDetonatePrefab, LightningChain, LightningDetonate;
    GameObject myTemporaryTeleport;

    //Skill Prefabs
    [SerializeField]
    GameObject flamePrefab;
    GameObject myInventoryUI;
    #endregion
    #region Misc
    Rigidbody2D rgd;

    [HideInInspector]
    public Vector3 direction;
    BoxCollider2D myColl;
    Transform FlameDirection;
    Transform myCam;

    [SerializeField]
    List<Monster> Aggro;
    Monster_List_Ref MonsterRef;
    Player_UI_Controller uiController;

    [SerializeField]
    Equipment myGear;
    public List<Item> myInventory;
    Skill lmbSkill, rmbSkill;
    #endregion

    void Start()
    {
        #region Setup
        rgd = GetComponent<Rigidbody2D>();
        gravScale = rgd.gravityScale;
        direction = transform.right;
        myColl = GetComponent<BoxCollider2D>();
        currentMana = TotalMana;
        currentHealth = TotalHealth;
        Aggro = new List<Monster>();
        //FlameDirection = transform.Find("FlameAnchor").transform;
        //FlameDirection.Find("FlameFX").GetComponent<ParticleSystem>().Stop();
        myCam = transform.Find("Main Camera").transform;
        MonsterRef = GameObject.Find("Monster_Ref").GetComponent<Monster_List_Ref>();
        uiController = GameObject.Find("PlayerUI").GetComponent<Player_UI_Controller>();
        #endregion
        #region Example for Equipment
        myGear = new Equipment(new Helmet("Helm of Mystery", 1, 1), new Shoulders("Shoulders of Stuff", 1, 1), new Torso("Torso of Existing", 1, 1),
            new Gloves("Gloves of Grabbing", 1, 1), new Legs("Legs of Also Existing", 1, 1), new Boots("Shoes", 1, 1));
        UpdateStats();
        #endregion
        #region UI Setup
        myInventoryUI = uiController.transform.Find("Inventory").gameObject;
        myInventory = new List<Item>();
        myInventory.Add(new Helmet("Hellgate Helmet", 25, 10));
        myInventory.Add(new Helmet("Netherspawn Helmet", 25, 10));
        myInventory.Add(new Helmet("Demonguard Helmet", 25, 10));
        myInventory.Add(new Shoulders("Hellgate Pauldrons", 25, 10));
        myInventory.Add(new Shoulders("Netherspawn Pauldrons", 125, 10));
        myInventory.Add(new Shoulders("Demonguard Pauldrons", 125, 10));
        myInventory.Add(new Torso("Hellgate Chestplate", 25, 10));
        myInventory.Add(new Torso("Netherspawn Chestplate", 25, 10));
        myInventory.Add(new Torso("Demonguard Chestplate", 25, 10));
        myInventory.Add(new Gloves("Hellgate Gauntlets", 25, 10));
        myInventory.Add(new Gloves("Netherspawn Gauntlets", 25, 10));
        myInventory.Add(new Gloves("Demonguard Gauntlets", 25, 10));
        myInventory.Add(new Legs("Hellgate Greaves", 25, 10));
        myInventory.Add(new Legs("Netherspawn Greaves", 25, 10));
        myInventory.Add(new Legs("Demonguard Greaves", 25, 10));
        myInventory.Add(new Boots("Hellgate Sabatons", 25, 10));
        myInventory.Add(new Boots("Netherspawn Sabatons", 25, 10));
        myInventory.Add(new Boots("Demonguard Sabatons", 25, 10));
        #endregion
        #region Flame
        lmbSkill = new Flame();
        (lmbSkill as Flame).myPrefab = flamePrefab;
        #endregion

        #region Lightning Strike
        rmbSkill = new LightningStrike();
        (rmbSkill as LightningStrike).myPrefab = LightningChain;
        (rmbSkill as LightningStrike).cooldown = lightningCD;
        (rmbSkill as LightningStrike).range = lightningRange;
        (rmbSkill as LightningStrike).MonsterRef = MonsterRef;
        (rmbSkill as LightningStrike).Init(lightningRatio);
        #endregion
    }
    void Update()
    {
        if (!isInUI)
        {
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
                rgd.velocity += new Vector2(0, 1) * verticalJump;
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
            if (Input.GetKeyDown(shift) && canDash)
            {
                myTemporaryTeleport = Instantiate(MyTelePrefab, transform.position + direction * teleportRange, Quaternion.identity);
            }

            if (Input.GetKeyUp(shift) && canDash && myTemporaryTeleport != null)
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
                    m.Damage(explosionRatio * Power);
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
            if (Input.GetKeyDown(lmb)) {
                lmbSkill.Init(flameRatio);
            }

            if (Input.GetKey(lmb)) {
                lmbSkill.Cast(Power);
            }

            if (Input.GetKeyUp(lmb)) {
                Destroy(transform.Find("FlameAnchor(Clone)").gameObject);
            }
            #endregion
            #region Lightning
            if (Input.GetKeyDown(rmb))
            {
                rmbSkill.Cast(Power);
            }
            #endregion
            #endregion
        }

        #region UI Controls
        if (Input.GetKeyDown(KeyCode.I))
        {
            myInventoryUI.SetActive(!myInventoryUI.activeSelf);
            isInUI = myInventoryUI.activeSelf;
            if (isInUI)
            {
                uiController.GetComponent<Player_UI_Controller>().UpdateStats();
            }
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
    public void Damage(float Damage)
    {
        currentHealth -= Damage;
        uiController.UpdateHealthValue();
        if (currentHealth <= 0)
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
                temp2 += Time.deltaTime * 5;
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
    }
    public Item UpdateGear(Item newItem)
    {
        myInventory.Remove(newItem);
        Item temp = myGear.SwapItem(newItem);
        myInventory.Add(temp);
        UpdateStats();
        return temp;
    }
    void UpdateStats()
    {
        Power = myGear.GetTotalPower();
        Defense = myGear.GetTotalDefense();
    }
}
