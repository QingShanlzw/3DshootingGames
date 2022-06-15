using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//UI���Ƶ���
public class HUD : MonoBehaviour
{
    //��HUD��Ϊ����ģʽ
    private static HUD instance;
    public static HUD GetInstance()
    {
        return instance;
    }
    private void Awake()
    {
        instance = this;
    }
    public Image weaponIcon;
    public Text bulletNum;
    public Text hpNum;

    // Start is called before the first frame update
    //����������������ͼ��
    public void UpdateWeaponUI(Sprite icon,int bullet_num)
    {
        weaponIcon.sprite = icon;
        bulletNum.text = bullet_num.ToString();
    }
    //����HP
    public void UpdateHpUI(int hp_num)
    {
        hpNum.text = hp_num.ToString();
    }

   //���Ч�ʣ�ɾ��start��update������
}
