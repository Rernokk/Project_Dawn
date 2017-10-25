using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball_Projectile : MonoBehaviour
{
  [SerializeField]
  float speed = 5f;
  Rigidbody2D rgd2d;
  public float dmg;
  public Player_Controller player;
  // Use this for initialization
  void Start()
  {
    rgd2d = GetComponent<Rigidbody2D>();
    if (GameObject.Find("Persistants").GetComponent<PersistantVariables>().currentBinds == KeybindSettings.KEYBOARDONLY || GameObject.Find("Persistants").GetComponent<PersistantVariables>().isControllerConnected)
    {
      rgd2d.AddForce(speed * new Vector2(Mathf.Sign(player.dir.x) * .7f, .1f).normalized, ForceMode2D.Impulse);
    }
    else
    {
      rgd2d.AddForce(-(transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition)).normalized * speed, ForceMode2D.Impulse);
    }
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
