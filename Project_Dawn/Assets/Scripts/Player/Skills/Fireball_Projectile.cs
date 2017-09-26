using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball_Projectile : MonoBehaviour
{
  [SerializeField]
  float speed = 5f;
  Rigidbody2D rgd2d;
  public float dmg;
  // Use this for initialization
  void Start()
  {
    rgd2d = GetComponent<Rigidbody2D>();
    rgd2d.AddForce(-(transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition)).normalized * speed, ForceMode2D.Impulse);
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.GetComponent<Monster>())
    {
      collision.GetComponent<Monster>().Damage(dmg);
    }
    Destroy(gameObject);
  }
}
