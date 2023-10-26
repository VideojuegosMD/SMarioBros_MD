using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlCamara : MonoBehaviour
{
    Rigidbody2D player;
    public Vector3 offset;
    public Vector3 limitPos, limitNeg, newPos;
    public float moveLimit, speedLimit;
    public float camSpeed;
    public bool camMoving, autoCam;
    

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Controlador>().GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        newPos = player.transform.position + offset;

        CheckLimit();

        Debug.Log(Vector3.Distance(newPos, transform.position));
        if (Vector3.Distance(newPos, transform.position) > moveLimit)
        {
            autoCam = true;          
        }
        
       
    }

    private void FixedUpdate()
    {
        if (autoCam)
        {
           
            transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * camSpeed / 4);

            if(Vector3.Distance(newPos, transform.position) < .001f)
            {
                autoCam = false;
            }
            
        }

        if (player.velocity.magnitude > speedLimit)
        {
            transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * camSpeed);
        }
    }

    void CheckLimit()
    {
        if (newPos.x > limitPos.x)
        {
            newPos.x = limitPos.x;
        }
        if (newPos.x < limitNeg.x)
        {
            newPos.x = limitNeg.x;
        }

        if (newPos.y > limitPos.y)
        {
            newPos.y = limitPos.y;
        }
        if (newPos.y < limitNeg.y)
        {
            newPos.y = limitNeg.y;
        }

        /* No nos movemos en Z (aún)
        if (newPos.z > limitPos.z)
        {
            newPos.z = limitPos.z;
        }
        if (newPos.z < limitNeg.z)
        {
            newPos.z = limitNeg.z;
        }
        */
    }

}
