using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 移动寻路测试
/// </summary>
public class PathTest : MonoBehaviour
{
    NavMeshAgent agent;

    public float distanceUp = 10f; //相机与目标的竖直高度参数
    public float distanceAway = 15f; //相机与目标的水平距离参数
    public float smooth = 2f; //位置平滑移动插值参数值
    public float camDepthSmooth = 20f;
    private Animator _animator;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        // _animator.SetTrigger("HumanoidIdle");
        _animator.SetFloat("speed",0);

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000))
            {
                Debug.Log($"点击位置：{hit.point}");
                agent.destination = hit.point;
                _animator.SetFloat("speed",agent.speed);
            }
        }
        

        // 鼠标轴控制相机的远近
        if ((Input.mouseScrollDelta.y < 0 && Camera.main.fieldOfView >= 3) ||
            Input.mouseScrollDelta.y > 0 && Camera.main.fieldOfView <= 80)
        {
            Camera.main.fieldOfView += Input.mouseScrollDelta.y * camDepthSmooth * Time.deltaTime;
        }
        
        // 距离判断
        if (Vector3.Distance(agent.destination, transform.position) < 1)
        {
            _animator.SetFloat("speed",0f);
        }
    }

    void LateUpdate()
    {
        //计算出相机的位置
        Vector3 disPos = transform.position + Vector3.up * distanceUp - transform.forward * distanceAway;

        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, disPos, Time.deltaTime * smooth);
        //相机的角度
        Camera.main.transform.LookAt(transform.position);
    }
}