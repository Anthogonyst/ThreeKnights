﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles swapping tiles between different boards.
/// </summary>
public class InterboardSwapHandler : TileSwapHandler
{
    [SerializeField] TileBoardController[] otherGameBoards;
    List<TileBoardController> canSwapBetween = new List<TileBoardController>();
    
    protected override void Awake()
    {
        base.Awake();
        canSwapBetween.Add(gameBoard);
        canSwapBetween.AddRange(otherGameBoards);
    }
    protected override bool ShouldIgnoreTile(TileController tile)
    {
        return TileNotOnGameBoards(tile);
    }

    bool TileNotOnGameBoards(TileController tile)
    {
        return canSwapBetween.Contains(tile.Board) == false;
    }

    protected override TileSwapType CanSwapTiles(TileController firstTile, TileController secondTile)
    {
        bool canSwap = NeitherTileIsAir(firstTile, secondTile) && TilesAreOnDifferentBoards(firstTile, secondTile);

        if (canSwap)
            return TileSwapType.adjacent; // Treat it as adjacent
        else
            return TileSwapType.none;
    }

    bool NeitherTileIsAir(TileController firstTile, TileController secondTile)
    {
        return firstTile.Type != AirTileType && secondTile.Type != AirTileType;
    }

    bool TilesAreOnDifferentBoards(TileController firstTile, TileController secondTile)
    {
        return firstTile.Board != secondTile.Board;
    }

    protected override IEnumerator AdjacentSwapCoroutine(TileController firstTile, TileController secondTile)
    {
        yield return StartCoroutine(base.AdjacentSwapCoroutine(firstTile, secondTile));

        SwapRegisteredBoardsBetween(firstTile, secondTile);
    }

    void SwapRegisteredBoardsBetween(TileController firstTile, TileController secondTile)
    {
        TileBoardController firstTilesBoard = firstTile.Board;
        firstTile.Board = secondTile.Board;
        secondTile.Board = firstTilesBoard;
    }
}