using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moneda : MonoBehaviour
{
    public int valor;

    private void OnDestroy()
    {
        ManagerMonedas.instance.SumarMonedas(valor);
    }

    public void DestruirMoneda()
    {
        Destroy(gameObject);
    }
}
