using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DEPRECATED AND BROKEN (for now; new tile gen has changed too much to be backwards compatabile without a lot of pointless work)
public class PerlinBoardGenerator : BoardGenAlgorithm
{

    public float scale;

    public bool randomCoords;


    // creates a perlin board and levels it based on the number of levels requested
    public override BoardTileRelationship CreateBoard(BoardStats bs)
    {
        BoardTileRelationship perlinBoard = CreateRawPerlinBoard(bs.gameObject, scale, bs.width, bs.height);

        for (int j = 0; j < perlinBoard.tiles.GetLength(1); j++)
        {
            for (int i = 0; i < perlinBoard.tiles.GetLength(0); i++)
            {
                // TODO: implement way to convert base tile to certain type with elevation
            }
        }
        return perlinBoard;
    }



}
