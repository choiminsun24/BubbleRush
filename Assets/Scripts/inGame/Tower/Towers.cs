using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ITower : MonoBehaviour
{
    // Tower.csv -> Read DB
    private static List<Dictionary<string, string>> db;
    private static Dictionary<string, Tower> towerData = new Dictionary<string, Tower>();

    public Tower curTower = new Tower();


    public static void InitializeDB()
    {
        db = ExelReader.Read("Data/inGame/Tower");
        foreach (var data in db)
        {
            Tower temp = new Tower();
            temp.name = data["//Name(한국어)"];
            temp.grade = (Grade)Enum.Parse(typeof(Grade), data["Grade"]);
            temp.scale = float.Parse(data["Scale"]);
            temp.attack = int.Parse(data["Attack"]);
            temp.attackSpeed = int.Parse(data["AttackSpeed"]);
            temp.range = int.Parse(data["Range"]);
            temp.targetCount = (TargetCount)Enum.Parse(typeof(TargetCount), data["TargetCount"]);
            temp.rangeOfAttack = int.Parse(data["RangeOfAttack"]);
            temp.toSmileBubble = (ToSmileBubble)Enum.Parse(typeof(ToSmileBubble), data["ToSmileBubble"]);
            temp.toExpressionlessBubble = (ToExpressionlessBubble)Enum.Parse(typeof(ToExpressionlessBubble), data["ToExpressionlessBubble"]);
            temp.toAngryBubble = (ToAngryBubble)Enum.Parse(typeof(ToAngryBubble), data["ToAngryBubble"]);
            temp.attribute = (Attribute)Enum.Parse(typeof(Attribute), data["Attribute"]);
            temp.cost = int.Parse(data["Cost"]);
            temp.skillCoolTime = int.Parse(data["SkillCoolTime"]);
            temp.skill = int.Parse(data["Skill"]);

            towerData[temp.name] = temp;
        }
    }

    public void SetVariable(string inputName)
    {
        curTower.name = towerData[inputName].name;
        curTower.grade = towerData[inputName].grade;
        curTower.scale = towerData[inputName].scale;
        curTower.attack = towerData[inputName].attack;
        curTower.attackSpeed = towerData[inputName].attackSpeed;
        curTower.range = towerData[inputName].range;
        curTower.targetCount = towerData[inputName].targetCount;
        curTower.rangeOfAttack = towerData[inputName].rangeOfAttack;
        curTower.toSmileBubble = towerData[inputName].toSmileBubble;
        curTower.toExpressionlessBubble = towerData[inputName].toExpressionlessBubble;
        curTower.toAngryBubble = towerData[inputName].toAngryBubble;
        curTower.attribute = towerData[inputName].attribute;
        curTower.cost = towerData[inputName].cost;
        curTower.skillCoolTime = towerData[inputName].skillCoolTime;
        curTower.skill = towerData[inputName].skill;
    }

    public abstract ITower Clone();
}


public class Daebak : ITower
{
    // Instantiated List
    public static List<GameObject> listDaebak = new List<GameObject>();
    public UnityEngine.Object daebakPrefab {get; set;}
    private Stack<GameObject> poolDaebak = new Stack<GameObject>();

    public Daebak()
    {
        // Initialize
        listDaebak = new List<GameObject>();
        poolDaebak = new Stack<GameObject>();
        daebakPrefab = Resources.Load("Daebak");
        // Set ITower's info
        SetVariable("대박이_1");
    }

