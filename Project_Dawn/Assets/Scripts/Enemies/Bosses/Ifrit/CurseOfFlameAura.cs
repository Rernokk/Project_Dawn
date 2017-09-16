using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurseOfFlameAura : MonoBehaviour {
  [SerializeField]
  float duration = 1f, range = 1f;
  Transform player;
	// Use this for initialization
	void Start () {
    player = GameObject.FindGameObjectWithTag("Player").transform;
    StartCoroutine(DebuffCountdown());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public void ApplyDebuff()
  {

  }

  IEnumerator DebuffCountdown()
  {
    yield return new WaitForSeconds(duration);
    if (Vector2.Distance(player.transform.position, transform.position) <= range)
    {
      transform.parent.GetComponent<Ifrit_FSM>().AuraMult = 1;
    }
  }
}
