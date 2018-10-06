using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Creature : MonoBehaviour
{

   private int level;
   private double coinsPerSec;
   private Vector2 position;

    public Creature (int level, Vector2 position, GameObject creature)
    {
        this.level = level;
        this.position = position;
        GameObject newCreature = Instantiate(creature, this.position, Quaternion.identity) as GameObject;
        coinsPerSec =fixCoins();
    }

    double fixCoins()
    {
        return Mathf.Pow(2, level) * (1 + (level - 1) / 10);
    }


}
