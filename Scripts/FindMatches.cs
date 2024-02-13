using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FindMatches : MonoBehaviour
{
    private Board board;
    public List<GameObject> currentMatches = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
    }

    public void FindAllMatches()
    {
        StartCoroutine(FindMatchesCo());
    }

    private IEnumerator FindMatchesCo()
    {
        yield return new WaitForSeconds(.2f);

        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                GameObject currentElement = board.allElements[i, j];

                if (currentElement != null)
                {
                    if (i > 0 && i < board.width - 1)
                    {
                        GameObject leftElement = board.allElements[i - 1, j];
                        GameObject rightElement = board.allElements[i + 1, j];

                        if (leftElement && rightElement)
                        {
                            if (leftElement.tag == currentElement.tag && rightElement.tag == currentElement.tag)
                            {
                                // Logic for row explosion when horizontal match
                                if (currentElement.GetComponent<Element>().isRowExplosion
                                    || leftElement.GetComponent<Element>().isRowExplosion
                                    || rightElement.GetComponent<Element>().isRowExplosion)
                                {
                                    currentMatches.Union(GetRowElements(j));
                                }
                                // Logic for column explosion when there's vertical match
                                if (currentElement.GetComponent<Element>().isColumnExplosion)
                                {
                                    currentMatches.Union(GetColumnElements(i)); // Center piece
                                }
                                if (leftElement.GetComponent<Element>().isColumnExplosion)
                                {
                                    currentMatches.Union(GetColumnElements(i - 1)); // Left piece
                                }
                                if (rightElement.GetComponent<Element>().isColumnExplosion)
                                {
                                    currentMatches.Union(GetColumnElements(i + 1)); // Right piece
                                }

                                if (!currentMatches.Contains(leftElement))
                                {
                                    currentMatches.Add(leftElement);
                                }
                                leftElement.GetComponent<Element>().isMatched = true;

                                if (!currentMatches.Contains(rightElement))
                                {
                                    currentMatches.Add(rightElement);
                                }
                                rightElement.GetComponent<Element>().isMatched = true;

                                if (!currentMatches.Contains(currentElement))
                                {
                                    currentMatches.Add(currentElement);
                                }
                                currentElement.GetComponent<Element>().isMatched = true;
                            }
                        }
                    }

                    if (j > 0 && j < board.height - 1)
                    {
                        GameObject upElement = board.allElements[i, j + 1];
                        GameObject downElement = board.allElements[i, j - 1];

                        if (upElement && downElement)
                        {
                            if (upElement.tag == currentElement.tag && downElement.tag == currentElement.tag)
                            {
                                // Check for column explosion
                                if (currentElement.GetComponent<Element>().isColumnExplosion
                                    || upElement.GetComponent<Element>().isColumnExplosion
                                    || downElement.GetComponent<Element>().isColumnExplosion)
                                {
                                    currentMatches.Union(GetColumnElements(i));
                                }
                                // Logic for row explosion when there's horizontal match
                                if (currentElement.GetComponent<Element>().isRowExplosion)
                                {
                                    currentMatches.Union(GetRowElements(j)); // Center piece
                                }
                                if (upElement.GetComponent<Element>().isRowExplosion)
                                {
                                    currentMatches.Union(GetRowElements(j + 1)); // Left piece
                                }
                                if (downElement.GetComponent<Element>().isRowExplosion)
                                {
                                    currentMatches.Union(GetRowElements(j - 1)); // Right piece
                                }

                                if (!currentMatches.Contains(upElement))
                                {
                                    currentMatches.Add(upElement);
                                }
                                upElement.GetComponent<Element>().isMatched = true;

                                if (!currentMatches.Contains(downElement))
                                {
                                    currentMatches.Add(downElement);
                                }
                                downElement.GetComponent<Element>().isMatched = true;

                                if (!currentMatches.Contains(currentElement))
                                {
                                    currentMatches.Add(currentElement);
                                }
                                currentElement.GetComponent<Element>().isMatched = true;
                            }
                        }
                    }
                }
            }
        }
    }

    // Get all Elements same type
    public void MatchAllSameElements(string type)
    {
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                // Check if piece exists in board
                if (board.allElements[i, j] != null)
                {
                    if (board.allElements)
                }
            }
        }
    }

    // Helper function to grab board column Elements
    List<GameObject> GetColumnElements(int column)
    {
        List<GameObject> elements = new List<GameObject>();

        for (int i = 0; i < board.height; i++)
        {
            if (board.allElements[column, i] != null)
            {
                elements.Add(board.allElements[column, i]);
                board.allElements[column, i].GetComponent<Element>().isMatched = true;
            }
        }

        return elements;
    }
    
    // Helper function to grab board row Elements
    List<GameObject> GetRowElements(int row)
    {
        List<GameObject> elements = new List<GameObject>();

        for (int i = 0; i < board.width; i++)
        {
            if (board.allElements[i, row] != null)
            {
                elements.Add(board.allElements[i, row]);
                board.allElements[i, row].GetComponent<Element>().isMatched = true;
            }
        }

        return elements;
    }

    public void CheckSkills()
    {
        // Did player move something?
        if (board.currentElement != null)
        {
            // Is moved element a match?
            if (board.currentElement.isMatched)
            {
                // Make it unmatched
                board.currentElement.isMatched = false;
                // Decide what kind of explosion skill should be generated
                if ((board.currentElement.swipeAngle > -45 && board.currentElement.swipeAngle <= 45)
                || (board.currentElement.swipeAngle < -135 || board.currentElement.swipeAngle >= 135))
                {
                    // If left/right swipe, generate rowExplosionSkill
                    board.currentElement.GenerateRowExplosionSkill();
                }
                else
                {
                    // Else up/down swipe, generate columnExplosionSkill
                    board.currentElement.GenerateColumnExplosionSkill();
                }
            }
            // Is neighbor element a match?
            else if (board.currentElement.neighborElement != null)
            {
                // Grab the neighbor element
                Element neighborElement = board.currentElement.neighborElement.GetComponent<Element>();
                // Is neighbor element a match?
                if (neighborElement.isMatched)
                {
                    // Unmatch neighbor element
                    neighborElement.isMatched = false;
                    // Decide what kind of explosion skill should be generated
                    if ((board.currentElement.swipeAngle > -45 && board.currentElement.swipeAngle <= 45)
                    || (board.currentElement.swipeAngle < -135 || board.currentElement.swipeAngle >= 135))
                    {
                        // If left/right swipe, generate rowExplosionSkill
                        neighborElement.GenerateRowExplosionSkill();
                    }
                    else
                    {
                        // Else up/down swipe, generate columnExplosionSkill
                        neighborElement.GenerateColumnExplosionSkill();
                    }
                }
            }

        }
    }

}
