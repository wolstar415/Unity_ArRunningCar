using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float coolTimeMax;
    public float coolTimeMin;

    float coolTime;
    float culTime;

    void Start()
    {
        coolTime = Random.Range(coolTimeMin, coolTimeMax);
    }

    void Update()
    {
        culTime += Time.deltaTime;

        if (culTime >= coolTime)
        {
            coolTime = Random.Range(coolTimeMin, coolTimeMax);
            culTime = 0;
            var lotto = Random.Range(0, 100);
            if (GameManager.Inst.isBooster)
            {
                lotto = 99;
            }

            if (lotto >= 25)
            {
                MakeEnemy();
            }
            else
            {
                MakeItem();
            }
        }
    }

    void MakeEnemy()
    {
        int i = Random.Range(0, GameManager.Inst.enemyName.Length);
        ObjectPooler.SpawnFromPool(GameManager.Inst.enemyName[i], transform.position, Quaternion.identity);
    }

    void MakeItem()
    {
        int i;
        int lotto = Random.Range(0, 100);
        if (lotto <= 5)

        {
            i = 0;
        }
        else if (lotto <= 11)
        {
            i = 2;
        }

        else if (lotto < 50)
        {
            i = 3;
        }
        else
        {
            i = 1;
        }

        ObjectPooler.SpawnFromPool(GameManager.Inst.itemName[i], transform.position, Quaternion.identity);
        coolTime = Random.Range(coolTimeMin * 0.7f, coolTimeMax * 0.7f);
    }
}