using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ifrit_FSM : Monster
{
  [SerializeField]
  float auraMultiplier = 1f;
  float fsmDelay = 4f, fightTime = 0f;
  bool cont = true, summoning = false;
  public enum State { IDLE, START, DEAD, AURA, MINES, BREATH }
  public State myState = State.START;
  State prevState = State.BREATH;
  [SerializeField]
  GameObject MinePrefab, AuraPrefab;
  bool CreatedRank = false;
  Text Timer;

  public float AuraMult
  {
    get
    {
      return auraMultiplier;
    }

    set
    {
      auraMultiplier += value;
    }
  }

  protected override void Aggro()
  {
    myState = State.START;
  }

  // Use this for initialization
  void Start()
  {
    base.Start();
    Timer = GameObject.Find("BossTimer").GetComponent<Text>();
    Timer.color = Color.clear;
  }

  // Update is called once per frame
  void Update()
  {
    base.Update();
    if (cont)
    {
      switch (myState)
      {
        case (State.START):
          Timer.color = Color.white;
          break;
        case (State.IDLE):
          StartCoroutine(Stall());
          break;
        case (State.AURA):
          CastAura();
          break;
        case (State.MINES):
          if (!summoning)
          {
            StartCoroutine(SummonMines());
          }
          break;
        case (State.BREATH):
          BreathFire();
          break;
      }
    }
    if (myState != State.START)
    {
      fightTime += Time.deltaTime;
      string seconds = Mathf.RoundToInt(fightTime % 60).ToString();
      if (fightTime % 60 < 10)
      {
        seconds = "0" + seconds;
      }
      Timer.text = Mathf.Floor(fightTime / 60) + ":" + seconds;
    }

    if (playerController.HealthPercent <= 0 && !CreatedRank)
    {
      print("Killed Player");
      CreatedRank = true;
    }
  }

  public override void Damage(float dmg)
  {
    Health -= dmg;
    myHealthMaterial.SetFloat("_Value", Health / TotalHealth);
    if (Health <= 0)
    {
      MonsterManager.Instance.CullMonster(this);
      GetComponent<Boss_Init>().ResetVariables();
      GameObject.Find("BossRank").GetComponent<Boss_Ranking>().DetermineScore(fightTime);
      Destroy(gameObject);
    }
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    base.OnTriggerEnter2D(collision);
  }

  IEnumerator Stall()
  {
    cont = false;
    yield return new WaitForSeconds(fsmDelay);
    StepToNextState();
    cont = true;
  }

  void StartFight()
  {
    prevState = State.BREATH;
    print("Starting");
    ReturnToIdle();
  }

  IEnumerator SummonMines()
  {
    summoning = true;
    prevState = myState;
    print("Summoning Mines");
    for (int i = 0; i < 5; i++)
    {
      Instantiate(MinePrefab, new Vector2(UnityEngine.Random.Range(transform.Find("minMineRange").position.x, transform.Find("maxMineRange").position.x), transform.position.y), Quaternion.identity).transform.parent = transform;
      yield return new WaitForSeconds(1.25f);
    }
    ReturnToIdle();
    summoning = false;
  }

  void CastAura()
  {
    prevState = myState;
    print("Casting Aura");
    GameObject tempAuraObj = Instantiate(AuraPrefab, player.transform.position, Quaternion.identity);
    ReturnToIdle();
  }

  void BreathFire()
  {
    prevState = myState;
    print("Breathing Fire");
    ReturnToIdle();
  }

  void ReturnToIdle()
  {
    myState = State.IDLE;
    print("Idling");
    StartCoroutine(Stall());
  }

  void StepToNextState()
  {
    switch (prevState)
    {
      case (State.AURA):
        myState = State.MINES;
        break;

      case (State.MINES):
        myState = State.AURA;
        break;

<<<<<<< HEAD
    void CastAura()
    {
        prevState = myState;
        print("Casting Aura");
        GameObject tempAuraObj = Instantiate(AuraPrefab, player.transform.position, Quaternion.identity);
        tempAuraObj.transform.parent = transform;
        ReturnToIdle();
    }

    void BreathFire()
    {
        prevState = myState;
        print("Breathing Fire");
        ReturnToIdle();
    }

    void ReturnToIdle()
    {
        myState = State.IDLE;
        print("Idling");
        StartCoroutine(Stall());
    }

    void StepToNextState()
    {
        switch (prevState)
        {
            case (State.AURA):
                myState = State.MINES;
                break;

            case (State.MINES):
                myState = State.AURA;
                break;

            case (State.BREATH):
                myState = State.AURA;
                break;
        }
=======
      case (State.BREATH):
        myState = State.AURA;
        break;
>>>>>>> 8f67f3fe753cebe9b8c11cdd9d6e610e2ccb4f94
    }
  }
}
