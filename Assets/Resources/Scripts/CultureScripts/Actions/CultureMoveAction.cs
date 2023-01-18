using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CultureMoveAction : CultureTurnInfo
{
    static float moveTime = .1f;

    float ActionCost = 2;

    protected GameObject TargetTile;


    protected CultureMoveAction(Culture c) : base(c)
    {
        TargetTile = GetTargetTile();
    }

    /// <summary>
    /// Must be implemented to inform the move turn what tile the culture will move to.
    /// </summary>
    /// <returns>The tile that the move action will move the culture to.</returns>
    protected abstract GameObject GetTargetTile();

    IEnumerator MoveTile(GameObject cultureObj, GameObject newTile)
    {
        //Debug.Log($"moving to {newTile}");
        Culture cultureToMove = RemoveCultureFromOldTile(cultureObj);

        Vector3 startPosition = cultureObj.transform.position;
        Vector3 endPosition = TargetTile.GetComponentInChildren<CulturePlacementHandler>().GetIncomingTilePlacement();

        for (float t = 0; t < moveTime; t += Time.deltaTime)
        {
            float curDistance = Mathf.InverseLerp(0, moveTime, t);
            cultureObj.transform.position = Vector3.Lerp(startPosition, endPosition, curDistance);
            yield return null;
        }

        cultureObj.transform.position = endPosition;
        //newTile.GetComponentInChildren<CulturePlacementHandler>().AddCulture(culture);

        //Debug.Log("hooking current turn from move");
        Turn.AddUpdate(CultureUpdateGetter.GetStateUpdate(this, cultureToMove, Culture.State.NewOnTile));
        Turn.AddUpdate(CultureUpdateGetter.GetTileUpdate(this, cultureToMove, newTile.GetComponent<Tile>()));

    }

    Culture RemoveCultureFromOldTile(GameObject cultureObj)
    {
        Culture cultureToMove = cultureObj.GetComponent<Culture>();
        

        //turn.UpdateCulture(cultureToMove).newTile = Tile.moveTile.GetComponent<Tile>();

        return cultureToMove;
    }

    /// <summary>
    /// Triggers the actual move. Culture will move to the tile defined in GetTargetTile().
    /// </summary>
    /// <returns></returns>
    protected Turn ExecuteMove()
    {
        if (Culture.Population > Culture.maxPopTransfer)
        {
            return MoveSplitCulture(); ;
        }
        return MoveWholeCulture();
    }

    Turn MoveSplitCulture()
    {
        GameObject splitCultureObj = Culture.SplitCultureFromParent();
        Culture splitCulture = splitCultureObj.GetComponent<Culture>();
        splitCulture.StartCoroutine(MoveTile(splitCulture.gameObject, TargetTile));
        Turn.AddUpdate(CultureUpdateGetter.GetStateUpdate(this, splitCulture, Culture.State.Moving));
        Turn.AddUpdate(CultureUpdateGetter.GetStateUpdate(this, Culture, Culture.State.Default));
        return turn;
    }

    Turn MoveWholeCulture()
    {
        Culture.StartCoroutine(MoveTile(Culture.gameObject, TargetTile));
        Turn.AddUpdate(CultureUpdateGetter.GetStateUpdate(this, Culture, Culture.State.Moving));
        return turn;
    }

}

