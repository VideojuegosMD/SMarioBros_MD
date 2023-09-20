using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritesPlayer : MonoBehaviour
{
    public SpriteRenderer[] sprites;
    // Start is called before the first frame update
    void Start()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();
    }

    public void ChangeSpriteLayer(int x)
    {
        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.sortingOrder = x;
        }
    }
   
}
