﻿using System.Collections;
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
    public GameObject empty;
    private bool check;

    public static Object[] creaturesOnBoard = new Object[20];
    
    public static List<int> mergeLst = new List<int>();
    int boxLevel;
    private IEnumerator boxDeliver;

    void Start()
    {
        boxLevel = 1;
        boxDeliver = Deliver();
        check = true;
        StartCoroutine(boxDeliver);
    }        
	
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickWorldPos
                = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            print("clickPos" + clickWorldPos);
            for (int i = 0; i < creaturesOnBoard.Length; i++)
            {
                if (creaturesOnBoard[i] is Box)
                {
                    ReturnObjByClick(i, clickWorldPos);
                }
                else if (creaturesOnBoard[i] is Creature)
                {
                    CreatureFunc(i, clickWorldPos);
                }

            }
        }        
    }


    GameObject creatureByLevel(int level)
    {
        switch (level)
        {
            case 1:  return first;
            case 2: return second;
            case 3: return third;
            case 4: return fourth;
            default: return fifth;
        }
    }
    bool CreatureFunc(int i, Vector2 clickPos)
    {
        Creature creature = creaturesOnBoard[i] as Creature;
        if (creature.findObjOnBoard(clickPos))
        {
           if(mergeLst.Count==0||mergeLst[0]!=i) mergeLst.Add(i);
        }
        if (mergeLst.Count == 2 )
        {
            if ((creaturesOnBoard[mergeLst[0]] as Creature).level ==
                (creaturesOnBoard[mergeLst[1]] as Creature).level)
            {
                Creature creature1 = creaturesOnBoard[mergeLst[0]] as Creature;
                Creature creature2 = creaturesOnBoard[mergeLst[1]] as Creature;
                creaturesOnBoard[mergeLst[0]] =
                    new Creature(creature1.level + 1, creature2.position, creatureByLevel(creature1.level+1));
                creaturesOnBoard[mergeLst[1]] = null;
                Destroy(creature1.obj);
                Destroy(creature2.obj);
            }
            mergeLst.Clear();
            return true;
        }
        return false;
    }
    bool ReturnObjByClick(int i,Vector2 clickPosition)
    {
        Box box = creaturesOnBoard[i] as Box;
        if (box.findObjOnBoard(clickPosition))
        {
            print("well done");
            creaturesOnBoard[i] = box.openBox() as Creature;
            Destroy(box.obj);
            return true;
        }
        return false;
    }
    bool Merging(int i, Vector2 clickPosition)
    {        
        return false;
    }
    Vector2 RandomPosition()
    {
        
        while (true)
        {
            bool has = false;
            Vector2 currposition = new Vector2(empty.transform.position.x + Random.Range(0, 5) * 4,
                                               empty.transform.position.y - Random.Range(0, 5) * 4);
            foreach(Object element in creaturesOnBoard)
            {
                if(element is Box)
                {
                    if ((element as Box).position == currposition) { has= true; break; }
                }
                if (element is Creature)
                {
                    if ((element as Creature).position==currposition) { has = true; break; }
                }
                
            }
            if(has==false) return currposition;
        }
    }

    IEnumerator Deliver()
    {
        while (true)
        {
            for (int i = 0; i < creaturesOnBoard.Length; i++)
            {
                int randomLevel = Random.Range(1, boxLevel);//проблема. в цикле создается до 25 . то есть если дошло до 25, но ячейки заполнені не все, фиаско
                if (!(creaturesOnBoard[i] is Box || creaturesOnBoard[i] is Creature))
                {
                    creaturesOnBoard[i] = new Box(randomLevel, creatureByLevel(randomLevel), box, RandomPosition());
                    break;
                }
                else print("full mass");
                
            }
            yield return new WaitForSeconds(1);
        }
    }
}
