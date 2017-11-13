using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour {
  protected Player_Controller playerController;
  protected Player_Abilities_Controller abilityController;
  
  [SerializeField]
  protected bool isOnCooldown = false;

  [SerializeField]
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
  private void Awake()
  {
    abilityController = GetComponent<Player_Abilities_Controller>();
    playerController = GetComponent<Player_Controller>();
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
}
