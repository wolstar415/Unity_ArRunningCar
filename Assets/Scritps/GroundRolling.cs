using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundRolling : MonoBehaviour
{
    public float speed;
    private float limitZ;
    public List<GameObject> childOb;
    public float limitPosZ = -15f;

    private void Awake()
    {
        limitZ = childOb[0].transform.position.z + limitPosZ;
        //다시 돌아올 거리 설정
    }

    void Update()
    {
        if (GameManager.Inst.isStart == false)
        {
            return;
        }

        foreach (var ob in childOb)
        {
            ob.transform.Translate(0, 0, -speed * Time.deltaTime);
            //배경 이동
            if (ob.transform.position.z < limitZ)
                //거리가 벗어나면 다시 돌아옴
            {
                ob.transform.Translate(0, 0, -limitZ * childOb.Count);
            }
        }
    }
}