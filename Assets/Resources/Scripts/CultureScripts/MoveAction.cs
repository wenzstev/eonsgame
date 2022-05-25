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

    public IEnumerator MoveTile(GameObject cultureObj, GameObject newTile)
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
        cultureObj.transform.SetParent(newTile.transform);

        turn.UpdateCulture(cultureToMove).newState = Culture.State.NewOnTile;

    }

    Culture RemoveCultureFromOldTile(GameObject cultureObj)
    {
        Culture cultureToMove = cultureObj.GetComponent<Culture>();
        cultureToMove.tile.GetComponent<TileInfo>().RemoveCulture(cultureToMove);

        if (cultureToMove.currentState != Culture.State.Repelled)
        {
            cultureToMove.GetComponent<CultureMemory>().previousTile = cultureToMove.tile;
        }

        turn.UpdateCulture(cultureToMove).newTile = Tile.moveTile.GetComponent<Tile>();

        return cultureToMove;
    }






}

