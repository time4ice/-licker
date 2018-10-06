using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public Vector2 position;
    GameObject creature;
    int creatureLevel;
     public Box(int creatureLevel, GameObject creature, GameObject box, Vector2 position)
    {
        this.creatureLevel = creatureLevel;
        this.creature = creature;
        this.position = position;
        GameObject newBox = Instantiate(box, this.position, Quaternion.identity) as GameObject;
        
    }

    public Creature openBox()
    {
        return new Creature(creatureLevel, position, creature);
    }



}
