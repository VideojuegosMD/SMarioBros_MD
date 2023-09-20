using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaDeFuego : MonoBehaviour
{
    public float valorChoque, velMax;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector2 velDir = rb.velocity.normalized;
        if (rb.velocity.x > velMax || rb.velocity.x < -velMax)
        {
            rb.velocity = new Vector2(velDir.x * velMax, rb.velocity.y);
        }
        if (rb.velocity.y > velMax || rb.velocity.y < -velMax)
        {
            rb.velocity = new Vector2(rb.velocity.x, velDir.y * velMax);
        }
    }
    private void Update()
    {
        Vector2 velDir = rb.velocity.normalized;
        if (rb.velocity.x > velMax)
        {           
            rb.velocity = new Vector2(velDir.x * velMax, rb.velocity.y);
        }
        if (rb.velocity.y > velMax)
        {
            rb.velocity = new Vector2(rb.velocity.x, velDir.y * velMax);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10 || collision.gameObject.layer == 7)
        {
            Vector2 down = transform.TransformDirection(Vector3.down).normalized;
            Vector2 toHit = (collision.GetContact(0).point - (Vector2)transform.position).normalized;

            float dotProduct = Vector2.Dot(down, toHit);
            Debug.Log(transform.name + " " + dotProduct);
            if (((dotProduct < valorChoque && dotProduct > 0) || (dotProduct > -valorChoque && dotProduct < 0)))
            {
                //Destruir/ChoqueVertical 
                Destroy(this.gameObject);
            }
            
        }
        else if(collision.gameObject.layer == 8)
        {  
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}
