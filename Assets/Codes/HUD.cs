using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//UI控制的类
public class HUD : MonoBehaviour
{
    //把HUD改为单例模式
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
    //更新武器的数量和图标
    public void UpdateWeaponUI(Sprite icon,int bullet_num)
    {
        weaponIcon.sprite = icon;
        bulletNum.text = bullet_num.ToString();
    }
    //更新HP
    public void UpdateHpUI(int hp_num)
    {
        hpNum.text = hp_num.ToString();
    }

   //提高效率，删除start和update方法。
}
