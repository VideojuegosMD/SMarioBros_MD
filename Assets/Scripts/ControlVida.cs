using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlVida : MonoBehaviour
{
    Controlador controlador;
    //N?mero de vidas de juego y n?mero de vidas de Mario
    public int toques;

    public bool invulnerable,crecido;
    public float tContador,tInvulnerable;
  

    // Start is called before the first frame update
    void Start()
    {
        //Asignamos el n?mero de toques que puede recibir el personaje antes de perder una vida
        toques = 1;

        controlador = GetComponent<Controlador>();
        crecido = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -100) { Caerse(); }
        if (invulnerable)
        {
            tContador += Time.deltaTime;
            if (tContador >= tInvulnerable)
            {
                invulnerable = false;
                tContador = 0;
            }
        }
    }

    public void TomarSeta(string s)
    {
        if(s == "Seta1Toque")
        {
            if (toques == 1)
            {
               // controlador.SetSize(new Vector2(1, 1), new Vector2(1, 2), new Vector3(1, 1, 1), new Vector3(1, 2, 1), false,false);
                StartCoroutine(controlador.ISetSize(new Vector2(1, 1), new Vector2(1, 2), new Vector3(1, 1, 1), new Vector3(1, 2, 1), false, false));
                toques++;
            }
        }
        else if(s == "Seta1Vida")
        {
            GameManager.instance.numVidas++;
        }
        else if(s == "FlorFuego")
        {
            if (toques == 1)
            {
                //controlador.SetSize(new Vector2(1, 1), new Vector2(1, 2), new Vector3(1,1, 1), new Vector3(1, 2, 1), false,false);
                StartCoroutine(controlador.ISetSize(new Vector2(1, 1), new Vector2(1, 2), new Vector3(1, 1, 1), new Vector3(1, 2, 1), false, false));
                toques++;
            }
            else if(toques == 2)
            {
                if(GetComponent<Lanzarfuego>() == null)
                {
                    toques++;
                    gameObject.AddComponent<Lanzarfuego>();
                }
            }
        }
       
    }


    public void Caerse()
    {
        GameManager.instance.numVidas--;
        if (!GameManager.instance.CheckLost())
        {
            GameManager.instance.ReloadActualScene();
        }
      
    }
    //Funci?n para reaparecer en el punto de Spawn
    public void Reaparecer()
    {
        //La posici?n del jugador es la de inicio
        transform.position = controlador.puntoSpawn;

        //Asignamos un toque
        toques = 1;

        controlador.VelocidadCero();
    }
    //Funci?n para restar toques al tocar un enemigo
    public void TocarEnemigo()
    {
        if (invulnerable) { return; }
        //Perdemos un toque
        toques -= 1;

        //Si los toques llegan a cero
        if (toques == 0)
        {
            //Quitamos una vida
            GameManager.instance.numVidas--;

            if (!GameManager.instance.CheckLost())
            {
                Reaparecer();
            }            
        }
        else
        {
            //Dependiendo del n?mero de toques habr? que quitarle ventajas a Mario
            //Quitar flor de fuego

            //ETC...
            invulnerable = true;
            if (toques == 1)
            {
                //controlador.SetSize(new Vector2(1, 2), new Vector2(1, 1), new Vector3(1, 2, 1), new Vector3(1, 1, 1), false,true);
                if (controlador.agachado)
                {
                    controlador.SetSizeOneTime(new Vector2(1, 2), new Vector3(1, 2, 1), false, false);
                }
                StartCoroutine(controlador.ISetSize(new Vector2(1, 2), new Vector2(1, 1), new Vector3(1, 2, 1), new Vector3(1, 1, 1), false, true));

            }
            else if (toques == 2)
            {
                Lanzarfuego lF;

                if (TryGetComponent<Lanzarfuego>(out lF))
                {
                    Destroy(lF);
                }
            }
        }
    }
}
