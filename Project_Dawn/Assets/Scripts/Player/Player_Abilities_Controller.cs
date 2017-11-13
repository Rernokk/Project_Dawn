using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Abilities_Controller : MonoBehaviour {
  public delegate void SpellCast();
  public SpellCast firstSpell;
  public SpellCast secondSpell;
  public SpellCast thirdSpell;
  public SpellCast fourthSpell;
  public SpellCast primarySkill;
  public SpellCast secondarySkill;

  [SerializeField]
  GameObject target;
  float globalCooldownDuration, globalCooldown = .5f;

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
    primarySkill = GetComponent<Hemorage>().Activate;
    secondarySkill = GetComponent<RendingGust>().Activate;
    firstSpell = GetComponent<AdrenalineRush>().Activate;
    secondSpell = GetComponent<BloodVampyrism>().Activate;
    thirdSpell = GetComponent<Terrify>().Activate;
    fourthSpell = GetComponent<Exsanguinate>().Activate;
  }
	
	// Update is called once per frame
	void Update () {
    if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Mouse0)){
      SelectTarget();
    } else if (Input.GetKeyDown(KeyCode.Mouse0) && !isGCD){
      primarySkill.Invoke();
      isGCD = true;
    }
    if (Input.GetKeyDown(KeyCode.Mouse1) && !isGCD)
    {
      secondarySkill.Invoke();
      isGCD = true;
    }
    if (Input.GetKeyDown(KeyCode.Alpha1) && !isGCD)
    {
      firstSpell.Invoke();
      isGCD = true;
    }
    if (Input.GetKeyDown(KeyCode.Alpha2) && !isGCD)
    {
      secondSpell.Invoke();
      isGCD = true;
    }
    if (Input.GetKeyDown(KeyCode.Alpha3) && !isGCD)
    {
      thirdSpell.Invoke();
      isGCD = true;
    }
    if (Input.GetKeyDown(KeyCode.Alpha4) && !isGCD)
    {
      fourthSpell.Invoke();
      isGCD = true;
    }

    if (isGCD){
      globalCooldownDuration += Time.deltaTime;
      if (globalCooldownDuration > globalCooldown){
        globalCooldownDuration = 0;
        isGCD = false;
      }
    }
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
}
