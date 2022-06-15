using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//掉落在地上可以拾取的类
public class DropedItem : MonoBehaviour
{
    public GameObject WeaponPrefet;//掉落的武器
    private void OnCollisionEnter(Collision collision)
    {
        //玩家碰到这个物体，玩家的武器库增加武器，同时删除这个obj;
        if (collision.gameObject.tag == "player")
        {
            RobotPlayer.GetInstance().addWeapon(WeaponPrefet);
            Destroy(this.gameObject);
        }
    }

}
