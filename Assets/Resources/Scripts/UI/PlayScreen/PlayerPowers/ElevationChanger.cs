using System.Collections;
using System.Linq;
using UnityEngine;

public class ElevationChanger : MonoBehaviour
{

    public MouseActionsController MouseActionsController;
    public ImprovedBoardGen ImprovedBoardGen;
    public BoardLoader BoardLoader;

    Board Board;

    public float ElevationChangeAmount;

    float timeSinceClicked = -1;
    public float refreshTime = 1; // seconds
    


    // Start is called before the first frame update
    void Awake()
    {
        MouseActionsController.MouseUpAction += MouseActionsController_MouseUpAction;
        BoardLoader.OnBoardCreated += BoardLoader_OnBoardCreated;
    }

    void RaiseElevation(GameObject TileToRaiseObj)
    {
        ChangeElevation(TileToRaiseObj, ElevationChangeAmount);
    }

    void LowerElevation(GameObject TileToLowerObj)
    {
        ChangeElevation(TileToLowerObj, -ElevationChangeAmount);
    }

    void ChangeElevation(GameObject TileToChangeObj, float amount)
    {
        Tile TileToChange = TileToChangeObj.GetComponent<Tile>();
        TileChars TileCharsToRaise = TileToChange.TileChars;
        TileCharsToRaise.absoluteHeight += amount;
        timeSinceClicked = 0;

        //TileCharsToRaise.temperature = ImprovedBoardGen.CalculateTileTemperature(TileCharsToRaise);
        //TileCharsToRaise.precipitation = TileToChange.TileLocation.GetAllNeighbors().Average(e => ImprovedBoardGen.calculatePrecipitation(TileToChangeObj, e));



//        TileToChange.GetComponent<TileDrawer>().UpdateDraw();
    }

    private void FixedUpdate()
    {
        IncrementTime();
    }

    void IncrementTime()
    {
        if (timeSinceClicked >= 0) timeSinceClicked += Time.deltaTime;
        if (timeSinceClicked > refreshTime)
        {
            StartCoroutine(UpdateBoard());
            timeSinceClicked = -1; //disable timer
        }
    }

    private void MouseActionsController_MouseUpAction(object sender, MouseActionsController.MouseActionEventArgs e)
    {
        if (Input.GetKey(KeyCode.Q) && e.GetFirstThatContains<Tile>() != null)
        {
            RaiseElevation(e.GetFirstThatContains<Tile>());
        }
        if (Input.GetKey(KeyCode.W) && e.GetFirstThatContains<Tile>() != null)
        {
            LowerElevation(e.GetFirstThatContains<Tile>());
        }

    }

    public void BoardLoader_OnBoardCreated(object sender, BoardLoader.OnBoardCreatedEventArgs e)
    {
        ImprovedBoardGen.SetBoardObject(e.BoardStats.gameObject);
        Board = e.BoardStats.GetComponent<Board>();
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
}

