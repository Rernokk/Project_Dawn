using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour {
  protected Player_Controller playerController;
  protected Player_Abilities_Controller abilityController;
  
  [SerializeField]
  protected string skillName, description;

  //Slots are primary/secondary/first/second/third/fourth as 0 - 5.
  [SerializeField]
  protected int requiredLevel, slot;
  
  [SerializeField]
  protected bool isOnCooldown = false;
  protected float cooldownDuration, cooldown;

  protected float Power
  {
    get
    {
      return playerController.power;
    }
  }
  protected float Defense
  {
    get
    {
      return playerController.defense;
    }
  }
  public string Name{
    get {
      return skillName;
    }
    set {
      skillName = value;
    }
  }
  public int RequiredLevel {
    get {
      return requiredLevel;
    }
    set {
      requiredLevel = value;
    }
  }
  public string Description{
    get{
      return description;
    }

     set{
      description = value;
     }
  }
  protected float Cooldown {
    get {
      if (cooldownDuration == 0){
        return 1;
      }
      return cooldownDuration / cooldown;
    }
  }
  public int Slot{
    get {
      return slot;
    }
  }

  private void Awake()
  {
    abilityController = GetComponent<Player_Abilities_Controller>();
    playerController = GetComponent<Player_Controller>();
  }
  protected void Update()
  {
    if (isOnCooldown)
    {
      cooldownDuration += Time.deltaTime;
      if (cooldownDuration > cooldown)
      {
        cooldownDuration = 0;
        isOnCooldown = false;
      }
    }
  }
  public abstract void Initialize();
  public abstract void Activate();
  public Vector3 VectorToMouse(){
    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    mousePos.z = 0;
    Vector3 dir = (mousePos- transform.position).normalized;
    return dir;
  }
  public Vector3 VectorToTarget(){
    Vector3 dir = (abilityController.Target.transform.position - transform.position).normalized;
    return dir;
  }
  public float GetCooldownRemaining(){
    return Cooldown;
  }
}
