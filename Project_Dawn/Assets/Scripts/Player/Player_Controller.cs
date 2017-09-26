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
  public int power, defense, level = 1;
  public int currentExp, TotalExp;

  [SerializeField]
  float playerSpeed = 1f, verticalJump = 1f, stealthDuration = .5f, explosionRange = 5f, lightningRange = 1f, FlameWakeSpeed = 1f;

  [HideInInspector]
  public float teleportRange = 1f;

  //Damage Ratios
  [SerializeField]
  float flameRatio = 8.28f, explosionRatio = 12f, lightningRatio = 15f;

  [HideInInspector]
  public float powerMult = 1f, defenseMult = 1f;

  //Cooldowns
  [Space(15f)]
  [Header("Skill Cooldowns")]
  [SerializeField]
  float dashCD = 1f;
  [SerializeField]
  float stealthCD = 1f, lightningCD = 1f, FlameWakeCD = 1f;
  float gravScale = 1f;

  [Space(15f)]
  [SerializeField]
  float TotalMana = 100;
  [SerializeField]
  float TotalHealth = 100;
  [SerializeField]
  float currentMana, currentHealth;

  #endregion
  #region Bools
  bool grounded = true, isStealth = false;
  bool canStealth = true, canTeleport = true, canDash = true, canBolt = true, canHeal = true;
  bool isInUI = false;
  public bool chainMode = false;
  public bool stunned = false, immobile = false;
  bool cont = false;
  #endregion
  #region GameObjects
  [SerializeField]
  GameObject MyTelePrefab, MyDetonatePrefab, LightningChain, HealFX, FlameWakeFX, FireballProjectilePrefab, ChainPrefab;
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
  SpriteRenderer pointyHat;

  [SerializeField]
  List<Monster> Aggro;
  [HideInInspector]
  public Player_UI_Controller uiController;


  [SerializeField]
  Equipment myGear;
  public List<Item> myInventory;
  Skill lmbSkill, rmbSkill, fourthSkill, thirdSkill, secondSkill, firstSkill;
  List<List<Skill>> skillArray;
  Vector2 dir;

  [HideInInspector]
  public List<string> BuffNames;
  #endregion

  #region Properties
  public float Power
  {
    get
    {
      return power * powerMult;
    }
    set
    {
      power = (int)value;
    }
  }
  public float Defense
  {
    get
    {
      return defense * defenseMult;
    }

    set
    {
      defense = (int)value;
    }
  }
  public float PowerMult
  {
    get
    {
      return powerMult;
    }

    set
    {
      powerMult = value;
    }
  }
  public Vector2 Direction
  {
    get
    {
      return dir;
    }

    set
    {
      dir = value;
    }
  }
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
  public int Level
  {
    get
    {
      return level;
    }
  }
  #endregion
  //Methods
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
    BuffNames = new List<string>();
    pointyHat = transform.Find("Pointy_Hat").GetComponent<SpriteRenderer>();
    for (int i = 0; i < 6; i++)
    {
      skillArray.Add(new List<Skill>());
    }
    dir = transform.right;

    //Populate Skill List
    skillArray[0].Add(new Chain_Grip(this, ChainPrefab, 10, 3f));
    skillArray[0].Add(new Flamewake(this, 25, FlameWakeFX, 2f, 10));
    skillArray[1].Add(new Flamewake(this, 25, FlameWakeFX, 2f, 10));
    skillArray[1].Add(new Flamewake(this, 25, FlameWakeFX, 2f, 10));
    skillArray[2].Add(new Shield(this, 15, 12f));
    skillArray[2].Add(new DamageAmp(this, 8, 2, 20f));
    skillArray[3].Add(new Medicate(this, 12, .25f, 4f, HealFX));
    skillArray[3].Add(new Flamewake(this, 25, FlameWakeFX, 2f, 10));
    skillArray[4].Add(new Flame(this, FireballProjectilePrefab, 5, 12, 1f));
    skillArray[4].Add(new Flamewake(this, 25, FlameWakeFX, 2f, 2, FlameWakeCD, FlameWakeSpeed));
    skillArray[5].Add(new Flamewake(this, 25, FlameWakeFX, 2f, 10));
    skillArray[5].Add(new LightningStrike(this, 2, 40, 5f, 9f, LightningChain));
    StartCoroutine(PopulateCurrentSkills());

    myCam = transform.Find("Main Camera").transform;
    uiController = GameObject.Find("PlayerUI").GetComponent<Player_UI_Controller>();
    #endregion
    #region Example for Equipment
    myGear = new Equipment(new Helmet("Helm of Mystery", 1, 1), new Shoulders("Shoulders of Stuff", 1, 1), new Torso("Torso of Existing", 1, 1),
        new Gloves("Gloves of Grabbing", 1, 1), new Legs("Legs of Also Existing", 1, 1), new Boots("Shoes", 1, 1));
    #endregion
    #region UI Setup
    myInventoryUI = uiController.transform.Find("Inventory").gameObject;
    myInventory = new List<Item>
    {
      new Helmet("Hellgate Helmet", 25, 10),
      new Helmet("Netherspawn Helmet", 25, 10),
      new Helmet("Demonguard Helmet", 25, 10),
      new Shoulders("Hellgate Pauldrons", 25, 10),
      //new Shoulders("Netherspawn Pauldrons", 125, 10),
      //new Shoulders("Demonguard Pauldrons", 125, 10),
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
      //new Boots("Slayer Sabatons", 225, 10)
    };
    uiController.GetComponent<Player_UI_Controller>().Populate("Helmet");
    #endregion
    TotalExp = 100;
    currentExp = 0;
    currentHealth = TotalHealth;
    currentMana = TotalMana;
    uiController.UpdateHealthValue();
    uiController.UpdateManaValue();
    uiController.UpdateExpValue();
    uiController.UpdateStats();
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
          pointyHat.flipX = true;
        }
        if (Input.GetKey(right))
        {
          direction = transform.right;
          rgd.AddForce(direction * Time.deltaTime * playerSpeed, ForceMode2D.Impulse);
          pointyHat.flipX = false;
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
      if ((Input.GetKeyDown(one) && firstSkill.IsCooledDown && firstSkill.ManaCost <= currentMana && Level >= firstSkill.levelReq) || chainMode)
      {
        //StartCoroutine(SkillCooldown(firstSkill));
        if (!chainMode)
        {
          currentMana -= firstSkill.ManaCost;
        }
        firstSkill.Cast(Power);
      }
      #endregion
      #region Second Skill Slot
      if (Input.GetKeyDown(two) && secondSkill.IsCooledDown && secondSkill.ManaCost <= currentMana && Level >= secondSkill.levelReq)
      {
        secondSkill.Cast(Power);
        //StartCoroutine(SkillCooldown(secondSkill));
        currentMana -= secondSkill.ManaCost;
      }
      #endregion
      #region Third Skill Slot
      if (Input.GetKeyDown(three) && thirdSkill.IsCooledDown && thirdSkill.ManaCost <= currentMana && Level >= thirdSkill.levelReq)
      {
        thirdSkill.Cast(Power);
        //StartCoroutine(SkillCooldown(thirdSkill));
        currentMana -= thirdSkill.ManaCost;
      }
      #endregion
      #region Fourth Skill Slot
      if (Input.GetKeyDown(four) && fourthSkill.IsCooledDown && fourthSkill.ManaCost <= currentMana && Level >= fourthSkill.levelReq)
      {
        fourthSkill.Cast(Power);
        //StartCoroutine(SkillCooldown(fourthSkill));
        currentMana -= fourthSkill.ManaCost;
      }
      #endregion
      #region LMB Slot
      if (Input.GetKeyDown(lmb) && lmbSkill.IsCooledDown && lmbSkill.ManaCost <= currentMana && Level >= lmbSkill.levelReq && !chainMode)
      {
        lmbSkill.Cast(Power);
        //StartCoroutine(SkillCooldown(lmbSkill));
        currentMana -= lmbSkill.ManaCost;
      }
      #endregion
      #region RMB Slot
      if (Input.GetKeyDown(rmb) && rmbSkill.IsCooledDown && rmbSkill.ManaCost <= currentMana && Level >= rmbSkill.levelReq)
      {
        rmbSkill.Cast(Power);
        //StartCoroutine(SkillCooldown(rmbSkill));
        currentMana -= rmbSkill.ManaCost;
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

    if (currentMana < TotalMana)
    {
      currentMana += 10 * Time.deltaTime;
      if (currentMana > TotalMana)
      {
        currentMana = TotalMana;
      }
    }

    #region UI Controls
    if (Input.GetKeyDown(KeyCode.I))
    {
      if (uiController.IsElementActive("Inventory") != 1)
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
      /*
      if (uiController.IsElementActive("Instructions") != 1)
      {
        uiController.ToggleOffAllElements();
        uiController.ToggleUIElementOn("Instructions");
        isInUI = true;
      }
      else
      {
        isInUI = false;
        uiController.ToggleUIElementOff("Instructions");
      }*/
      SceneManager.LoadScene("Instructions");
    }

    if (Input.GetKeyDown(KeyCode.K))
    {
      if (uiController.IsElementActive("Skills") != 1)
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

    if (Input.GetKeyDown(KeyCode.Q))
    {
      print("Quitting");
      Application.Quit();
    }
    #endregion
    Direction = new Vector2(Mathf.Sign(transform.position.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x), 0);
    if (currentExp >= TotalExp)
    {
      currentExp -= TotalExp;
      level++;
      currentMana = TotalMana;
      currentHealth = TotalHealth;
      uiController.UpdateLevel();
    }
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
    uiController.UpdateSkills();
  }
  IEnumerator SkillCooldown(Skill skill)
  {
    skill.IsCooledDown = false;
    float remaining = 0;
    while (remaining < skill.CooldownDuration)
    {
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
  public Skill SetSkillActive(int row, int col)
  {
    switch (row)
    {
      case (0):
        firstSkill = skillArray[row][col];
        return firstSkill;
      case (1):
        secondSkill = skillArray[row][col];
        return secondSkill;
      case (2):
        thirdSkill = skillArray[row][col];
        return thirdSkill;
      case (3):
        fourthSkill = skillArray[row][col];
        return fourthSkill;
      case (4):
        //LMB
        lmbSkill = skillArray[row][col];
        return lmbSkill;
      case (5):
        //RMB
        rmbSkill = skillArray[row][col];
        return rmbSkill;
      default:
        return null;
    }
  }
  public void AddBuff(IEnumerator buff, string buffName)
  {
    if (BuffNames.IndexOf(buffName) == -1)
    {
      StartCoroutine(buff);
      BuffNames.Add(buffName);
      uiController.UpdateStats();
    }
  }
  public void StartCooldown(Skill skill){
    StartCoroutine(SkillCooldown(skill));
  }
  public void ChainDelay(){
    StartCoroutine(callFrameDelay());
  }
  IEnumerator callFrameDelay(){
    yield return null;
    chainMode = false;
  }
}
