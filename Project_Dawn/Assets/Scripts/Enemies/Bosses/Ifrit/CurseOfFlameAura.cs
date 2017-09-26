using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurseOfFlameAura : MonoBehaviour
{
  [SerializeField]
  float duration = 1f, range = 1f;
  float TimeLeft;
  Transform player;
  Material myMat;
  Material playerMat;
  // Use this for initialization
  void Start()
  {
    player = GameObject.FindGameObjectWithTag("Player").transform;
    StartCoroutine(DebuffCountdown());
    TimeLeft = duration;
    myMat = GetComponent<SpriteRenderer>().material;
    playerMat = player.Find("Sprite").GetComponent<SpriteRenderer>().material;
  }

  // Update is called once per frame
  void Update()
  {
    TimeLeft -= Time.deltaTime;
    myMat.SetFloat("_Value", 2 - 2 * (TimeLeft / duration) - 1);
  }

  IEnumerator DebuffCountdown()
  {
    yield return new WaitForSeconds(duration);
    if (Vector2.Distance(player.transform.position, transform.position) <= GetComponent<CircleCollider2D>().bounds.extents.x * 2)
    {
      GameObject.Find("Ifrit").GetComponent<Ifrit_FSM>().AuraMult = 1;
      playerMat.color = Color.red;
      myMat.color = Color.clear;
      yield return new WaitForSeconds(.2f);
      playerMat.color = Color.white;
    }
    myMat.SetFloat("_Value", -1f);
    Destroy(gameObject);
  }
}
