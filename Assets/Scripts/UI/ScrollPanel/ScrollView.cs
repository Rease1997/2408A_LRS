using System;
using System.Collections;
using System.Collections.Generic;
using HotFix;
using LitJson;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using Random = System.Random;

public class BtnItem
{
    Text text;
    Button btn;
    public void Init(GameObject gameObject,SkillData skilldata,ScrollView view)
    {
        btn = gameObject.GetComponent<Button>();
        text = gameObject.transform.GetChild(0).GetComponent<Text>();
        text.text = skilldata.Name;
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() =>
        {
            view.ShowDes(skilldata);
        });
    }
}

public class ScrollView : ViewBase
{
    Transform content;//左侧技能按钮的父节点
    GameObject item;//左侧技能按钮item
    Text title;//标题
    Text des;//描述文本
    Button leftBtn;
    Button rightBtn;
    Dictionary<int, SkillData> skillDic = new Dictionary<int, SkillData>();
    List<int> newList = new List<int>();
    List<int> myList = new List<int>();
    List<int> allList = new List<int>();
    Transform newText;
    Transform oldText;
    int page = 0;

    public override void Init(UIWindow uiWindow)
    {
        base.Init(uiWindow);
        content = uiWindow.transform.Find("Left/Viewport/Content");
        newText = uiWindow.transform.Find("Left/Viewport/Content/NewText");
        oldText = uiWindow.transform.Find("Left/Viewport/Content/OldText");
        item = uiWindow.transform.Find("Left/Viewport/Content/Item").gameObject;
        title = uiWindow.transform.Find("Right/Title").GetComponent<Text>();
        des = uiWindow.transform.Find("Right/Des").GetComponent<Text>();
        leftBtn = uiWindow.transform.Find("Right/Left").GetComponent<Button>();
        rightBtn = uiWindow.transform.Find("Right/Right").GetComponent<Button>();
        skillDic = GetSkillDicFunc();
        leftBtn.onClick.AddListener(() =>
        {
            page--;
            if (page < 0)
            {
                page = allList.Count - 1;
            }
            ShowDes(skillDic[allList[page]]);
        });
        rightBtn.onClick.AddListener(() =>
        {
            page++;
            if (page > allList.Count - 1)
                page = 0;
            ShowDes(skillDic[allList[page]]);
        });
    }

    public Dictionary<int, SkillData> GetSkillDicFunc()
    {
        Dictionary<int, SkillData> dic = new Dictionary<int, SkillData>();
        AnalyzeJson("SkillData", (JsonData temp) =>
        {
            foreach (JsonData item in temp["data"])
            {
                Debug.Log("AllSkillData itemStr:" + item.ToJson());
                SkillData data = new SkillData();
                data.ID = int.Parse(item["ID"].ToString());
                data.Name = item["Name"].ToString();
                data.Title = item["Title"].ToString();
                data.Des = item["Des"].ToString();
                data.Type = int.Parse(item["Type"].ToString());
                dic.Add(data.ID,data);
                Debug.Log("AllSkillData.count:" + dic.Count);
            }
        });
        return dic;
    }

    private void AnalyzeJson(string jsonName, Action<JsonData> callback)
    {
        string jsonPath = "Assets/GameData/Data/Json/" + jsonName + ".json";
        string jsonStr = AssetDatabase.LoadAssetAtPath<TextAsset>(jsonPath).ToString();
        JsonData temp = JsonMapper.ToObject(jsonStr);
        callback(temp);
    }


    public override void OnEnable()
    {
        base.OnEnable();
        newText.SetParent(uiWindow.transform);
        oldText.SetParent(uiWindow.transform);
        int index = 0;
        allList.Clear();
        for (int i = 0; i < newList.Count; i++)
        {
            BtnItem btnItem = new BtnItem();
            if (content.childCount > index)
            {
                btnItem.Init(content.GetChild(index).gameObject, skillDic[newList[i]], this);
            }
            else
            {
                btnItem.Init(GameObject.Instantiate(item,content).gameObject, skillDic[newList[i]], this);
            }
            index++;
            allList.Add(newList[i]);
        }
        for (int i = 0; i < myList.Count; i++)
        {
            BtnItem btnItem = new BtnItem();
            if (content.childCount > index)
            {
                btnItem.Init(content.GetChild(index).gameObject, skillDic[myList[i]], this);
            }
            else
            {
                btnItem.Init(GameObject.Instantiate(item,content).gameObject, skillDic[myList[i]], this);
            }
            index++;
            allList.Add(myList[i]);
        }
        content.GetChild(0).GetComponent<Button>().onClick.Invoke();
        page = 0;
        newText.SetParent(content);
        newText.SetAsFirstSibling();
        oldText.SetParent(content);
        oldText.SetSiblingIndex(newList.Count + 1);
    }

    float time = 0;
    int testCount = 0;
    public override void Update()
    {
        base.Update();
        time += Time.deltaTime;
        if (time > 0.1f)
        {
            testCount++;
            if (testCount > 15)
                return;
            time = 0;
            for (int i = 0; i < newList.Count; i++)
            {
                myList.Add(newList[i]);
            }
            newList.Clear();
            myList.Sort();
            AddNewSkillToList();
            UIManager.Instance.OpenWindow("ScrollPanel");
        }
    }
    Random random = new Random();
    private void AddNewSkillToList()
    {
        if(newList.Count + myList.Count >= 60)
        {
            return;
        }
        //随机数随机key值
        int id = random.Next(1001, 1060);
        if(!newList.Contains(id)&&!myList.Contains(id))
        {
            newList.Add(id);
        }
        else
        {
            AddNewSkillToList();
        }
    }

    internal void ShowDes(SkillData skilldata)
    {
        title.text = skilldata.Title;
        des.text = skilldata.Des;
        int index = allList.IndexOf(skilldata.ID);
        page = index;
    }
}
