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
                                // Check for row explosion
                                if (currentElement.GetComponent<Element>().isRowBomb
                                    || leftElement.GetComponent<Element>().isRowBomb
                                    || rightElement.GetComponent<Element>().isRowBomb)
                                    {
                                        currentMatches.Union(GetRowElements(j));
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

}
