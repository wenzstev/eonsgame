using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GenerateNewMapFromOptions : MonoBehaviour
{

    public OptionPickerSystem sizePicker;
    public Slider tempSlider;
    public Slider precipitationSlider;
    public Slider waterlevelSlider;

    public GameObject BoardTemplate;
    public GameObject MapGeneratorTemplate;

    (int, int)[] sizes = { (75, 50), (150, 100), (300, 200) };

    public void CreateAndLoadMap()
    {
        var mapSize = sizes[sizePicker.curOption];
        float temperature = tempSlider.value;
        float precipitation = precipitationSlider.value;
        float waterlevel = waterlevelSlider.value;

        GameObject boardObj = Instantiate(BoardTemplate);
        GameObject boardCreator = Instantiate(MapGeneratorTemplate);

        BoardStats bs = boardObj.GetComponent<BoardStats>();
        bs.height = mapSize.Item2;
        bs.width = mapSize.Item1;
        bs.LandRisePoint = waterlevel;
        bs.globalPrecipitation = precipitation;
        bs.globalTemp = temperature;

        boardObj.GetComponent<BoardInputReader>().bg = boardCreator.GetComponent<BoardGenAlgorithm>();
        boardObj.GetComponent<Board>().CreateBoard();

        CreateSaveAndLoadMap(boardObj.GetComponent<Board>());
    }

    void CreateSaveAndLoadMap(Board b)
    {
        Save save = new Save(b);
        Save.SerializeSave(save, "untitled");
        Save.CreatePersistantSave(save, "untitled");
        SceneManager.LoadScene("PlayScene");
    }
}
