using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//武器类
public class WeaponBase : MonoBehaviour
{
    //设置武器信息
    public Sprite icon;//武器图标
    public Transform muzzle;//枪口
    public GameObject bulletPrefab;//子弹
    public int bulletNum;//子弹数量
    public float bulletSpeed = 12f;//子弹速度

   //vector 表示子弹的方向
   public void OpenFire(Vector3 dir)
    {
        if (bulletNum > 0 || 1==1)
        {
            var bullet = GameObject.Instantiate(bulletPrefab, muzzle.position, Quaternion.identity);//设置类型，位置，默认方向
            bullet.GetComponent<Rigidbody>().velocity = dir * bulletSpeed;//设置方向和速度
            bulletNum--;
        }
    }
}
