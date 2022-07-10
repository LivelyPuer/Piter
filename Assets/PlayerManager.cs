using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void FirstTime()
    {
        hp_.text = "Здоровье - " + Hp + "/" + HpMax;
        hunger_.text = "Сытость - " + Hunger + "/" + HungerMax;
        money_.text = money.ToString();
        magazineContent.text = enterMess;
        magazine.SetBool("show", true);
    }

    public int Hp, HpMax, HpOut, Hunger, HungerMax, HungerOut;
    public int HpCost, HungerCost;
    public Text hp_, hunger_, money_;
    public int money;

    public string[] EventTexts;
    public string enterMess;

    public int EndingCounter;

    public int EventCount, EventType, EventDiff;
    public bool EventCar, IsEvent;

    public int score, errs;

    public int DayCount;
    public void GoHome(int score_, int errs_)
    {
        score = score_;
        errs = errs_;
        DayCount++;
        SceneManager.LoadScene(3);
    }
    public void OnEnterHome()
    {
        if (DayCount == 0) FirstTime();
        else
        {
            Hp -= HpOut;
            Hunger -= HungerOut;
            money += score;
            money -= errs * 2;

            if (money < 0) SceneManager.LoadScene(7);
            if (money > 250) SceneManager.LoadScene(6);

            IsEvent = false;

            hp_.text = "Здоровье - " + Hp + "/" + HpMax;
            hunger_.text = "Сытость - " + Hunger + "/" + HungerMax;
            money_.text = money.ToString();

            if ((Hp <= 0) | (Hunger <= 0)) SceneManager.LoadScene(5);
            if (EventCount > 0)
            {
                EndingCounter++;
                if (EndingCounter >= 5) SceneManager.LoadScene(6);
            }
        }
    }

    public void Buy(bool hp)
    {
        if(hp && (HpMax - Hp >= HpOut))
        {
            Hp += HpOut;
            money -= HpCost;
            hp_.text = "Здоровье - " + Hp + "/" + HpMax;
        }
        if(!hp && (HungerMax - Hunger >= HungerOut))
        {
            Hunger += HungerOut;
            money -= HungerCost;
            hunger_.text = "Сытость - " + Hunger + "/" + HungerMax;
        }
        money_.text = money.ToString();
        
        if (money < 0) SceneManager.LoadScene(7);
        if (money > 250) SceneManager.LoadScene(6);
    }

    public Animator magazine;
    public Text magazineContent;

    public string[] Colors = { "׸Черных", "Синих", "Жёлтых", "Красных" };
    public string[] Types = { "Кораблей", "Мишин" };
    public void ShowMagazine()
    {
        string mess = "";
        mess += EventTexts[Random.Range(0, EventTexts.Length)] + " ";
        mess += Random.Range(EventDiff, EventDiff + 2) + " ";
        EventType = Random.Range(0, 4);
        EventCar = Random.Range(0, 2) == 0;
        IsEvent = true;
        mess += Colors[EventType] + " ";
        mess += Types[EventCar.GetHashCode()];
        magazineContent.text = mess;
        magazine.SetBool("show", true);
    }

    public void AddToEvent(bool car, int type)
    {
        if (IsEvent)
        {
            if((car == EventCar)&(type == EventType))
            {
                EventCount--;
                if(EventCount <= 0)
                {
                    IsEvent = false;
                    EndingCounter--;
                    if (EndingCounter <= -5) SceneManager.LoadScene(7);
                }
            }
        }
    }
}