    public GameObject GetDaebak()
    {
        GameObject tempDaebak;
        if(!daebakPrefab)
        {
            print("daebakPrefab is null");
            return null;
        }
        // Initial Pooling
        if(poolDaebak.Count == 0)
        {
            for (int i = 0; i < 20; ++i)
            {
                tempDaebak = Instantiate(daebakPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
                SetVariable("대박이_1");
                tempDaebak.GetComponent<TowerController>().data = curTower;
                poolDaebak.Push(tempDaebak);
                tempDaebak.SetActive(false);
            }
        }
        tempDaebak = poolDaebak.Pop();
        tempDaebak.SetActive(true);
        listDaebak.Add(tempDaebak);
        return tempDaebak;
    }

    public void ReturnDaebak(GameObject obj)
    {
        obj.SetActive(false);
        listDaebak.Remove(obj);
        Destroy(obj);
        GameObject tempDaebak = Instantiate(daebakPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        SetVariable("대박이_1");
        tempDaebak.GetComponent<TowerController>().data = curTower;
        poolDaebak.Push(tempDaebak);
        tempDaebak.SetActive(false);
    }

    public override ITower Clone()
    {
        // deep copy
        Daebak clonedDaebak = new Daebak();
        clonedDaebak.curTower = new Tower();
        // copy data
        clonedDaebak.curTower.name = curTower.name;
        clonedDaebak.curTower.grade = curTower.grade;
        clonedDaebak.curTower.scale = curTower.scale;
        clonedDaebak.curTower.attack = curTower.attack;
        clonedDaebak.curTower.attackSpeed = curTower.attackSpeed;
        clonedDaebak.curTower.range = curTower.range;
        clonedDaebak.curTower.targetCount = curTower.targetCount;
        clonedDaebak.curTower.rangeOfAttack = curTower.rangeOfAttack;
        clonedDaebak.curTower.toSmileBubble = curTower.toSmileBubble;
        clonedDaebak.curTower.toExpressionlessBubble = curTower.toExpressionlessBubble;
        clonedDaebak.curTower.toAngryBubble = curTower.toAngryBubble;
        clonedDaebak.curTower.attribute = curTower.attribute;
        clonedDaebak.curTower.cost = curTower.cost;
        clonedDaebak.curTower.skillCoolTime = curTower.skillCoolTime;
        clonedDaebak.curTower.skill = curTower.skill;
        return clonedDaebak;
    }
}

public class Nabi : ITower
{
    // Instantiated List
    public static List<GameObject> listNabi = new List<GameObject>();
    public UnityEngine.Object NabiPrefab {get; set;}
    private Stack<GameObject> poolNabi = new Stack<GameObject>();

    public Nabi()
    {
        // Initialize
        listNabi = new List<GameObject>();
        poolNabi = new Stack<GameObject>();
        NabiPrefab = Resources.Load("Nabi");
        // Set ITower's info
        SetVariable("나비_1");
    }

    public GameObject GetNabi()
    {
        GameObject tempNabi;
        if(!NabiPrefab)
        {
            print("NabiPrefab is null");
            return null;
        }
        // Initial Pooling
        if(poolNabi.Count == 0)
        {
            for (int i = 0; i < 20; ++i)
            {
                tempNabi = Instantiate(NabiPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
                SetVariable("나비_1");
                tempNabi.GetComponent<TowerController>().data = curTower;
                poolNabi.Push(tempNabi);
                tempNabi.SetActive(false);
            }
        }
        tempNabi = poolNabi.Pop();
        tempNabi.SetActive(true);
        listNabi.Add(tempNabi);
        return tempNabi;
    }

    public void ReturnNabi(GameObject obj)
    {
        obj.SetActive(false);
        listNabi.Remove(obj);
        Destroy(obj);
        GameObject tempNabi = Instantiate(NabiPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        SetVariable("나비_1");
        tempNabi.GetComponent<TowerController>().data = curTower;
        poolNabi.Push(tempNabi);
        tempNabi.SetActive(false);
    }

    public override ITower Clone()
    {
        // deep copy
        Nabi clonedNabi = new Nabi();
        clonedNabi.curTower = new Tower();
        // copy data
        clonedNabi.curTower.name = curTower.name;
        clonedNabi.curTower.grade = curTower.grade;
        clonedNabi.curTower.scale = curTower.scale;
        clonedNabi.curTower.attack = curTower.attack;
        clonedNabi.curTower.attackSpeed = curTower.attackSpeed;
        clonedNabi.curTower.range = curTower.range;
        clonedNabi.curTower.targetCount = curTower.targetCount;
        clonedNabi.curTower.rangeOfAttack = curTower.rangeOfAttack;
        clonedNabi.curTower.toSmileBubble = curTower.toSmileBubble;
        clonedNabi.curTower.toExpressionlessBubble = curTower.toExpressionlessBubble;
        clonedNabi.curTower.toAngryBubble = curTower.toAngryBubble;
        clonedNabi.curTower.attribute = curTower.attribute;
        clonedNabi.curTower.cost = curTower.cost;
        clonedNabi.curTower.skillCoolTime = curTower.skillCoolTime;
        clonedNabi.curTower.skill = curTower.skill;
        return clonedNabi;
    }
}


public class Tori : ITower
{
    // Instantiated List
    public static List<GameObject> listTori = new List<GameObject>();
    public UnityEngine.Object ToriPrefab {get; set;}
    private Stack<GameObject> poolTori = new Stack<GameObject>();

    public Tori()
    {
        // Initialize
        listTori = new List<GameObject>();
        poolTori = new Stack<GameObject>();
        ToriPrefab = Resources.Load("Tori");
        // Set ITower's info
        SetVariable("토리_1");
    }

    public GameObject GetTori()
    {
        GameObject tempTori;
        if(!ToriPrefab)
        {
            print("ToriPrefab is null");
            return null;
        }
        // Initial Pooling
        if(poolTori.Count == 0)
        {
            for (int i = 0; i < 20; ++i)
            {
                tempTori = Instantiate(ToriPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
                SetVariable("토리_1");
                tempTori.GetComponent<TowerController>().data = curTower;
                poolTori.Push(tempTori);
                tempTori.SetActive(false);
            }
        }
        tempTori = poolTori.Pop();
        tempTori.SetActive(true);
        listTori.Add(tempTori);
        return tempTori;
    }

    public void ReturnTori(GameObject obj)
    {
        obj.SetActive(false);
        listTori.Remove(obj);
        Destroy(obj);
        GameObject tempTori = Instantiate(ToriPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        SetVariable("토리_1");
        tempTori.GetComponent<TowerController>().data = curTower;
        poolTori.Push(tempTori);
        tempTori.SetActive(false);
    }

    public override ITower Clone()
    {
        // deep copy
        Tori clonedTori = new Tori();
        clonedTori.curTower = new Tower();
        // copy data
        clonedTori.curTower.name = curTower.name;
        clonedTori.curTower.grade = curTower.grade;
        clonedTori.curTower.scale = curTower.scale;
        clonedTori.curTower.attack = curTower.attack;
        clonedTori.curTower.attackSpeed = curTower.attackSpeed;
        clonedTori.curTower.range = curTower.range;
        clonedTori.curTower.targetCount = curTower.targetCount;
        clonedTori.curTower.rangeOfAttack = curTower.rangeOfAttack;
        clonedTori.curTower.toSmileBubble = curTower.toSmileBubble;
        clonedTori.curTower.toExpressionlessBubble = curTower.toExpressionlessBubble;
        clonedTori.curTower.toAngryBubble = curTower.toAngryBubble;
        clonedTori.curTower.attribute = curTower.attribute;
        clonedTori.curTower.cost = curTower.cost;
        clonedTori.curTower.skillCoolTime = curTower.skillCoolTime;
        clonedTori.curTower.skill = curTower.skill;
        return clonedTori;
    }
}

public class Goby : ITower
{
    // Instantiated List
    public static List<GameObject> listGoby = new List<GameObject>();
    public UnityEngine.Object GobyPrefab {get; set;}
    private Stack<GameObject> poolGoby = new Stack<GameObject>();

    public Goby()
    {
        // Initialize
        listGoby = new List<GameObject>();
        poolGoby = new Stack<GameObject>();
        GobyPrefab = Resources.Load("Goby");
        // Set ITower's info
        SetVariable("꼬비_1");
    }

    public GameObject GetGoby()
    {
        GameObject tempGoby;
        if(!GobyPrefab)
        {
            print("GobyPrefab is null");
            return null;
        }
        // Initial Pooling
        if(poolGoby.Count == 0)
        {
            for (int i = 0; i < 20; ++i)
            {
                tempGoby = Instantiate(GobyPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
                SetVariable("꼬비_1");
                tempGoby.GetComponent<TowerController>().data = curTower;
                poolGoby.Push(tempGoby);
                tempGoby.SetActive(false);
            }
        }
        tempGoby = poolGoby.Pop();
        tempGoby.SetActive(true);
        listGoby.Add(tempGoby);
        return tempGoby;
    }

    public void ReturnGoby(GameObject obj)
    {
        obj.SetActive(false);
        listGoby.Remove(obj);
        Destroy(obj);
        GameObject tempGoby = Instantiate(GobyPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        SetVariable("꼬비_1");
        tempGoby.GetComponent<TowerController>().data = curTower;
        poolGoby.Push(tempGoby);
        tempGoby.SetActive(false);
    }

    public override ITower Clone()
    {
        // deep copy
        Goby clonedGoby = new Goby();
        clonedGoby.curTower = new Tower();
        // copy data
        clonedGoby.curTower.name = curTower.name;
        clonedGoby.curTower.grade = curTower.grade;
        clonedGoby.curTower.scale = curTower.scale;
        clonedGoby.curTower.attack = curTower.attack;
        clonedGoby.curTower.attackSpeed = curTower.attackSpeed;
        clonedGoby.curTower.range = curTower.range;
        clonedGoby.curTower.targetCount = curTower.targetCount;
        clonedGoby.curTower.rangeOfAttack = curTower.rangeOfAttack;
        clonedGoby.curTower.toSmileBubble = curTower.toSmileBubble;
        clonedGoby.curTower.toExpressionlessBubble = curTower.toExpressionlessBubble;
        clonedGoby.curTower.toAngryBubble = curTower.toAngryBubble;
        clonedGoby.curTower.attribute = curTower.attribute;
        clonedGoby.curTower.cost = curTower.cost;
        clonedGoby.curTower.skillCoolTime = curTower.skillCoolTime;
        clonedGoby.curTower.skill = curTower.skill;
        return clonedGoby;
    }
}

public class Tutu : ITower
{
    // Instantiated List
    public static List<GameObject> listTutu = new List<GameObject>();
    public UnityEngine.Object TutuPrefab {get; set;}
    private Stack<GameObject> poolTutu = new Stack<GameObject>();

    public Tutu()
    {
        // Initialize
        listTutu = new List<GameObject>();
        poolTutu = new Stack<GameObject>();
        TutuPrefab = Resources.Load("Tutu");
        // Set ITower's info
        SetVariable("투투_1");
    }

    public GameObject GetTutu()
    {
        GameObject tempTutu;
        if(!TutuPrefab)
        {
            print("TutuPrefab is null");
            return null;
        }
        // Initial Pooling
        if(poolTutu.Count == 0)
        {
            for (int i = 0; i < 20; ++i)
            {
                tempTutu = Instantiate(TutuPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
                SetVariable("투투_1");
                tempTutu.GetComponent<TowerController>().data = curTower;
                poolTutu.Push(tempTutu);
                tempTutu.SetActive(false);
            }
        }
        tempTutu = poolTutu.Pop();
        tempTutu.SetActive(true);
        listTutu.Add(tempTutu);
        return tempTutu;
    }

    public void ReturnTutu(GameObject obj)
    {
        obj.SetActive(false);
        listTutu.Remove(obj);
        Destroy(obj);
        GameObject tempTutu = Instantiate(TutuPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        SetVariable("투투_1");
        tempTutu.GetComponent<TowerController>().data = curTower;
        poolTutu.Push(tempTutu);
        tempTutu.SetActive(false);
    }

    public override ITower Clone()
    {
        // deep copy
        Tutu clonedTutu = new Tutu();
        clonedTutu.curTower = new Tower();
        // copy data
        clonedTutu.curTower.name = curTower.name;
        clonedTutu.curTower.grade = curTower.grade;
        clonedTutu.curTower.scale = curTower.scale;
        clonedTutu.curTower.attack = curTower.attack;
        clonedTutu.curTower.attackSpeed = curTower.attackSpeed;
        clonedTutu.curTower.range = curTower.range;
        clonedTutu.curTower.targetCount = curTower.targetCount;
        clonedTutu.curTower.rangeOfAttack = curTower.rangeOfAttack;
        clonedTutu.curTower.toSmileBubble = curTower.toSmileBubble;
        clonedTutu.curTower.toExpressionlessBubble = curTower.toExpressionlessBubble;
        clonedTutu.curTower.toAngryBubble = curTower.toAngryBubble;
        clonedTutu.curTower.attribute = curTower.attribute;
        clonedTutu.curTower.cost = curTower.cost;
        clonedTutu.curTower.skillCoolTime = curTower.skillCoolTime;
        clonedTutu.curTower.skill = curTower.skill;
        return clonedTutu;
    }
}

public class Orang : ITower
{
    // Instantiated List
    public static List<GameObject> listOrang = new List<GameObject>();
    public UnityEngine.Object OrangPrefab {get; set;}
    private Stack<GameObject> poolOrang = new Stack<GameObject>();

    public Orang()
    {
        // Initialize
        listOrang = new List<GameObject>();
        poolOrang = new Stack<GameObject>();
        OrangPrefab = Resources.Load("Orang");
        // Set ITower's info
        SetVariable("오랑이_1");
    }

    public GameObject GetOrang()
    {
        GameObject tempOrang;
        if(!OrangPrefab)
        {
            print("OrangPrefab is null");
            return null;
        }
        // Initial Pooling
        if(poolOrang.Count == 0)
        {
            for (int i = 0; i < 20; ++i)
            {
                tempOrang = Instantiate(OrangPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
                SetVariable("오랑이_1");
                tempOrang.GetComponent<TowerController>().data = curTower;
                poolOrang.Push(tempOrang);
                tempOrang.SetActive(false);
            }
        }
        tempOrang = poolOrang.Pop();
        tempOrang.SetActive(true);
        listOrang.Add(tempOrang);
        return tempOrang;
    }

    public void ReturnOrang(GameObject obj)
    {
        obj.SetActive(false);
        listOrang.Remove(obj);
        Destroy(obj);
        GameObject tempOrang = Instantiate(OrangPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        SetVariable("오랑이_1");
        tempOrang.GetComponent<TowerController>().data = curTower;
        poolOrang.Push(tempOrang);

        tempOrang.SetActive(false);
    }

    public override ITower Clone()
    {
        // deep copy
        Orang clonedOrang = new Orang();
        clonedOrang.curTower = new Tower();
        // copy data
        clonedOrang.curTower.name = curTower.name;
        clonedOrang.curTower.grade = curTower.grade;
        clonedOrang.curTower.scale = curTower.scale;
        clonedOrang.curTower.attack = curTower.attack;
        clonedOrang.curTower.attackSpeed = curTower.attackSpeed;
        clonedOrang.curTower.range = curTower.range;
        clonedOrang.curTower.targetCount = curTower.targetCount;
        clonedOrang.curTower.rangeOfAttack = curTower.rangeOfAttack;
        clonedOrang.curTower.toSmileBubble = curTower.toSmileBubble;
        clonedOrang.curTower.toExpressionlessBubble = curTower.toExpressionlessBubble;
        clonedOrang.curTower.toAngryBubble = curTower.toAngryBubble;
        clonedOrang.curTower.attribute = curTower.attribute;
        clonedOrang.curTower.cost = curTower.cost;
        clonedOrang.curTower.skillCoolTime = curTower.skillCoolTime;
        clonedOrang.curTower.skill = curTower.skill;
        return clonedOrang;
    }
}

public class Tower
{
    // Member Variable
    public string name {get; set;}
    public Grade grade {get; set;}
    public float scale {get; set;}
    public int attack {get; set;}
    public int attackSpeed {get; set;}
    public int range {get; set;}
    public TargetCount targetCount {get; set;}
    public int rangeOfAttack {get; set;}
    public ToSmileBubble toSmileBubble {get; set;}
    public ToExpressionlessBubble toExpressionlessBubble {get; set;}
    public ToAngryBubble toAngryBubble {get; set;}
    public Attribute attribute {get; set;}
    public int cost {get; set;}
    public int skillCoolTime {get; set;}
    public int skill {get; set;}
}

// Enum

public enum Grade
{
    Normal, Legendary, Rare, Advanced
}

public enum TargetCount
{
    Single, Range, NULL
}

public enum ToSmileBubble
{
    Strong, Weak, NULL
}

public enum ToExpressionlessBubble
{
    Normal, VeryStrong, NULL
}

public enum ToAngryBubble
{
    VeryWeak, Normal, Strong, Weak, NULL
}

public enum Attribute
{
    Ground, Water
}