using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    [Header("Board Variables")]
    public int column;
    public int row;
    public int previousColumn;
    public int previousRow;
    public int targetX;
    public int targetY;
    public bool isMatched = false;

    private Board board;
    private GameObject neighborElement;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 tempPosition;
    public float swipeAngle;

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        targetX = (int)transform.position.x;
        targetY = (int)transform.position.y;
        row = targetY;
        column = targetX;
        previousRow = row;
        previousColumn = column;
    }

    // Update is called once per frame
    void Update()
    {
        FindMatches();
        if (isMatched){
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            mySprite.color = new Color(1f, 1f, 1f, .25f);
        }

        targetX = column;
        targetY = row;
        // Moving logic for left & right
        if (Mathf.Abs(targetX - transform.position.x) > .1) { 
            // Move towards the target
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
        } else {
            // Directly set the position
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
            board.allElements[column, row] = this.gameObject;
        }

        // Moving logic for up & down
        if (Mathf.Abs(targetY - transform.position.y) > .1) { 
            // Move towards the target
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
        } else {
            // Directly set the position
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
            board.allElements[column, row] = this.gameObject;
        }
    }

    public IEnumerator CheckMoveCo() 
    {
        yield return new WaitForSeconds(.5f);

        if (neighborElement != null){
            if (!isMatched && !neighborElement.GetComponent<Element>().isMatched){
                neighborElement.GetComponent<Element>().row = row;
                neighborElement.GetComponent<Element>().column = column;
                row = previousRow;
                column = previousColumn;
            }
            neighborElement = null;
        }
    }

    private void OnMouseDown()
    {
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
    }

    void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
        SwapElements();
    }

    void SwapElements()
    {
        if(swipeAngle > -45 && swipeAngle <= 45 && column < board.width - 1){
            //Swap right
            neighborElement = board.allElements[column + 1, row];
            neighborElement.GetComponent<Element>().column -= 1;
            column += 1;
        } else if(swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1){
            //Swap up
            neighborElement = board.allElements[column, row + 1];
            neighborElement.GetComponent<Element>().row -= 1;
            row += 1;
        } else if((swipeAngle > 135 || swipeAngle <= -135) && column > 0){
            //Swap left
            neighborElement = board.allElements[column - 1, row];
            neighborElement.GetComponent<Element>().column += 1;
            column -= 1;
        } else if(swipeAngle < -45 && swipeAngle >= -135 && row > 0){
            //Swap down
            neighborElement = board.allElements[column, row - 1];
            neighborElement.GetComponent<Element>().row += 1;
            row -= 1;
        }
        StartCoroutine(CheckMoveCo());
    }

    void FindMatches()
    {
        if (column > 0 && column < board.width - 1){
            GameObject leftElement1 = board.allElements[column - 1, row];
            GameObject rightElement1 = board.allElements[column + 1, row];

            if (leftElement1.tag == this.gameObject.tag && rightElement1.tag == this.gameObject.tag){
                leftElement1.GetComponent<Element>().isMatched = true;
                rightElement1.GetComponent<Element>().isMatched = true;
                isMatched = true;
            }
        }

        if (row > 0 && row < board.height - 1){
            GameObject upElement1 = board.allElements[column, row + 1];
            GameObject downElement1 = board.allElements[column, row - 1];

            if (upElement1.tag == this.gameObject.tag && downElement1.tag == this.gameObject.tag){
                upElement1.GetComponent<Element>().isMatched = true;
                downElement1.GetComponent<Element>().isMatched = true;
                isMatched = true;
            }
        }
    }
}
