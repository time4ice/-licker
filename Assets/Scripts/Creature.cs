using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Creature  {

   private int level;
   private double coinsPerSec;

    Creature (int level)
    {
        this.level = level;
        coinsPerSec=fixCoins();
    }

    double fixCoins()
    {
        return Mathf.Pow(2, level) * (1 + (level - 1) / 10);
    }


}
