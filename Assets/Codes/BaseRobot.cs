using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//��ɫ�Ļ���
public class BaseRobot : MonoBehaviour
{
    public int hp = 100;//Ѫ��
    public bool IsAlive()
    {
        return hp > 0;
    }
    public void GetDamage(int dmg)//�ܵ��˺�����
    {
        hp -= dmg;
        if (!IsAlive())
        {
            Die();
        }
    }
    public virtual void Die()//�������麯��
    {
        Destroy(this.gameObject);
    }
    public virtual void OpenFire()
    {

    }

  
}
