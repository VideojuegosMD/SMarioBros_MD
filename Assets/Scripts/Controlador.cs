using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Clase base para el controlador de personaje, tan solo contendr� lo necesario para trabajar el movimiento y detecciones del entorno,
 * El sonido, animaciones y dem�s tendremos que llevarlo a scripts separados para que quede m�s limpito.
 */

public class Controlador : MonoBehaviour
{
    /*
    * Pr�cticamente todas las variables y funciones est�n como p�blicas por comodidad durante el desarrollo
    * Es recomendable hacer una buena encapsulaci�n y si tenemos que mostrar una variable en el Editor usar el atributo [SerializeField]
    */

    #region Variables     

    public bool controlPlayer = true;

    //Booleano de orientación
    public bool orientRight = true;

    //Script de control de vida
    ControlVida controlVida;

    //Punto de aparici�n
    public Vector3 puntoSpawn;   

    //Valor n�merico de velocidad y de salto
    public float velocidad, fuerzaSalto;

    //Input de movimiento, valor de -1 a 1, 0 si no pulsa nada
    public float inputMovHor, inputMovVer;
    public Vector2 inputDir;

    //Input de si pulsamos la tecla de salto
    public bool teclaSalto, teclaAgacharse;

    //bool para saber si tocamos el suelo
    public bool enSuelo,agachado;

    //Componente de Rigidbody
    public Rigidbody2D rb;

    //Componente de Collider
    [SerializeField]
    internal CapsuleCollider2D capsuleCollider;

    //Materiales f�sicos
    public PhysicsMaterial2D friction, noFriction;

    //Componente SpriteRenderer
    public SpriteRenderer spriteR;

    #endregion

    #region FuncionesMonobehaviour
    // Start is called before the first frame update
    void Start()
    {

        controlPlayer = true;

        //Asignamos el componente de la capsula
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        //Asignamos el componente del Rigidbody
        rb = GetComponent<Rigidbody2D>();

        //Obtenemos el componente de SpriteRenderer desde el padre
        spriteR = GetComponentInChildren<SpriteRenderer>();

        //Al comenzar podriamos cargar datos guardados
        
        //Movemos el personaje al punto de inicio
        transform.position = puntoSpawn;        

        //Obtener el script de vida
        controlVida = GetComponent<ControlVida>();

        //Orientar PJ
        orientRight = true;
        OrientarPJ();
     
    }

    // Update se ejecuta cada fotograma
    void Update()
    {
        if (!controlPlayer) 
        {
            inputMovVer = 0;
            inputMovHor = 0;
            teclaSalto = false;
            return; 
        }

        //Tomamos el Input de movimiento del eje horizontal
        inputMovHor = Input.GetAxis("Horizontal");
        inputMovVer = Input.GetAxis("Vertical");

        inputDir = new Vector2(inputMovHor, inputMovVer);

        if (inputMovHor > 0)
        {
            orientRight = true;
            OrientarPJ();
        }
        else if (inputMovHor < 0)
        {
            orientRight = false;
            OrientarPJ();
        }

        //Tomamos el Input de la tecla de salto
        teclaSalto = Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.Space);

