using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public Transform a, b, c;

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(Ayudante.DotProduct(a.position, b.position, c.position));

        Vector3 up = a.TransformDirection(Vector3.up).normalized;
        Vector3 toOther = (b.position - a.position);

        /*if (Vector3.Dot(forward, toOther) < 0)
        {
            print("The other transform is behind me!");
        }*/
        Debug.Log(Vector3.Dot(up, toOther));
    }
}
