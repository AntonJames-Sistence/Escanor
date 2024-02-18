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
    public GameObject neighborElement;


    private FindMatches findMatches;
    private Board board;
    private Vector2 firstTouchPosition;
    private Vector2 lastTouchPosition;
    private Vector2 tempPosition;

    [Header("Swipe Variables")]
    public float swipeAngle;
    public float swipeResistance = .7f;

    [Header("Powerup Variables")]
    public bool isSameElementExplosion;
    public bool isColumnExplosion;
    public bool isRowExplosion;
    public bool isCircleExplosion;
    public GameObject columnExplosionSkill;
    public GameObject rowExplosionSkill;
    public GameObject sameElementExplosionSkill;
    public GameObject circleExplosionSkill;

    // Start is called before the first frame update
    void Start()
    {
        isColumnExplosion = false;
        isRowExplosion = false;
        isCircleExplosion = false;
        isSameElementExplosion = false;

        board = FindObjectOfType<Board>();
        findMatches = FindObjectOfType<FindMatches>();
        // targetX = (int)transform.position.x;
        // targetY = (int)transform.position.y;
        // row = targetY;
        // column = targetX;
        // previousRow = row;
        // previousColumn = column;
    }

    // Testing and debuggin purposes
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isCircleExplosion = true;
            GameObject explosion = Instantiate(circleExplosionSkill, transform.position, Quaternion.identity);
            explosion.transform.parent = this.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        targetX = column;
        targetY = row;
        // Moving logic for left & right
        if (Mathf.Abs(targetX - transform.position.x) > .1) 
        { 
            // Move towards the target
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .3f);
            if (board.allElements[column, row] != this.gameObject)
            {
                board.allElements[column, row] = this.gameObject;
            }

            findMatches.FindAllMatches();
        } 
        else 
        {
            // Directly set the position
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
        }

        // Moving logic for up & down
        if (Mathf.Abs(targetY - transform.position.y) > .1) 
        { 
            // Move towards the target
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .3f);
            if (board.allElements[column, row] != this.gameObject)
            {
                board.allElements[column, row] = this.gameObject;
            }

            findMatches.FindAllMatches();
        } 
        else 
        {
            // Directly set the position
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
        }
    }

    public IEnumerator CheckMoveCo() 
    {
        if (isSameElementExplosion)
        {
            // This piece is "Same Element Explosion" and the swapping element is the element to destroy
            findMatches.MatchAllSameElements(neighborElement.tag);
            isMatched = true;
        }
        else if (neighborElement.GetComponent<Element>().isSameElementExplosion)
        {
            // The swapping element is "Same Element Explosion" and this element is the element to destroy
            findMatches.MatchAllSameElements(this.gameObject.tag);
            neighborElement.GetComponent<Element>().isMatched = true;
        }

        yield return new WaitForSeconds(.5f);

        if (neighborElement != null)
        {
            if (!isMatched && !neighborElement.GetComponent<Element>().isMatched)
            {
                neighborElement.GetComponent<Element>().row = row;
                neighborElement.GetComponent<Element>().column = column;
                row = previousRow;
                column = previousColumn;
                yield return new WaitForSeconds(.3f);
                board.currentElement = null;
                board.currentState = GameState.move;
            }
            else
            {
                board.DestroyMatches();
            }
            // neighborElement = null;
        }
    }

    private void OnMouseDown()
    {
        if (board.currentState == GameState.move)
        {
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void OnMouseUp()
    {
        if (board.currentState == GameState.move)
        {
            lastTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
            // Freeze interaction state
            board.currentState = GameState.wait; 
        }
        else
        {
            // Unfreeze interaction state
            board.currentState = GameState.move; 
        }
    }

    void CalculateAngle()
    {
        if (Mathf.Abs(lastTouchPosition.y - firstTouchPosition.y) > swipeResistance || 
            Mathf.Abs(lastTouchPosition.x - firstTouchPosition.x) > swipeResistance)
        {
            swipeAngle = Mathf.Atan2(lastTouchPosition.y - firstTouchPosition.y, lastTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
            SwapElements();
            board.currentElement = this;
        }
    }

    void SwapElements()
    {
        if(swipeAngle > -45 && swipeAngle <= 45 && column < board.width - 1){
            //Swap right
            neighborElement = board.allElements[column + 1, row];
            previousRow = row;
            previousColumn = column;
            neighborElement.GetComponent<Element>().column -= 1;
            column += 1;
        } else if(swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1){
            //Swap up
            neighborElement = board.allElements[column, row + 1];
            previousRow = row;
            previousColumn = column;
            neighborElement.GetComponent<Element>().row -= 1;
            row += 1;
        } else if((swipeAngle > 135 || swipeAngle <= -135) && column > 0){
            //Swap left
            neighborElement = board.allElements[column - 1, row];
            previousRow = row;
            previousColumn = column;
            neighborElement.GetComponent<Element>().column += 1;
            column -= 1;
        } else if(swipeAngle < -45 && swipeAngle >= -135 && row > 0){
            //Swap down
            neighborElement = board.allElements[column, row - 1];
            previousRow = row;
            previousColumn = column;
            neighborElement.GetComponent<Element>().row += 1;
            row -= 1;
        }
        StartCoroutine(CheckMoveCo());
    }

    public void GenerateRowExplosionSkill()
    {
        isRowExplosion = true;
        GameObject skill = Instantiate(rowExplosionSkill, transform.position, Quaternion.identity);
        skill.transform.parent = this.transform;
    }

    public void GenerateColumnExplosionSkill()
    {
        isColumnExplosion = true;
        GameObject skill = Instantiate(columnExplosionSkill, transform.position, Quaternion.identity);
        skill.transform.parent = this.transform;
    }

    public void GenerateSameElementExplosionSkill()
    {
        isSameElementExplosion = true;
        GameObject skill = Instantiate(sameElementExplosionSkill, transform.position, Quaternion.identity);
        skill.transform.parent = this.transform;
    }

    public void GenerateCircleExplosionSkill()
    {
        isCircleExplosion = true;
        GameObject skill = Instantiate(circleExplosionSkill, transform.position, Quaternion.identity);
        skill.transform.parent = this.transform;
    }

}
