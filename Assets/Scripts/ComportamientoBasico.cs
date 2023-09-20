using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportamientoBasico : MonoBehaviour
{
    [SerializeField]
    internal Vector3 direccionMovimiento;

    private Rigidbody2D rb;

    public float velocidad, velocidadCaida;

    public bool enAire;
    [Range(0,1)]
    public float valorChoque = 0.6f;

    // Start is called before the first frame update
    void Start()
    {
        direccionMovimiento = -transform.right;
        rb = GetComponent<Rigidbody2D>();
        valorChoque = 0.6f;
        enAire = true;
    }

    // Update is called once per frame
    void Update()
    {
        Comportamiento();
    }

    internal virtual void Comportamiento()
    {
        if (enAire)
        {
            rb.velocity = Vector2.down * velocidadCaida;
        }
        else
        {
            rb.velocity = direccionMovimiento * velocidad;
        }
    }

    internal virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.layer);
        if(collision.gameObject.layer == 9)
        {
            collision.transform.GetComponent<ControlVida>().TomarSeta(transform.tag);
            Destroy(this.gameObject);
        }
        else if(collision.gameObject.layer == 10 || collision.gameObject.layer == 7)
        {
            Vector2 down = transform.TransformDirection(Vector3.down).normalized;
            Vector2 toHit = (collision.GetContact(0).point - (Vector2)transform.position).normalized;

            float dotProduct = Vector2.Dot(down, toHit);
            if(((dotProduct < valorChoque && dotProduct>0) || (dotProduct<-valorChoque && dotProduct < 0)) && !enAire)
            {
                direccionMovimiento *= -1;
            }
            else
            {
                enAire = false;
            }
        }
        
    }
    internal virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10 || collision.gameObject.layer == 7)
        {
            enAire = true;
        }
    }
    internal virtual void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10 || collision.gameObject.layer == 7)
        {
            enAire = false;
        }
    }
}
