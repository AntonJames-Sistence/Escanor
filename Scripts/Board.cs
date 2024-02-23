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
    public GameObject[] elements;
    public GameObject[,] allElements;
    public GameObject destroyEffect;
    public TileType[] boardLayout;
    public Element currentElement;

    private BackgroundTile[,] allTiles;
    private FindMatches findMatches;

    // Start is called before the first frame update
    void Start()
    {
        findMatches = FindObjectOfType<FindMatches>();
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
        StartCoroutine(DecreaseRowCo());
    }

    private IEnumerator DecreaseRowCo()
    {
        yield return new WaitForSeconds(.5f);

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

        yield return new WaitForSeconds(.5f);
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
        yield return new WaitForSeconds(.3f);

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
