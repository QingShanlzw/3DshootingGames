using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//�Դ��Զ�Ѱ·����

public class RobotEmeny : BaseRobot
{
    private NavMeshAgent nav;//���浼��������
    private Animator animator;//����״̬��
    private EnemySight enemySight;//������Ұ
    private EnemySight weaponSight;//������Ұ
    public WeaponBase CurWeapon;//��ǰ����
    private GameObject player;//���
    //patrolling
    public float patrolSpeed = 5;//Ѳ���ٶ�
    public int wayPointIndex;//��ǰ·���������
    public float patrolWaitTime = 1f;//Ѳ�ߵ�·�����ĵȴ�ʱ��
    public Transform patrolWayPoints;//Ѳ��·�����transform�������������е�Ѳ��·����
    private float patrolTimer;//Ѳ�ߵ�·�����ļ�ʱ����
    //shasing׷��
    public float chaseSpeed = 8;//׷���ٶ�
    public float chaseWaitTime = 5f;//׷���ȴ�ʱ��
    private float chaseTimer;//׷����ʱ��
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
    //��дĸ��Ŀ�����
    
    public void Shooting()
    {
        nav.isStopped = true;
        nav.speed = 0;

        Vector3 lookPos = player.transform.position;
        lookPos.y = transform.position.y;
        Vector3 targetDir =lookPos- transform.position;//�������ָ��player�ķ�������
        //
        float step = 5 * Time.deltaTime;
        //Vector.rotateTowards  ���������ת��ǰ���������ǵ�ǰλ�ú�Ҫ�ƶ�ȥ��λ�ã�
        //���������ƶ��ٶȵķ���
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
