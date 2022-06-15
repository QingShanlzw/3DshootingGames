using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//�ӵ���
public class Bullet : MonoBehaviour
{
    public int power = 10;//�ӵ�����
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag =="Player")
        {
            other.GetComponent<RobotPlayer>().GetDamage(power);
        }
        else if(other.gameObject.tag == "Enemy")
        {
            other.GetComponent<RobotEmeny>().GetDamage(power);
        }
        Destroy(this.gameObject); 
    }


}
