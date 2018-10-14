using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Runtime;
using UnityEngine.SceneManagement;

public class SceneWithBox : MonoBehaviour {
    
    public GameObject box;
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
    int boxLevel;
    private IEnumerator boxDeliver;
    private IEnumerator moneyMaker;

    void Start()
    {
        print("hello");
        money = 0;
        GetCreatures();
        boxLevel = 1;
        boxDeliver = Deliver();
        moneyMaker = Maker();
        check = true;
        StartCoroutine(boxDeliver);
        StartCoroutine(moneyMaker);
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

    void OnApplicationQuit()
    {
        writeInfo();
    }

    void writeInfo()
    {
        PlayerPrefs.SetInt("0", findBoxAmount());
        PlayerPrefs.SetInt("1", findCreaturesAmount(1));
        PlayerPrefs.SetInt("2", findCreaturesAmount(2));
        PlayerPrefs.SetInt("3", findCreaturesAmount(3));
        PlayerPrefs.SetInt("4", findCreaturesAmount(4));
        PlayerPrefs.SetInt("5", findCreaturesAmount(5));
        PlayerPrefs.SetString("date",  System.DateTime.Now.ToString());
        PlayerPrefs.SetFloat("money", money);
    }

    int findBoxAmount()
    {
        int amount = 0;
        foreach (Object s in creaturesOnBoard)
        {
            if (s is Box) amount++;
        }
        return amount;

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

       
        if (!PlayerPrefs.HasKey("0")) return;
        int j = 0;
        money = PlayerPrefs.GetFloat("money");
        for (int k = 1; k <= 5; k++)
        {
            for (int i = 0; i < PlayerPrefs.GetInt(k.ToString()); i++)
            {
                creaturesOnBoard[j] = new Creature(k, RandomPosition(), creatureByLevel(k));
                j++;
            }
        }
        int boxesWhileAbsent = PlayerPrefs.GetInt("0")+(int)System.DateTime.Now.Subtract(System.DateTime.Parse(PlayerPrefs.GetString("date"))).TotalSeconds / time;
        if (boxesWhileAbsent < creaturesOnBoard.Length - j)
        {
            for (int i = 0; i < boxesWhileAbsent; i++)
            {

                int randomLevel = Random.Range(1, boxLevel);
                creaturesOnBoard[j] = new Box(randomLevel, creatureByLevel(randomLevel), box, RandomPosition());
                j++;
            }
        }
        else
        {
            for(int i=0; i<creaturesOnBoard.Length; i++)
            {
                if (!(creaturesOnBoard[i] is Creature))
                {
                    int randomLevel = Random.Range(1, boxLevel);
                    creaturesOnBoard[i] = new Box(randomLevel, creatureByLevel(randomLevel), box, RandomPosition());
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
            case 5: return fifth;
            default: return first;
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
                if (creature1.level == 5)
                {
                    if (!PlayerPrefs.HasKey("6")) PlayerPrefs.SetInt("6", 1);
                    else PlayerPrefs.SetInt("6", PlayerPrefs.GetInt("6") + 1);
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
           yield return new WaitForSeconds(time);
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
            
        }
    }

    IEnumerator Maker()
    {
        while(true)
        {
            foreach (Object obj in creaturesOnBoard)
                if (obj is Creature) money += (obj as Creature).coinsPerSec;
            coinstext.GetComponent<Text>().text = money.ToString();
            yield return new WaitForSeconds(1);
        }

    }

   public void gotoscene (string scenename)
    {
        print("here");
        writeInfo();
        SceneManager.LoadScene("second");
    }
}
