using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Abilities_Controller : MonoBehaviour {
  delegate void SpellCast();
  SpellCast firstSpell;
  SpellCast secondSpell;
  SpellCast thirdSpell;
  SpellCast fourthSpell;
  SpellCast primarySkill;
  SpellCast secondarySkill;
	// Use this for initialization
	void Start () {
    gameObject.AddComponent<Hemorage>();
    primarySkill = GetComponent<Hemorage>().Activate;
    secondarySkill = GetComponent<Hemorage>().Activate;
    firstSpell = GetComponent<Hemorage>().Activate;
    secondSpell = GetComponent<Hemorage>().Activate;
    thirdSpell = GetComponent<Hemorage>().Activate;
    fourthSpell = GetComponent<Hemorage>().Activate;
  }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Mouse0)){
      primarySkill.Invoke();
    }
    if (Input.GetKeyDown(KeyCode.Mouse1))
    {
      secondarySkill.Invoke();
    }
    if (Input.GetKeyDown(KeyCode.Alpha1))
    {
      secondarySkill.Invoke();
    }
    if (Input.GetKeyDown(KeyCode.Alpha2))
    {
      secondarySkill.Invoke();
    }
    if (Input.GetKeyDown(KeyCode.Alpha3))
    {
      secondarySkill.Invoke();
    }
    if (Input.GetKeyDown(KeyCode.Alpha4))
    {
      secondarySkill.Invoke();
    }

    if (Input.GetKeyDown(KeyCode.T)){
      primarySkill += GetComponent<Hemorage>().AddFunctionality;
    }
  }
}
