using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GenerateNewMapFromOptions : MonoBehaviour
{

    public OptionPickerSystem sizePicker;
    public Slider tempSlider;
    public Slider precipitationSlider;
    public Slider waterlevelSlider;

    public TMP_InputField MapName;

    public GameObject BoardTemplate;
    public GameObject MapGeneratorTemplate;

    public FadeFromBlack fader;

    public (int, int)[] sizes = { (50, 35), (75, 50), (100, 65) };

    Board board;
    BoardGenAlgorithm bga;

    private void Awake()
    {
        fader.OnFadeComplete += Fader_OnFadeComplete;
    }

    void Fader_OnFadeComplete(object sender, EventArgs e)
    {
        CreateAndLoadMap();
    }

    public void CreateAndLoadMap()
    {
        var mapSize = sizes[sizePicker.curOption];
        float temperature = tempSlider.value;
        float precipitation = precipitationSlider.value;
        float waterlevel = waterlevelSlider.value;

        GameObject boardObj = Instantiate(BoardTemplate);
        GameObject boardCreator = Instantiate(MapGeneratorTemplate);

        boardCreator.GetComponent<BoardGenAlgorithm>().OnBoardCalculationsCompleted += ImprovedBoardGen_OnBoardCompleted; // board is created with coroutines, so we need to wait till it's done to load the map
        board = boardObj.GetComponent<Board>();

        BoardStats bs = boardObj.GetComponent<BoardStats>();
        bs.SetDimensions(mapSize.Item1, mapSize.Item2);
        bs.SetTileWidth(1);
        bs.LandRisePoint = waterlevel;
        bs.globalPrecipitation = precipitation;
        bs.globalTemp = temperature;

        boardObj.GetComponent<BoardInputReader>().bg = boardCreator.GetComponent<BoardGenAlgorithm>();
        boardObj.GetComponent<Board>().CreateBoard();


        //CreateSaveAndLoadMap(boardObj.GetComponent<Board>(), MapName.text);
    }

    void CreateSaveAndLoadMap(Board b, string mapName)
    {
        if (mapName == "") mapName = "New World";
        Save save = new Save(b);
        Save.SerializeSave(save, mapName);
        Save.CreatePersistantSave(save, mapName);
        SceneManager.LoadScene("PlayScene");
    }

    void ImprovedBoardGen_OnBoardCompleted(object sender, EventArgs empty)
    {
        CreateSaveAndLoadMap(board, MapName.text);
    }
}
