using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    public int column;
    public int row;
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
    }

    // Update is called once per frame
    void Update()
    {
        FindMatches();
        if (isMatched){
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            mySprite.color = new Color(0f, 0f, 0f, .25f);
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
        if(swipeAngle > -45 && swipeAngle <= 45 && column < board.width){
            //Swap right
            neighborElement = board.allElements[column + 1, row];
            neighborElement.GetComponent<Element>().column -= 1;
            column += 1;
        } else if(swipeAngle > 45 && swipeAngle <= 135 && row < board.height){
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
    }

    void FindMatches(){
        if (column > 0 && column < board.width - 1){
            GameObject leftElement1 = board.allElements[column - 1, row];
            GameObject rightElement1 = board.allElements[column + 1, row];

            if (leftElement1.tag == this.gameObject.tag && rightElement1.tag == this.gameObject.tag){
                leftElement1.GetComponent<Element>().isMatched = true;
                rightElement1.GetComponent<Element>().isMatched = true;
                isMatched = true;
            }
        }
    }
}