        //Tomamos el Input de la tecla de agacharse
        teclaAgacharse = Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftControl);

        //Asignamos la velocidad del personaje (podr�a ser que queramos usar AddForce en vez de velocity)****************************
        rb.velocity = new Vector2(inputMovHor * velocidad, rb.velocity.y);

        //Funci�n para comprobar que tenemos debajo
        ComprobacionPies();
        //ComprobacionCabeza();

        //Comprobaci�n de si estamos en el suelo o no para asignar un material con o sin fricci�n
        if (enSuelo)
        {
            capsuleCollider.sharedMaterial = friction;
        }
        else
        {
            capsuleCollider.sharedMaterial = noFriction;

        }


        if (controlVida.toques > 1)
        {
            Agacharse();
        }
      
    }
    //FixedUpdate se ejecuta al ritmo de las f�sicas, puede haber fotogramas que no se ejecute o que se ejecute varias veces.
    private void FixedUpdate()
    {
        //Si pulsamos la tecla de salto y estamos en el suelo llamamos a la funci�n de salto
        if (teclaSalto && enSuelo)
        {
            Saltar();

        }
    }

    //OnCollisionEnter se ejecuta una vez al entrar en contacto con un Collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Si chocamos con un enemigo
        if (collision.gameObject.layer == 8) //Layer 8 Enemigo
        {
            //En el momento de chocar detectamos si tenemos algo en los pies
            ComprobacionPies(lmEnemigos);

            //Si no tenemos nada en los pies, hemos chocado con un enemigo y nos tiene que hacer da�o
            if (rHitDown.collider == null)
            {               
                controlVida.TocarEnemigo();
            }
            //Si tenemos algo en los pies
            else
            {
                Debug.Log(rHitDown.collider.gameObject.layer);
                //Comprobamos que sea un enemigo
                if (rHitDown.collider.gameObject.layer == 8)
                {
                    
                    //Aqu� habr�a que hacer diferencias entre enemigos, dependiendo del tipo puede que se actue de un modo u otro al matarlos

                    //Destruimos el objeto que tenemos detectado y con el que hemos chocado en 5 segundos 
                    Destroy(rHitDown.collider.gameObject, 5f);

                    //Destruimos su colisionador para que atraviese el suelo y caiga
                    Destroy(rHitDown.collider);

                    //Nos impulsamos con un salto
                    Saltar();
                }
                //Sino, hemos tocado un enemigo pero no es lo que tenemos bajo los pies
                else
                {
                    controlVida.TocarEnemigo();
                }
            }
        }

        //Si chocamos con un bloque
        else if (collision.gameObject.layer == 10) //Layer 10 Bloques
        {
            //Comprobamos si hay algo encima nuesto
            ComprobacionCabeza();

            //Si tenemos algo encima
            if (rHitUp.collider != null)
            {           
                //Comprobamos que sea un bloque
                if (rHitUp.collider.gameObject.layer == 10)
                {
                    //Decidir que tipo de bloque hemos tocado y que hacemos
                    //Pendiente de cambios****************************                   
                    if (GolpeDesdeAbajo(collision.collider))
                    {
                        AccederABloque(collision.transform);
                    }

                            
                }
            }
        }
        
    }

    //OnTriggerEnter se ejecuta una vez al entrar en contacto con un Collider de tipo Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Si detectamos un bloque invisible
        if (collision.gameObject.layer == 10) //Layer bloques
        {
            //Comprobamos si tenemos algo encima de la cabeza
            ComprobacionCabeza();

            //Si tenemos algo en la cabeza
            if (rHitUp.collider != null)
            {
                //Si lo que detectamos es un bloque invisible
                if (rHitUp.collider.gameObject.layer == 10)
                {   
                    //Comprobamos si le hemos chocado desde abajo
                    if (GolpeDesdeAbajo(collision))
                    {                       
                        AccederABloque(collision.transform); 
                    }
                }
            }

        }
        //Si chocamos con una moneda
        else if (collision.gameObject.layer == 12)
        {
            Destroy(collision.gameObject);
        }

    }

    //La condici�n #if nos permite diferenciar la plataforma para compilar c�digo o no al momento de Buildear
    #if UNITY_EDITOR
    //OnDrawGizmos se ejecuta solo en el Editor de Unity
    private void OnDrawGizmos()
    {
        //Dibujamos la detecci�n de los pies
        Gizmos.DrawWireSphere(transform.position - transform.up * capsuleCollider.size.y / 2 - transform.up * offset, radioDetect);

        //Dibujamos la detecci�n de la cabeza
        Gizmos.DrawWireSphere(transform.position + transform.up * capsuleCollider.size.y / 2 + transform.up * offset, radioDetect);
    }
    #endif
    #endregion

    #region Detecciones

    #region Variables para las detecciones
    //RaycastHit almacena la informaci�n de una detecci�n
    RaycastHit2D rHitUp, rHitDown;

    //Valor n�merico para el radio de la detecci�n
    public float radioDetect;

    //M�scaras de tipo Layer, permiten diferenciar y filtrar objetos
    public LayerMask lmPies,lmCabeza, lmEnemigos;

    //Valor n�merico para modificar hasta donde hacemos la detecci�n
    public float offset;

    #endregion
    
    //Funci�n para comprobar que tenemos debajo del personaje
    public void ComprobacionPies()
    {
        //Asignamos el RaycastHit con el m�todo de CircleCast, es la proyecci�n de un c�rculo que detecta lo que toca.
        rHitDown = Physics2D.CircleCast(transform.position, radioDetect, -transform.up, capsuleCollider.size.y / 2 + offset, lmPies);

        //Si la detecci�n es nula
        if (rHitDown.collider == null)
        {
            /*
             * //Mensaje de consola
            Debug.Log("No detecto nada");
            */

            //No tocamos el suelo
            enSuelo = false;
        }
        else
        {
            /*
             * //Mensaje de consola
            Debug.Log("Detecto: " + rHitDown.collider.name);
            */

            //Si el objeto es Suelo (7), Bloques(10) y NO son colliders de tipo Trigger
            if (rHitDown.collider.gameObject.layer == 7 || rHitDown.collider.gameObject.layer == 10 && !rHitDown.collider.isTrigger)            
            {
                //Tocamos el suelo
                enSuelo = true;
            }
            else
            {
                //No tocamos el suelo
                enSuelo = false;
            }
        }
    }
    public void ComprobacionPies(LayerMask lm)
    {
        //Asignamos el RaycastHit con el m�todo de CircleCast, es la proyecci�n de un c�rculo que detecta lo que toca.
        rHitDown = Physics2D.CircleCast(transform.position, radioDetect, -transform.up, capsuleCollider.size.y / 2 + offset, lm);

        //Si la detecci�n es nula
        if (rHitDown.collider == null)
        {
            /*
             * //Mensaje de consola
            Debug.Log("No detecto nada");
            */

            //No tocamos el suelo
            enSuelo = false;
        }
        else
        {
            /*
             * //Mensaje de consola
            Debug.Log("Detecto: " + rHitDown.collider.name);
            */

            //Si el objeto es Suelo (7), Bloques(10) y NO son colliders de tipo Trigger
            if (rHitDown.collider.gameObject.layer == 7 || rHitDown.collider.gameObject.layer == 10 && !rHitDown.collider.isTrigger)
            {
                //Tocamos el suelo
                enSuelo = true;
            }
            else
            {
                //No tocamos el suelo
                enSuelo = false;
            }
        }
    }

    //Funci�n para comprobar que tenemos encima del personaje
    public void ComprobacionCabeza()
    {
        //Asignamos el RaycastHit con el m�todo de CircleCast, es la proyecci�n de un c�rculo que detecta lo que toca.
        rHitUp = Physics2D.CircleCast(transform.position ,radioDetect, transform.up,capsuleCollider.size.y / 2 + offset, lmCabeza);
       
        /*
         * //Mensajes de consola si detectamos algo         
        if (rHitUp.collider == null)
        {
            Debug.Log("No detecto nada");
        }
        else
        {
            Debug.Log("Detecto: " + rHitUp.collider.name);
           
        }
        */
    }

    public float limiteAnguloDetectBloque;
    //M�todo para saber si estamos justo debajo de un Collider
    public bool GolpeDesdeAbajo(Collider2D collision)
    {
        //Obtenemos el punto m�s cercano del Collider en base al jugador
        Vector3 puntoMasCercano = collision.ClosestPoint(transform.position);

        //Obtenemos la direcci�n entre el punto m�s cercano y el jugador
        Vector3 dirDetect = puntoMasCercano - transform.position;

        
       //Mensaje de consola con el angulo de la detecci�n
        Debug.Log(Vector2.Angle(dirDetect.normalized, transform.up));
        float angle = Vector2.Angle(dirDetect.normalized, transform.up);

        //Si el punto m�s cercano est� m�s arriba que el jugador y el �ngulo de dirDetect con la direcci�n hac�a arriba del jugador es 0
        if (puntoMasCercano.y > transform.position.y && angle <= limiteAnguloDetectBloque && angle >= -limiteAnguloDetectBloque)
        {
            //Estamos justo debajo, devolvemos valor true
            return true;
        }
        else
        {
            //No estamos justo debajo, devolvemos valor false
            return false;
        }
    }
    #endregion

    #region FuncionesPropias
    public float tAgacharse, velAgacharse;
    private float posYAgachar = 10000;
    //¡Variable de Editor, ojo cuidado!
    public bool overrideJump;

    public void Agacharse()
    {
        if ((teclaAgacharse && controlVida.toques>1 )|| overrideJump)
        {
            if (!agachado)
            {
                SetSize(new Vector2(1, 2), new Vector2(1, 1), new Vector3(1, 2, 1), new Vector3(1, 1, 1), true,true);
            }
        }
        else
        {
            if (agachado)
            {
                SetSize(new Vector2(1, 1), new Vector2(1, 2), new Vector3(1, 1, 1), new Vector3(1, 2, 1), false,false);

            }

        }
    }




    public void SetSize(Vector2 capsulaSizeIni, Vector2 capsulaSizeFin, Vector3 spriteSizeIni, Vector3 spriteSizeFin, bool _agachado, bool paAbajo )
    {
        if (posYAgachar == 10000)
        {
            posYAgachar = transform.position.y;

        }
        tAgacharse += Time.deltaTime * velAgacharse;
        tAgacharse = Mathf.Clamp01(tAgacharse);

        capsuleCollider.size = Vector2.Lerp(capsulaSizeIni, capsulaSizeFin, tAgacharse);
        transform.GetChild(0).transform.localScale = Vector3.Lerp(spriteSizeIni, spriteSizeFin, tAgacharse);
        float x = 0.5f;
        if (paAbajo)
        {
            x *= -1;
        }
       
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(posYAgachar, posYAgachar + x, tAgacharse), transform.position.z);

        if (tAgacharse >= 1)
        {
            agachado = _agachado;
            tAgacharse = 0;
            posYAgachar = 10000;
        }
    }
    public void SetSizeOneTime(Vector2 capsulaSizeFin,Vector3 spriteSizeFin, bool _agachado, bool paAbajo)
    {
        if (posYAgachar == 10000)
        {
            posYAgachar = transform.position.y;
        }   
        capsuleCollider.size =capsulaSizeFin;
        transform.GetChild(0).transform.localScale =  spriteSizeFin;
        float x = 0.5f;
        if (paAbajo)
        {
            x *= -1;
        }
        transform.position = new Vector3(transform.position.x, posYAgachar + x, transform.position.z);
       
        agachado = _agachado;
        tAgacharse = 0;
        posYAgachar = 10000;
        
    }
    public IEnumerator ISetSize(Vector2 capsulaSizeIni, Vector2 capsulaSizeFin, Vector3 spriteSizeIni, Vector3 spriteSizeFin, bool _agachado, bool paAbajo)
    {
        float t = 0;
        while (t < 1)
        {
            if (posYAgachar == 10000)
            {
                posYAgachar = transform.position.y;

            }
            tAgacharse += Time.deltaTime * velAgacharse;
            t += Time.deltaTime * velAgacharse;
            tAgacharse = Mathf.Clamp01(tAgacharse);

            capsuleCollider.size = Vector2.Lerp(capsulaSizeIni, capsulaSizeFin, tAgacharse);
            transform.GetChild(0).transform.localScale = Vector3.Lerp(spriteSizeIni, spriteSizeFin, tAgacharse);
            float x = 0.5f;
            if (paAbajo)
            {
                x *= -1;
            }

            transform.position = new Vector3(transform.position.x, Mathf.Lerp(posYAgachar, posYAgachar + x, tAgacharse), transform.position.z);

            if (tAgacharse >= 1)
            {
                agachado = _agachado;
                tAgacharse = 0;
                posYAgachar = 10000;
            }
            yield return null;
        }      
    }

    public void OrientarPJ()
    {
        //Obtener el spawn de bolas de fuego  
        Transform t = transform.GetChild(2);
        if (orientRight)
        {                      
            t.localPosition = new Vector3(Mathf.Abs(t.localPosition.x), t.localPosition.y, t.localPosition.z);
            spriteR.flipX = false;
        }
        else
        {
            t.localPosition = new Vector3(-Mathf.Abs(t.localPosition.x), t.localPosition.y, t.localPosition.z);
            spriteR.flipX = true;
        }
    }

    public void AccederABloque(Transform t)
    {
        if (t.GetComponentInParent<Bloque>() != null)
        {
            Bloque b = t.GetComponentInParent<Bloque>();
            b.RestarToque();
        }  
    }

    //Funci�n para saltar
    public void Saltar()
    {
        // A�adir la velocidad del salto directamente al rb.velocity
        // rb.velocity = new Vector2(rb.velocity.x,fuerzaSalto);

        //A�adir la velocidad de salto mediante un AddForce en modo impulso
        rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
    }   

    public void VelocidadCero()
    {
        rb.velocity= Vector2.zero;
    }

  

    //Funci�n para reiniciar la partida (Recomendable llevar este comportamiento a un GameManager)
    public void ReiniciarPartida()
    {
        //Recargamos escena activa
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion
}
