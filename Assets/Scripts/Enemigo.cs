using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : ComportamientoBasico
{
    internal override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10 || collision.gameObject.layer == 7 || collision.gameObject.layer == 8)
        {
            Vector2 down = transform.TransformDirection(Vector3.down).normalized;
            Vector2 toHit = (collision.GetContact(0).point - (Vector2)transform.position).normalized;

            float dotProduct = Vector2.Dot(down, toHit);
            //Debug.Log(transform.name +" "+ dotProduct);
            if (((dotProduct < valorChoque && dotProduct > 0) || (dotProduct > -valorChoque && dotProduct < 0)) && !enAire)
            {
                direccionMovimiento *= -1;
                transform.position = transform.position + Vector3.right * direccionMovimiento.x * 0.05f;
               // Debug.Log(transform.name + " " + dotProduct + " " + direccionMovimiento.x);
            }
            else
            {
                enAire = false;
            }
        }       
    }
    internal override void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10 || collision.gameObject.layer == 7)
        {
            enAire = true;
        }
    }
    internal override void OnCollisionStay2D(Collision2D collision)
    {
       
       enAire = false;      

    }
}
