using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//自带自动寻路功能

public class RobotEmeny : BaseRobot
{
    private NavMeshAgent nav;//保存导航代理体
    private Animator animator;//动画状态机
    private EnemySight enemySight;//敌人视野
    private EnemySight weaponSight;//武器视野
    public WeaponBase CurWeapon;//当前武器
    private GameObject player;//玩家
    //patrolling
    public float patrolSpeed = 5;//巡逻速度
    public int wayPointIndex;//当前路径点的索引
    public float patrolWaitTime = 1f;//巡逻到路径点后的等待时间
    public Transform patrolWayPoints;//巡逻路径点的transform，用来保存所有的巡逻路径点
    private float patrolTimer;//巡逻到路径点后的计时器。
    //shasing追击
    public float chaseSpeed = 8;//追击速度
    public float chaseWaitTime = 5f;//追击等待时间
    private float chaseTimer;//追击计时器
    private void Awake()
    {
        enemySight = transform.Find("EnemySight").GetComponent<EnemySight>();
        weaponSight = transform.Find("WeaponSight").GetComponent<EnemySight>();
        animator  =GetComponent<Animator>();
        nav =GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    //重写母体的开火函数
    
    public void Shooting()
    {
        nav.isStopped = true;
        nav.speed = 0;

        Vector3 lookPos = player.transform.position;
        lookPos.y = transform.position.y;
        Vector3 targetDir =lookPos- transform.position;//获得物体指向player的方向向量
        //
        float step = 5 * Time.deltaTime;
        //Vector.rotateTowards  控制人物的转向，前两个参数是当前位置和要移动去的位置，
        //后两个是移动速度的幅度
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0);
        transform.rotation = Quaternion.LookRotation(newDir);
        //
        animator.SetBool("shoot", true);

    }
    public override void OpenFire()
    {
        base.OpenFire();
        CurWeapon.OpenFire(transform.forward);
    }

    // Update is called once per frame
    void Update()
    {
        if (!RobotPlayer.GetInstance().IsAlive()) return;
        if (weaponSight.playerIsInSight)
        {
            //shoot
            Shooting();
        }
        else if (enemySight.playerIsInSight)
        {
            //chase
            Chasing();
        }
        else
        {
            //patrol
            Patrolling();
        }
        animator.SetFloat("Speed",nav.speed/chaseSpeed);
        
    }
    public void Chasing()
    {
        nav.isStopped = false;
        nav.speed = chaseSpeed;
        Vector3 sightingDeltaPos = enemySight.personalLastInSight - transform.position;
        if(sightingDeltaPos.sqrMagnitude>4)
        {
            nav.destination = enemySight.personalLastInSight;
            if(nav.remainingDistance<=nav.stoppingDistance)
            {
                chaseTimer += Time.deltaTime;
                if (chaseTimer >= chaseWaitTime)
                {
                    chaseTimer = 0;
                    nav.speed = 0;
                }
            }
            else
            {
                chaseTimer = 0;
            }
        }
    }
    public void Patrolling()
    {
        nav.isStopped = false;
        nav.speed = patrolSpeed;
        if(nav.remainingDistance <= nav.stoppingDistance)
        {
            patrolTimer +=Time.deltaTime;
            if(patrolTimer >= patrolWaitTime)
            {
                if (wayPointIndex == patrolWayPoints.childCount - 1)
                {
                    wayPointIndex = 0;
                }
                else
                {
                    wayPointIndex++;
                }
                patrolTimer = 0; 
            }
        }
        else
        {
            patrolTimer = 0;
        }
        nav.destination = patrolWayPoints.GetChild(wayPointIndex).position;
    }
}
