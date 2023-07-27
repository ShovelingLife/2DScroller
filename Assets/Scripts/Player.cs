using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    Rigidbody2D rb2D;

    public Vector3 position
    {
        get => transform.position;
        set => transform.position = value;
    }

    public void SetVelocity(float vel)
    {
        if (rb2D != null)
            rb2D.velocity = new Vector2(rb2D.velocity.x, Mathf.Clamp(rb2D.velocity.y, vel, float.MaxValue));

        //else if (rb != null)
        //    rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -vel, float.MaxValue));
    }

    private void Awake()
    {
        InitComponents();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(transform.position, transform.right, Color.red, 1);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Global.inputComp.CheckCollision(collision);
    }

    void InitComponents()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
            rb2D = GetComponent<Rigidbody2D>();
    }
}
