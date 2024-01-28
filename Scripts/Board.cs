using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject tilePrefab;
    public GameObject[] elements;
    private BackgroundTile[,] allTiles;

    // Start is called before the first frame update
    void Start()
    {
        allTiles = new BackgroundTile[width, height];
        SetUp();
    }

    private void SetUp()
    {
        for (int i = 0; i < width; i++){
            for (int j = 0; j < height; j++){
                Vector2 tempPosition = new Vector2(i, j);
                GameObject backgroundTile = Instantiate(tilePrefab, tempPosition, Quaternion.identity) as GameObject;
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "( " + i + ", " + j + " )";

                if (elements.Length > 0)
                {
                    int elementToUse = Random.Range(0, elements.Length);
                    GameObject element = Instantiate(elements[elementToUse], tempPosition, Quaternion.identity);
                    element.transform.parent = this.transform;
                    element.name = "( " + i + ", " + j + " )";
                }
                else
                {
                    Debug.LogError("No elements assigned to the BackgroundTile.");
                }
            }
        }
    }
}
