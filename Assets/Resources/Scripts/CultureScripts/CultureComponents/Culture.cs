using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Culture : MonoBehaviour
{

    [SerializeField]
    new string name;
    public string Name { get { return name; } }

    [SerializeField]
    Color color;
    public Color Color { get { return color; } }

    public Tile Tile { get; private set; }
    public TileInfo tileInfo { get; private set; }


    CultureMemory cultureMemory;
    public CultureMemory CultureMemory
    {
        get
        {
            if(cultureMemory == null)
            {
                cultureMemory = GetComponent<CultureMemory>();
            }
            return cultureMemory;
        }
    }

    DecisionMaker decisionMaker;

    public GameObject CultureTemplate;

    SpriteRenderer layerMode;
    SpriteRenderer circleMode;

    [SerializeField]
    int population;
    public int Population { get { return population; } }

    public float spreadChance = 1f;

    [Range(0, .05f)]
    public float shareCultureChance = .05f;

    [Range(0, .05f)]
    public float baseMutationMax = .01f;

    [Range(0, .1f)]
    public float sameCultureCutoff = .1f;

    [Range(0, .01f)]
    public float growPopulationChance = .01f;


    [Range(0, 1)]
    public float gainAffinityChance = .003f;

    public int maxPopTransfer = 1;

    [Range(0, .05f)]
    public float influenceRate = .05f;


    public TileDrawer.BiomeType affinity { get; private set; }

    public int maxOnTile;



    public State currentState; 

    public enum State
    {
        Default, 
        Repelled,
        Invaded,
        Invader,
        NewCulture,
        Moving,
        NewOnTile,
        PendingRemoval,
        Overpopulated
    }

    private void Awake()
    {
        EventManager.StartListening("Tick", OnTick);
        layerMode = transform.GetChild(0).GetComponent<SpriteRenderer>();
        //circleMode = transform.GetChild(1).GetComponent<SpriteRenderer>();
        decisionMaker = new DecisionMaker(this);
    }

    public void LoadFromSerialized(GameObject t, GameObject cultureTemplate)
    {
        Tile = t.GetComponent<Tile>();
        SetColor(color);
        CultureTemplate = cultureTemplate;
        gameObject.name = name;
    }

    public void Init(Tile t)
    {
        currentState = State.Default;
        name = getRandomString(5);
        population = 1;
        color = new Color(Random.value, Random.value, Random.value);
        transform.SetParent(t.gameObject.transform);
        gameObject.name = name;

        SetTile(t);

        SetColor(color);
    }

    public void Init(Tile t, Color parent, int pop, string n)
    {
        currentState = State.Default;
        population = pop;
        color = parent;
        name = n;
        gameObject.name = name;
        transform.SetParent(t.gameObject.transform);
        //SetTileWithoutInformingTileInfo(t);
    }


    void ExecuteTurn()
    {
        Turn turn = decisionMaker.ExecuteTurn();
        turn.UpdateAllCultures();
    }

    public void UpdateForTurn(CultureTurnUpdate t)
    {

        if (t.newState == State.PendingRemoval)
        {
            //Debug.Log("state is pending removal");
            DestroyCulture();
            return;
        }

        AddPopulation(t.popChange);



        if (t.newName != null)
        {
            RenameCulture(t.newName);
        }



        SetColor(t.newColor);
        //Debug.Log("setting state for " + GetHashCode() + " to " + t.newState);

        ChangeState(t.newState);

        if (t.newAffinity > 0)
        {
            GainAffinity(t.newAffinity);
        }
        if (t.newTile != null)
        {
            SetTile(t.newTile); // maybe give them some offscreen placeholder tile?
        }


        EventManager.TriggerEvent("CultureUpdated" + name, new Dictionary<string, object> { { "culture", this } });
    }

    private void OnTick(Dictionary<string, object> empty)
    {
        ExecuteTurn();
    }


    private void ChangeState(State newState)
    {
        if(CultureMemory.previousState != newState)
        {
            CultureMemory.previousState = currentState;
        }
        if(newState == State.Default)
        {
            CultureMemory.wasRepelled = false;
        }
        currentState = newState;
    }

    private void GainAffinity(int newAffinity)
    {
        affinity = (TileDrawer.BiomeType) newAffinity;
        maxOnTile = tileInfo.tileType == affinity ? tileInfo.popBase + 2 : tileInfo.popBase;
        tileInfo.UpdateCultureSurvivability();
        tileInfo.UpdateMaxOnTile(this);
    }


    public GameObject SplitCultureFromParent() // creates new culture group from parent
    {
        GameObject newCultureObj = Instantiate(CultureTemplate, transform.position, Quaternion.identity);
        Culture newCulture = newCultureObj.GetComponent<Culture>();
        newCulture.Init(Tile, color, maxPopTransfer, name);
        AddPopulation(-maxPopTransfer);
        newCulture.CultureMemory.previousTile = Tile;
        return newCultureObj;
    }

    public void SetColor(Color c)
    {
        color = c;
        layerMode.color = c;
        //circleMode.color = c;
    }


    public bool CanMerge(Culture other)
    {
        float mergeThreshold = Mathf.Min(sameCultureCutoff, other.sameCultureCutoff);
        float cultureDistance = CultureHelperMethods.GetCultureDistance(this, other);
        if(cultureDistance < mergeThreshold )
        {
            return true;
        }
        return false;

    }

    void DestroyCulture()
    {
        //Debug.Log("Destroying " + name + "(" + GetHashCode() + ")");
        EventManager.StopListening("Tick", OnTick);
        EventManager.TriggerEvent("CultureRemoved" + name, new Dictionary<string, object>() { { "culture", this } });
        if(tileInfo != null)
        {
            tileInfo.RemoveCulture(this);
        }
        Destroy(gameObject);
    }

    public void AddPopulation(int num)
    { 
        //Debug.Log("adding " + num + " to  " + GetHashCode());
        population += num;
        if(population == 0)
        {
            DestroyCulture();
        }
    }

    void SetTileWithoutInformingTileInfo(Tile newTile)
    {
        Tile = newTile;
        tileInfo = newTile.GetComponent<TileInfo>();
        maxOnTile = tileInfo.tileType == affinity ? tileInfo.popBase + 2 : tileInfo.popBase;
    }

    void SetTile(Tile newTile)
    {
        //Debug.Log("setting culture " + name + "("+GetHashCode()+") to tile " + newTile);
        if(tileInfo != null)
        {
            tileInfo.RemoveCulture(this);
        }

        if (newTile.gameObject != Tile.moveTile) // move tile is a special tile that holds all moving cultures, can't add it to tileinfo because there will be more than one of the same culture
        {
            newTile.GetComponent<TileInfo>().AddCulture(this);
        }

        CultureMemory cm = GetComponent<CultureMemory>();

        cm.previousTile = Tile;


        SetTileWithoutInformingTileInfo(newTile);


    }

    void RenameCulture(string newName)
    {
        EventManager.TriggerEvent("CultureRemoved" + name, new Dictionary<string, object> { { "culture", this } });
        CultureMemory.cultureParentName = name;
        name = newName;
        gameObject.name = newName;
        EventManager.TriggerEvent("CultureUpdated" + newName, new Dictionary<string, object> { { "culture", this } });
        if(tileInfo != null)
        {
            Debug.LogWarning("Culture has tileInfo, not changing. Unless this is a test it may be a bug.");
        }
    
    }

    public Color mutateColor(Color parentColor)
    {
        float getMutationRate()
        {
            return (Random.value * baseMutationMax) - (baseMutationMax / 2);
        }
        return new Color(getMutationRate() + parentColor.r, getMutationRate() + parentColor.g, getMutationRate() + parentColor.b);
    }

    private void OnDestroy()
    {
        EventManager.StopListening("Tick", OnTick);

    }

    public static Color influenceColor(Culture parent, Culture influencer)
    {
        float rate = Random.value * influencer.influenceRate;
        return Color.Lerp(parent.color, influencer.color, rate);
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


}
