using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Magazine : MonoBehaviour
{
    public Text Content;
    public Animator anim;

    public Text Hp, Hunger, Money; 

    private void Start()
    {
        anim = GetComponent<Animator>();

        PlayerManager.instance.magazine = anim;
        PlayerManager.instance.magazineContent = Content;

        PlayerManager.instance.hp_ = Hp;
        PlayerManager.instance.hunger_ = Hunger;
        PlayerManager.instance.money_ = Money;

        PlayerManager.instance.OnEnterHome();
    }
    public void OnLevelWasLoaded(int level)
    {
        //if (level == 2) Start();
    }
    public void UsePlayerManager(bool hp)
    {
        PlayerManager.instance.Buy(hp);
    }
    public void UsePlayerManager2()
    {
        PlayerManager.instance.ShowMagazine();
    }
}
