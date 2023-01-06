using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CultureListPanel : MonoBehaviour
{
    public GameObject CultureButton;

    CultureHandler cultureHandler;

    GameObject[] ButtonList;

    public void Initialize(GameObject SelectedTile)
    {
        cultureHandler = SelectedTile.GetComponentInChildren<CultureHandler>();
        cultureHandler.OnPopulationChanged += CultureListPanel_OnPopulationChanged;
        CreateAndSortList();

    }

    void CreateAndSortList()
    {
        Culture[] CultureList = cultureHandler.GetAllCultures();
        CultureList = CultureList.OrderByDescending(c => c.Population).ToArray();

        ButtonList = CultureList.Select(c => CreateCultureButton(c)).ToArray();
    }

    GameObject CreateCultureButton(Culture c)
    {
        GameObject CurCultureButton = Instantiate(CultureButton, transform);
        CurCultureButton.GetComponent<CultureListButton>().Initialize(c);
        return CurCultureButton;
    }

    void RemovePreviousList()
    {
        foreach (GameObject button in ButtonList) Destroy(button);
    }

    void CultureListPanel_OnPopulationChanged(object sender, CultureHandler.OnPopulationChangedEventArgs e)
    {
        RemovePreviousList();
        CreateAndSortList();
    }


}
