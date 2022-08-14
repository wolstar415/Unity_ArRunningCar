using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    private readonly float OFFSET = 0.83f;

    void Update()
    {
        if (GameManager.Inst.isStart == false)
        {
            return;
        }

        if (GameManager.Inst.clickL)
        {
            transform.Translate(-GameManager.Inst.moveSpeed * Time.deltaTime, 0, 0);
        }

        else if (GameManager.Inst.clickR)
        {
            transform.Translate(GameManager.Inst.moveSpeed * Time.deltaTime, 0, 0);
        }

        if (transform.position.x < GameManager.Inst.mapLimit - OFFSET)
        {
            transform.position =
                new Vector3(GameManager.Inst.mapLimit - OFFSET, transform.position.y, transform.position.z);
        }

        if (transform.position.x > GameManager.Inst.mapLimit + OFFSET)
        {
            transform.position =
                new Vector3(GameManager.Inst.mapLimit + OFFSET, transform.position.y, transform.position.z);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy":

                if (GameManager.Inst.isNoDamage)
                {
                    other.gameObject.SetActive(false);
                    ObjectPooler.SpawnFromPool("Effect_0", other.transform.position, Quaternion.identity);
                    int s = other.GetComponent<EnemyScore>().score;
                    GameManager.Inst.PlusScore(s);
                }
                else
                {
                    other.gameObject.SetActive(false);
                    ObjectPooler.SpawnFromPool("Effect_0", other.transform.position, Quaternion.identity);
                    GameManager.Inst.PlayerDamage().Forget();
                }

                break;
            case "Coin":

                GameManager.Inst.PlusScore(1);
                GameManager.Inst.Mp += 3;
                ObjectPooler.SpawnFromPool("Effect_1", other.transform.position, Quaternion.identity);
                other.gameObject.SetActive(false);
                break;
            case "Star":
                GameManager.Inst.PlusScore(3);
                GameManager.Inst.Mp += 6;
                ObjectPooler.SpawnFromPool("Effect_2", other.transform.position, Quaternion.identity);
                other.gameObject.SetActive(false);
                break;
            case "Heart":
                other.gameObject.SetActive(false);
                ObjectPooler.SpawnFromPool("Effect_3", other.transform.position, Quaternion.identity);
                GameManager.Inst.Hp++;
                break;

            case "Clock":
                other.gameObject.SetActive(false);
                ObjectPooler.SpawnFromPool("Effect_4", other.transform.position, Quaternion.identity);
                GameManager.Inst.TimeTextSet(10);
                break;
            default:
                Debug.LogWarning("오류" + other.gameObject.name);
                break;
        }
    }
}