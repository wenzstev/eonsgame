using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class TileSpriteLoader : MonoBehaviour
{
    public SpriteAtlas groundAtlas;
    public SpriteAtlas topAtlas;

    public int spriteGroundId { get; private set; }
    public int spriteTopId { get; private set; }

    public SpriteRenderer groundSr;
    public SpriteRenderer topSr;


    public void ChooseSprites()
    {
        spriteGroundId = GetRandomInt(groundAtlas.spriteCount);
        if(topAtlas)
        {
            spriteTopId = GetRandomInt(topAtlas.spriteCount);
        }
    }

    public void Load(int groundId, int topId)
    {
        if (groundId < 0)
        {
            ChooseSprites();
            return;
        }
        spriteGroundId = groundId;
        spriteTopId = topId;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("loading sprite");
        Debug.Log("ground id is " + spriteGroundId);
        if(spriteGroundId == -1)
        {
            Debug.LogError("Attempting to load tile without setting it's sprite Id");
            return;
        }
        Sprite[] groundSprites = new Sprite[groundAtlas.spriteCount];
        Sprite[] topSprites = new Sprite[topAtlas.spriteCount];

        groundAtlas.GetSprites(groundSprites);
        topAtlas.GetSprites(topSprites);

        if (spriteGroundId > groundSprites.Length || spriteTopId > topSprites.Length)
        {
            Debug.LogError("Trying to load a sprite with ID outside of range!");
            return;
        }


        Sprite groundSprite = groundSprites[spriteGroundId];
        Sprite topSprite = topSprites[spriteTopId];

        groundSr.sprite = groundSprite;
        topSr.sprite = topSprite;

    }

    static int GetRandomInt(int size)
    {
        return Mathf.FloorToInt(Random.value * size);
    }

}
