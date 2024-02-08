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