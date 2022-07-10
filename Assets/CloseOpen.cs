using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseOpen : MonoBehaviour
{
    public Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(OpenToClose());
    }

    public IEnumerator OpenToClose()
    {
        while (true)
        {
            anim.SetBool("IsOpened", !anim.GetBool("IsOpened"));
            yield return new WaitForSeconds(2f);
        }
    }
}
