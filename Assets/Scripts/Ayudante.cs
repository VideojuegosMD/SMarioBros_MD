using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Ayudante
{
  public static float DotProduct(Vector3 p0, Vector3 p1, Vector3 c)
    {
        Vector3 a = (c - p0).normalized;
        Vector3 b = (c - p1).normalized;

        return (a.x * b.x) + (a.y * b.y) + (a.z * b.z);
    }
}
