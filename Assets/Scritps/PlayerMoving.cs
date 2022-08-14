using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        if (GameManager.inst.startBool == false)
        {
            return;
        }
        if (GameManager.inst.clickL)
        {
            transform.Translate(-GameManager.inst.moveSpeed*Time.deltaTime,0,0);
            
        }

        else if (GameManager.inst.clickR)
        {
            transform.Translate(GameManager.inst.moveSpeed * Time.deltaTime, 0, 0);
        }

        if (transform.position.x <GameManager.inst.mapLimit-0.83f)
        {
            transform.position = new Vector3(GameManager.inst.mapLimit-0.83f, transform.position.y, transform.position.z);

        }
        if (transform.position.x > GameManager.inst.mapLimit+0.83f)
        {
            transform.position = new Vector3(GameManager.inst.mapLimit+0.83f, transform.position.y, transform.position.z);

        }
    }
    
    
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy":

                if (GameManager.inst.isNoDamage)
                {
                    other.gameObject.SetActive(false);
                    ObjectPooler.SpawnFromPool("Effect_0", other.transform.position, Quaternion.identity);

                    int s = other.GetComponent<EnemyScore>().score;
                    GameManager.inst.PlusScore(s);
                }
                else
                {
                    other.gameObject.SetActive(false);
                    ObjectPooler.SpawnFromPool("Effect_0", other.transform.position, Quaternion.identity);
                    GameManager.inst.PlayerDamage();
                }
                
                break;
            case "Coin":

                GameManager.inst.PlusScore(1);
                GameManager.inst.mp+=3;
                ObjectPooler.SpawnFromPool("Effect_1", other.transform.position, Quaternion.identity);
                other.gameObject.SetActive(false);
                break;
            case "Star":
                GameManager.inst.PlusScore(3);
                GameManager.inst.mp+=6;
                ObjectPooler.SpawnFromPool("Effect_2", other.transform.position, Quaternion.identity);
                other.gameObject.SetActive(false);
                break;
            case "Heart":
                other.gameObject.SetActive(false);
                ObjectPooler.SpawnFromPool("Effect_3", other.transform.position, Quaternion.identity);
                GameManager.inst.hp++;
                break;

            case "Clock":
                //print(other.gameObject.tag);
                other.gameObject.SetActive(false);
                ObjectPooler.SpawnFromPool("Effect_4", other.transform.position, Quaternion.identity);
                GameManager.inst.gameTime+=10;
                GameManager.inst.TimeTextSet();
                break;
            default:
                break;
        }


        //if(other.gameObject.tag ==)
    }
    
}
