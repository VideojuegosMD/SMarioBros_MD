using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerMonedas : MonoBehaviour
{
    public static ManagerMonedas instance { get; private set; }

    public int monedas;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;      
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SumarMonedas(int x)
    {
        monedas += x;
    }
}
