using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Creature : MonoBehaviour
{

    public  int level;
    private double coinsPerSec;
    public Vector2 position;
    public GameObject obj;
    public Creature(int level, Vector2 position, GameObject creature)
    {
        this.level = level;
        this.position = position;
        obj = Instantiate(creature, this.position, Quaternion.identity) as GameObject;
        coinsPerSec = fixCoins();
    }

    double fixCoins()
    {
        return Mathf.Pow(2, level) * (1 + (level - 1) / 10);
    }

    public bool findObjOnBoard(Vector2 clickPos)
    {
        Vector2 v2 = new Vector2(position.x - clickPos.x, position.y - clickPos.y);

        if (Mathf.Abs(v2.x) <= 2 && Mathf.Abs(v2.y) <= 2)
        {
            return true;
        }
        return false;
    }
}