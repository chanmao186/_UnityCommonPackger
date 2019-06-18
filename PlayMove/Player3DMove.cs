/*
 * 在3d系统中控制角色移动
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMove : MonoBehaviour
{
    float ver = 0;
    float hor = 0;
    public float turnspeed = 10f;
    public float speed = 6f;
    // Use this for initialization
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        hor = Input.GetAxis("Horizontal");
        ver = Input.GetAxis("Vertical");
    }
    void move(float hor, float ver)
    {
        transform.Translate(hor * speed * Time.deltaTime, 0, ver * speed * Time.deltaTime);
    }
    void FixedUpdate()
    {
        if (hor != 0 || ver != 0)
        {
            //转身
            move(hor, ver);
        }
    }
}
