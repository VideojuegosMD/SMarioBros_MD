using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tuberia : MonoBehaviour
{
    public bool horizontal;
    public Transform pStartSpawn,pEndSpawn;

    public void Teletransporte(Transform player)
    {
       StartCoroutine(GameManager.instance.IMovePlayerTowardsPipe(player,this));
    }   
}
