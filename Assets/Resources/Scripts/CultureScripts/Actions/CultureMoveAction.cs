using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CultureMoveAction
{
    static float moveTime = .1f;


    static IEnumerator MoveTile(CultureTurnInfo cultureTurnInfo, Culture culture, Tile targetTile)
    {
        culture.ChangeState(Culture.State.Moving);
        culture.RemoveFromTile();

        Vector3 startPosition = culture.transform.position;
        Vector3 endPosition = targetTile.GetComponentInChildren<CulturePlacementHandler>().GetIncomingTilePlacement();


        for (float t = 0; t < moveTime; t += Time.deltaTime)
        {

            float curDistance = Mathf.InverseLerp(0, moveTime, t);
            
            culture.transform.position = Vector3.Lerp(startPosition, endPosition, curDistance);

            yield return null; // after this point, no longer guaranteed to to be in ExecuteChange
        }

        culture.transform.position = endPosition;
        //newTile.GetComponentInChildren<CulturePlacementHandler>().AddCulture(culture);

        //Debug.Log("hooking current turn from move");
        Turn.AddUpdate(CultureUpdateGetter.GetStateUpdate(cultureTurnInfo, culture, Culture.State.NewOnTile));
        Turn.AddUpdate(CultureUpdateGetter.GetTileUpdate(cultureTurnInfo, culture, targetTile));

    }



    /// <summary>
    /// Triggers the actual move. Culture will split if too large to move at once.
    /// </summary>
    /// <returns></returns>
    public static void ExecuteMove(CultureTurnInfo cultureTurnInfo, Tile targetTile)
    {
        Culture Culture = cultureTurnInfo.Culture;
        if (Culture.Population > Culture.maxPopTransfer)
        {
            MoveSplitCulture(cultureTurnInfo, targetTile);
            return;
        }
        MoveWholeCulture(cultureTurnInfo, targetTile);
    }

    static void MoveSplitCulture(CultureTurnInfo cultureTurnInfo, Tile TargetTile)
    {
        Culture Culture = cultureTurnInfo.Culture;
        GameObject splitCultureObj = Culture.SplitCultureFromParent();
        Culture splitCulture = splitCultureObj.GetComponent<Culture>();
        splitCulture.StartCoroutine(MoveTile(cultureTurnInfo, splitCulture, TargetTile));

        // because this is called in the `ExecuteChange` function, we can make instant changes to cultures
        Culture.ChangeState(Culture.State.Default);
    }

    static void MoveWholeCulture(CultureTurnInfo cultureTurnInfo, Tile targetTile)
    {
        Culture Culture = cultureTurnInfo.Culture;
        Culture.StartCoroutine(MoveTile(cultureTurnInfo, Culture, targetTile));
    }

}

