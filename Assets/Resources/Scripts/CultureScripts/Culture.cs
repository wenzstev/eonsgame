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
    public float baseMutationMax = .001f;

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

    [System.NonSerialized]
    public bool isAnimationPlaying = false;



    public float moveTime = .1f;

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
        ChangeTile(t);
        transform.SetParent(t.gameObject.transform);
    }

    void ExecuteTurn()
    {
        Turn turn = decisionMaker.ExecuteTurn();
        turn.pushChangesToCulture(this);
    }

   

    private void OnTick(Dictionary<string, object> empty)
    {
        if (!isAnimationPlaying)
        {
            ExecuteTurn();
        }
    }

    public bool AttemptMove(GameObject tileToMoveTo)
    {
        if (population > maxPopTransfer)
        {
            GameObject splitCulture = SplitCultureFromParent();
            bool didMove = splitCulture.GetComponent<Culture>().AttemptMove(tileToMoveTo);
            if (!didMove)
            {
                splitCulture.GetComponent<Culture>().MergeWith(this);
            }
            return didMove;
        }

        if(affinity == tileToMoveTo.GetComponent<TileInfo>().tileType || Random.value < .01f)
        {
            StartCoroutine(MoveTile(gameObject, tileToMoveTo));
            return true;
        }
        return false;
    }

    public void MoveBack()
    {
        Debug.Log(name + " is repelled, moving back to " + GetComponent<CultureMemory>().previousTile.gameObject);
        StartCoroutine(MoveTile(gameObject, GetComponent<CultureMemory>().previousTile.gameObject));
        currentState = State.Default;
    }

    public void AttemptRepel()
    {
        foreach(Culture c in tileInfo.orderToRemoveCulturesIn)
        {
            if(c.currentState == State.Invader)
            {
                // ability to repel is function of population and affinity (and later tech)
                float hasAffinityAdvantage = c.affinity == affinity ? 0 : .2f;
                float popAdvantage = ((float)population - c.population) / 10f;
                float repelThreshold = .6f + hasAffinityAdvantage + popAdvantage;
                Debug.Log("repel threshold = .6 + " + hasAffinityAdvantage + " + " + popAdvantage);
                if(Random.value < repelThreshold)
                {
                    c.currentState = State.Repelled;
                    Debug.Log(c.name + " is repelled by " + name);
                    break;
                }
            }
        }
        currentState = State.Default;
    }

    public void Flee()
    {
        // split into pieces small enough to move
        // for each piece, attempt to move to adjacent tile
        // if each piece can't move after 3 tries, destroy that piece
        // set each piece back to default behavior
    }



    public void Mutate()
    {
        SetColor(mutateColor(color));
        EventManager.TriggerEvent("CultureUpdated", new Dictionary<string, object> { { "culture", this } });

    }

    public bool InteractWithNeighbor(GameObject tileToShareWith)
    {
        if (tileToShareWith == null || tileToShareWith.GetComponentInChildren<Culture>() == null)
        {
            return false;
        }

        Culture otherCulture = tileToShareWith.GetComponent<TileInfo>().GetRandomCulture();
        if(otherCulture.name == name && CultureAggregation.GetCultureDistance(otherCulture.color, color) > sameCultureCutoff)
        {
            CreateAsNewCulture();
            return true;
        }

        if(currentState == State.NewCulture && otherCulture.name == GetComponent<CultureMemory>().cultureParentName && CultureAggregation.GetCultureDistance(otherCulture.color, color) < sameCultureCutoff)
        {
            otherCulture.ShiftCulture(name);
        }
        


        //tileToShareWith.GetComponentInChildren<Culture>().InfluenceByColor(this);
        return true;
    }

    public void InfluenceByColor(Culture influencer)
    {
        if (name == influencer.name)
        {
            SetColor(Color.Lerp(color, influencer.color, .5f));
        }
        else
        {
            SetColor(Color.Lerp(color, influencer.color, transferRate));
        }
    }

    public void GainAffinity()
    {
        affinity = tileInfo.tileType;
        maxOnTile = tileInfo.tileType == affinity ? tileInfo.popBase + 2 : tileInfo.popBase;
        tileInfo.UpdateCultureSurvivability();
    }

    // --- change state functions

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
        //Debug.Log("Destroying " + name + "(" + GetHashCode() + ")");
        EventManager.StopListening("Tick", OnTick);
        EventManager.TriggerEvent("CultureRemoved", new Dictionary<string, object>() { { "culture", this } });
        Destroy(gameObject);

    }

  

    public bool GrowPopulation()
    {
        
        if(population < tile.gameObject.GetComponent<TileInfo>().popBase)
        {
     
            AddPopulation(1);
            return true;
        }
        return false;
    }

    public void AddPopulation(int num)
    {
        //Debug.Log("Adding one population to " + name + "(" + this.GetHashCode() + ")");
        population += num;
        EventManager.TriggerEvent("CultureUpdated", new Dictionary<string, object> { { "culture", this } });

    }

    Culture RemoveCultureFromOldTile(GameObject cultureObj)
    {
        Culture cultureToMove = cultureObj.GetComponent<Culture>();
        cultureToMove.tile.GetComponent<TileInfo>().RemoveCulture(cultureToMove);

        if (currentState != State.Repelled)
        {
            GetComponent<CultureMemory>().previousTile = cultureToMove.tile;
        }

        cultureToMove.tile = null;
        cultureToMove.tileInfo = null;

        return cultureToMove;
    }

    void CombineCultureWithNewTile(GameObject cultureObj, GameObject newTile, Culture cultureToMove)
    {
        cultureObj.transform.SetParent(newTile.transform);
        TileInfo newTileInfo = newTile.GetComponent<TileInfo>();
        Debug.Log("move complete. changing state to default");
        cultureToMove.currentState = State.Default;

        Culture potentialSameCulture = null;
        if (newTileInfo.cultures.TryGetValue(cultureToMove.name, out potentialSameCulture))
        {
            cultureToMove.MergeWith(potentialSameCulture);
        }
        else
        {
            newTileInfo.AddCulture(cultureToMove);
            cultureToMove.ChangeTile(newTile.GetComponent<Tile>());
        }

        if (newTileInfo.cultures.Count > 1)
        {
            SetInvadersAndInvaded(newTileInfo, cultureToMove);
        }

    }

    void SetInvadersAndInvaded(TileInfo newTileInfo, Culture cultureToMove)
    {
        cultureToMove.currentState = State.Invader;
        foreach (Culture c in newTileInfo.orderToRemoveCulturesIn)
        {
            if (c.name != name)
            {
                c.currentState = State.Invaded;
            }
        }

    }

    public IEnumerator MoveTile (GameObject cultureObj, GameObject newTile)
    {

        Culture cultureToMove = RemoveCultureFromOldTile(cultureObj);

        Vector3 startPosition = cultureObj.transform.position;

        for (float t = 0; t < moveTime; t += Time.deltaTime)
        {
            float curDistance = Mathf.InverseLerp(0, moveTime, t);
            cultureObj.transform.position = Vector3.Lerp(startPosition, newTile.transform.position, curDistance);
            yield return null;
        }

        cultureObj.transform.position = newTile.transform.position;

        CombineCultureWithNewTile(cultureObj, newTile, cultureToMove);
    }



    public void CreateAsNewCulture()
    {
        string newName = getRandomString(5);
        Debug.Log("renaming " + name + "(" + GetHashCode() + ") to " + newName);
        GetComponent<CultureMemory>().cultureParentName = name;

        EventManager.TriggerEvent("CultureRemoved", new Dictionary<string, object> { { "culture", this } });


        tile.GetComponent<TileInfo>().UpdateCultureName(name, newName);
        name = newName;
        gameObject.name = newName;

        EventManager.TriggerEvent("CultureUpdated", new Dictionary<string, object> { { "culture", this } });
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

    void ChangeTile(Tile newTile)
    {
        Debug.Log("changing tile to " + newTile);
        tile = newTile;
        tileInfo = newTile.GetComponent<TileInfo>();
        maxOnTile = tileInfo.tileType == affinity ? tileInfo.popBase + 2 : tileInfo.popBase;
        Debug.Log("tile is " + tile);
    }

   

    // ----- helper functions



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
     
}
