using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy") && GameManager.Inst.isCarRotate && GameManager.Inst.isBooster == false)
        {
            GameManager.Inst.TimeTextSet(1);
            GameManager.Inst.Mp += 6;
            ObjectPooler.SpawnFromPool("Effect_5", other.transform.position, Quaternion.identity);
        }

    }
}
