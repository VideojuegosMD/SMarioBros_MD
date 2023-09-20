using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeteccionTuberias : MonoBehaviour
{
    public Tuberia tuberia;
    public Controlador controlador;
    public bool enPosicion;   

    private void Start()
    {
        controlador = GetComponent<Controlador>();
        enPosicion = false;
    }

    private void Update()
    {
        if (tuberia != null)
        {
            Vector2 dirPlayerToPipe = (tuberia.transform.position-transform.position).normalized;

            Debug.Log(Vector2.Dot(controlador.inputDir, dirPlayerToPipe));
            if (Vector2.Dot(controlador.inputDir,dirPlayerToPipe) >= .95f && enPosicion)
            {
                GameManager.instance.FundidoOn();
                Debug.Log("TP");
                enPosicion = false;
                tuberia.Teletransporte(transform);
            }
            
          
        }
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "EntradaTuberia")
        {
            tuberia = collision.GetComponent<Tuberia>();
            enPosicion = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "EntradaTuberia")
        {
            tuberia = null;
            enPosicion = false;

        }
    }
}
