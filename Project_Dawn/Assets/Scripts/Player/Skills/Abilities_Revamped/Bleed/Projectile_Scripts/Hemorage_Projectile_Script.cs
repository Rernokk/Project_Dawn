using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hemorage_Projectile_Script : MonoBehaviour
{
  [HideInInspector]
  public Vector2 dir;

  [HideInInspector]
  public int MaxStack;

  [HideInInspector]
  public float duration;

  [SerializeField]
  float speed;


  Rigidbody2D rgd;
  BoxCollider2D coll;

  [HideInInspector]
  public float damageValue;
  // Use this for initialization
  void Start()
  {
    rgd = GetComponent<Rigidbody2D>();
    rgd.velocity = dir * speed;
    coll = GetComponent<BoxCollider2D>();
    coll.isTrigger = true;
    StartCoroutine(StallCollision());
    Destroy(gameObject, 3f);
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    Monster monster = collision.gameObject.GetComponent<Monster>();
    if (monster != null){
      monster.AddDot("Hemorage", duration, damageValue, 1, DamageType.BLEED, MaxStack);
    }
    DestroyObject(gameObject);
  }

  IEnumerator StallCollision(){
    yield return null;
    coll.isTrigger = false;
  }
}
