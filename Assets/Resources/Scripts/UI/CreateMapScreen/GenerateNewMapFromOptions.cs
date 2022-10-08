using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateNewMapFromOptions : MonoBehaviour
{

    public OptionPickerSystem sizePicker;
    public Slider tempSlider;
    public Slider humiditySlider;
    public Slider waterlevelSlider;

    public GameObject BoardTemplate;
    public GameObject MapGeneratorTemplate;

    (int, int)[] sizes = { (75, 50), (150, 100), (300, 200) };

    public void CreateAndLoadMap()
    {
        var mapSize = sizes[sizePicker.curOption];
        float temperature = tempSlider.value;
        float humidity = humiditySlider.value;
        float waterlevel = waterlevelSlider.value;

        GameObject boardObj = Instantiate(BoardTemplate);
        GameObject boardCreator = Instantiate(MapGeneratorTemplate);

        BoardStats bs = boardObj.GetComponent<BoardStats>();
        bs.height = mapSize.Item2;
        bs.width = mapSize.Item1;
        bs.percentUnderWater = waterlevel;
        bs.globalHumidity = humidity;
        bs.globalTemp = temperature;

        boardObj.GetComponent<BoardInputReader>().bg = boardCreator.GetComponent<BoardGenAlgorithm>();
        boardObj.GetComponent<Board>().CreateBoard();
    }
}
