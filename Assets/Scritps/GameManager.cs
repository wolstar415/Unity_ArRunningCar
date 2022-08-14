using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject startBtnOb;
    public static GameManager inst;
    public float moveSpeed = 1f;
    public float mapLimit;
    public GameObject playerOb;
    public GameObject playerObRotation;
    public float rotationAngle;
    public bool startBool = false;
    public GameObject ground;
    public ArManager arManager;

    public GameObject[] levelOb;

    public GameObject boosterEffectOb;

    public bool isBooster;

    public GameObject lobyCanvans;
    public GameObject gameCanvans;

    public bool isGameOver;

    public string[] enemyName;
    public string[] itemName;

    public bool isNoDamage;
    public bool isDamaged;

    public GameObject gameOverOb;

    [Header("스텟")] public bool isCarRotate = false;
    private int _hp;

    public int hp
    {
        get => _hp;
        set
        {
            int v = value;
            if (v >= 5)
            {
                v = 5;
            }
            else if (v <= 0)
            {
                
                GameOver();
                return;
            }
            _hp = v;
            hpSlider.value = _hp;
        }
    }
    private int _mp;


    public void TimeTextSet()
    {
        timeText.text = $"Time : {gameTime:F2}";
    }
    public int mp
    {
        get => _mp;
        set
        {
            if (isBooster)
            {
                return;
            }
            
            int v = value;
            v = Mathf.Clamp(v, 0, 100);
            if (v >= 100)
            {
                
                BoosterOn();
                return;
            }
            _mp = v;
            mpSlider.value = _mp;
        }
    }
    public float gameTime = 30f;
    public int score;

    [Header("UI")] 
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;
    public Slider hpSlider;
    public Slider mpSlider;
    private void Awake()
    {
        inst = this;
    }

    public bool clickL;
    public bool clickR;


    public void PlusScore(int i)
    {
        score += i;
        scoreText.text = $"Score : {score}";
    }

    public void PluseHp()
    {
        
    }

    public void ClickDownBtnL()
    {
        
            clickL = true;
            isCarRotate = true;
            playerObRotation.transform.rotation = Quaternion.Euler(new Vector3(0, -rotationAngle, 0));
        
    }
    public void ClickUpBtnL()
    {
        
            clickL = false;
            if (clickR == false && clickL == false)
            {
                playerObRotation.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                isCarRotate = false;
            }
        
        
    }
    public void ClickDownBtnR()
    {
        
            clickR = true;
            isCarRotate = true;
            playerObRotation.transform.rotation = Quaternion.Euler(new Vector3(0, rotationAngle, 0));
        
    }
    public void ClickUpBtnR()
    {
        
            clickR = false;
            if (clickR == false && clickL == false)
            {
                playerObRotation.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                isCarRotate = false;
            }
        
    }

    public void Setting()
    {
        arManager.PlaceOrigin();
        if (!playerOb.activeSelf)
        {
            playerOb.SetActive(true);
            startBtnOb.SetActive(true);
        }
    }

    public void GameStart()
    {
        gameTime = 30f;
        TimeTextSet();
        startBool = true;
        ground.SetActive(!ground.activeSelf);
        mapLimit = playerOb.transform.position.x;
        arManager.PlaneOff();
        hp = 5;
        mp = 50;
        StartCoroutine(LevelStart());

        lobyCanvans.SetActive(false); 
        gameCanvans.SetActive(true);
    }

    IEnumerator LevelStart()
    {
        levelOb[0].SetActive(true);
        yield return new WaitForSeconds(30f);
        levelOb[1].SetActive(true);
        yield return new WaitForSeconds(60f);
        levelOb[2].SetActive(true);
    }

    private void Update()
    {
        if (startBool==false)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ClickDownBtnL();
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            ClickUpBtnL();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ClickDownBtnR();
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            ClickUpBtnR();
        }
        if (isBooster == true)
        {
            if (playerOb.transform.position.z <= 1)
            {
                playerOb.transform.Translate(0, 0, Time.deltaTime);
            }
            mpSlider.value = mpSlider.value - 0.5f;
            if (mpSlider.value == 0)
            {
                isBooster = false;
                StartCoroutine(Co_BoosterOff());
                

            }

        }
        else

        {
            
            if (gameTime > 0 && isGameOver == false)
            {

                gameTime = gameTime - Time.deltaTime;
                TimeTextSet();
            }

            else
            {
                
                GameOver();
            }
            

        }
        
        
        
    }
    public void GameOver()
    {
        gameTime = 0;
        TimeTextSet();
        isGameOver = true;

        hpSlider.value=0;
        mpSlider.value=0;
        Destroy(playerOb);
        gameOverOb.SetActive(true);
        Time.timeScale = 0f;

    }
    public void PlayerDamage()
    {
        if (isDamaged)
        {
            return;
        }

        StartCoroutine(Co_Damager());

    }

    IEnumerator Co_Damager()
    {
        hp--;
        isDamaged = true;
        yield return new WaitForSeconds(0.5f);
        isDamaged = false;
        
    }



    public void BoosterOn()
    {
        _mp = 0;
        isBooster = true;
        isNoDamage = true;
        Time.timeScale = 2f;
        boosterEffectOb.SetActive(true);
    }
    
    IEnumerator  Co_BoosterOff()
    {
        Time.timeScale = 1.0f;
        while (playerOb.transform.position.z>0)
        {
            playerOb.transform.Translate(0, 0, -Time.deltaTime);
            yield return null;
        }
        playerOb.transform.position = new Vector3(transform.position.x, playerOb.transform.position.y, 0);
        yield return new WaitForSeconds(1f);
        boosterEffectOb.SetActive(false);
        isNoDamage = false;
    }
}
