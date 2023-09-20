using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interfaces
{/*
  * Testing
    interface IMovimiento
    {
        internal void Chocar(Transform player, Collision2D collision, float valorChoque, out bool enAire, out bool direccionMovimiento)
        {
            Vector2 down = player.TransformDirection(Vector3.down).normalized;
            Vector2 toHit = (collision.GetContact(0).point - (Vector2)player.position).normalized;

            float dotProduct = Vector2.Dot(down, toHit);
            Debug.Log("Mensaje Seta: " + Vector2.Dot(down, toHit));
            if (((dotProduct < valorChoque && dotProduct > 0) || (dotProduct < -valorChoque && dotProduct < 0)) && !enAire)
            {
                direccionMovimiento *= -1;
            }
            else
            {
                enAire = false;
            }
        }
    }*/
}
