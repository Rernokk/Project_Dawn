using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eruption_Script : MonoBehaviour
{
    Player_Controller player;

    [SerializeField]
    float Range, Damage;
    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player_Controller>();
        StartCoroutine(Delay());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Delay()
    {
        transform.Find("Init").GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(1f);
        if (Vector2.Distance(transform.position, player.transform.position) < Range)
        {
            player.Damage(transform.parent.GetComponent<Ifrit_FSM>().AuraMult * Damage);
        }
        transform.Find("Torch").GetComponent<ParticleSystem>().Play();
        Destroy(gameObject, 3f);
    }
}
