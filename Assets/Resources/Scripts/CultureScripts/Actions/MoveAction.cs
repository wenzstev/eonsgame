using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CultureMoveAction : CultureAction
{
    static float moveTime = .1f;

    float ActionCost = 2;

    protected GameObject prospectiveTile;


    protected CultureMoveAction(Culture c) : base(c)
    {
        prospectiveTile = c.Tile.GetRandomNeighbor();
    }

    public IEnumerator MoveTile(GameObject cultureObj, GameObject newTile)
    {
        //Debug.Log($"moving to {newTile}");
        Culture cultureToMove = RemoveCultureFromOldTile(cultureObj);

        Vector3 startPosition = cultureObj.transform.position;
        Vector3 endPosition = prospectiveTile.GetComponentInChildren<CulturePlacementHandler>().GetIncomingTilePlacement();

        for (float t = 0; t < moveTime; t += Time.deltaTime)
        {
            float curDistance = Mathf.InverseLerp(0, moveTime, t);
            cultureObj.transform.position = Vector3.Lerp(startPosition, endPosition, curDistance);
            yield return null;
        }

        cultureObj.transform.position = endPosition;
        //newTile.GetComponentInChildren<CulturePlacementHandler>().AddCulture(culture);

        //Debug.Log("hooking current turn from move");
        Turn.AddUpdate(new StateUpdate(this, cultureToMove, Culture.State.NewOnTile));
        Turn.AddUpdate(new TileUpdate(this, cultureToMove, newTile.GetComponent<Tile>()));

    }

    Culture RemoveCultureFromOldTile(GameObject cultureObj)
    {
        Culture cultureToMove = cultureObj.GetComponent<Culture>();
        

        //turn.UpdateCulture(cultureToMove).newTile = Tile.moveTile.GetComponent<Tile>();

        return cultureToMove;
    }
}

