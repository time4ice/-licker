using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SceneWithBox : MonoBehaviour {
    public GameObject box;
    public GameObject first;
    public GameObject second;
    public GameObject third;
    public GameObject fourth;
    public GameObject fifth;
    private bool check;

    public static Object[,] creaturesOnBoard = new Object[6, 6];
   
    int boxLevel;

    private IEnumerator boxDeliver;

    IEnumerator Deliver()
    {
        while (true)
        {
            
            creaturesOnBoard[0, 0] = new Box(boxLevel, creatureByLevel(boxLevel), box, randomPosition());
            yield return new WaitForSeconds(20);
        }
    }

    // Use this for initialization
    void Start()
    {
        //if (PlayerPrefs.HasKey("boxLevel"))
        // boxLevel = PlayerPrefs.GetInt("boxLevel");
        // else 
        boxLevel = 0;
        
        boxDeliver = Deliver();
        check = true;
        StartCoroutine(boxDeliver);
    }

        
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickWorldPos
                = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            returnObjByClick(clickWorldPos);
        }        
    }

    Vector2 randomPosition()
    {
        return new Vector2(Random.Range(-8, 8), Random.Range(-14, 6));
    }

    GameObject creatureByLevel(int level)
    {

        switch (level)
        { case 1:  return first;
            case 2: return second;
            case 3: return third;
            case 4: return fourth;
            default: return fifth;
        }
    }

    bool returnObjByClick(Vector2 clickPosition)
    {
        for(int i = 0; i < creaturesOnBoard.Length; i++) {
            for (int j = 0; j < creaturesOnBoard.Length; j++)
            {
                if (creaturesOnBoard[i,j] is Box)
                {
                    if ((creaturesOnBoard[i, j] as Box).position == clickPosition)
                    {
                        creaturesOnBoard[i, j]=(creaturesOnBoard[i, j] as Box).openBox();
                        return true;
                    }
                }
            }
            
        }
        return false;
    }

}
