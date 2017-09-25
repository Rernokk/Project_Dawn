using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Monster : MonoBehaviour
{
  protected GameObject player;
  protected Player_Controller playerController;
  [SerializeField]
  protected bool Triggered = false, burning = false;

  [SerializeField]
  protected float Health, TotalHealth = 100;

  public Color MyHealthColor;
  public Material HealthRefMat;

  protected Material myHealthMaterial;
  protected GameObject myHealthBar;
  protected Vector3 direction;

  float dotDmg = 0;
  // Use this for initialization
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
  }

  protected abstract void Aggro();
  public virtual void Damage(float damageValue)
  {
    Health -= damageValue;
    myHealthMaterial.SetFloat("_Value", Health / TotalHealth);
    if (Health <= 0)
    {
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

  protected void Update()
  {
    if (burning)
    {
      Damage(dotDmg * Time.deltaTime);
    }
  }

  public IEnumerator DoT(float dmg)
  {
    burning = true;
    dotDmg += dmg;
    yield return new WaitForSeconds(2f);
    burning = false;
    dotDmg -= dmg;
  }
}