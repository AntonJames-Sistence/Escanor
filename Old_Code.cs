        // targetX = (int)transform.position.x;
        // targetY = (int)transform.position.y;
        // row = targetY;
        // column = targetX;
        // previousRow = row;
        // previousColumn = column;

// void FindMatches()
// {
//     if (column > 0 && column < board.width - 1){
//         GameObject leftElement1 = board.allElements[column - 1, row];
//         GameObject rightElement1 = board.allElements[column + 1, row];

//         if (leftElement1 != null && rightElement1 != null)
//         {
//             if (leftElement1.tag == this.gameObject.tag && rightElement1.tag == this.gameObject.tag){
//                 leftElement1.GetComponent<Element>().isMatched = true;
//                 rightElement1.GetComponent<Element>().isMatched = true;
//                 isMatched = true;
//             }
//         }
//     }

//     if (row > 0 && row < board.height - 1){
//         GameObject upElement1 = board.allElements[column, row + 1];
//         GameObject downElement1 = board.allElements[column, row - 1];

//         if (upElement1 != null && downElement1 != null)
//         {
//             if (upElement1.tag == this.gameObject.tag && downElement1.tag == this.gameObject.tag){
//                 upElement1.GetComponent<Element>().isMatched = true;
//                 downElement1.GetComponent<Element>().isMatched = true;
//                 isMatched = true;
//             }
//         }
//     }
// }

// Testing and debuggin purposes
// private void OnMouseOver()
// {
//     if (Input.GetMouseButtonDown(1))
//     {
//         isRowExplosion = true;
//         GameObject explosion = Instantiate(rowExplosionSkill, transform.position, Quaternion.identity);
//         explosion.transform.parent = this.transform;
//     }
// }

// FindMatches();

// if (isMatched){
//     SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
//     mySprite.color = new Color(1f, 1f, 1f, .25f);
// }

// int typeOfExplosion = Random.Range(0, 100);
// if (typeOfExplosion < 50)
// {
//     // Make rowExplosionSkill
//     board.currentElement.GenerateRowExplosionSkill();
// }
// else
// {
//     // Make columnExplosionSkill
//     board.currentElement.GenerateColumnExplosionSkill();
// }

// int typeOfExplosion = Random.Range(0, 100);
// if (typeOfExplosion < 50)
// {
//     // Make rowExplosionSkill
//     neighborElement.GenerateRowExplosionSkill();
// }
// else
// {
//     // Make columnExplosionSkill
//     neighborElement.GenerateColumnExplosionSkill();
// }
// }

                                // if (currentElement.GetComponent<Element>().isRowExplosion)
                                // {
                                //     currentMatches.Union(GetRowElements(j)); // Center piece
                                // }
                                // if (upElement.GetComponent<Element>().isRowExplosion)
                                // {
                                //     currentMatches.Union(GetRowElements(j + 1)); // Left piece
                                // }
                                // if (downElement.GetComponent<Element>().isRowExplosion)
                                // {
                                //     currentMatches.Union(GetRowElements(j - 1)); // Right piece
                                // }
                                // if (currentElement.GetComponent<Element>().isRowExplosion
                                //     || leftElement.GetComponent<Element>().isRowExplosion
                                //     || rightElement.GetComponent<Element>().isRowExplosion)
                                // {
                                //     currentMatches.Union(GetRowElements(j));
                                // }

                                // if (currentElement.GetComponent<Element>().isColumnExplosion)
                                // {
                                //     currentMatches.Union(GetColumnElements(i)); // Center piece
                                // }
                                // if (leftElement.GetComponent<Element>().isColumnExplosion)
                                // {
                                //     currentMatches.Union(GetColumnElements(i - 1)); // Left piece
                                // }
                                // if (rightElement.GetComponent<Element>().isColumnExplosion)
                                // {
                                //     currentMatches.Union(GetColumnElements(i + 1)); // Right piece
                                // }
                                // if (currentElement.GetComponent<Element>().isColumnExplosion
                                //     || upElement.GetComponent<Element>().isColumnExplosion
                                //     || downElement.GetComponent<Element>().isColumnExplosion)
                                // {
                                //     currentMatches.Union(GetColumnElements(i));
                                // }

                                // if (!currentMatches.Contains(leftElement))
                                // {
                                //     currentMatches.Add(leftElement);
                                // }
                                // leftElement.GetComponent<Element>().isMatched = true;

                                // if (!currentMatches.Contains(rightElement))
                                // {
                                //     currentMatches.Add(rightElement);
                                // }
                                // rightElement.GetComponent<Element>().isMatched = true;

                                // if (!currentMatches.Contains(currentElement))
                                // {
                                //     currentMatches.Add(currentElement);
                                // }
                                // currentElement.GetComponent<Element>().isMatched = true;

                                // if (!currentMatches.Contains(upElement))
                                // {
                                //     currentMatches.Add(upElement);
                                // }
                                // upElement.GetComponent<Element>().isMatched = true;

                                // if (!currentMatches.Contains(downElement))
                                // {
                                //     currentMatches.Add(downElement);
                                // }
                                // downElement.GetComponent<Element>().isMatched = true;

                                // if (!currentMatches.Contains(currentElement))
                                // {
                                //     currentMatches.Add(currentElement);
                                // }
                                // currentElement.GetComponent<Element>().isMatched = true;

            //Swap right
            // neighborElement = board.allElements[column + 1, row];
            // previousRow = row;
            // previousColumn = column;
            // neighborElement.GetComponent<Element>().column -= 1;
            // column += 1;
            // StartCoroutine(CheckMoveCo());
            // neighborElement = board.allElements[column, row + 1];
            // previousRow = row;
            // previousColumn = column;
            // neighborElement.GetComponent<Element>().row -= 1;
            // row += 1;
            // StartCoroutine(CheckMoveCo());
            //             neighborElement = board.allElements[column - 1, row];
            // previousRow = row;
            // previousColumn = column;
            // neighborElement.GetComponent<Element>().column += 1;
            // column -= 1;
            // StartCoroutine(CheckMoveCo());
            //             neighborElement = board.allElements[column, row - 1];
            // previousRow = row;
            // previousColumn = column;
            // neighborElement.GetComponent<Element>().row += 1;
            // row -= 1;
            // StartCoroutine(CheckMoveCo());

    // private IEnumerator DecreaseRowCo()
    // {
    //     yield return new WaitForSeconds(.5f);

    //     int nullCount = 0;
    //     for (int i = 0; i < width; i++)
    //     {
    //         for (int j = 0; j < height; j++)
    //         {
    //             if (allElements[i, j] == null)
    //             {
    //                 nullCount++;
    //             }
    //             else if (nullCount > 0)
    //             {
    //                 allElements[i, j].GetComponent<Element>().row -= nullCount;
    //                 allElements[i, j] = null;
    //             }
    //         }
    //         nullCount = 0;
    //     }

    //     yield return new WaitForSeconds(.5f);
    //     StartCoroutine(FillBoardCo());
    // }