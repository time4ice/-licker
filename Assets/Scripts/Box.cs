using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public Vector2 position;
    GameObject creature;
    public GameObject obj;
    int creatureLevel;
     public Box(int creatureLevel, GameObject creature, GameObject box, Vector2 position)
    {
        this.creatureLevel = creatureLevel;
        this.creature = creature;
        this.position = position;
        obj = Instantiate(box, this.position, Quaternion.identity) as GameObject;
        print("box "+this.position);
    }

    public Creature openBox()
    {
        return new Creature(creatureLevel, position, creature);
    }
    public bool findObjOnBoard(Vector2 clickPos)
    {
        Vector2 v2= new Vector2(position.x - clickPos.x, position.y - clickPos.y);

        if (Mathf.Abs(v2.x) < 2 && Mathf.Abs(v2.y) < 2)
        {
            return true;
        }
        return false;
    }



}
