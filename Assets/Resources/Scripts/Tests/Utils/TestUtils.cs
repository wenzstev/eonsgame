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


    public static Culture.State GetLastStateInUpdateList(INonGenericCultureUpdate[] UpdateList)
    {
        return (Culture.State)UpdateList.Where(u => u.GetType() == typeof(StateUpdate)).Last().GetCultureChange();
    }

    public static int GetCombinedPopulationInUpdateList(INonGenericCultureUpdate[] UpdateList)
    {
        return UpdateList.Where(u => u.GetType() == typeof(PopulationUpdate)).Sum(u => (int)u.GetCultureChange());
    }

    public static float GetCombinedFoodChangeInUpdateList(INonGenericCultureUpdate[] UpdateList)
    {
        return UpdateList.Where(u => u.GetType() == typeof(FoodUpdate)).Sum(u => (int)u.GetCultureChange());
    }

    public static Color GetLastColorInUpdateList(INonGenericCultureUpdate[] UpdateList)
    {
        return (Color)UpdateList.Where(u => u.GetType() == typeof(ColorUpdate)).Last().GetCultureChange();
    }

    public static Tile GetLastTileInUpdateList(INonGenericCultureUpdate[] UpdateList)
    {
        return (Tile)UpdateList.Where(u => u.GetType() == typeof(TileUpdate)).Last().GetCultureChange();
    }

}