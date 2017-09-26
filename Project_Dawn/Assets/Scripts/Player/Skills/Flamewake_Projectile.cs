using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamewake_Projectile : MonoBehaviour {
  public Vector2 dir;
  public float speed;
  public float damage = 0f;
	// Update is called once per frame
	void Update () {
    transform.position += new Vector3(dir.x * speed, 0, 0) * Time.deltaTime;
	}

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.GetComponent<Monster>() != null){
      collision.GetComponent<Monster>().Damage(damage);
    }
  }
}
