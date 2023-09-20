using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * Clase base para los bloques.
 * 
 * Esta clase puede derivarse mediante herencia, al utilizar funciones de tipo virtual podemos sobrescribir los comportamientos si queremos,
 * esto permite mucha flexibilidad de cara a hacer diversos bloques que actuen de distinta manera.
 * 
 * IMPORTANTE, si queremos interactuar desde otro script con un bloque deberiamos de comunicarnos con la clase base, aunque hayamos hecho una herencia,
 * de esta manera podemos comunicarnos con todos los bloques de la misma forma.
 *
 * Esta clase sirve para los bloques simples que pueden romperse o que son indestructibles.
 */

public class Bloque : MonoBehaviour
{
    /*
    * Pr�cticamente todas las variables y funciones est�n como p�blicas por comodidad durante el desarrollo
    * Es recomendable hacer una buena encapsulaci�n y si tenemos que mostrar una variable en el Editor usar el atributo [SerializeField]
    */

    internal Animator animator;

    //Enumeraci�n para diferenciar los tipos de bloque, en casos como este es mejor usar las tags
    public enum TipoDeBloque { Indestructible, Destructible, ConContenido, Invisible, Especial}

    //Variable de la enumeraci�n del tipo de bloque
    public TipoDeBloque tipoBloque;

    //Numero de toques del bloque, asignar desde el prefab o desde el editor
    public int nToques = 1;

    //Habr� que a�adir las variables para la reproducci�n del sonido.

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public virtual void RestarToque()
    {
        //Si el bloque no es indestructible entonces restamos toques y hacemos la interacci�n
        if(tipoBloque != TipoDeBloque.Indestructible)
        {
            //Restamos un toque, es lo mismo que escribir "nToques = nToques-1;"
            nToques--;
          
            InteraccionConToquesRestantes();

            //Si no quedan toques restantes
            if (nToques == 0)
            {
                //Hacemos la interacci�n correspondiente al bloque
                InteraccionSinToquesRestantes();
            }
        }
        else
        {
            //Hacemos la interacci�n correspondiente al bloque
            InteraccionConToquesRestantes();
        }
    }
   
    public virtual void InteraccionConToquesRestantes()
    {
        ReproducirAnim("Base Layer.Golpe");        
    }
    public virtual void InteraccionSinToquesRestantes()
    {
        ReproducirSonido();

        ReproducirAnim("Base Layer.Destruir");
    }
    //Funci�n para reproducir el sonido
    public virtual void ReproducirSonido()
    {
        //Sonido de golpeo
    }

    public void DestruirBloque()
    {
        Destroy(gameObject);
    }

    //Funci�n para reproducir la animaci�n
    public virtual void ReproducirAnim(string animName)
    {
        animator.Play(animName);
    }
}
