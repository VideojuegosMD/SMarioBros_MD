using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public Controlador controlador;

    public Image fundido;
    public float tFundido;  

    public bool enPausa;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        controlador= FindObjectOfType<Controlador>();
        fundido.canvasRenderer.SetAlpha(0);
    }

    public void PausarDespausar()
    {
        enPausa = !enPausa;
        if (enPausa)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        enPausa = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PausarDespausar();
        }
        if(fundido.canvasRenderer.GetAlpha() == 1)
        {
            FundidoOff();
        }
        
    }

    public void FundidoOff()
    {
        fundido.CrossFadeAlpha(0, tFundido, true);
        fundido.canvasRenderer.SetAlpha(0.99f);       

    }
    public void FundidoOffFinal()
    {
        enPausa = false;
        Time.timeScale = 1;
        controlador.controlPlayer = true;
    }
    public void FundidoOn()
    {
        fundido.CrossFadeAlpha(1, tFundido, true);  
        enPausa = true;
        Time.timeScale = 0;
        controlador.controlPlayer = false;
    }

    public IEnumerator IMovePlayerTowardsPipe(Transform player, Tuberia pipe)
    {
        if (pipe.horizontal)
        {
            player.position = new Vector2( player.position.x, pipe.transform.position.y);
            player.GetChild(0).transform.up = pipe.transform.up;
        }
        else
        {
            player.position = new Vector2(pipe.transform.position.x, player.position.y);
            player.GetChild(0).transform.up = pipe.transform.up;

        }



        Vector2 posIniPlayer = player.position;
        float t = 0;
        float tLimit = tFundido;
        player.GetComponent<SpritesPlayer>().ChangeSpriteLayer(-19);

        while (t < tLimit)
        {
            t += Time.unscaledDeltaTime;
            player.position = Vector2.Lerp(posIniPlayer, pipe.transform.position,t/tLimit);            
            yield return null;
        }

        t = 0;

        player.position = pipe.pStartSpawn.position;
        player.GetChild(0).transform.up = pipe.pStartSpawn.transform.up;

        posIniPlayer = player.position;
        while (t < tLimit)
        {
            t += Time.unscaledDeltaTime;
            player.position = Vector2.Lerp(posIniPlayer, pipe.pEndSpawn.position, t / tLimit);
            yield return null;
        }

        player.up = Vector2.up;
        player.GetComponent<SpritesPlayer>().ChangeSpriteLayer(1);
        FundidoOffFinal();

        yield return null;
    }
}
