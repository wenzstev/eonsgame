using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class TileSpriteLoader : MonoBehaviour
{
    public SpriteAtlas spriteAtlas;
    public int spriteId { get; private set; }

    public void ChooseSprite()
    {
        spriteId = Mathf.FloorToInt(Random.value * spriteAtlas.spriteCount);
    }

    public void Load(int id)
    {
        if (id < 0)
        {
            ChooseSprite();
            return;
        }
        spriteId = id;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("loading sprite");
        Debug.Log("id is " + spriteId);
        if(spriteId == -1)
        {
            Debug.LogError("Attempting to load tile without setting it's sprite Id");
            return;
        }
        Sprite[] tileSprites = new Sprite[spriteAtlas.spriteCount];
        spriteAtlas.GetSprites(tileSprites);

        if (spriteId > tileSprites.Length)
        {
            Debug.LogError("Trying to load a sprite with ID outside of range!");
            return;
        }


        Sprite tileSprite = tileSprites[spriteId];
        Debug.Log($"Tilesprite is {tileSprite}");
        GetComponent<SpriteRenderer>().sprite = tileSprite;
    }

}
