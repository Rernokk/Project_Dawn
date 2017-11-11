using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain_Grip : Skill
{
  GameObject[] Targets;
  GameObject[] FX;
  GameObject myChainPrefab;
  GameObject mySelecitonPrefab;
  GameObject mySelectedPointPrefab;
  int curr = 0;
  public Chain_Grip(Player_Controller controller, GameObject prefab, GameObject SelectionFX, GameObject PointPrefab, int manaCost, float cd) : base(controller, manaCost, cd)
  {
    Targets = new GameObject[3];
    FX = new GameObject[3];
    mySelecitonPrefab = SelectionFX;
    myChainPrefab = prefab;
    mySelectedPointPrefab = PointPrefab;
  }

  public override void Cast(float damage = 0)
  {
    if (!player.chainMode)
    {
      player.chainMode = true;
    }

    if (Input.GetKeyDown(KeyCode.Mouse0))
    {
      if (curr == 3)
      {
        //PullTargetsToPoint();
        player.StartCoroutine(PullOverTime());
        Debug.Log("Pulling");
      }
      else
      {
        GatherTargetAtPoint();
        Debug.Log("Moving thru chains");
      }
    }
  }

  void GatherTargetAtPoint()
  {
    Ray thisRay = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit2D[] info = Physics2D.RaycastAll(thisRay.origin, thisRay.direction);
    foreach (RaycastHit2D myHit in info)
    {
      if (myHit && myHit.transform.GetComponent<Monster>() != null)
      {
        for (int i = 0; i < Targets.Length; i++)
        {
          if (Targets[i] == myHit.transform.gameObject)
          {
            return;
          }
        }
        Targets[curr] = myHit.transform.gameObject;
        FX[curr] = Instantiate(mySelecitonPrefab, myHit.transform, false);
        curr++;
        break;
      }
    }
  }

  void PullTargetsToPoint()
  {
    Ray thisRay = Camera.main.ScreenPointToRay(Input.mousePosition);
    for (int i = 0; i < Targets.Length; i++)
    {
      Vector3 tempPos = thisRay.origin;
      tempPos.z = -3;
      GameObject temp = Instantiate(myChainPrefab, tempPos, Quaternion.identity);
      Vector3 pos = Vector3.zero;
      pos.z = Targets[i].transform.position.z - 1;
      Vector3 tarPos = Targets[i].transform.position - temp.transform.position;
      tarPos.z = pos.z;
      temp.GetComponent<LineRenderer>().SetPosition(0, pos);
      temp.GetComponent<LineRenderer>().SetPosition(1, tarPos);
      Targets[i].GetComponent<Rigidbody2D>().AddForce(ForceTowardsPoint(Targets[i].transform.position, thisRay.origin).normalized * 10f, ForceMode2D.Impulse);
    }

    //Reset Mode
    //player.StartCooldown(this);
    for (int i = 0; i < Targets.Length; i++)
    {
      Targets[i] = null;
    }
    player.ChainDelay();
    curr = 0;
  }

  Vector2 ForceTowardsPoint(Vector2 origin, Vector2 point)
  {
    Vector2 ret = Vector2.zero;
    ret = (point - origin);
    return ret;
  }

  IEnumerator PullOverTime()
  {
    Ray thisRay = Camera.main.ScreenPointToRay(Input.mousePosition);
    Destroy(Instantiate(mySelectedPointPrefab, thisRay.origin, Quaternion.identity), 2f);
    yield return new WaitForSeconds(1f);
    yield return player.StartCoroutine(ExtendChains(thisRay));
    for (int i = 0; i < Targets.Length; i++)
    {
      //Vector3 tempPos = thisRay.origin;
      //tempPos.z = -3;
      //GameObject temp = Instantiate(myChainPrefab, tempPos, Quaternion.identity);
      //Vector3 pos = Vector3.zero;
      //pos.z = Targets[i].transform.position.z - 1;
      //Vector3 tarPos = Targets[i].transform.position - temp.transform.position;
      //tarPos.z = pos.z;
      //temp.GetComponent<LineRenderer>().SetPosition(0, pos);
      //temp.GetComponent<LineRenderer>().SetPosition(1, tarPos);
      Targets[i].GetComponent<Rigidbody2D>().AddForce(ForceTowardsPoint(Targets[i].transform.position, thisRay.origin).normalized * 10f, ForceMode2D.Impulse);
    }
    yield return player.StartCoroutine(RetractChains());
    //Reset Mode
    //player.StartCooldown(this);
    for (int i = 0; i < Targets.Length; i++)
    {
      Targets[i] = null;
    }
    player.ChainDelay();
    curr = 0;
  }

  IEnumerator ExtendChains(Ray thisRay)
  {
    FX = new GameObject[Targets.Length];
    Vector3 pos = Vector3.zero;
    pos.z = Targets[0].transform.position.z - 1;
    for (int i = 0; i < Targets.Length; i++)
    {
      Vector3 tempPos = thisRay.origin;
      tempPos.z = -3;
      FX[i] = Instantiate(myChainPrefab, tempPos, Quaternion.identity);
      FX[i].GetComponent<LineRenderer>().SetPosition(0, pos);
    }

    for (int i = 0; i < 6; i++)
    {
      for (int j = 0; j < Targets.Length; j++)
      {
        Vector3 tarPos = Targets[j].transform.position - FX[j].transform.position;
        tarPos.z = Targets[j].transform.position.z - 1;
        FX[j].GetComponent<LineRenderer>().SetPosition(1, Vector3.Lerp(pos, tarPos, (float)i / 6));
      }
      yield return null;
    }
    yield return new WaitForSeconds(.25f);
  }
  IEnumerator RetractChains()
  {
    Vector3 pos = Vector3.zero;
    pos.z = Targets[0].transform.position.z - 1;
    

    for (int i = 6; i  >= 0; i--)
    {
      for (int j = 0; j < Targets.Length; j++)
      {
        Vector3 tarPos = Targets[j].transform.position - FX[j].transform.position;
        tarPos.z = Targets[j].transform.position.z - 1;
        FX[j].GetComponent<LineRenderer>().SetPosition(1, Vector3.Lerp(pos, tarPos, (float)i / 6));
      }
      yield return null;
    }
    yield return new WaitForSeconds(.25f);
    for (int i = 0; i < FX.Length; i++)
    {
      Destroy(FX[i]);
    }
  }
}
