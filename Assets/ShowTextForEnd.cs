using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowTextForEnd : MonoBehaviour
{
    public Text Content;
    public string Message;

    private void Start()
    {
        Content.text = "";
        StartCoroutine(Show());
    }
    public IEnumerator Show()
    {
        yield return new WaitForSeconds(4);
        for (int i = 0; i < Message.Length; i++)
        {
            Content.text += Message[i];
            if(Message[i] == '.') yield return new WaitForSeconds(0.8f);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
