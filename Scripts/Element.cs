using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    public int column;
    public int row;
    public int targetX;
    public int targetY;
    private Board board;
    private GameObject neighborElement;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
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
        targetX = column;
        targetY = row;
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
        if(swipeAngle > -45 && swipeAngle <= 45 && column < board.width)
        {
            //Swap right
            neighborElement = board.allElements[column + 1, row];
            neighborElement.GetComponent<Element>().column -= 1;
            column += 1;
        } else if(swipeAngle > 45 && swipeAngle <= 135 && row < board.height)
        {
            //Swap up
            neighborElement = board.allElements[column, row + 1];
            neighborElement.GetComponent<Element>().row -= 1;
            row += 1;
        } else if((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
        {
            //Swap left
            neighborElement = board.allElements[column + 1, row];
            neighborElement.GetComponent<Element>().column += 1;
            column -= 1;
        } else if(swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            //Swap down
            neighborElement = board.allElements[column, row - 1];
            neighborElement.GetComponent<Element>().row += 1;
            row -= 1;
        }
    }
}
