using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy") && GameManager.inst.isCarRotate && GameManager.inst.isBooster == false)
        {
            GameManager.inst.gameTime += 1f;
            GameManager.inst.TimeTextSet();
            GameManager.inst.mp += 6;
            ObjectPooler.SpawnFromPool("Effect_5", other.transform.position, Quaternion.identity);
        }

    }
}
