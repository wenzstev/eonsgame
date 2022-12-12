using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CulturePlacementHandler : MonoBehaviour
{

    public float AnimationTransferTime = .1f;
    public float Radius = 1f;
    public int NumDisplayedCultures = 3;
    public float[] AngleOffset = new float[]{0, Mathf.PI, Mathf.PI / 2f};
    public CultureContainer CultureContainer;

    Culture[] currentList;

    Vector3[][] Positions;

    private void Start()
    {
        currentList = new Culture[0];
        CalculateCulturePositionList();
        CultureContainer.OnListChanged += CulturePlacementHandler_OnListChanged;
    }

    void CalculateCulturePositionList()
    {
        Positions = new Vector3[NumDisplayedCultures][];
        Positions[0] = new Vector3[] { Vector3.zero };

        

        for (int i = 1; i < NumDisplayedCultures; i++)
        {
            int curNumOfCulturesShown = i + 1;
            float theta = (Mathf.PI * 2) / curNumOfCulturesShown;

            Positions[i] = new Vector3[curNumOfCulturesShown];
            for(int j = 0; j < curNumOfCulturesShown; j++)
            {
                int curAngleAmount = j + 1;
                Positions[i][j] = TrigUtils.GetLocationOnCircleRadians(Radius, (theta * j) + AngleOffset[i]);
            }
        }
    }

    public void CulturePlacementHandler_OnListChanged(object sender, CultureContainer.OnListChangedEventArgs e)
    {
        if (e.CultureList.Length > 0) CompareOldPositionsToNew(e.CultureList);
        currentList = e.CultureList;
    }

    void CompareOldPositionsToNew(Culture[] newCulturelist)
    {
        Vector3[] ExpectedPositions = CalculateExpectedLocations(newCulturelist);
        for (int i = 0; i < ExpectedPositions.Length; i++)
        {
            Culture newCultureAtPosition = newCulturelist[i];
            Culture oldCultureAtPosition = i < currentList.Length ? currentList[i] : null;

            if (oldCultureAtPosition == null
                || oldCultureAtPosition != newCultureAtPosition
                || oldCultureAtPosition.transform.position != ExpectedPositions[i]
                || !newCultureAtPosition.Equals(null)) // culture can be destroyed
            {
                StartCoroutine(MoveCulture(newCultureAtPosition, ExpectedPositions[i]));
            }
        }
    }

    Vector3[] CalculateExpectedLocations(Culture[] cultureList)
    {
        int CurPositionIndex = cultureList.Length < NumDisplayedCultures ? cultureList.Length - 1: NumDisplayedCultures - 1;
        Vector3[] CurPositions = Positions[CurPositionIndex];
        Vector3[] ExpectedLocations = new Vector3[cultureList.Length];

        for (int i = 0; i < cultureList.Length; i++)
        {
            if (i >= CurPositions.Length) break;
            ExpectedLocations[i] = CurPositions[i];
        }

        for (int i = CurPositions.Length; i < cultureList.Length; i++)
        {
            ExpectedLocations[i] = CurPositions[CurPositions.Length - 1];
        }

        return ExpectedLocations;
    }

    IEnumerator MoveCulture(Culture culture, Vector3 endPosition)
    {
        if (culture.Equals(null)) yield break;
        float curTime = 0;
        Vector3 startPosition = culture.transform.localPosition;
        yield return null;

        while(curTime <= AnimationTransferTime)
        {
            if (culture.Equals(null)) yield break; // culture was destroyed in the middle of transferring position
            curTime += Time.deltaTime;
            float curDistance = Mathf.InverseLerp(0, AnimationTransferTime, curTime);
            culture.transform.localPosition = Vector3.Lerp(startPosition, endPosition, curDistance);
            yield return null;
        }
    }


    public Vector3 GetIncomingTilePlacement()
    {
        return transform.position;
    }
}
