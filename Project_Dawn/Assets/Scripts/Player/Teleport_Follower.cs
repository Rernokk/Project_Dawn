using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Teleport_Follower : MonoBehaviour
{
    Player_Controller player;
    BoxCollider2D boxColl;
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Controller>();
        boxColl = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D[] info = Physics2D.RaycastAll(player.transform.position, player.direction * player.teleportRange, player.teleportRange);
        bool RangeTele = true;
        foreach (RaycastHit2D hit in info)
        {
            if (hit.transform.tag == "Unpassable")
            {
                RangeTele = false;
            }
        }
        if (RangeTele)
        {
            transform.position = player.transform.position + player.teleportRange * player.direction;
        }
        else
        {
            IEnumerable<RaycastHit2D> infoQ = from i in info where i.transform.tag == "Unpassable" select i;
            RaycastHit2D closest = info[0];
            float closestDist = 9999;
            foreach (RaycastHit2D hitInfo in infoQ)
            {
                float tempDist = Vector2.Distance(hitInfo.transform.position, player.transform.position);
                if (tempDist < closestDist)
                {
                    closestDist = tempDist;
                    closest = hitInfo;
                }
            }
            transform.position = (Vector2)closest.point - (Mathf.Sign(closest.point.x - player.transform.position.x) * new Vector2(boxColl.bounds.extents.x, 0));
        }
    }
}
