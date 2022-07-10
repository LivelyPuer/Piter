using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transport : MonoBehaviour
{
    public float speed, checkRadius, checkOffset;
    public Vector3 direction;
    public bool isMoving;
    public LayerMask checking, bridge;
    public Rigidbody2D rg;
    public float i, t;

    public int type;
    public bool car;
    private void Start()
    {
        rg = GetComponent<Rigidbody2D>();
    }
    public void Update()
    {
        if (!Physics2D.OverlapCircle(transform.position + (direction * checkOffset), checkRadius, checking))
        {
            rg.velocity = direction * speed;
            isMoving = true;
            GetComponent<ParticleSystem>().Play();
            i = 0;
        }
        else
        {
            rg.velocity = Vector2.zero;
            isMoving = false;
            GetComponent<ParticleSystem>().Stop(); 
            i += Time.deltaTime;
            if (i > t)
            {
                GameManager.instance.AddTransport(false, type, this, car);
                i = 0;
            }
        }
    }

}
