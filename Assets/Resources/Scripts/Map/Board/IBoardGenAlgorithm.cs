using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoardGenAlgorithm : MonoBehaviour
{
    public abstract int[,] getLevelledBoard(int numLevels, int boardWidth, int boardHeight);
}
