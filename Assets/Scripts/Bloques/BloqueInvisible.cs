using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloqueInvisible : Bloque
{
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;
    [SerializeField]
    private bool invisible, indestructible;


    public void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        boxCollider = GetComponentInChildren<BoxCollider2D>();

        tipoBloque = TipoDeBloque.Invisible;     
        invisible = true;      
        spriteRenderer.enabled = false;
        boxCollider.isTrigger = true;   
    }

    public override void RestarToque()
    {
        if (invisible)
        {
            InteraccionConToquesRestantes();
        }       
    }        
   
    public override void InteraccionConToquesRestantes()
    {
        //Reproducimos el sonido y animación del golpeo
        ReproducirSonido();
        ReproducirAnim("Base Layer.Golpe");

        spriteRenderer.enabled = true;        
        boxCollider.isTrigger = false;

        Bloque b = gameObject.AddComponent<Bloque>();
        if (indestructible)
        {
            b.tipoBloque = TipoDeBloque.Indestructible;  
        }
        else
        {
            b.tipoBloque = TipoDeBloque.Destructible;
            b.nToques = nToques;
        }

        Destroy(this);
        
    }
  
    //Función para reproducir el sonido
    public override void ReproducirSonido()
    {
        //Sonido de golpeo
    }
}
