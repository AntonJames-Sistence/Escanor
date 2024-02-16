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