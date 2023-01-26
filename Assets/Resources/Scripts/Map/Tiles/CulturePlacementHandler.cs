using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class CulturePlacementHandler : MonoBehaviour
{

    public float AnimationTransferTime = .1f;
    public float Radius = 1f;
    public int NumDisplayedCultures = 3;
    public float[] AngleOffset = new float[]{0, Mathf.PI, Mathf.PI / 2f};
    public CultureContainer CultureContainer;

    List<Culture> currentList;

    Vector3[][] Positions;
    List<Vector3> ExpectedPositions; // cached


    public void Initialize()
    {
        currentList = new List<Culture>();
        CalculateCulturePositionList();
        CultureContainer.OnListChanged += CulturePlacementHandler_OnListChanged;
        ExpectedPositions = new List<Vector3>();
    }

    void CalculateCulturePositionList()
    {
        Positions = new Vector3[NumDisplayedCultures][];
        Positions[0] = new Vector3[] { (Vector3.zero + transform.position) };

        

        for (int i = 1; i < NumDisplayedCultures; i++)
        {
            int curNumOfCulturesShown = i + 1;
            float theta = (Mathf.PI * 2) / curNumOfCulturesShown;

            Positions[i] = new Vector3[curNumOfCulturesShown];
            for(int j = 0; j < curNumOfCulturesShown; j++)
            {
                int curAngleAmount = j + 1;
                Vector2 circlePosition = TrigUtils.GetLocationOnCircleRadians(Radius, (theta * j) + AngleOffset[i]);
                Positions[i][j] = new Vector3(circlePosition.x, circlePosition.y, 0) + transform.position;
            }
        }
    }

    public void CulturePlacementHandler_OnListChanged(object sender, CultureContainer.OnListChangedEventArgs e)
    {
        if (e.CultureList.Count > 0) CompareOldPositionsToNew(e.CultureList);
        currentList = e.CultureList;
    }

    void CompareOldPositionsToNew(List<Culture> newCulturelist)
    {
        if (!gameObject.activeInHierarchy) return; // don't attempt if object is inactive (such as when exiting to main menu)
        ExpectedPositions = CalculateExpectedLocations(newCulturelist);
        for (int i = 0; i < newCulturelist.Count; i++)
        {
            Culture newCultureAtPosition = newCulturelist[i];
            Culture oldCultureAtPosition = i < currentList.Count ? currentList[i] : null;

            if (oldCultureAtPosition == null
                || oldCultureAtPosition != newCultureAtPosition
                || oldCultureAtPosition.transform.position != ExpectedPositions[i]
                || !newCultureAtPosition.Equals(null))  // culture can be destroyed
            {
                //StartCoroutine(MoveCulture(newCultureAtPosition, ExpectedPositions[i]));
                newCultureAtPosition.transform.position = ExpectedPositions[i];
            }
        }
        ExpectedPositions.Clear();

    }

    List<Vector3> CalculateExpectedLocations(List<Culture> cultureList)
    {
        int CurPositionIndex = cultureList.Count < NumDisplayedCultures ? cultureList.Count - 1: NumDisplayedCultures - 1;
        Vector3[] CurPositions = Positions[CurPositionIndex];
        ExpectedPositions.Clear();

        for (int i = 0; i < cultureList.Count; i++)
        {
            if (i >= CurPositions.Length) break;
            ExpectedPositions.Add(CurPositions[i]);
        }

        for (int i = CurPositions.Length; i < cultureList.Count; i++)
        {
            ExpectedPositions.Add(CurPositions[CurPositions.Length - 1]);
        }

        return ExpectedPositions;
    }

    IEnumerator MoveCulture(Culture culture, Vector3 endPosition)
    {
        if (!culture.isActiveAndEnabled) yield break;
        float curTime = 0;
        Vector3 startPosition = culture.transform.position;
        yield return null;
        while(curTime <= AnimationTransferTime)
        {
            if (!culture.isActiveAndEnabled || culture.currentState == Culture.State.Moving) yield break; // culture was destroyed or changed tiles in the middle of transferring position (ties it to Culture.State, unsure if good)
            curTime += Time.deltaTime;
            float curDistance = Mathf.InverseLerp(0, AnimationTransferTime, curTime);
            culture.transform.position = Vector3.Lerp(startPosition, endPosition, curDistance);
            yield return null;

        }
    }


    public Vector3 GetIncomingTilePlacement()
    {
        return transform.position;
    }
}
