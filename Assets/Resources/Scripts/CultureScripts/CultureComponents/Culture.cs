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

    public TileComponents TileComponents { get; private set; }



    CultureHandler cultureHandler;

    public CultureHandler CultureHandler { get { return cultureHandler; } }


    public event EventHandler<OnPopulationChangedEventArgs> OnPopulationChanged;
    public event EventHandler<OnCultureNameChangedEventArgs> OnNameChanged;
    public event EventHandler<OnCultureDestroyedEventArgs> OnCultureDestroyed;
    public event EventHandler<OnColorChangedEventArgs> OnColorChanged;

    bool isQuitting = false;

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

    AffinityManager _affinityManager;
    public AffinityManager AffinityManager { get { return _affinityManager; } }


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

    public void Init(Tile t, int InitialPopulation)
    {
        currentState = State.Default;
        name = getRandomString(5);
        population = InitialPopulation;
        color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
        gameObject.name = name;

        SetTile(t, true);

        SetColor(color);
        _affinityManager = GetComponent<AffinityManager>();
        AffinityManager.Initialize();
        EventManager.TriggerEvent("CultureCreated", new Dictionary<string, object> { { "culture", this } });
    }

    public void InitFromParent(Culture parent, int newPop)
    {
        currentState = State.Default;
        population = newPop;
        name = parent.Name;
        gameObject.name = name;
        //transform.SetParent(t.gameObject.transform);
        SetColor(parent.Color);
        _affinityManager = GetComponent<AffinityManager>();
        AffinityManager.Initialize();
        AffinityManager.SetStats(parent.GetComponent<AffinityManager>().GetStatCopy());


        EventManager.TriggerEvent("CultureCreated", new Dictionary<string, object> { { "culture", this } });

    }

    public void ChangeState(State newState)
    {
        if(newState == State.PendingRemoval)
        {
            //Debug.Log("Setting to destroy " + this);
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
        newCulture.InitFromParent(this, numInNewCulture);
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

    public void AddPopulation(int num)
    { 
        //Debug.Log($"Adding {num} to {this} for new pop of {population + num}");
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

    public void RemoveFromTile()
    {
        //Debug.Log($"removing culture {this} from {Tile}");
        if(Tile == null)
        {
            //Debug.LogWarning("Tried to remove from nonexistant tile!");
            return;
        }
        cultureHandler?.RemoveCulture(this);
        CultureMemory.previousTile = Tile;
        Tile = null;
        tileInfo = null;
        cultureHandler = null;
    }

    public void SetTile(Tile newTile, bool bypassArrival)
    {
        if(Tile != null) RemoveFromTile();

        //Debug.Log($"Setting tile for culture {this} to tile {newTile}");
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
        Color newColor = new Color(getMutationRate() + parentColor.r, getMutationRate() + parentColor.g, getMutationRate() + parentColor.b);
        OnColorChanged?.Invoke(this, new OnColorChangedEventArgs() { color = newColor });
        return newColor;
    }
    
    public override string ToString()
    {
        return $"{name}({GetHashCode()})";
    }

    public void DestroyCulture()
    {
        if (!isQuitting)
        {
            RemoveFromTile();
            EventManager.TriggerEvent("CultureDestroyed", new Dictionary<string, object> { { "culture", this } });
            OnCultureDestroyed?.Invoke(this, new OnCultureDestroyedEventArgs() { DestroyedCulture = this });
        }
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {

    }

    public static Color influenceColor(Culture parent, Culture influencer)
    {
        float rate = UnityEngine.Random.value * influencer.influenceRate;
        return Color.Lerp(parent.color, influencer.color, rate);
    }

    public static string MutateString(string original)
    {
        int mutationIndex = (int)(UnityEngine.Random.value * original.Length);
        string newChar = getRandomString(1);
        return original.Substring(0, mutationIndex) + newChar + original.Substring(mutationIndex + 1);

    }

    public static string CombineStrings(string first, string second)
    {
        int combineIndex = (int)first.Length / 2;
        //Debug.Log($"Combining {first} and {second} into {first.Substring(0, combineIndex) + second.Substring(combineIndex)}");

        return first.Substring(0, combineIndex) + second.Substring(combineIndex);
    }

    public static string getRandomString(int length)
    {
        char[] characters = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

        string rand = "";
        while(rand.Length < length)
        {
            rand += characters[Mathf.FloorToInt(UnityEngine.Random.value * characters.Length)];
        }
        return rand;
    }

    private void OnApplicationQuit()
    {
        isQuitting = true;
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

    public class OnColorChangedEventArgs : EventArgs
    {
        public Color color;
    }

}
