using System.Collections;
using UnityEngine;
using System.Linq;

public class TestUtils
{

    public static void TearDownTest()
    {
        Debug.Log("clearing");
        foreach (GameObject o in Object.FindObjectsOfType<GameObject>())
        {
            Object.Destroy(o);
        }
    }

    public static float ThreeDecimals(float val)
    {
        return Mathf.Round(val * 1000f) / 1000f;
    }


    public static Culture.State GetLastStateInUpdateList(CultureUpdate<Culture.State>[] UpdateList, Culture c)
    {
        return UpdateList.Where(u => u.Target == c).Last().CultureChangeValue;
    }

    public static int GetCombinedPopulationInUpdateList(CultureUpdate<int>[] UpdateList, Culture c)
    {
        return UpdateList.Where(u => u.Target == c).Sum(u => u.CultureChangeValue);
    }

    public static float GetCombinedFoodChangeInUpdateList(CultureUpdate<float>[] UpdateList, Culture c)
    {
        return UpdateList.Where(u => u.Target == c).Sum(u => u.CultureChangeValue);
    }

    public static Color GetLastColorInUpdateList(CultureUpdate<Color>[] UpdateList, Culture c)
    {
        return (Color)UpdateList.Where(u => u.Target == c).Last().CultureChangeValue;
    }

    public static Tile GetLastTileInUpdateList(CultureUpdate<Tile>[] UpdateList, Culture c)
    {
        return (Tile)UpdateList.Where(u => u.Target == c).Last().CultureChangeValue;
    }

}