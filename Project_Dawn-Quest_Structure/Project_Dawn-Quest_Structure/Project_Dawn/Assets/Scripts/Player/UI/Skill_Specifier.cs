using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Specifier : MonoBehaviour
{
  [SerializeField]
  int slotNumber, skillNumber, levelReq = -1;
  public void PickSkillSlot(int slot)
  {
    slotNumber = slot;
  }
  public void SetSkill(int number)
  {
    skillNumber = number;
  }

  private void Start()
  {
    StartCoroutine(Delay());
  }

  IEnumerator Delay(){
    yield return new WaitForSeconds(.1f);
    UpdateInteractive(GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Controller>().level);
  }

  public void UpdateInteractive(int level){
    if (level >= levelReq){
      GetComponent<Button>().interactable = true;
    } else {
      GetComponent<Button>().interactable = false;
    }
  }
}
