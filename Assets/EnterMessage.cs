using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnterMessage : MonoBehaviour
{
    public Animator anim;
    public string message;
    public GameObject MayorMessage, SimpleMessage;
    public Text Mayor, Simple, Allways;
    private void Start()
    {
        GameManager.instance.enterMessage = this;
        anim = GetComponent<Animator>();
    }
    public void SetMessage(string mess, bool isMayor)
    {
        Time.timeScale = 0.1f;
        anim.speed = 10;
        if (isMayor)
        {
            MayorMessage.SetActive(true);
            Mayor.text = mess;
        }
        else
        {
            SimpleMessage.SetActive(true);
            Simple.text = mess;
            Allways.text = mess;
        }
        anim.SetBool("show", true);
    }
    public void Continue()
    {
        anim.speed = 1;
        Time.timeScale = 1;
        anim.SetBool("show", false);
        MayorMessage.SetActive(false);
        SimpleMessage.SetActive(false);
        GameManager.instance.StartSpawn();
    }
}
