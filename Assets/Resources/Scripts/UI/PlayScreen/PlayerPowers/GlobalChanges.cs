using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalChanges : MonoBehaviour
{
    public float tempChangePerEvent;
    public float precipitationChangePerEvent;
    public float seaLevelChangePerEvent;
    public int eventFrequency;


    public BoardLoader BoardLoader;
    public ImprovedBoardGen ImprovedBoardGen;

    BoardStats BoardStats;
    Board Board;

    bool isListening;

    ChangeType CurrentState;

    enum ChangeType
    {
        Static,
        Warming,
        Cooling,
        Wetter,
        Dryer,
        SeaRise,
        SeaFall
    }

    void OnNewBoardAge(Dictionary<string, object> age)
    {
        if ((int)age["age"] % eventFrequency == 0) ExecuteChangeEvent();
    }

    private void Awake()
    {
        BoardLoader.OnBoardCreated += BoardLoader_OnBoardCreated;
    }


    void ExecuteChangeEvent()
    {
        switch(CurrentState)
        {
            case ChangeType.Warming:
                WarmWorld();
                break;
            case ChangeType.Cooling:
                CoolWorld();
                break;
            case ChangeType.Wetter:
                MakeWetter();
                break;
            case ChangeType.Dryer:
                MakeDryer();
                break;
            case ChangeType.SeaRise:
                RaiseSeaLevel();
                break;
            case ChangeType.SeaFall:
                LowerSeaLevel();
                break;
        }
        StartCoroutine(UpdateBoard());

    }

    void WarmWorld()
    {
        BoardStats.globalTemp += tempChangePerEvent;
    }

    void CoolWorld()
    {
        BoardStats.globalTemp -= tempChangePerEvent;
    }

    void MakeWetter()
    {
        BoardStats.globalPrecipitation += precipitationChangePerEvent;
    }

    void MakeDryer()
    {
        BoardStats.globalPrecipitation -= precipitationChangePerEvent;
    }

    void RaiseSeaLevel()
    {
        BoardStats.NormalizedSeaLevel += seaLevelChangePerEvent;
    }

    void LowerSeaLevel()
    {
        BoardStats.NormalizedSeaLevel -= seaLevelChangePerEvent;
    }

    public IEnumerator UpdateBoard()
    {
        yield return StartCoroutine(ImprovedBoardGen.CalculateBoardInfo(Board.Tiles));

        for (int i = 0; i < Board.Tiles.GetLength(0); i++)
        {
            for (int j = 0; j < Board.Tiles.GetLength(1); j++)
            {
                Board.Tiles[i, j].GetComponent<TileDrawer>().UpdateDraw();
            }
            yield return null;



        }
    }

    public void BoardLoader_OnBoardCreated(object sender, BoardLoader.OnBoardCreatedEventArgs e)
    {
        //Debug.Log("adding board to poers");
        ImprovedBoardGen.SetBoardObject(e.BoardStats.gameObject);
        BoardStats = e.BoardStats;
        Board = BoardStats.GetComponent<Board>();
    }

    void StartListening()
    {
        EventManager.StartListening("NewBoardAge", OnNewBoardAge);
        isListening = true;
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.LeftBracket))
        {
            CurrentState = ChangeType.Warming;
            if (!isListening) StartListening();
        }
        if(Input.GetKey(KeyCode.RightBracket))
        {
            CurrentState = ChangeType.Cooling;
            if (!isListening) StartListening();
        }
        if (Input.GetKey(KeyCode.Semicolon))
        {
            CurrentState = ChangeType.Wetter;
            if (!isListening) StartListening();
        }
        if (Input.GetKey(KeyCode.Quote))
        {
            CurrentState = ChangeType.Dryer;
            if (!isListening) StartListening();
        }
        if (Input.GetKey(KeyCode.Period))
        {
            CurrentState = ChangeType.SeaRise;
            if (!isListening) StartListening();
        }
        if (Input.GetKey(KeyCode.Slash))
        {
            CurrentState = ChangeType.SeaFall;
            if (!isListening) StartListening();
        }
        if (Input.GetKey(KeyCode.Backslash))
        {
            CurrentState = ChangeType.Static;
            EventManager.StopListening("NewBoardAge", OnNewBoardAge);
            isListening = false;
        }
    }


}
