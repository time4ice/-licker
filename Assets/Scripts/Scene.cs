using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Runtime;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{

    public int scenelevel;
    public GameObject first;
    public GameObject second;
    public GameObject third;
    public GameObject fourth;
    public GameObject fifth;
    public GameObject empty;
    public GameObject coinstext;
    private bool check;
    private int time = 5;
    private float money;

    public static Object[] creaturesOnBoard = new Object[20];

    public static List<int> mergeLst = new List<int>();
    private IEnumerator moneyMaker;

    void Start()
    {
        money = 0;
        
    }

    void Awake()
    {
        GetCreatures();
        moneyMaker = Maker();
        StartCoroutine(moneyMaker);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickWorldPos
                = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            print("clickPos" + clickWorldPos);
            for (int i = 0; i < creaturesOnBoard.Length; i++)
            {
                if (creaturesOnBoard[i] is Creature)
                {
                    CreatureFunc(i, clickWorldPos);
                }

            }
        }
    }

    void OnApplicationQuit()
    {
        writeInfo();
    }

    void writeInfo()
    {
        PlayerPrefs.SetInt((5*scenelevel+1).ToString(), findCreaturesAmount(5 * scenelevel + 1));
        PlayerPrefs.SetInt((5 * scenelevel + 2).ToString(), findCreaturesAmount(5 * scenelevel + 2));
        PlayerPrefs.SetInt((5 * scenelevel + 3).ToString(), findCreaturesAmount(5 * scenelevel +3));
        PlayerPrefs.SetInt((5 * scenelevel + 4).ToString(), findCreaturesAmount(5 * scenelevel + 4));
        PlayerPrefs.SetInt((5 * scenelevel + 5).ToString(), findCreaturesAmount(5 * scenelevel + 5));
        PlayerPrefs.SetFloat("money", money);
    }

    int findCreaturesAmount(int level)
    {
        int amount = 0;
        foreach (Object s in creaturesOnBoard)
        {
            if (s is Creature)
            {
                if ((s as Creature).level == level) amount++;
            }
        }
        return amount;

    }

    void GetCreatures()
    {


        if (!PlayerPrefs.HasKey("6")) return;
        int j = 0;
        money = PlayerPrefs.GetFloat("money");
        for (int k = 1; k <= 5; k++)
        {
            for (int i = 0; i < PlayerPrefs.GetInt((5 * scenelevel + k).ToString()); i++)
            {
                creaturesOnBoard[j] = new Creature(5 * scenelevel + k, RandomPosition(), creatureByLevel(5 * scenelevel + k));
                j++;
            }
        }
        
    }

    GameObject creatureByLevel(int level)
    {
        switch (level%5)
        {
            case 1: return first;
            case 2: return second;
            case 3: return third;
            case 4: return fourth;
            case 5: return fifth;
            default: return first;
        }
    }
    bool CreatureFunc(int i, Vector2 clickPos)
    {
        Creature creature = creaturesOnBoard[i] as Creature;
        if (creature.findObjOnBoard(clickPos))
        {
            if (mergeLst.Count == 0 || mergeLst[0] != i) mergeLst.Add(i);
        }
        if (mergeLst.Count == 2)
        {
            if ((creaturesOnBoard[mergeLst[0]] as Creature).level ==
                (creaturesOnBoard[mergeLst[1]] as Creature).level)
            {
                Creature creature1 = creaturesOnBoard[mergeLst[0]] as Creature;
                Creature creature2 = creaturesOnBoard[mergeLst[1]] as Creature;
                if (creature1.level == 5*scenelevel+5)
                {
                    if (!PlayerPrefs.HasKey((5 * (scenelevel+1) + 1).ToString())) PlayerPrefs.SetInt((5 * (scenelevel + 1) + 1).ToString(), 1);
                    else PlayerPrefs.SetInt((5 * (scenelevel + 1) + 1).ToString(), PlayerPrefs.GetInt((5 * (scenelevel + 1) + 1).ToString()) + 1);
                    creaturesOnBoard[mergeLst[0]] = null;
                }
                else
                {
                    creaturesOnBoard[mergeLst[0]] =
                        new Creature(creature1.level + 1, creature2.position, creatureByLevel(creature1.level + 1));
                }
                creaturesOnBoard[mergeLst[1]] = null;
                Destroy(creature1.obj);
                Destroy(creature2.obj);
            }
            mergeLst.Clear();
            return true;
        }
        return false;
    }
   

    Vector2 RandomPosition()
    {

        while (true)
        {
            bool has = false;
            Vector2 currposition = new Vector2(empty.transform.position.x + Random.Range(0, 5) * 4,
                                               empty.transform.position.y - Random.Range(0, 5) * 4);
            foreach (Object element in creaturesOnBoard)
            {
                
                if (element is Creature)
                {
                    if ((element as Creature).position == currposition) { has = true; break; }
                }

            }
            if (has == false) return currposition;
        }
    }

  

    IEnumerator Maker()
    {
        while (true)
        {
            foreach (Object obj in creaturesOnBoard)
                if (obj is Creature) money += (obj as Creature).coinsPerSec;
            int i = 0;
            while(PlayerPrefs.HasKey(i.ToString()))
            {
                if (i!=scenelevel*5+1&& i != scenelevel * 5 + 2 && i != scenelevel * 5 + 3 && i != scenelevel * 5 + 4&& i != scenelevel * 5 + 5)
                    money += PlayerPrefs.GetInt(i.ToString())*Creature.fixCoins(i);
                i++;
            }
            coinstext.GetComponent<Text>().text = money.ToString();
            yield return new WaitForSeconds(1);
        }
    }

    public void gotoscene(string scenename)
    {
        writeInfo();
        SceneManager.LoadScene(scenename);
    }

}
