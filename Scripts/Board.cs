using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    wait,
    move
}

public enum TileKind
{
    Breakable,
    Blank,
    Normal,
}

[System.Serializable]
public class TileType
{
    public int x;
    public int y;
    public TileKind tileKind;

}

public class Board : MonoBehaviour
{
    public GameState currentState = GameState.move;
    public int width;
    public int height;
    public int offSet;
    public GameObject tilePrefab;
    public GameObject breakableTilePrefab;
    public GameObject[] elements;
    public GameObject[,] allElements;
    public GameObject destroyEffect;
    public TileType[] boardLayout;
    public Element currentElement;

    private bool[,] blankSpaces;
    private BackgroundTile[,] breakableTiles;
    private FindMatches findMatches;

    // Start is called before the first frame update
    void Start()
    {
        breakableTiles = new BackgroundTile[width, height];
        findMatches = FindObjectOfType<FindMatches>();
        blankSpaces = new bool[width, height];
        allElements = new GameObject[width, height];
        SetUp();
    }

    public void GenerateBlackSpaces()
    {
        for (int i = 0; i < boardLayout.Length; i++)
        {
            if (boardLayout[i].tileKind == TileKind.Blank)
            {
                blankSpaces[boardLayout[i].x, boardLayout[i].y] = true;
            }
        }
    }

    public void GenerateBreakableTiles()
    {
        // Look at all tiles in the layout
        for (int i = 0; i < boardLayout.Length; i++)
        {
            // If tile is "ice" tile, 
            if (boardLayout[i].tileKind == TileKind.Breakable)
            {
                // Create "Ice" tile object
                Vector2 tempPosition = new Vector2(boardLayout[i].x, boardLayout[i].y);
                GameObject tile = Instantiate(breakableTilePrefab, tempPosition, Quaternion.identity);
                // puts ice piece into array
                breakableTiles[boardLayout[i].x, boardLayout[i].y] = tile.GetComponent<BackgroundTile>();
            }
        }
    }

