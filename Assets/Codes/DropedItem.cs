using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//�����ڵ��Ͽ���ʰȡ����
public class DropedItem : MonoBehaviour
{
    public GameObject WeaponPrefet;//���������
    private void OnCollisionEnter(Collision collision)
    {
        //�������������壬��ҵ�����������������ͬʱɾ�����obj;
        if (collision.gameObject.tag == "player")
        {
            RobotPlayer.GetInstance().addWeapon(WeaponPrefet);
            Destroy(this.gameObject);
        }
    }

}
