using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    wait,
    move
}

public class Board : MonoBehaviour
{
    public GameState currentState = GameState.move;
    public int width;
    public int height;
    public int offSet;
    public GameObject tilePrefab;
    public GameObject[] elements;
    private BackgroundTile[,] allTiles;
    public GameObject[,] allElements;

    // Start is called before the first frame update
    void Start()
    {
        allTiles = new BackgroundTile[width, height];
        allElements = new GameObject[width, height];
        SetUp();
    }

    private void SetUp()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
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
                else
                {
                    Debug.LogError("No elements assigned to the BackgroundTile.");
                }
            }
        }
    }

    private bool MatchesAt(int column, int row, GameObject piece)
    {
        if (column > 1 && row > 1)
        {
            if (allElements[column - 1, row].tag == piece.tag &&
                allElements[column - 2, row].tag == piece.tag)
                {
                    return true;
                }

            if (allElements[column, row - 1].tag == piece.tag &&
                allElements[column, row - 2].tag == piece.tag)
                {
                    return true;
                }
        }
        else if (column <= 1 || row <= 1)
        {
            if (row > 1)
            {
                if (allElements[column, row - 1].tag == piece.tag &&
                    allElements[column, row - 2].tag == piece.tag)
                    {
                        return true;
                    }
            }
            if (column > 1)
            {
                if (allElements[column - 1, row].tag == piece.tag &&
                    allElements[column - 2, row].tag == piece.tag)
                    {
                        return true;
                    }
            }
        }

        return false;
    }

    private void DestroyMatchesAt(int column, int row)
    {
        if (allElements[column, row].GetComponent<Element>().isMatched)
        {
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

        StartCoroutine(DecreaseRowCo());
    }

    private IEnumerator DecreaseRowCo()
    {
        int nullCount = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allElements[i, j] == null)
                {
                    nullCount++;
                }
                else if (nullCount > 0)
                {
                    allElements[i, j].GetComponent<Element>().row -= nullCount;
                    allElements[i, j] = null;
                }
            }
            nullCount = 0;
        }

        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoardCo());
    }

    private void RefillBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allElements[i, j] == null) // has additional hasBeenUsed?
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
        yield return new WaitForSeconds(.5f);
        currentState = GameState.move;
    }

}
