using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerVacio : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.layer)
        {
            case 8:
            case 11:

                Destroy(collision.gameObject);

                break;

            case 9:

                collision.GetComponent<ControlVida>().Caerse();
                break;
        }
    }
}
