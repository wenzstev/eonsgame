using System.Linq;
using UnityEngine;

public class AffinityPanelController : MonoBehaviour
{
    public AffinityBarController[] Bars;



    public float MaxAffinityAmount;
    
    AffinityManager AffinityManager;



  
    public void SetValues(Culture c)
    {
        AffinityManager = c.GetComponent<AffinityManager>();
        AffinityManager.OnAffinityChanged += AffinityManager_OnAffinityChanged;
        SetAffinities();

    }

    void SetAffinities()
    {

        (TileDrawer.BiomeType, float)[] affinities = AffinityManager.GetAllAffinities()
            .OrderByDescending(v => v.Item2).ToArray();


        for (int i = 0; i < 3; i++)
        {
            Bars[i].SetValues(affinities[i].Item1, affinities[i].Item2, MaxAffinityAmount);
        }
    }

    private void AffinityManager_OnAffinityChanged(object sender, AffinityManager.OnAffinityChangedEventArgs e)
    {
        SetAffinities();
    }

    private void OnDestroy()
    {
        AffinityManager.OnAffinityChanged -= AffinityManager_OnAffinityChanged;
    }
}
