using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//角色的基类
public class BaseRobot : MonoBehaviour
{
    public int hp = 100;//血量
    public bool IsAlive()
    {
        return hp > 0;
    }
    public void GetDamage(int dmg)//受到伤害函数
    {
        hp -= dmg;
        if (!IsAlive())
        {
            Die();
        }
    }
    public virtual void Die()//死亡的虚函数
    {
        Destroy(this.gameObject);
    }
    public virtual void OpenFire()
    {

    }

  
}
