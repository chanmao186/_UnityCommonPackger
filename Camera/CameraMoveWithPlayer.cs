/*
 * 相机跟随玩家移动，并控制玩家转向，适合第三人称游戏
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Header("相机距离")]
    public float freeDistance = 2;
    [Header("相机最近距离")]
    public float minDistance = 0.5f;
    [Header("相机最远距离")]
    public float maxDistance = 3;
    [Header("是否可控制相机距离(鼠标中键)")]
    public bool canControlDistance = true;
    [Header("更改相机距离的速度")]
    public float distanceSpeed = 1;

    [Header("视角灵敏度")]
    public float rotateSpeed = 1;
    [Header("物体转向插值(灵敏度,取值为0到1)")]
    public float TargetBodyRotateLerp = 0.3f;
    [Header("需要转向的物体")]
    public GameObject TargetBody;//此脚本能操作转向的物体
    [Header("相机焦点物体")]
    public GameObject CameraPivot;//相机焦点物体  

    [Header("是否可控制物体转向")]
    public bool CanControlDirection = true;

    private Vector3 offset;//偏移
    // Start is called before the first frame update
    void Start()
    {
        //获取目标与相机之间的差值
        offset = transform.position - CameraPivot.transform.position;
    }

    void FreeCamera()
    {
        offset = offset.normalized * freeDistance;

        transform.position = Vector3.Lerp(transform.position, CameraPivot.transform.position + offset, 1f);//更新位置

        if (CanControlDirection)//控制角色方向开关
        {
            Quaternion TargetBodyCurrentRotation = TargetBody.transform.rotation;
            TargetBody.transform.rotation = Quaternion.Lerp(TargetBodyCurrentRotation, Quaternion.Euler(new Vector3(TargetBody.transform.localEulerAngles.x, transform.localEulerAngles.y, TargetBody.transform.localEulerAngles.z)), TargetBodyRotateLerp);

        }

        //限制相机距离
        if (canControlDistance)//控制距离开关
        {
            freeDistance -= Input.GetAxis("Mouse ScrollWheel") * distanceSpeed;
        }
        freeDistance = Mathf.Clamp(freeDistance, minDistance, maxDistance);


        //保证相机的注视
        transform.LookAt(CameraPivot.transform.position);


        //控制相机随鼠标的旋转
        float inputY = Input.GetAxis("Mouse Y");
        float inputX = Input.GetAxis("Mouse X");
        transform.RotateAround(CameraPivot.transform.position, Vector3.up, rotateSpeed * inputX);
        transform.RotateAround(CameraPivot.transform.position, TargetBody.transform.right, -rotateSpeed * inputY);


        //旋转之后以上方向发生了变化,需要更新方向向量
        offset = transform.position - CameraPivot.transform.position;
        offset = offset.normalized * freeDistance;
        transform.position = CameraPivot.transform.position + offset;
    }


    private void PreventThroughWall()
    {
        RaycastHit hitInfo;
        Vector3 fwd = Camera.main.transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(Camera.main.transform.position, fwd, out hitInfo, 0.5f))
        {
            float dis = hitInfo.distance;
            Vector3 correction = Vector3.Normalize(Camera.main.transform.TransformDirection(Vector3.back)) * dis * Time.deltaTime;
            Camera.main.transform.position += correction;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        FreeCamera();
    }

    // Update is called once per frame
    void Update()
    {
        //PreventThroughWall();
    }
}
