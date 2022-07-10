using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitCollider : MonoBehaviour
{
    public bool isFront;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Transport>())
        {
            GameManager.instance.AddTransport(isFront, collision.gameObject.GetComponent<Transport>().type, null, collision.gameObject.GetComponent<Transport>().car);
            Destroy(collision.gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Transport>())
        {
            Destroy(collision.gameObject);
            GetComponent<AudioSource>().Play();
        }
    }
}
