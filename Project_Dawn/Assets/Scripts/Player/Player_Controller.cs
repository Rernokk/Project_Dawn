using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player_Controller : MonoBehaviour
{
  #region KeyCodes
  KeyCode left = KeyCode.A, right = KeyCode.D, jump = KeyCode.Space,
      lmb = KeyCode.Mouse0, rmb = KeyCode.Mouse1,
      one = KeyCode.Alpha1, two = KeyCode.Alpha2, three = KeyCode.Alpha3, four = KeyCode.Alpha4;
  #endregion
  #region Floats
  public int power, defense;
  public int currentExp, TotalExp;

  [SerializeField]
  float playerSpeed = 1f, verticalJump = 1f, stealthDuration = .5f, explosionRange = 5f, lightningRange = 1f;

  [HideInInspector]
  public float teleportRange = 1f;

  //Damage Ratios
  [SerializeField]
  float flameRatio = 8.28f, explosionRatio = 12f, lightningRatio = 15f;

  [HideInInspector]
  public float powerMult = 1f;

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
  bool grounded = true, isStealth = false;
  bool canStealth = true, canTeleport = true, canDash = true, canBolt = true, canHeal = true;
  bool isInUI = false;
  public bool stunned = false, immobile = false;
  bool cont = false;
  #endregion
  #region GameObjects
  [SerializeField]
  GameObject MyTelePrefab, MyDetonatePrefab, LightningChain, LightningDetonate;
  GameObject myTemporaryTeleport;

  //Skill Prefabs
  [SerializeField]
  GameObject flamePrefab;
  GameObject myInventoryUI;

  Dictionary<string, CanvasGroup> UIElements;
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
  Player_UI_Controller uiController;


  [SerializeField]
  Equipment myGear;
  public List<Item> myInventory;
  Skill lmbSkill, rmbSkill, fourthSkill, thirdSkill, secondSkill, firstSkill;
  List<List<Skill>> skillArray;
  #endregion

  public float Power{
    get{
      return power * powerMult;
    }
    set {
      power = (int) value;
    }
  }
  public float Defense{
    get {
      return defense;
    }

    set {
      defense = (int) value;
    }
  }
  public float PowerMult{
    get {
      return powerMult;
    }

    set {
      powerMult = value;
    }
  }

  void Start()
  {
    #region Setup
    rgd = GetComponent<Rigidbody2D>();
    gravScale = rgd.gravityScale;
    direction = transform.right;
    myColl = GetComponent<BoxCollider2D>();
    currentMana = TotalMana;
    currentHealth = 1;
    Aggro = new List<Monster>();
    skillArray = new List<List<Skill>>();
    for (int i = 0; i < 6; i++)
    {
      skillArray.Add(new List<Skill>());
    }

    //Populate Skill List
    skillArray[0].Add(new Teleport(this, 8f));
    skillArray[1].Add(new Stealth(this, 18f));
    skillArray[2].Add(new DamageAmp(this));
    skillArray[3].Add(new Medicate(this, .25f, 4f));
    skillArray[4].Add(new Flame(this));
    skillArray[5].Add(new LightningStrike(this, 3f, 1f, LightningChain));
    StartCoroutine(PopulateCurrentSkills());

    myCam = transform.Find("Main Camera").transform;
    uiController = GameObject.Find("PlayerUI").GetComponent<Player_UI_Controller>();
    #endregion
    #region Example for Equipment
    myGear = new Equipment(new Helmet("Helm of Mystery", 1, 1), new Shoulders("Shoulders of Stuff", 1, 1), new Torso("Torso of Existing", 1, 1),
        new Gloves("Gloves of Grabbing", 1, 1), new Legs("Legs of Also Existing", 1, 1), new Boots("Shoes", 1, 1));
    UpdateStats();
    #endregion
    #region UI Setup
    myInventoryUI = uiController.transform.Find("Inventory").gameObject;
    myInventory = new List<Item>
    {
      new Helmet("Hellgate Helmet", 25, 10),
      new Helmet("Netherspawn Helmet", 25, 10),
      new Helmet("Demonguard Helmet", 25, 10),
      new Shoulders("Hellgate Pauldrons", 25, 10),
      new Shoulders("Netherspawn Pauldrons", 125, 10),
      new Shoulders("Demonguard Pauldrons", 125, 10),
      new Torso("Hellgate Chestplate", 25, 10),
      new Torso("Netherspawn Chestplate", 25, 10),
      new Torso("Demonguard Chestplate", 25, 10),
      new Gloves("Hellgate Gauntlets", 25, 10),
      new Gloves("Netherspawn Gauntlets", 25, 10),
      new Gloves("Demonguard Gauntlets", 25, 10),
      //myInventory.Add(new Legs("Hellgate Greaves", 25, 10));
      new Legs("Netherspawn Greaves", 25, 10),
      new Legs("Demonguard Greaves", 25, 10),
      new Boots("Hellgate Sabatons", 25, 10),
      new Boots("Netherspawn Sabatons", 25, 10),
      new Boots("Demonguard Sabatons", 25, 10),
      new Boots("Slayer Sabatons", 225, 10)
    };
    uiController.GetComponent<Player_UI_Controller>().Populate("Helmet");
    #endregion
    TotalExp = 100;
    currentExp = 0;
  }
  void Update()
  {
    if (!isInUI && !stunned)
    {
      #region Movement
      if (!immobile)
      {
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
      }
      #endregion
      #region Skills
      #region First Skill Slot
      if (Input.GetKeyDown(one) && firstSkill.IsCooledDown)
      {
        firstSkill.Cast();
        StartCoroutine(SkillCooldown(firstSkill));
      }
      #endregion
      #region Second Skill Slot
      if (Input.GetKeyDown(two) && secondSkill.IsCooledDown)
      {
        secondSkill.Cast();
        StartCoroutine(SkillCooldown(secondSkill));
      }
      #endregion
      #region Third Skill Slot
      if (Input.GetKeyDown(three) && thirdSkill.IsCooledDown)
      {
        thirdSkill.Cast();
        StartCoroutine(SkillCooldown(thirdSkill));
      }
      #endregion
      #region Fourth Skill Slot
      if (Input.GetKeyDown(four) && fourthSkill.IsCooledDown)
      {
        fourthSkill.Cast(Power);
        StartCoroutine(SkillCooldown(fourthSkill));
      }
      #endregion
      #region LMB Slot
      if (Input.GetKeyDown(lmb) && lmbSkill.IsCooledDown)
      {
        lmbSkill.Cast(Power);
        StartCoroutine(SkillCooldown(lmbSkill));
      }
      #endregion
      #region RMB Slot
      if (Input.GetKeyDown(rmb) && rmbSkill.IsCooledDown)
      {
        rmbSkill.Cast(Power);
        StartCoroutine(SkillCooldown(rmbSkill));
      }
      #endregion
      #endregion
      if (Input.anyKeyDown && currentHealth <= 0)
      {
        cont = true;
      }
    }

    if (currentHealth < TotalHealth)
    {
      currentHealth += 5 * Time.deltaTime;
      if (currentHealth > TotalHealth)
      {
        currentHealth = TotalHealth;
      }
    }

    #region UI Controls
    if (Input.GetKeyDown(KeyCode.I))
    {
      if (uiController.isElementActive("Inventory") != 1)
      {
        uiController.ToggleOffAllElements();
        uiController.ToggleUIElementOn("Inventory");
        isInUI = true;
      }
      else
      {
        isInUI = false;
        uiController.ToggleUIElementOff("Inventory");
      }
    }

    if (Input.GetKeyDown(KeyCode.Escape))
    {
      if (uiController.isElementActive("Instructions") != 1)
      {
        uiController.ToggleOffAllElements();
        uiController.ToggleUIElementOn("Instructions");
        isInUI = true;
      }
      else
      {
        isInUI = false;
        uiController.ToggleUIElementOff("Instructions");
      }
    }

    if (Input.GetKeyDown(KeyCode.K))
    {
      if (uiController.isElementActive("Skills") != 1)
      {
        uiController.ToggleOffAllElements();
        uiController.ToggleUIElementOn("Skills");
        isInUI = true;
      }
      else
      {
        uiController.ToggleUIElementOff("Skills");
        isInUI = false;
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
      GameObject.Find("BossRank").GetComponent<Boss_Ranking>().YouFailed();
      StartCoroutine(WaitForInputScreen());
    }
  }
  public void Heal(float heal)
  {
    currentHealth += heal;
    if (currentHealth > TotalHealth)
    {
      currentHealth = TotalHealth;
    }
  }
  IEnumerator WaitForInputScreen()
  {
    while (!cont)
    {
      yield return null;
    }
    SceneManager.LoadScene("Playground");
  }
  IEnumerator PopulateCurrentSkills()
  {
    yield return null;
    for (int i = 0; i < 6; i++)
    {
      SetSkillActive(i, 0);
    }
    UpdateStats();
  }
  IEnumerator SkillCooldown(Skill skill)
  {
    skill.IsCooledDown = false;
    float remaining = 0;
    while (remaining < skill.CooldownDuration){
      remaining += Time.deltaTime;
      yield return null;
    }
    skill.IsCooledDown = true;
  }
  public void AddAggression(GameObject ctx)
  {
    Aggro.Add(ctx.GetComponent<Monster>());
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
  public void SetSkillActive(int row, int col)
  {
    switch (row)
    {
      case (0):
        firstSkill = skillArray[row][col];
        break;
      case (1):
        secondSkill = skillArray[row][col];
        break;
      case (2):
        thirdSkill = skillArray[row][col];
        break;
      case (3):
        fourthSkill = skillArray[row][col];
        break;
      case (4):
        //LMB
        lmbSkill = skillArray[row][col];
        break;
      case (5):
        //RMB
        rmbSkill = skillArray[row][col];
        break;
    }
  }
}
