using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Specifier : MonoBehaviour {
  int slotNumber, skillNumber;
  public void PickSkillSlot(int slot)
  {
    slotNumber = slot;
  }
  public void SetSkill(int number)
  {
    skillNumber = number;
  }
}
