using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CultureMoveAction : CultureAction
{
    static float moveTime = .1f;


    protected GameObject prospectiveTile;


    protected CultureMoveAction(Culture c) : base(c)
    {
        prospectiveTile = c.tile.GetRandomNeighbor();
    }

    public static IEnumerator MoveTile(GameObject cultureObj, GameObject newTile)
    {

        Culture cultureToMove = RemoveCultureFromOldTile(cultureObj);

        Vector3 startPosition = cultureObj.transform.position;

        for (float t = 0; t < moveTime; t += Time.deltaTime)
        {
            float curDistance = Mathf.InverseLerp(0, moveTime, t);
            cultureObj.transform.position = Vector3.Lerp(startPosition, newTile.transform.position, curDistance);
            yield return null;
        }

        cultureObj.transform.position = newTile.transform.position;

        CombineCultureWithNewTile(cultureObj, newTile, cultureToMove);
    }

    static Culture RemoveCultureFromOldTile(GameObject cultureObj)
    {
        Culture cultureToMove = cultureObj.GetComponent<Culture>();
        cultureToMove.tile.GetComponent<TileInfo>().RemoveCulture(cultureToMove);

        if (cultureToMove.currentState != Culture.State.Repelled)
        {
            cultureToMove.GetComponent<CultureMemory>().previousTile = cultureToMove.tile;
        }

        cultureToMove.tile = null;
        cultureToMove.tileInfo = null;

        return cultureToMove;
    }


    static void CombineCultureWithNewTile(GameObject cultureObj, GameObject newTile, Culture cultureToMove)
    {
        cultureObj.transform.SetParent(newTile.transform);
        TileInfo newTileInfo = newTile.GetComponent<TileInfo>();
        cultureToMove.currentState = Culture.State.Default;

        Culture potentialSameCulture = null;
        if (newTileInfo.cultures.TryGetValue(cultureToMove.name, out potentialSameCulture))
        {
            bool didMerge = cultureToMove.AttemptMerge(potentialSameCulture);
            if(!didMerge)
            {
                cultureToMove.CreateAsNewCulture();
                newTileInfo.AddCulture(cultureToMove);
                cultureToMove.SetTile(newTile.GetComponent<Tile>());
            }
        }
        else if (newTileInfo.cultures.TryGetValue(cultureToMove.GetComponent<CultureMemory>().cultureParentName, out potentialSameCulture))
        {
            potentialSameCulture.AttemptMerge(cultureToMove);
            newTileInfo.AddCulture(cultureToMove);
            cultureToMove.SetTile(newTile.GetComponent<Tile>());
        }
        else
        {
            newTileInfo.AddCulture(cultureToMove);
            cultureToMove.SetTile(newTile.GetComponent<Tile>());
        }

        if (newTileInfo.cultures.Count > 1)
        {
            SetInvadersAndInvaded(newTileInfo, cultureToMove);
        }

    }

    static void SetInvadersAndInvaded(TileInfo newTileInfo, Culture cultureToMove)
    {
        cultureToMove.currentState = Culture.State.Invader;
        foreach (Culture c in newTileInfo.orderToRemoveCulturesIn)
        {
            if (c.name != cultureToMove.name)
            {
                c.currentState = Culture.State.Invaded;
            }
        }

    }


}

