using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPlayer : BaseRobot
{
    //设置单间模式
    static RobotPlayer instance;
    public static RobotPlayer GetInstance()
    {
        return instance;
    }
    private void Awake()
    {
        instance = this;
    }
    //
    public List<WeaponBase> Weapons;//玩家的武器背包
    public Transform Head;//挂接武器的地方
    public float WalkSpeed = 10f;//移动速度
    //
    private WeaponBase CurWeapon;//当前手中的武器
    private int CurWeaponIdx;//当前武器在背包位置的索引
    //
    private Animator animator;  //animator组件
    private CharacterController cc;//cc组件
    // Start is called before the first frame update
    void Start()
    {
        //获取组件
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        CurWeapon = Weapons[CurWeaponIdx];
    }

    // Update is called once per frame
    void Update()
    {
        //1、设置角色的移动。
        var trans = Camera.main.transform;
        var forward = Vector3.ProjectOnPlane(trans.forward, Vector3.up);
        var right = Vector3.ProjectOnPlane(trans.right, Vector3.up);
        //movement
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        var moveDirection = v * forward + h * right;
        cc.Move(moveDirection.normalized * WalkSpeed * Time.deltaTime);
        animator.SetFloat("Speed", cc.velocity.magnitude / WalkSpeed);

        //2、rotate 设置主角的转向随鼠标而动
        //每一帧获取鼠标位置来改变角色方向
        var r = GetAimPoint();
        RotateToTarget(r);

        //5、switch weapon
        float f = Input.GetAxis("Mouse ScrollWheel");//鼠标滚轮
        if (f > 0) { NextWeapon(1); }
        else if (f < 0) { NextWeapon(-1); }
        //3、开火设置
        if (Input.GetButton("Fire1"))
        {
            Shoot();
        }
        //4、对接ui和player
        //通过HUD的单间获取其实例
        //HUD.GetInstance().UpdateHpUI(hp);//由于text文本的hp还未挂载到GameObject，所以暂时无法使用。
       // HUD.GetInstance().UpdateWeaponUI(CurWeapon.icon, CurWeapon.bulletNum);//这里也有问题,文本未绑定的问题
    }
    public void NextWeapon(int step)
    {
        var idx = (CurWeaponIdx + step + Weapons.Count) % Weapons.Count;
        CurWeapon.gameObject.SetActive(false);//把之前武器设置为false
        CurWeapon = Weapons[idx];
        CurWeapon.gameObject.SetActive(true);//设置新武器的状态
        CurWeaponIdx = idx;
    }
    public void addWeapon(GameObject Weapon)
    {
        for(int i = 0; i < Weapons.Count; i++)
        {
            if (Weapons[i].gameObject.name == Weapon.name)
            {
                Weapons[i].bulletNum += 15;
                return;
            }
        }
        var new_weapon = GameObject.Instantiate(Weapon);
        new_weapon.name = Weapon.name;
        new_weapon.transform.localRotation = CurWeapon.transform.localRotation;
        Weapons.Add(new_weapon.GetComponent<WeaponBase>());
        NextWeapon(Weapons.Count - 1 - CurWeaponIdx);
    }
    //获得目标点和主角方向的目标向量
    //鼠标从屏幕从三维空间投射 射线 ，射线与地面（layer==floor）做射线的检测，
    //检测成功，-->鼠标投出来的射线投到地面了。利用向量相减计算出角色位置和鼠标投到地面的点之间的向量
    //再吧y设为0，这样就相当角色和向量处于同一个平面。
    public Vector3 GetAimPoint()
    {
        Ray camRay =Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        if (Physics.Raycast(camRay, out floorHit, 100.0f, LayerMask.GetMask("Floor"))){
            Vector3 playerToMouse =floorHit.point -transform.position;
            playerToMouse.y = 0;
            return playerToMouse;
        }
        return Vector3.zero;
    }
    public void RotateToTarget(Vector3 rot)
    {
        //对准方向
        transform.LookAt(rot + transform.position);
    }
    //配合主角的animator的状态机
    //动画层（0是第一层）1是idle的话，把bool参数的状态设置为true
    public void Shoot()
    {
        if(animator.GetCurrentAnimatorStateInfo(1).IsName("idle"))
        {
            animator.SetBool("shoot", true);//shoot默认为false，点击开火会短暂调出开火动画.
        }

    }
    public override void OpenFire()
    {
        base.OpenFire();
        //
        Debug.Log("OpenFire shoot bullet");
        //
        CurWeapon.OpenFire(transform.forward);
    }
}
