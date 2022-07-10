using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }

    public bool isOpenedBridge;
    public Animator Bridge, Lever;
    public BoxCollider2D StoperShip, StoperCar;

    public EnterMessage enterMessage;
    private void Start()
    {
        SetLeverState(false);
        DayStart();
        DLCon = true;
        ChangeDLC();
        PauseMenu.SetActive(false);
    }
    public void SetLeverState(bool IsOpened)
    {
        isOpenedBridge = IsOpened;
        StoperShip.enabled = !IsOpened;
        StoperCar.enabled = IsOpened;
        Lever.SetBool("IsUp", !isOpenedBridge);
        Bridge.SetBool("IsOpened", isOpenedBridge);
    }
    public void DayStart()
    {
        string mess = "";
        if (Random.Range(0, 2) == 0)
        {
            Timer.gameObject.SetActive(false);
            for (int i = 0; i < Plan.Count; i++)
            {
                Plan[i] = 0;
                canGo[i] = true;
                if (Random.Range(0, 3) < 2 & ((i != PlayerManager.instance.EventType) | !PlayerManager.instance.IsEvent))
                {
                    Plan[i] = Random.Range(Diff - 1, Diff + 3);
                    mess += Colors[i] + " - " + Plan[i] + "\n";
                }
            }
        }
        else
        {
            Timer.gameObject.SetActive(true);
            mess = "Запрещенно :" + "\n";
            int count = 0;
            for (int i = 0; i < canGo.Count; i++)
            {
                Plan[i] = 0;
                canGo[i] = true;
                if ((Random.Range(0, 3) < 2 && count<2) | ((count == 0) & (i==2)))
                {
                    count++;
                    canGo[i] = false;
                    mess += Colors[i] + "\n";
                }
            }
            StartCoroutine(Waiter());
        }
        enterMessage.SetMessage(mess, false);
    }

    public int Diff;
    public string[] Colors = { "Чёрные", "Синие", "Жёлтые", "Красные" };
    public List<int> Plan; //Black -> Blue -> Yellow -> Red
    public List<bool> canGo; //Black -> Blue -> Yellow -> Red

    public Transform ShipSpawn, ShipEx, CarSpawn, CarEx;
    public GameObject[] Cars;
    public GameObject[] Ships;
    public float SpawnWait, DayLong;

    public bool isSpawn;
    public LayerMask CarAndShip;
    public float CarAndShipCheck;
    public void StartSpawn()
    {
        StopCoroutine(SpawnCar());
        StopCoroutine(SpawnShip());
        isSpawn = true;
        StartCoroutine(SpawnCar());
        StartCoroutine(SpawnShip());
    }
    public IEnumerator SpawnCar()
    {
        while (isSpawn)
        {
            if(!Physics2D.OverlapCircle(CarSpawn.position, CarAndShipCheck, CarAndShip))
                GameObject.Instantiate(Cars[Random.Range(0, Cars.Length)], CarSpawn);
            if(!isEvent && (Random.Range(0, 18) == 0))
            {
                StartEvent();
            }
            yield return new WaitForSeconds(SpawnWait + Random.Range(0, SpawnWait / 3f));
        }
    }
    public IEnumerator SpawnShip()
    {
        while (isSpawn)
        {
            if (!Physics2D.OverlapCircle(ShipSpawn.position, CarAndShipCheck, CarAndShip))
                GameObject.Instantiate(Ships[Random.Range(0, Ships.Length)], ShipSpawn);
            if (!isEvent && (Random.Range(0, 18) == 0))
            {
                StartEvent();
            }
            yield return new WaitForSeconds(SpawnWait + Random.Range(0, SpawnWait/3f));
        }
    }
    public IEnumerator Waiter()
    {
        float timeLeft = DayLong;
        while (timeLeft > 0)
        {
            timeLeft -= 0.5f;
            Timer.fillAmount = timeLeft / DayLong;
            yield return new WaitForSeconds(0.5f);
        }
        PlayerManager.instance.GoHome(score, errors);
    }
    public int score, errors;
    public Text score_, errors_;
    public Image Timer;

    public AudioSource Clip, Background;
    public AudioClip coin, turn;
    public AudioClip StandartBack, NYBack;
    public void AddTransport(bool front, int type, Transport trans, bool car)
    {
        if (trans != null)
        {
            Clip.clip = turn;
            Clip.Play();
            trans.direction *= -1;
            trans.transform.rotation = Quaternion.identity;
            if (trans.direction.x == 0)
            {
                trans.transform.Rotate(new Vector3(0, 0, 180));
                trans.transform.position = CarEx.position;
            }
            else
            {
                trans.transform.localScale = new Vector2(-trans.transform.localScale.x, trans.transform.localScale.y);
                trans.transform.position = ShipEx.transform.position;
            }
        }
        else
        {
            if (!isEvent)
            {
                PlayerManager.instance.AddToEvent(car, type);
                if (canGo[type] == front)
                {
                    score++; 
                    Clip.clip = coin;
                    Clip.Play();
                    if (Plan[type] > 0)
                    {
                        Plan[type]--;
                        if (Plan[type] < 0) Plan[type] = 0;
                        score++;
                        string mess = "";
                        for (int i = 0; i < Plan.Count; i++)
                        {
                            if (Plan[i] != 0) mess += Colors[i] + " - " + Plan[i] + "\n";
                        }
                        
                        enterMessage.Allways.text = mess;
                        if ((Plan[0] == 0) & (Plan[1] == 0) & (Plan[2] == 0) & (Plan[3] == 0)) PlayerManager.instance.GoHome(score, errors);
                    }
                }
                if (!canGo[type] && front) 
                    errors++;
            }
            else
            {
                if(front && (car == EventCar))
                {
                    EventCount--;
                    if(EventCount == 0)
                    {
                        isEvent = false;
                        SpawnWait *= 10;
                        score += 10;
                        Clip.clip = coin;
                        Clip.Play();
                    }
                }
            }
        }

        score_.text = "Счёт " + score;
        errors_.text = "Ошибки " + errors;
    }
    public bool isEvent;
    public int EventCount;
    public bool EventCar;
    public void StartEvent()
    {
        EventCount = Random.Range(Diff - 1, Diff + 4);
        isEvent = true;
        EventCar = Random.Range(0, 2) == 0;
        SpawnWait /= 10;
        string mess = "!Срочно!" + "\n" + EventCount;
        if (EventCar) mess += " Машин";
        else mess += " Кораблей";
        enterMessage.SetMessage(mess, true);
    }

    public List<GameObject> StandartContent, DLCContent;
    public bool DLCon;

    public GameObject PauseMenu, dot_;
    public ParticleSystem Snow;
    public Color BackColorStandart, BackColorNY;
    public Camera camera;
    public void ChangeDLC()
    {
        DLCon = !DLCon;
        dot_.SetActive(DLCon);
        foreach (GameObject obj in StandartContent)
        {
            obj.SetActive(!DLCon);
        }
        foreach (GameObject obj in DLCContent)
        {
            obj.SetActive(DLCon);
        }
        Snow.gameObject.SetActive(DLCon);
        Snow.Play();
        if (DLCon)
        {
            Background.clip = NYBack;
            camera.backgroundColor = BackColorNY;
            Bridge = DLCContent[0].GetComponent<Animator>();
            Lever = DLCContent[1].GetComponent<Animator>();
        }
        else
        {
            Background.clip = StandartBack;
            camera.backgroundColor = BackColorStandart;
            Bridge = StandartContent[0].GetComponent<Animator>();
            Lever = StandartContent[1].GetComponent<Animator>();
        }
        Background.Play();
    }
    public void OpenPauseMenu()
    {
        Time.timeScale = 1.1f - Time.timeScale;
        PauseMenu.SetActive(!PauseMenu.activeInHierarchy);
    }
}
