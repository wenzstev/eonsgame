using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Culture : MonoBehaviour
{
    new public string name;
    public Color color;
    public Tile tile;
    public TileInfo tileInfo;

    DecisionMaker decisionMaker;

    public GameObject cultureTemplate;

    SpriteRenderer layerMode;
    SpriteRenderer circleMode;

    public int population;

    public float spreadChance = 1f;

    [Range(0, .05f)]
    public float shareCultureChance = .05f;

    [Range(0, .05f)]
    public float baseMutationMax = .01f;

    [Range(0, .01f)]
    public float transferRate = .001f;

    [Range(0, .1f)]
    public float sameCultureCutoff = .1f;

    [Range(0, .01f)]
    public float growPopulationChance = .01f;

    [Range(0, 1)]
    public float repelChance = .9f;

    [Range(0, 1)]
    public float gainAffinityChance = .003f;

    public int maxPopTransfer = 1;


    public string affinity = "";

    public int maxOnTile;
 


    public State currentState;

    public enum State
    {
        Default, 
        Refugee, 
        Repelled,
        Invaded,
        Invader,
        NewCulture,
        StartMove,
        Moving,
       
    }

    private void Awake()
    {
        EventManager.StartListening("Tick", OnTick);
        layerMode = transform.GetChild(0).GetComponent<SpriteRenderer>();
        circleMode = transform.GetChild(1).GetComponent<SpriteRenderer>();
        decisionMaker = new DecisionMaker(this);
    }

    public void Init(Tile t)
    {
        currentState = State.Default;
        name = getRandomString(5);
        population = 1;
        color = new Color(Random.value, Random.value, Random.value);
        InitParentTile(t);
        SetColor(color);
        gameObject.name = name;

    }

    public void Init(Tile t, Color parent, int pop, string n)
    {
        currentState = State.Default;
        population = pop;
        color = parent;
        name = n;
        gameObject.name = name;
        InitParentTile(t);
    }

    void InitParentTile(Tile t)
    {
        SetTile(t);
        transform.SetParent(t.gameObject.transform);
    }

    void ExecuteTurn()
    {
        Turn turn = decisionMaker.ExecuteTurn();
        turn.pushChangesToCulture(this);
    }

    private void OnTick(Dictionary<string, object> empty)
    {
        ExecuteTurn();
    }


    public void GainAffinity(string newAffinity)
    {
        affinity = newAffinity;
        maxOnTile = tileInfo.tileType == affinity ? tileInfo.popBase + 2 : tileInfo.popBase;
        tileInfo.UpdateCultureSurvivability();
    }


    public GameObject SplitCultureFromParent() // creates new culture group from parent
    {
        GameObject newCultureObj = Instantiate(cultureTemplate, transform.position, Quaternion.identity);
        Culture newCulture = newCultureObj.GetComponent<Culture>();
        newCulture.Init(tile, color, maxPopTransfer, name);
        AddPopulation(-maxPopTransfer);
        
        return newCultureObj;
    }

    public void SetColor(Color c)
    {
        color = c;
        layerMode.color = c;
        circleMode.color = c;
    }


    public bool AttemptMerge(Culture other)
    {
        Debug.Log("Attempting to merge " + this + "(" + this.GetHashCode() + ")" + " with " + other + "(" + other.GetHashCode() + ")");
        float mergeThreshold = Mathf.Min(sameCultureCutoff, other.sameCultureCutoff);
        float cultureDistance = CultureHelperMethods.GetCultureDistance(this, other);

        if(cultureDistance < mergeThreshold )
        {
            MergeWith(other);
            return true;
        }

        return false;

    }

    public void MergeWith(Culture other)
    {
        //Debug.Log("merging " + name + " code " + this.GetHashCode() + " with " + other.name + " code " + other.GetHashCode());
        float percentThisPopulation = (float)population / (population + other.maxPopTransfer);
        other.SetColor(Color.Lerp(color, other.color, percentThisPopulation));
        other.AddPopulation(population);
        DestroyCulture();
        
    }

    public void DestroyCulture()
    {
        Debug.Log("Destroying " + name + "(" + GetHashCode() + ")");
        EventManager.StopListening("Tick", OnTick);
        EventManager.TriggerEvent("CultureRemoved", new Dictionary<string, object>() { { "culture", this } });
        if(tileInfo != null)
        {
            tileInfo.RemoveCulture(this);
        }
        Destroy(gameObject);
    }

    public void AddPopulation(int num)
    {
        population += num;
    }



    public void ShiftCulture(string newName)
    {
        GetComponent<CultureMemory>().cultureParentName = name;
        tile.GetComponent<TileInfo>().UpdateCultureName(name, newName);

        EventManager.TriggerEvent("CultureRemoved", new Dictionary<string, object> { { "culture", this } });


        name = newName;
        gameObject.name = newName;
        EventManager.TriggerEvent("CultureUpdated", new Dictionary<string, object> { { "culture", this } });
    }


    public void SetTile(Tile newTile)
    {
        tile = newTile;
        tileInfo = newTile.GetComponent<TileInfo>();
        maxOnTile = tileInfo.tileType == affinity ? tileInfo.popBase + 2 : tileInfo.popBase;
    }

    public Color mutateColor(Color parentColor)
    {
        float getMutationRate()
        {
            return (Random.value * baseMutationMax) - (baseMutationMax / 2);
        }
        return new Color(getMutationRate() + parentColor.r, getMutationRate() + parentColor.g, getMutationRate() + parentColor.b);
    }

    public static string getRandomString(int length)
    {
        char[] characters = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

        string rand = "";
        while(rand.Length <= length)
        {
            rand += characters[Mathf.FloorToInt(Random.value * characters.Length)];
        }
        return rand;
    }

    public void CreateAsNewCulture()
    {
        string newName = Culture.getRandomString(5);
        Debug.Log("renaming " + name + "(" + GetHashCode() + ") to " + newName);
        GetComponent<CultureMemory>().cultureParentName = name;

        EventManager.TriggerEvent("CultureRemoved", new Dictionary<string, object> { { "culture", this } });

        name = newName;
        gameObject.name = newName;

        EventManager.TriggerEvent("CultureUpdated", new Dictionary<string, object> { { "culture", this } });
    }

}
