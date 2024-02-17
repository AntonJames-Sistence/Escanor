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

    private List<GameObject> IsRowExplosion(Element element1, Element element2, Element element3)
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

    private List<GameObject> IsColumnExplosion(Element element1, Element element2, Element element3)
    {
        // Create list of current elements
        List<GameObject> currentElements = new List<GameObject>();

        if (element1.isColumnExplosion)
        {
            currentMatches.Union(GetColumnElements(element1.column));
        }
        if (element2.isColumnExplosion)
        {
            currentMatches.Union(GetColumnElements(element2.column));
        }
        if (element3.isColumnExplosion)
        {
            currentMatches.Union(GetColumnElements(element3.column));
        }

        return currentElements;
    }

    private void AddToListAndMatch(GameObject element)
    {
        if (!currentMatches.Contains(element))
        {
            currentMatches.Add(element);
        }
        element.GetComponent<Element>().isMatched = true;
    }

    private void GetNearbyElements(GameObject element1, GameObject element2, GameObject element3)
    {
        AddToListAndMatch(element1);
        AddToListAndMatch(element2);
        AddToListAndMatch(element3);
    }

    private IEnumerator FindMatchesCo()
    {
        yield return new WaitForSeconds(.4f);

        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                GameObject currentElement = board.allElements[i, j];

                if (currentElement != null)
                {
                    // Refference to current element, make future refference easier
                    Element simplifiedCurrentElement = currentElement.GetComponent<Element>();
                    if (i > 0 && i < board.width - 1)
                    {
                        GameObject leftElement = board.allElements[i - 1, j];
                        GameObject rightElement = board.allElements[i + 1, j];

                        if (leftElement != null && rightElement != null)
                        {
                            // Make sure elements exist before creating refference
                            Element simplifiedLeftElement = leftElement.GetComponent<Element>();
                            Element simplifiedRightElement = rightElement.GetComponent<Element>();

                            if (leftElement.tag == currentElement.tag && rightElement.tag == currentElement.tag)
                            {
                                // Logic for row explosion when horizontal match
                                currentMatches.Union(IsRowExplosion(simplifiedLeftElement, simplifiedCurrentElement, simplifiedRightElement));

                                // Logic for column explosion when there's vertical match
                                currentMatches.Union(IsColumnExplosion(simplifiedLeftElement, simplifiedCurrentElement, simplifiedRightElement));

                                GetNearbyElements(leftElement, currentElement, rightElement);
                            }
                        }
                    }

                    if (j > 0 && j < board.height - 1)
                    {
                        GameObject upElement = board.allElements[i, j + 1];
                        GameObject downElement = board.allElements[i, j - 1];

                        if (upElement != null && downElement != null)
                        {
                            // Make sure elements exists before creating refference
                            Element simplifiedUpElement = upElement.GetComponent<Element>();
                            Element simplifiedDownElement = downElement.GetComponent<Element>();

                            if (upElement.tag == currentElement.tag && downElement.tag == currentElement.tag)
                            {
                                // Check for column explosion
                                currentMatches.Union(IsColumnExplosion(simplifiedUpElement, simplifiedCurrentElement, simplifiedDownElement));

                                // Logic for row explosion when there's horizontal match
                                currentMatches.Union(IsRowExplosion(simplifiedUpElement, simplifiedCurrentElement, simplifiedDownElement));

                                GetNearbyElements(upElement, currentElement, downElement);
                            }
                        }
                    }
                }
            }
        }
    }

    // Match all Elements of the same type
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

    // Helper function for adjacent pieces
    List<GameObject> GetAdjacentElements(int column, int row)
    {
        List<GameObject> elements = new List<GameObject>();

        // Iterate double loop, but only col-1 -> col+1 / row - 1 -> row + 1 from the element
        for (int i = column - 1; i <= column + 1; i++)
        {
            for (int j = row - 1; j <= row + 1; j++)
            {
                // Check if index is valid (inside the board)
                if (i >= 0 && i < board.width && j >= 0 && j < board.height)
                {
                    // add element to array
                    elements.Add(board.allElements[i, j]);
                    board.allElements[i, j].GetComponent<Element>().isMatched = true;
                }
            }
        }

        return elements;
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
