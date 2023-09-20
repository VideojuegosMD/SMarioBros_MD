using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lanzarfuego : MonoBehaviour
{
    public GameObject pBolaFuego;
    public SpriteRenderer sR;
    public Transform tSpawn;
    public Vector3 dir = Vector3.right + Vector3.down;
    public float force = 3;
    // Start is called before the first frame update
    void Start()
    {
        sR = GetComponentInChildren<SpriteRenderer>();
        tSpawn = transform.GetChild(2);
        Object o = Resources.Load<Object>("Prefabs/BolaFuego");
        pBolaFuego = (GameObject)o ;
        Resources.UnloadUnusedAssets();
    }

    // Update is called once per frame
    void Update()
    {
        if (sR.flipX)
        {
            dir = Vector3.left* force*2 + Vector3.down* force;
        }
        else
        {
            dir = Vector3.right* force*2 + Vector3.down* force;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            Rigidbody2D rb = Instantiate(pBolaFuego, tSpawn.transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();

            rb.velocity = dir;

            Destroy(rb.gameObject, 4);
           
        }
    }
}
