using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box  {
    int randomRange;

    Box(int range)
    {
        this.randomRange = range;
    }

    int openBox()
    {
        return Random.Range(0, randomRange);
    }

}
