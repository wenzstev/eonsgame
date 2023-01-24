using System.Collections.Generic;
using UnityEngine;


// ugly code to minimize garbage allocation every turn
public class UpdateHolder
{
    // Lists for different types of updates
    List<CultureUpdate<int>> IntUpdates;
    List<CultureUpdate<float>> FloatUpdates;
    List<CultureUpdate<string>> StringUpdates;
    List<CultureUpdate<Tile>> TileUpdates;
    List<CultureUpdate<AffinityStats>> AffinityUpdates;
    List<CultureUpdate<Color>> ColorUpdates;
    List<CultureUpdate<Culture.State>> StateUpdates;

    public UpdateHolder()
    {
        IntUpdates = new List<CultureUpdate<int>>();
        FloatUpdates = new List<CultureUpdate<float>>();
        StringUpdates = new List<CultureUpdate<string>>();
        TileUpdates = new List<CultureUpdate<Tile>>();
        AffinityUpdates = new List<CultureUpdate<AffinityStats>>();
        ColorUpdates = new List<CultureUpdate<Color>>();
        StateUpdates = new List<CultureUpdate<Culture.State>>();
    }

    // add different types of updates
    public void AddIntUpdate(CultureUpdate<int> update)
    {
        IntUpdates.Add(update);
    }
    public void AddFloatUpdate(CultureUpdate<float> update)
    {
        FloatUpdates.Add(update);
    }
    public void AddStringUpdate(CultureUpdate<string> update)
    {
        StringUpdates.Add(update);
    }
    public void AddTileUpdate(CultureUpdate<Tile> update)
    {
        TileUpdates.Add(update);
    }
    public void AddAffinityUpdate(CultureUpdate<AffinityStats> update)
    {
        AffinityUpdates.Add(update);
    }
    public void AddColorUpdate(CultureUpdate<Color> update)
    {
        ColorUpdates.Add(update);
    }
    public void AddStateUpdate(CultureUpdate<Culture.State> update)
    {
        StateUpdates.Add(update);
    }

    // get different types of updates
    public CultureUpdate<int>[] GetIntUpdates()
    {
        return IntUpdates.ToArray();
    }
    public CultureUpdate<float>[] GetFloatUpdates()
    {
        return FloatUpdates.ToArray();
    }
    public CultureUpdate<string>[] GetStringUpdates()
    {
        return StringUpdates.ToArray();
    }
    public CultureUpdate<Tile>[] GetTileUpdates()
    {
        return TileUpdates.ToArray();
    }
    public CultureUpdate<AffinityStats>[] GetAffinityStatsUpdates()
    {
        return AffinityUpdates.ToArray();
    }
    public CultureUpdate<Color>[] GetColorUpdates()
    {
        return ColorUpdates.ToArray();
    }
    public CultureUpdate<Culture.State>[] GetStateUpdates()
    {
        return StateUpdates.ToArray();
    }

    public void ExecuteAllUpdates()
    {
        for (int i = 0; i < IntUpdates.Count; i++)
        {
            IntUpdates[i].ExecuteChange();
        }
        IntUpdates.Clear();

        for (int i = 0; i < FloatUpdates.Count; i++)
        {
            FloatUpdates[i].ExecuteChange();
        }
        FloatUpdates.Clear();

        for (int i = 0; i < StringUpdates.Count; i++)
        {
            StringUpdates[i].ExecuteChange();
        }
        StringUpdates.Clear();

        for (int i = 0; i < TileUpdates.Count; i++)
        {
            TileUpdates[i].ExecuteChange();
        }
        TileUpdates.Clear();

        for (int i = 0; i < AffinityUpdates.Count; i++)
        {
            AffinityUpdates[i].ExecuteChange();
        }
        AffinityUpdates.Clear();

        for (int i = 0; i < ColorUpdates.Count; i++)
        {
            ColorUpdates[i].ExecuteChange();
        }
        ColorUpdates.Clear();

        for(int i = 0; i < StateUpdates.Count; i++)
        {
            StateUpdates[i].ExecuteChange();
        }
        StateUpdates.Clear();
    }


}