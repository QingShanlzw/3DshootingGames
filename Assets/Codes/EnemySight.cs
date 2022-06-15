using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//敌人的视野类
public class EnemySight : MonoBehaviour
{
    public float fov = 110f;//视野的范围
    public bool playerIsInSight;//玩家是否在视野之内
    public Vector3 personalLastInSight;//玩家最后出现在视野的位置
    public static Vector3 resetPos = Vector3.back;
    private GameObject player;
    private SphereCollider col;//视野球的碰撞体
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<SphereCollider>();
        player = GameObject.FindGameObjectWithTag("Player");
        personalLastInSight = resetPos;
    }

    // Update is called once per frame
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)//玩家进入视野球
        {
            playerIsInSight = false;
            //玩家和敌人位置的连线
            Vector3 direction = other.transform.position - transform.position;
            //敌人朝向和连线的夹角
            float angle  = Vector3.Angle(direction,transform.forward);
            if (angle < fov * 0.5f)//判断夹角是否超过视野范围
            {
                RaycastHit hit;
                //采用射线检查，看看玩家和敌人之间有无障碍物
                if (Physics.Raycast(transform.position + transform.up,direction.normalized,out hit, col.radius))
                {
                    if (hit.collider.gameObject == player)
                    {
                        Debug.Log("Has In Sight");
                        playerIsInSight = true;
                        personalLastInSight = player.transform.position;
                    }
                }
            }
        }

    }
    //玩家离开视野球
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            Debug.Log("Has out of Sight");
            playerIsInSight = false;  
        }
    }
}
