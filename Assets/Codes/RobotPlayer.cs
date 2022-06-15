using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPlayer : BaseRobot
{
    //���õ���ģʽ
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
    public List<WeaponBase> Weapons;//��ҵ���������
    public Transform Head;//�ҽ������ĵط�
    public float WalkSpeed = 10f;//�ƶ��ٶ�
    //
    private WeaponBase CurWeapon;//��ǰ���е�����
    private int CurWeaponIdx;//��ǰ�����ڱ���λ�õ�����
    //
    private Animator animator;  //animator���
    private CharacterController cc;//cc���
    // Start is called before the first frame update
    void Start()
    {
        //��ȡ���
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        CurWeapon = Weapons[CurWeaponIdx];
    }

    // Update is called once per frame
    void Update()
    {
        //1�����ý�ɫ���ƶ���
        var trans = Camera.main.transform;
        var forward = Vector3.ProjectOnPlane(trans.forward, Vector3.up);
        var right = Vector3.ProjectOnPlane(trans.right, Vector3.up);
        //movement
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        var moveDirection = v * forward + h * right;
        cc.Move(moveDirection.normalized * WalkSpeed * Time.deltaTime);
        animator.SetFloat("Speed", cc.velocity.magnitude / WalkSpeed);

        //2��rotate �������ǵ�ת����������
        //ÿһ֡��ȡ���λ�����ı��ɫ����
        var r = GetAimPoint();
        RotateToTarget(r);

        //5��switch weapon
        float f = Input.GetAxis("Mouse ScrollWheel");//������
        if (f > 0) { NextWeapon(1); }
        else if (f < 0) { NextWeapon(-1); }
        //3����������
        if (Input.GetButton("Fire1"))
        {
            Shoot();
        }
        //4���Խ�ui��player
        //ͨ��HUD�ĵ����ȡ��ʵ��
        //HUD.GetInstance().UpdateHpUI(hp);//����text�ı���hp��δ���ص�GameObject��������ʱ�޷�ʹ�á�
       // HUD.GetInstance().UpdateWeaponUI(CurWeapon.icon, CurWeapon.bulletNum);//����Ҳ������,�ı�δ�󶨵�����
    }
    public void NextWeapon(int step)
    {
        var idx = (CurWeaponIdx + step + Weapons.Count) % Weapons.Count;
        CurWeapon.gameObject.SetActive(false);//��֮ǰ��������Ϊfalse
        CurWeapon = Weapons[idx];
        CurWeapon.gameObject.SetActive(true);//������������״̬
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
    //���Ŀ�������Ƿ����Ŀ������
    //������Ļ����ά�ռ�Ͷ�� ���� ����������棨layer==floor�������ߵļ�⣬
    //���ɹ���-->���Ͷ����������Ͷ�������ˡ�������������������ɫλ�ú����Ͷ������ĵ�֮�������
    //�ٰ�y��Ϊ0���������൱��ɫ����������ͬһ��ƽ�档
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
        //��׼����
        transform.LookAt(rot + transform.position);
    }
    //������ǵ�animator��״̬��
    //�����㣨0�ǵ�һ�㣩1��idle�Ļ�����bool������״̬����Ϊtrue
    public void Shoot()
    {
        if(animator.GetCurrentAnimatorStateInfo(1).IsName("idle"))
        {
            animator.SetBool("shoot", true);//shootĬ��Ϊfalse������������ݵ������𶯻�.
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
