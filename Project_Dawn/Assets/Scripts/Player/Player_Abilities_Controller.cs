using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Abilities_Controller : MonoBehaviour {
  public delegate void SpellCast();
  public SpellCast[] skillSet;
  Ability[] abilityList;

  [SerializeField]
  GameObject target;
  float globalCooldownDuration, globalCooldown = .5f;
  Player_Controller controller;
  Player_UI_Controller uiController;

  [SerializeField]
  bool isGCD = false;

  public GameObject Target{
    get{
      return target;
    }
    set {
      target = value;
    }
  }

	// Use this for initialization
	void Start () {
    skillSet = new SpellCast[] { GetComponent<Hemorage>().Activate, GetComponent<RendingGust>().Activate, GetComponent<AdrenalineRush>().Activate,
                                GetComponent<BloodVampyrism>().Activate, GetComponent<Terrify>().Activate, GetComponent<Exsanguinate>().Activate };
    abilityList = GetComponents<Ability>();
    uiController = GetComponent<Player_UI_Controller>();
    controller = GetComponent<Player_Controller>();
  }
	
	// Update is called once per frame
	void Update () {
    if (!controller.isInUI)
    {
      if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Mouse0))
      {
        SelectTarget();
      }
      else if (Input.GetKeyDown(KeyCode.Mouse0) && !isGCD)
      {
        skillSet[0].Invoke();
        isGCD = true;
      }
      if (Input.GetKeyDown(KeyCode.Mouse1) && !isGCD)
      {
        skillSet[1].Invoke();
        isGCD = true;
      }
      if (Input.GetKeyDown(KeyCode.Alpha1) && !isGCD)
      {
        skillSet[2].Invoke();
        isGCD = true;
      }
      if (Input.GetKeyDown(KeyCode.Alpha2) && !isGCD)
      {
        skillSet[3].Invoke();
        isGCD = true;
      }
      if (Input.GetKeyDown(KeyCode.Alpha3) && !isGCD)
      {
        skillSet[4].Invoke();
        isGCD = true;
      }
      if (Input.GetKeyDown(KeyCode.Alpha4) && !isGCD)
      {
        skillSet[5].Invoke();
        isGCD = true;
      }
    }

    if (isGCD){
      globalCooldownDuration += Time.deltaTime;
      if (globalCooldownDuration > globalCooldown){
        globalCooldownDuration = 0;
        isGCD = false;
      }
    }
  }

  public void SetAbility(Ability skill){
    skillSet[skill.Slot] = skill.Activate;
    print("Skill is now " + skill.Name);
  }

  public Ability GetAbility(string name)
  {
    foreach(Ability a in abilityList){
      if (a.Name == name){
        return a;
      }
    }
    print("Defaulting to Hemorage!");
    return GetComponent<Hemorage>();
  }

  public void SelectTarget()
  {
    RaycastHit2D info = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);
    if (info.transform != null && info.transform.tag == "Enemy")
    {
      if (target != null)
      {
        target.transform.Find("Sprite").GetComponent<SpriteRenderer>().material.SetFloat("_Threshold", 0f);
      }
      target = info.transform.gameObject;
      target.transform.Find("Sprite").GetComponent<SpriteRenderer>().material.SetFloat("_Threshold", .05f);
    } else if (info.transform == null && target != null){
      target.transform.Find("Sprite").GetComponent<SpriteRenderer>().material.SetFloat("_Threshold", 0f);
      target = null;
    }
  }

  public void AccelerateCooldownRate(float val){ 
    foreach (Ability t in GetComponents<Ability>()){
      print(t.Name);
      t.cooldownRate += val;
    }
  }
}
