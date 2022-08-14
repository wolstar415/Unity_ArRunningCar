using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst;
    private void Awake() => Inst = this;
    [HideInInspector]
    public float mapLimit;
    [Header("컴포넌트")]
    public ArManager arManager;
    [Header("실수")]
    public float moveSpeed = 1f;
    public float rotationAngle;

    [Header("오브젝트")]
    public GameObject groundOb;//배경
    public GameObject startBtnOb;//시작버튼
    public GameObject playerOb;//플레이어
    public GameObject playerObRotation;//회전 오브젝트
    public GameObject[] levelOb;//레벨
    public GameObject gameOverOb;//게임오버 UI
    public GameObject boosterEffectOb;//이펙트

    
    [Header("Bool")]
    public bool isStart;//게임시작
    public bool isBooster;//부스터
    public bool isGameOver;//게임오버
    public bool isNoDamage;//무적
    public bool isDamaged;//데미지받음
    public bool isCarRotate;//이동중

    [Header("Canvans")]
    public GameObject lobyCanvans;
    public GameObject gameCanvans;

    [Header("ObjectPool")]
    public string[] enemyName;
    public string[] itemName;


    CancellationTokenSource gameOverCancellation = new CancellationTokenSource();

    [Header("스텟")] 
    private int _hp;//체력
    private int _mp;//게이지
    public int Hp
    {
        get => _hp;
        set
        {
            int v = Mathf.Clamp(value, 0, 5);
            
            if (v <= 0)
            {
                GameOver();
                return;
            }

            _hp = v;
            hpSlider.value = _hp;
        }
    }
    public int Mp
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
    

    [Header("게임정보")]
    public float gameTime;//남은시간
    public int score;//점수

    [Header("UI")] public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;
    public Slider hpSlider;
    public Slider mpSlider;



    public bool clickL;
    public bool clickR;


    public void PlusScore(int i)
    //점수 추가
    {
        score += i;
        scoreText.text = $"Score : {score}";
    }
    public void TimeTextSet()
    {
        timeText.text = $"Time : {gameTime:F2}";
    }
    public void TimeTextSet(float f)
    {
        gameTime += f;
        timeText.text = $"Time : {gameTime:F2}";
    }
    public void ClickDownBtnL()
    //왼쪽이동 버튼 누름
    {
        clickL = true;
        isCarRotate = true;
        playerObRotation.transform.rotation = Quaternion.Euler(new Vector3(0, -rotationAngle, 0));
    }

    public void ClickUpBtnL()
    //왼쪽이동 버튼 뗌
    {
        clickL = false;
        if (clickR != false || clickL != false) return;
        playerObRotation.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        isCarRotate = false;
    }

    public void ClickDownBtnR()
    //오른쪽이동 버튼 누름
    {
        clickR = true;
        isCarRotate = true;
        playerObRotation.transform.rotation = Quaternion.Euler(new Vector3(0, rotationAngle, 0));
    }

    public void ClickUpBtnR()
    //오른쪽이동 버튼 뗌
    {
        clickR = false;
        if (clickR != false || clickL != false) return;
        playerObRotation.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        isCarRotate = false;
    }

    public void Setting()
    //바닥설정
    {
        arManager.PlaceOrigin();
        if (playerOb.activeSelf) return;
        playerOb.SetActive(true);
        startBtnOb.SetActive(true);
    }

    public async UniTask GameStart()
    //게임시작버튼
    {
        gameTime = 30f;
        TimeTextSet();
        isStart = true;
        groundOb.SetActive(true);
        mapLimit = playerOb.transform.position.x;
        arManager.PlaneOff();
        Hp = 5;
        Mp = 50;
        lobyCanvans.SetActive(false);
        gameCanvans.SetActive(true);
        levelOb[0].SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(30f), false, PlayerLoopTiming.Update, gameOverCancellation.Token);
        levelOb[1].SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(30f), false, PlayerLoopTiming.Update, gameOverCancellation.Token);
        levelOb[2].SetActive(true);

    }
    

    private void Update()
    {
        if (isStart == false)
        {
            return;
        }

        if (isBooster)
        {
            if (playerOb.transform.position.z <= 1)
            {
                playerOb.transform.Translate(0, 0, Time.deltaTime);
            }

            mpSlider.value = mpSlider.value - 0.5f;
            if (mpSlider.value == 0)
            {
                
                BoosterOff().Forget();
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

    private void GameOver()
    //게임 종료
    {
        gameTime = 0;
        TimeTextSet();
        isGameOver = true;
        hpSlider.value = 0;
        mpSlider.value = 0;
        Destroy(playerOb);
        gameOverOb.SetActive(true);
        Time.timeScale = 0f;
        gameOverCancellation.Cancel();
    }

    public async UniTaskVoid PlayerDamage()
    {
        if (isDamaged)
        {
            return;
        }
        Hp--;
        isDamaged = true;
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        isDamaged = false;
    }


    private void BoosterOn()
    {
        _mp = 0;
        isBooster = true;
        isNoDamage = true;
        Time.timeScale = 2f;
        boosterEffectOb.SetActive(true);
    }

    private async UniTask BoosterOff()
    {
        isBooster = false;
        Time.timeScale = 1.0f;
        while (playerOb.transform.position.z > 0)
        {
            playerOb.transform.Translate(0, 0, -Time.deltaTime);
            await UniTask.Yield();
        }
        playerOb.transform.position = new Vector3(transform.position.x, playerOb.transform.position.y, 0);
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        boosterEffectOb.SetActive(false);
        isNoDamage = false;
    }
}