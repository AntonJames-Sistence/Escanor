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

    private List<GameObject> IsRowExplosionSkill(Element element1, Element element2, Element element3)
    {
        // Create list of current elements
        List<GameObject> currentElements = new List<GameObject>();

        if (element1.isRowExplosion)
        {
            currentMatches.Union(GetRowElements(element1.row));
        }
        if (element2.isRowExplosion)
        {
            currentMatches.Union(GetRowElements(element2.row));
        }
        if (element3.isRowExplosion)
        {
            currentMatches.Union(GetRowElements(element3.row));
        }

        return currentElements;
    }

    private IEnumerator FindMatchesCo()
    {
        yield return new WaitForSeconds(.2f);

        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                GameObject currentElement = board.allElements[i, j];
                Element simplifiedCurrentElement = currentElement.GetComponent<Element>();

                if (currentElement != null)
                {
                    if (i > 0 && i < board.width - 1)
                    {
                        GameObject leftElement = board.allElements[i - 1, j];
                        Element simplifiedLeftElement = leftElement.GetComponent<Element>();
                        GameObject rightElement = board.allElements[i + 1, j];
                        Element simplifiedRightElement = rightElement.GetComponent<Element>();

                        if (leftElement && rightElement)
                        {
                            if (leftElement.tag == currentElement.tag && rightElement.tag == currentElement.tag)
                            {
                                // Logic for row explosion when horizontal match
                                currentMatches.Union(simplifiedLeftElement, simplifiedCurrentElement, simplifiedRightElement);

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
                        Element simplifiedUpElement = upElement.GetComponent<Element>();
                        GameObject downElement = board.allElements[i, j - 1];
                        Element simplifiedDownElement = downElement.GetComponent<Element>();

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
                                currentMatches.Union(IsRowExplosionSkill(simplifiedUpElement, currentElement, simplifiedDownElement));

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
    public void MatchAllSameElements(string elementsType)
    {
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                // Check if piece exists in board
                if (board.allElements[i, j] != null)
                {
                    // Check tag of the Element
                    if (board.allElements[i, j].tag == elementsType)
                    {
                        // Set this Element to be matched
                        board.allElements[i, j].GetComponent<Element>().isMatched = true;
                    }
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
