using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float coolTimeMax;
    public float coolTimeMin;
    //public Object[] enemy1;
    //public Object[] Items;
    float coolTime;
    float culTime;
    // Start is called before the first frame update
    void Start()
    {
        coolTime = Random.Range(coolTimeMin, coolTimeMax);
    }

    // Update is called once per frame
    void Update()
    {
        culTime += Time.deltaTime;

        if (culTime >= coolTime)
        {
            coolTime = Random.Range(coolTimeMin, coolTimeMax);
            culTime = 0;
            int lotto = Random.Range(0, 100);
            //if (GameObject.Find("Player").transform.position.z > 0)
            //{
                
            //    lotto = 100;
                
                
            //}
            if (GameManager.inst.isBooster)
            {
                lotto = 99;
                
            }
            if (lotto >= 25 )

            {
                MakeEnemy();
            }
            else
            {
                MakeItem();


            }
            // Instantiate(enemy, transform.position, transform.rotation);
        }
    }
    void MakeEnemy()
    {
        int i = Random.Range(0, GameManager.inst.enemyName.Length);
        ObjectPooler.SpawnFromPool(GameManager.inst.enemyName[i], transform.position, Quaternion.identity);
    }

    void MakeItem()
    {
        int i = 1;
        int lotto = Random.Range(0, 100);
        if (lotto <= 5)
        
        {
            i = 0;
        }
        else if (5 < lotto && lotto <= 11 )
        {
            i = 2;
        }

        else if (11 < lotto && lotto < 50)
        {

            i = 3;
        }
        else
        {

            i = 1;
        }
        //Instantiate(Items[i], transform.position, transform.rotation);
        ObjectPooler.SpawnFromPool(GameManager.inst.itemName[i], transform.position, Quaternion.identity);
        coolTime = Random.Range(coolTimeMin * 0.7f, coolTimeMax * 0.7f);
    }
}
