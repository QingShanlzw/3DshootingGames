using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//������
public class WeaponBase : MonoBehaviour
{
    //����������Ϣ
    public Sprite icon;//����ͼ��
    public Transform muzzle;//ǹ��
    public GameObject bulletPrefab;//�ӵ�
    public int bulletNum;//�ӵ�����
    public float bulletSpeed = 12f;//�ӵ��ٶ�

   //vector ��ʾ�ӵ��ķ���
   public void OpenFire(Vector3 dir)
    {
        if (bulletNum > 0 || 1==1)
        {
            var bullet = GameObject.Instantiate(bulletPrefab, muzzle.position, Quaternion.identity);//�������ͣ�λ�ã�Ĭ�Ϸ���
            bullet.GetComponent<Rigidbody>().velocity = dir * bulletSpeed;//���÷�����ٶ�
            bulletNum--;
        }
    }
}
