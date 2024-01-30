using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
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
                Vector2 tempPosition = new Vector2(i, j);
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
                }
            }
            nullCount = 0;
        }

        yield return new WaitForSeconds(.4f);
    }

}
