using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
public class Culture : MonoBehaviour
{

    [SerializeField]
    new string name = "culture";
    public string Name { get { return name; } }

    [SerializeField]
    Color color;
    public Color Color { get { return color; } }

    public Tile Tile { get; private set; }
    public TileInfo tileInfo { get; private set; }

    CultureHandler cultureHandler;

    public CultureHandler CultureHandler { get { return cultureHandler; } }


    public event EventHandler<OnPopulationChangedEventArgs> OnPopulationChanged;
    public event EventHandler<OnCultureNameChangedEventArgs> OnNameChanged;
    public event EventHandler<OnCultureDestroyedEventArgs> OnCultureDestroyed;

    CultureFoodStore cultureFoodStore;
    public CultureFoodStore CultureFoodStore
    {
        get
        {
            if(cultureFoodStore == null)
            {
                cultureFoodStore = GetComponent<CultureFoodStore>();
            }
            return cultureFoodStore;
        }
    }

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
    public DecisionMaker DecisionMaker { get { return decisionMaker; } }

    public GameObject CultureTemplate
    {
        get
        {
            return Resources.Load<GameObject>("Prefabs/Board/Inhabitants/Culture");
        }
    }

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

    public float FertilityRate = .00007f;


    public int maxPopTransfer = 20;
    public int minPopTransfer = 5;

    [Range(0, .05f)]
    public float influenceRate = .05f;

    public float FoodGatherRate = 1f;

    public float AffinityGainRate = .01f;




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
        Overpopulated,
        SeekingFood,
        Starving
    }




    private void Awake()
    {
        layerMode = GetComponent<SpriteRenderer>();
        //circleMode = transform.GetChild(1).GetComponent<SpriteRenderer>();
        decisionMaker = new DecisionMaker(this);
    }

    public void LoadFromSerialized(GameObject t, GameObject cultureTemplate)
    {
        Tile = t.GetComponent<Tile>();
        SetColor(color);
        gameObject.name = name;
    }

    public void Init(Tile t, int InitialPopulation)
    {
        currentState = State.Default;
        name = getRandomString(5);
        population = InitialPopulation;
        color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
        gameObject.name = name;

        SetTile(t, true);

        SetColor(color);
        GetComponent<AffinityManager>().Initialize();
        EventManager.TriggerEvent("CultureCreated", new Dictionary<string, object> { { "culture", this } });
    }

    public void InitFromParent(Tile t, Color parent, int pop, string n)
    {
        currentState = State.Default;
        population = pop;
        name = n;
        gameObject.name = name;
        transform.SetParent(t.gameObject.transform);
        SetColor(parent);
        GetComponent<AffinityManager>().Initialize();
        EventManager.TriggerEvent("CultureCreated", new Dictionary<string, object> { { "culture", this } });

    }

    public void ChangeState(State newState)
    {
        if(newState == State.PendingRemoval)
        {
            DestroyCulture();
            return;
        }
        if(CultureMemory.previousState != newState)
        {
            CultureMemory.previousState = currentState;
        }
        if(newState == State.Default)
        {
            CultureMemory.wasRepelled = false;
        }
        if(newState == State.Moving)
        {
            RemoveFromTile();
        }
        
        currentState = newState;
    }

    public GameObject SplitCultureFromParent() // creates new culture group from parent
    {
        GameObject newCultureObj = Instantiate(CultureTemplate, transform.position, Quaternion.identity);
        Culture newCulture = newCultureObj.GetComponent<Culture>();
        int numInNewCulture = UnityEngine.Random.Range(minPopTransfer, maxPopTransfer);
        newCulture.InitFromParent(Tile, color, numInNewCulture, name);
        AddPopulation(-numInNewCulture);
        newCulture.CultureMemory.previousTile = Tile;
       // Debug.Log($"Split {newCulture} from {this} on tile {this.Tile}");
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

    public void DestroyCulture()
    {
        //Debug.Log($"Destroying {this}");
        EventManager.TriggerEvent("CultureDestroyed", new Dictionary<string, object> { { "culture", this } });
        OnCultureDestroyed?.Invoke(this, new OnCultureDestroyedEventArgs() { DestroyedCulture = this });
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

        if (num != 0) OnPopulationChanged?.Invoke(this, new OnPopulationChangedEventArgs() { PopChange = num});
    }

    void SwapTile(Tile newTile)
    {
        RemoveFromTile();
        SetTile(newTile, false);
    }

    void RemoveFromTile()
    {
        //Debug.Log($"removing culture {this} ({this.GetHashCode()}) from {Tile}");
        if(Tile == null)
        {
            Debug.LogWarning("Tried to remove from nonexistant tile!");
            return;
        }
        cultureHandler.RemoveCulture(this);
        CultureMemory.previousTile = Tile;
        Tile = null;
        tileInfo = null;
        cultureHandler = null;
    }

    public void SetTile(Tile newTile, bool bypassArrival)
    {
        if (newTile == null)
        {
            RemoveFromTile();
            return;
        }
        //Debug.Log($"Setting tile for culture {this}({this.GetHashCode()}) to tile {newTile}");
        CultureHandler newCultureHandler = newTile.GetComponentInChildren<CultureHandler>();

        if (bypassArrival) newCultureHandler.BypassArrival(this); else newCultureHandler.AddNewArrival(this);

        cultureHandler = newCultureHandler;
        Tile = newTile;
        tileInfo = newTile.GetComponent<TileInfo>();
    }


    public void RenameCulture(string newName)
    {
        //Debug.Log($"Renaming culture {this} to have a name of {newName}");
        string oldName = Name;
        name = newName;
        gameObject.name = newName;
        CultureHandler?.RenameCulture(this, oldName);
        OnNameChanged?.Invoke(this, new OnCultureNameChangedEventArgs() {NewName = newName, OldName = oldName});
    }

    public Color mutateColor(Color parentColor)
    {
        float getMutationRate()
        {
            return (UnityEngine.Random.value * baseMutationMax) - (baseMutationMax / 2);
        }
        return new Color(getMutationRate() + parentColor.r, getMutationRate() + parentColor.g, getMutationRate() + parentColor.b);
    }
    
    public override string ToString()
    {
        return $"{name}({GetHashCode()})";
    }


    public static Color influenceColor(Culture parent, Culture influencer)
    {
        float rate = UnityEngine.Random.value * influencer.influenceRate;
        return Color.Lerp(parent.color, influencer.color, rate);
    }

    public static string getRandomString(int length)
    {
        char[] characters = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

        string rand = "";
        while(rand.Length <= length)
        {
            rand += characters[Mathf.FloorToInt(UnityEngine.Random.value * characters.Length)];
        }
        return rand;
    }

    public class OnPopulationChangedEventArgs : EventArgs
    {
        public int PopChange;
    }

    public class OnCultureNameChangedEventArgs : EventArgs
    {
        public string NewName;
        public string OldName;
    }

    public class OnCultureDestroyedEventArgs : EventArgs
    {
        public Culture DestroyedCulture;
    }

}
