using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//���˵���Ұ��
public class EnemySight : MonoBehaviour
{
    public float fov = 110f;//��Ұ�ķ�Χ
    public bool playerIsInSight;//����Ƿ�����Ұ֮��
    public Vector3 personalLastInSight;//�������������Ұ��λ��
    public static Vector3 resetPos = Vector3.back;
    private GameObject player;
    private SphereCollider col;//��Ұ�����ײ��
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
        if (other.gameObject == player)//��ҽ�����Ұ��
        {
            playerIsInSight = false;
            //��Һ͵���λ�õ�����
            Vector3 direction = other.transform.position - transform.position;
            //���˳�������ߵļн�
            float angle  = Vector3.Angle(direction,transform.forward);
            if (angle < fov * 0.5f)//�жϼн��Ƿ񳬹���Ұ��Χ
            {
                RaycastHit hit;
                //�������߼�飬������Һ͵���֮�������ϰ���
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
    //����뿪��Ұ��
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            Debug.Log("Has out of Sight");
            playerIsInSight = false;  
        }
    }
}
