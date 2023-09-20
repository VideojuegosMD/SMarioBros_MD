using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonedasConTiempo : MonoBehaviour
{
    //Tendremos una jerarquía de objetos con todas las monedas preparadas.
    //Este script tendrá referencia del padre de los objetos de las monedas
    //Necesitamos un contador con un limite.
    //Solo se activa una vez
    //Hay que activarlo con un botón

    public Transform padreMonedas;
    public Material mMonedas;
    public float tLimite, tContador, tColor,velColor;
    public bool activo, monedasActivas, invertir;

    // Start is called before the first frame update
    void Start()
    {
        tColor = 0;
        tContador = 0;
        activo = false;
        monedasActivas = false;
        invertir = false;
        ActivarDesactivarMonedas(false);
       
    }

    // Update is called once per frame
    void Update()
    {
        if (activo)
        {
            //Activar monedas (solo una vez)
            if (!monedasActivas)
            {
                ActivarDesactivarMonedas(true);
                monedasActivas = true;
            }
            if(tContador> tLimite - 5f)
            {
                ChangeAlpha();
                velColor += Time.deltaTime * 2;
            }
            //Contamos tiempo para desactivar
            if (tContador < tLimite)
            {
                tContador+=Time.deltaTime;
            }
            else 
            {
                activo = false;
                //Desactivar monedas (solo una vez)
                ActivarDesactivarMonedas(false);
                Destroy(padreMonedas.gameObject);
                Destroy(this);
            }
        }
    }

    public void ActivarDesactivarMonedas(bool activarONo)
    {
        foreach(Transform t in padreMonedas)
        {
            t.gameObject.SetActive(activarONo);
        }
    }

    public void ChangeAlpha()
    {
        if (!invertir)
        {
            tColor += Time.deltaTime * velColor;

        }
        else
        {
            tColor -= Time.deltaTime * velColor;

        }

        if (tColor > 1)
        {
            invertir = true;
        }
        else if(tColor<0)
        {
            invertir=false;
        }

        mMonedas.SetColor("_Color", new Color(1, 0, 0, tColor));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            activo = true;
        }
    }
}
