using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Init : MonoBehaviour {
    Transform Cage, TriggerZone, BossCamera, Player;

    void Start()
    {
        Cage = transform.Find("BossCage");
        TriggerZone = transform.Find("Start_Trigger");
        BossCamera = transform.Find("BossCamera");
        Player = GameObject.FindGameObjectWithTag("Player").transform;

        //Setting up Arena
        BossCamera.gameObject.SetActive(false);
        Cage.gameObject.SetActive(false);
    }
	
	void Update () {
		
	}

    public void TriggerFight()
    {
        BossCamera.gameObject.SetActive(true);
        Cage.gameObject.SetActive(true);
        TriggerZone.gameObject.SetActive(false);
        Player.transform.Find("Main Camera").gameObject.SetActive(false);
        GameObject.Find("PlayerUI").GetComponent<Canvas>().worldCamera = transform.Find("BossCamera").GetComponent<Camera>();
        GameObject.Find("Ifrit").GetComponent<Ifrit_FSM>().myState = Ifrit_FSM.State.IDLE;
    }

    public void ResetVariables() {
        BossCamera.gameObject.SetActive(false);
        Cage.gameObject.SetActive(false);
        TriggerZone.gameObject.SetActive(false);
        Player.transform.Find("Main Camera").gameObject.SetActive(true);
        GameObject.Find("PlayerUI").GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        print("Reset Variables");
    }
}
