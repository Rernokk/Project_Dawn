using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DamageType { BLEED, DISEASE, NORMAL };

[Serializable]
public class DamageOverTime
{
  public string source;
  public float timeRemaining;
  public float totalTime;
  public float damageValue;
  public float stack;
  public int maxStacks;
  public DamageType dmgType;

  public DamageOverTime(string src, float duration, float dmg, float count, DamageType type = DamageType.NORMAL, int max = 1)
  {
    source = src;
    timeRemaining = duration;
    totalTime = duration;
    damageValue = dmg;
    stack = count;
    maxStacks = max;
    dmgType = type;
  }
  public void RefreshDuration()
  {
    timeRemaining = totalTime;
  }
  public void AddStack()
  {
    stack++;
  }
  public void RemoveStack()
  {
    stack--;
  }
  public void ReduceDuration()
  {
    timeRemaining -= Time.deltaTime;
  }

  public DamageType GetDamageType(){
    return dmgType;
  }
}

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Monster : MonoBehaviour
{
  [SerializeField]
  protected bool Triggered = false, burning = false;

  [SerializeField]
  protected float Health, TotalHealth = 100;

  protected int xpValue;
  public Color MyHealthColor;
  public Material HealthRefMat;
  protected Material myHealthMaterial;
  protected GameObject myHealthBar;
  protected Vector3 direction;
  protected GameObject player;
  protected Player_Controller playerController;

  [SerializeField]
  protected List<DamageOverTime> DotsOnMe;

  public List<DamageOverTime> DoTs {
    get {
      return DotsOnMe;
    }
  }

  public List<DamageOverTime> GetDamageOverTimeByType(DamageType type){
    List<DamageOverTime> dots = new List<DamageOverTime>();
    foreach (DamageOverTime dot in DotsOnMe){
      if(dot.GetDamageType() == type){
        dots.Add(dot);
      }
    }
    return dots;
  }

  protected void Start()
  {
    player = GameObject.FindGameObjectWithTag("Player");
    playerController = player.GetComponent<Player_Controller>();
    Health = TotalHealth;
    myHealthMaterial = new Material(HealthRefMat);
    myHealthMaterial.SetColor("_Color", MyHealthColor);
    myHealthBar = transform.Find("Canvas").Find("Health").gameObject;
    myHealthMaterial.SetFloat("_Value", Health / TotalHealth);
    myHealthBar.GetComponent<Image>().material = myHealthMaterial;
    MonsterManager.Instance.AddMonsterToList(this);
    direction = transform.right * Mathf.Sign((player.transform.position - transform.position).x);
    transform.Find("Sprite").GetComponent<SpriteRenderer>().material = new Material(transform.Find("Sprite").GetComponent<SpriteRenderer>().material);
    DotsOnMe = new List<DamageOverTime>();
    transform.Find("Sprite").GetComponent<SpriteRenderer>().sortingLayerName = "Primary";
    transform.Find("Canvas").GetComponent<Canvas>().sortingLayerName = "Primary";
  }
  protected void Update()
  {
    //Going through & applying damage, reducing time
    for (int i = 0; i < DotsOnMe.Count; i++)
    {
      DotsOnMe[i].ReduceDuration();
      Damage(DotsOnMe[i].damageValue * DotsOnMe[i].stack * Time.deltaTime);
    }

    //Culling any DoTs that are expiring.
    for (int i = 0; i < DotsOnMe.Count; i++)
    {
      if (DotsOnMe[i].timeRemaining <= 0)
      {
        if (DotsOnMe[i].stack <= 1)
        {
          DotsOnMe.RemoveAt(i);
          i--;
        }
        else
        {
          DotsOnMe[i].RemoveStack();
          DotsOnMe[i].RefreshDuration();
        }
      }
    }
  }
  protected abstract void Aggro();
  public virtual void Damage(float damageValue)
  {
    Health -= damageValue;
    myHealthMaterial.SetFloat("_Value", Health / TotalHealth);
    if (Health <= 0)
    {
      playerController.currentExp += 25;
      playerController.uiController.UpdateExpValue();
      playerController.myInventory.Add(Item.GenerateItem());
      MonsterManager.Instance.CullMonster(this);
      Destroy(gameObject);
    }
  }
  public void Heal(float healValue)
  {
    Health += healValue;
    if (Health > TotalHealth)
    {
      Health = TotalHealth;
    }
    myHealthMaterial.SetFloat("_Value", Health / TotalHealth);
  }
  protected void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.GetComponent<Flame_Script>())
    {
      collision.GetComponent<Flame_Script>().targets.Add(this);
    }
  }
  private void OnTriggerExit2D(Collider2D collision)
  {
    if (collision.GetComponent<Flame_Script>())
    {
      collision.GetComponent<Flame_Script>().targets.Remove(this);
    }
  }
  public void AddDot(string src, float dur, float dmg, float count, DamageType dmgType, int size = 1)
  {
    if (DotsOnMe == null)
    {
      DotsOnMe = new List<DamageOverTime>();
    }
    foreach (DamageOverTime dot in DotsOnMe)
    {
      if (dot.source == src)
      {
        if (dot.stack < dot.maxStacks)
        {
          dot.AddStack();
        }
        dot.RefreshDuration();
        return;
      }
    }
    DotsOnMe.Add(new DamageOverTime(src, dur, dmg, count, dmgType, size));
  }
}