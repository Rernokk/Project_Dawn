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
  public int power = 10, defense = 10, level = 1;
  public int currentExp, TotalExp;
  
  public float playerSpeed = 1f, verticalJump = 1f;

  [HideInInspector]
  public float teleportRange = 1f;
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
  [HideInInspector]
  public bool isInUI = false;
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
  public Vector2 dir;

  [HideInInspector]
  public List<string> BuffNames;
  #endregion
  #endregion

  #region Properties
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
    power = myGear.GetTotalPower();
    defense = myGear.GetTotalDefense();
    TotalExp = 100;
    currentExp = 0;
    currentHealth = TotalHealth;
    currentMana = TotalMana;
    uiController.UpdateHealthValue();
    uiController.UpdateManaValue();
    uiController.UpdateExpValue();
    uiController.SetAbilityCooldown(0, GetComponent<AdrenalineRush>().GetCooldownRemaining);
    uiController.SetAbilityCooldown(1, GetComponent<AdrenalineRush>().GetCooldownRemaining);
    uiController.SetAbilityCooldown(2, GetComponent<AdrenalineRush>().GetCooldownRemaining);
    uiController.SetAbilityCooldown(3, GetComponent<AdrenalineRush>().GetCooldownRemaining);
  }
  void Update()
  {
    if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton10))
    {
      SceneManager.LoadScene("Instructions");
    }

    //Developer
    if (Input.GetKeyDown(KeyCode.J)){
      level = 10;
      uiController.UpdateLevel();
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
        Time.timeScale = 0;
      }
      else
      {
        isInUI = false;
        Time.timeScale = 1;
        uiController.ToggleUIElementOff("Inventory");
      }
    }

    if (Input.GetKeyDown(skillsKey)){
      if (uiController.IsElementActive("Skills") != 1){
        uiController.ToggleOffAllElements();
        uiController.ToggleUIElementOn("Skills");
        isInUI = true;
        Time.timeScale = 0;
      } else {
        isInUI = false;
        Time.timeScale = 1;
        uiController.ToggleUIElementOff("Skills");
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

    uiController.UpdateSkills();
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
    power = myGear.GetTotalPower();
    defense = myGear.GetTotalDefense();
    uiController.UpdateStats(power, defense);
    return temp;
  }
  public void AddBuff(IEnumerator buff, string buffName)
  {
    if (BuffNames.IndexOf(buffName) == -1)
    {
      StartCoroutine(buff);
      BuffNames.Add(buffName);
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
