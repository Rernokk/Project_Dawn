using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player_Controller : MonoBehaviour
{
  #region Variables
  #region KeyCodes
  //Normal Binds
  KeyCode left = KeyCode.A, right = KeyCode.D, jump = KeyCode.Space,
      lmb = KeyCode.Mouse0, rmb = KeyCode.Mouse1,
      one = KeyCode.Alpha1, two = KeyCode.Alpha2, three = KeyCode.Alpha3, four = KeyCode.Alpha4;
  KeyCode inventoryKey = KeyCode.I, skillsKey = KeyCode.K, creditsKey = KeyCode.C;
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
  float s1CD, s2CD, s3CD, s4CD;

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
  GameObject MyTelePrefab, MyDetonatePrefab, LightningChain, HealFX, FlameWakeFX, FireballProjectilePrefab, ChainPrefab, SelectionPrefab, PointPrefab, myLevelPrefab;
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
  public GameObject uiControllerRef;


  [SerializeField]
  Equipment myGear;
  public List<Item> myInventory;

  [HideInInspector]
  public Skill lmbSkill, rmbSkill, fourthSkill, thirdSkill, secondSkill, firstSkill;

  List<List<Skill>> skillArray;
  public Vector2 dir;

  [HideInInspector]
  public List<string> BuffNames;
  #endregion
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
  public float FirstSkillCooldown
  {
    get {
      if (firstSkill == null)
      {
        return .5f;
      }
      return 1 - s1CD / firstSkill.CooldownDuration;
    }
  }
  public float SecondSkillCooldown{
    get
    {
      if (secondSkill == null)
      {
        return .5f;
      }
      return 1 - s2CD / secondSkill.CooldownDuration;
    }
  }
  public float ThirdSkillCooldown{
    get
    {
      if (thirdSkill == null)
      {
        return .5f;
      }
      return 1 - s3CD / thirdSkill.CooldownDuration;
    }
  }
  public float FourthSkillCooldown{
    get
    {
      if (fourthSkill == null)
      {
        return .5f;
      }
      return 1 - s4CD / fourthSkill.CooldownDuration;
    }
  }
  #endregion

  #region Methods
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

    if (PersistantVariables.isControllerConnected)
    {
      jump = KeyCode.JoystickButton1;
      lmb = KeyCode.JoystickButton4;
      rmb = KeyCode.JoystickButton5;
      one = KeyCode.JoystickButton6;
      two = KeyCode.JoystickButton7;
      three = KeyCode.JoystickButton0;
      four = KeyCode.JoystickButton3;
      inventoryKey = KeyCode.JoystickButton2;
      skillsKey = KeyCode.JoystickButton8;
      creditsKey = KeyCode.JoystickButton9;
    }
    else
    {
      //Movement
      if (PersistantVariables.Instance.currentBinds == KeybindSettings.NORMAL || PersistantVariables.Instance.currentBinds == KeybindSettings.KEYBOARDONLY)
      {
        left = KeyCode.A; right = KeyCode.D; jump = KeyCode.Space;
      }
      else
      {
        //Southpaw
        left = KeyCode.J; right = KeyCode.L; jump = KeyCode.Space;
      }

      //Primary Skills
      if (PersistantVariables.Instance.currentBinds == KeybindSettings.NORMAL)
      {
        lmb = KeyCode.Mouse0; rmb = KeyCode.Mouse1;
      }
      else if (PersistantVariables.Instance.currentBinds == KeybindSettings.SOUTHPAW)
      {
        //Southpaw
        lmb = KeyCode.Mouse1; rmb = KeyCode.Mouse0;
      }
      else
      {

        lmb = KeyCode.LeftArrow; rmb = KeyCode.RightArrow;
      }

      //Skill Binds
      if (PersistantVariables.Instance.currentBinds == KeybindSettings.NORMAL || PersistantVariables.Instance.currentBinds == KeybindSettings.KEYBOARDONLY)
      {
        one = KeyCode.Alpha1; two = KeyCode.Alpha2; three = KeyCode.Alpha3; four = KeyCode.Alpha4;
      }
      else
      {
        //Southpaw
        one = KeyCode.Alpha7; two = KeyCode.Alpha8; three = KeyCode.Alpha9; four = KeyCode.Alpha0;
      }

      //Interface Binds
      if (PersistantVariables.Instance.currentBinds == KeybindSettings.NORMAL || PersistantVariables.Instance.currentBinds == KeybindSettings.KEYBOARDONLY)
      {
        inventoryKey = KeyCode.I;
        skillsKey = KeyCode.K;
        creditsKey = KeyCode.C;
      }
      else
      {
        inventoryKey = KeyCode.W;
        skillsKey = KeyCode.S;
        creditsKey = KeyCode.Comma;
      }
    }
    for (int i = 0; i < 6; i++)
    {
      skillArray.Add(new List<Skill>());
    }
    dir = transform.right;

    myCam = transform.Find("Main Camera").transform;
    if (GameObject.Find("PlayerUI"))
    {
      uiController = GameObject.Find("PlayerUI").GetComponent<Player_UI_Controller>();
    } else {
      uiController = Instantiate(uiControllerRef, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Player_UI_Controller>();
    }
    uiController.GetComponent<Canvas>().worldCamera = myCam.GetComponent<Camera>();
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
    if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton10))
    {
      SceneManager.LoadScene("Instructions");
    }

    if (!isInUI && !stunned)
    {
      #region Movement
      if (!immobile)
      {
        #region Left/Right
        if ((!PersistantVariables.isControllerConnected && Input.GetKey(left)) || (PersistantVariables.isControllerConnected && Input.GetAxis("Horizontal") < 0))
        {
          direction = -transform.right;
          dir = -transform.right;
          rgd.AddForce(direction * Time.deltaTime * playerSpeed, ForceMode2D.Impulse);
          pointyHat.flipX = true;
          print("Left");
        }
        if ((!PersistantVariables.isControllerConnected && Input.GetKey(right)) || (PersistantVariables.isControllerConnected && Input.GetAxis("Horizontal") > 0))
        {
          direction = transform.right;
          dir = transform.right;
          rgd.AddForce(direction * Time.deltaTime * playerSpeed, ForceMode2D.Impulse);
          pointyHat.flipX = false;
        }
        #endregion
        #region Jumping
        if (Input.GetKey(jump) && grounded)
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
<<<<<<< HEAD
      #region Skills
      /*
      #region First Skill Slot
      if ((Input.GetKeyDown(one) && firstSkill.IsCooledDown && firstSkill.ManaCost <= currentMana && Level >= firstSkill.levelReq) || chainMode)
      {
        //StartCoroutine(SkillCooldown(firstSkill));
        if (!chainMode)
        {
          currentMana -= firstSkill.ManaCost;
        }
        firstSkill.Cast(Power);
        s1CD = firstSkill.CooldownDuration;
      }
      #endregion
      #region Second Skill Slot
      if (Input.GetKeyDown(two) && secondSkill.IsCooledDown && secondSkill.ManaCost <= currentMana && Level >= secondSkill.levelReq)
      {
        secondSkill.Cast(Power);
        //StartCoroutine(SkillCooldown(secondSkill));
        currentMana -= secondSkill.ManaCost;
        s2CD = secondSkill.CooldownDuration;
      }
      #endregion
      #region Third Skill Slot
      if (Input.GetKeyDown(three) && thirdSkill.IsCooledDown && thirdSkill.ManaCost <= currentMana && Level >= thirdSkill.levelReq)
      {
        thirdSkill.Cast(Power);
        //StartCoroutine(SkillCooldown(thirdSkill));
        currentMana -= thirdSkill.ManaCost;
        s3CD = thirdSkill.CooldownDuration;
      }
      #endregion
      #region Fourth Skill Slot
      if (Input.GetKeyDown(four) && fourthSkill.IsCooledDown && fourthSkill.ManaCost <= currentMana && Level >= fourthSkill.levelReq)
      {
        fourthSkill.Cast(Power);
        //StartCoroutine(SkillCooldown(fourthSkill));
        currentMana -= fourthSkill.ManaCost;
        s4CD = fourthSkill.CooldownDuration;
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

      if (firstSkill != null && !firstSkill.IsCooledDown){
        s1CD -= Time.deltaTime;
        if (s1CD < 0){
          s1CD = 0;
        }
      }
      if (secondSkill != null && !secondSkill.IsCooledDown){
        s2CD -= Time.deltaTime;
        if (s2CD < 0){
          s2CD = 0;
        }
      }
      if (thirdSkill != null && !thirdSkill.IsCooledDown){
        s3CD -= Time.deltaTime;
        if (s3CD < 0){
          s3CD = 0;
        }
      }
      if (fourthSkill != null && !fourthSkill.IsCooledDown){
        s4CD -= Time.deltaTime;
        if (s4CD < 0){
          s4CD = 0;
        }
      }*/
      #endregion
=======

>>>>>>> ae2d1d71ad638f2719d7ecf47a9ba2412e783d63
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
    if (Input.GetKeyDown(inventoryKey))
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

    if (Input.GetKeyDown(creditsKey))
    {
      SceneManager.LoadScene("Credits");
    }

    if (Input.GetKeyDown(KeyCode.JoystickButton11) || Input.GetKeyDown(KeyCode.Q))
    {
      print("Quitting");
      Application.Quit();
    }
    #endregion
    if (PersistantVariables.Instance.currentBinds != KeybindSettings.KEYBOARDONLY && !PersistantVariables.isControllerConnected)
    {
      Direction = new Vector2(Mathf.Sign(transform.position.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x), 0);
    }

    if (currentExp >= TotalExp)
    {
      currentExp -= TotalExp;
      level++;
      currentMana = TotalMana;
      currentHealth = TotalHealth;
      uiController.UpdateLevel();
      Destroy(Instantiate(myLevelPrefab, transform), 3f);
      StartCoroutine(DelayedParticleStop(uiController.transform.Find("Player_HUD/Level/GameObject/Particle System").GetComponent<ParticleSystem>(), 3));
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
  
  IEnumerator DelayedParticleStop(ParticleSystem system, float time)
  {
    system.Play();
    yield return new WaitForSeconds(time);
    system.Stop();
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
  public void ChainDelay()
  {
    StartCoroutine(CallFrameDelay());
  }
  IEnumerator CallFrameDelay()
  {
    yield return null;
    chainMode = false;
  }
  #endregion
}
