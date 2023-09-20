using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloqueContenido : Bloque
{
    [SerializeField]
    private bool abierto;

    [SerializeField]
    internal GameObject prefabContenido;

    [SerializeField]
    internal float distUp;

    [SerializeField]
    internal bool tieneMonedas;

    public void Start()
    {
        animator = GetComponent<Animator>();
        tipoBloque = TipoDeBloque.ConContenido;
        abierto = false;
       
    }

    public override void RestarToque()
    {
        if (!abierto)
        {
            if (tieneMonedas)
            {
                nToques--;
                InteraccionConToquesRestantes();
                //Si no quedan toques restantes
                if (nToques <= 0)
                {
                    //Hacemos la interacción correspondiente al bloque                    
                    InteraccionSinToquesRestantes();
                    abierto = true;
                }
                
            }
            else
            {
                InteraccionConToquesRestantes();
                abierto = true;
            }          
        }
        else
        {
            ReproducirSonido();
            ReproducirAnim("Base Layer.Golpe");
        }
    }     
    
   
    public override void InteraccionConToquesRestantes()
    {
        //Reproducimos el sonido y animación del golpeo
        ReproducirSonido();
        ReproducirAnim("Base Layer.Golpe");

        if (tieneMonedas)
        {
            GameObject g = Instantiate(prefabContenido, transform.position + transform.up * distUp, Quaternion.identity);
            g.GetComponent<Animator>().SetBool("DesdeBloque",true);
        }
        else
        {
            Instantiate(prefabContenido, transform.position + transform.up * distUp, Quaternion.identity);

        }
    }
    public override void InteraccionSinToquesRestantes()
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.gray;
       
    }

    //Función para reproducir el sonido
    public override void ReproducirSonido()
    {
        //Sonido de golpeo
    }
}