    private void SetUp()
    {
        GenerateBreakableTiles();
        GenerateBlackSpaces();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (blankSpaces[i, j] == false)
                {
                    Vector2 tempPosition = new Vector2(i, j + offSet);
                    GameObject backgroundTile = Instantiate(tilePrefab, tempPosition, Quaternion.identity) as GameObject;
                    backgroundTile.transform.parent = this.transform;
                    backgroundTile.name = "( " + i + ", " + j + " )";

                    if (elements.Length > 0)
                    {
                        int elementToUse = Random.Range(0, elements.Length);

                        int maxIterations = 0;
                        while (MatchesAt(i, j, elements[elementToUse]) && maxIterations < 100)
                        {
                            elementToUse = Random.Range(0, elements.Length);
                            maxIterations++;
                        }
                        maxIterations = 0;

                        GameObject element = Instantiate(elements[elementToUse], tempPosition, Quaternion.identity);
                        element.GetComponent<Element>().row = j;
                        element.GetComponent<Element>().column = i;
                        element.transform.parent = this.transform;
                        element.name = "( " + i + ", " + j + " )";
                        allElements[i, j] = element;
                    }
                }
            }
        }
    }

    private bool MatchesAt(int column, int row, GameObject piece)
    {
        if (column > 1 && row > 1)
        {
            if (allElements[column - 1, row] != null && allElements[column - 2, row] != null)
            {
                if (allElements[column - 1, row].tag == piece.tag &&
                    allElements[column - 2, row].tag == piece.tag)
                {
                    return true;
                }
            }

            if (allElements[column, row - 1] != null && allElements[column, row - 2] != null)
            {
                if (allElements[column, row - 1].tag == piece.tag &&
                    allElements[column, row - 2].tag == piece.tag)
                {
                    return true;
                }
            }
        }
        else if (column <= 1 || row <= 1)
        {
            if (row > 1)
            {
                if (allElements[column, row - 1] != null && allElements[column, row - 2] != null)
                {
                    if (allElements[column, row - 1].tag == piece.tag &&
                        allElements[column, row - 2].tag == piece.tag)
                    {
                        return true;
                    }
                }
            }

            if (column > 1)
            {
                if (allElements[column - 1, row] != null && allElements[column - 2, row] != null)
                {
                    if (allElements[column - 1, row].tag == piece.tag &&
                        allElements[column - 2, row].tag == piece.tag)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private bool ColumnOrRow()
    {
        int numberHorizontal = 0;
        int numberVertical = 0;

        Element firstPiece = findMatches.currentMatches[0].GetComponent<Element>();

        if (firstPiece != null)
        {
            foreach(GameObject currentPiece in findMatches.currentMatches)
            {
                Element element = currentPiece.GetComponent<Element>();
                if (element.row == firstPiece.row)
                {
                    numberHorizontal++;
                }
                if (element.column == firstPiece.column)
                {
                    numberVertical++;
                }
            }
        }
        
        return (numberVertical == 5 || numberHorizontal == 5);
    }

    private void CheckToGenerateSkills()
    {
        if (findMatches.currentMatches.Count == 4 || findMatches.currentMatches.Count == 7)
        {
            findMatches.CheckSkills();
        }

        if (findMatches.currentMatches.Count == 5 || findMatches.currentMatches.Count == 8)
        {
            if (ColumnOrRow())
            {
                // Make same explosion skill
                // If current element is matched we want to unmatch it and add SameExplosionSkill to it
                if (currentElement != null)
                {
                    if (currentElement.isMatched)
                    {
                        if (!currentElement.isSameElementExplosion) // If element is not skill holder
                        {
                            currentElement.isMatched = false;
                            currentElement.GenerateSameElementExplosionSkill();
                        }
                    } else {
                        // Same run but for neighbor element
                        if (currentElement.neighborElement != null)
                        {
                            Element neighborElement = currentElement.neighborElement.GetComponent<Element>();
                            if (neighborElement.isMatched)
                                if (!neighborElement.isSameElementExplosion)
                                {
                                    neighborElement.isMatched = false;
                                    neighborElement.GenerateSameElementExplosionSkill();
                                }
                        }
                    }
                }
            } else {
                // Make circle explosion skill
                // If current element is matched we want to unmatch it and add CircleExplosionSkill to it
                if (currentElement != null)
                {
                    if (currentElement.isMatched)
                    {
                        if (!currentElement.isCircleExplosion) // If element is not skill holder
                        {
                            currentElement.isMatched = false;
                            currentElement.GenerateCircleExplosionSkill();
                        }
                    } else {
                        // Same run but for neighbor element
                        if (currentElement.neighborElement != null)
                        {
                            Element neighborElement = currentElement.neighborElement.GetComponent<Element>();
                            if (neighborElement.isMatched)
                                if (!neighborElement.isCircleExplosion)
                                {
                                    neighborElement.isMatched = false;
                                    neighborElement.GenerateCircleExplosionSkill();
                                }
                        }
                    }
                }
            }
        }
    }

    private void DestroyMatchesAt(int column, int row)
    {
        if (allElements[column, row].GetComponent<Element>().isMatched)
        {
            // Count ammount of Elements that match
            if (findMatches.currentMatches.Count >= 4)
            {
                CheckToGenerateSkills();
            }

            // Does this tile need to break?
            if (breakableTiles[column, row] != null)
            {
                // if it does, give one damage
                breakableTiles[column, row].TakeDamage(1);
                // Debug.Log("Tile: " + breakableTiles[column, row].hitPoints +  "HP left:");
            }

            GameObject destroyAnimation = Instantiate(destroyEffect, allElements[column, row].transform.position, Quaternion.identity);
            Destroy(destroyAnimation, .5f);
            Destroy(allElements[column, row]);
            allElements[column, row] = null;
        }
    }

    public void DestroyMatches()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allElements[i, j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }

        findMatches.currentMatches.Clear();
        StartCoroutine(DecreaseRowCo2());
    }

    private IEnumerator DecreaseRowCo2()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                // If current spot is not blanc and is empty
                if (!blankSpaces[i, j] && allElements[i, j] == null)
                {
                    // loop from space above to the top of the column
                    for (int k = j + 1; k < height; k++)
                    {
                        // If element is found
                        if (allElements[i, k] != null)
                        {
                            // move element to that empty space
                            allElements[i, k].GetComponent<Element>().row = j;
                            // set neighbour to null
                            allElements[i, k] = null;
                            // break out of the loop
                            break;
                        }
                    }
                }
            }
        }
        yield return new WaitForSeconds(.6f);
        StartCoroutine(FillBoardCo());
    }


    private void RefillBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allElements[i, j] == null && !blankSpaces[i, j])
                {
                    Vector2 tempPosition = new Vector2(i, j + offSet);
                    int elementToUse = Random.Range(0, elements.Length);
                    GameObject piece = Instantiate(elements[elementToUse], tempPosition, Quaternion.identity);
                    allElements[i, j] = piece;
                    piece.GetComponent<Element>().row = j;
                    piece.GetComponent<Element>().column = i;

                }
            }
        }
    }

    private bool MatchesOnBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allElements[i, j] != null)
                {
                    if (allElements[i, j].GetComponent<Element>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private IEnumerator FillBoardCo()
    {
        RefillBoard();
        yield return new WaitForSeconds(.5f);

        while (MatchesOnBoard())
        {
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }

        // Clear list of matches to avoid false matches
        findMatches.currentMatches.Clear();
        // Clear currentElement to avoid bugs
        currentElement = null;
        yield return new WaitForSeconds(.5f);
        currentState = GameState.move;
    }

}
