using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundRolling : MonoBehaviour
{
    private Vector3 originPos;
    public float speed;
    private float limitz;
    public List<GameObject> childOb;

    private void Awake()
    {
        originPos = childOb[0].transform.position;
        limitz = childOb[0].transform.position.z- 15;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.inst.startBool == false)
        {
            return;
        }
        foreach (var ob in childOb)
        {
            ob.transform.Translate(0, 0, -speed*Time.deltaTime);
            if (ob.transform.position.z < limitz)
            {
                ob.transform.Translate(0, 0, -limitz*2);
            }
        }
        
    }
}
