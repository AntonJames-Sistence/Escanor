using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTile : MonoBehaviour
{
    public GameObject[] elements;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {
        if (elements.Length > 0)
        {
            int elementToUse = Random.Range(0, elements.Length);
            GameObject element = Instantiate(elements[elementToUse], transform.position, Quaternion.identity);
            element.transform.parent = this.transform;
            element.name = this.gameObject.name;
        }
        else
        {
            Debug.LogError("No elements assigned to the BackgroundTile.");
        }
    }
}
