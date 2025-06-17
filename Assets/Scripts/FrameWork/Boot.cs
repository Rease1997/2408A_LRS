using UnityEngine;
using LitJson;
using System;
using UnityEditor;
using HotFix;
using System.Resources;
using System.Collections.Generic;

public class Boot : UnitySingleton<Boot>
{
    public Camera uiCamera;
    //����
    public AudioSource audio;

    //ui�����㼶����ҪTransform 
    //����canvas canvas����camera��Ⱦ���� ͨ�����ھ�����Ʋ㼶
    //���������depthonly ���Ҹĳ���������
    public Transform BackRoot;
    public Transform UIRoot;
    public Transform TipRoot;


    void Start()
    {
        Init();
        uiCamera = transform.GetComponentInChildren<Camera>();
        UIManager.Instance.OpenWindow("LoginPanel");

        GetShopDataList();
        //GameScenesManager.Instance.LoadSceneAsync("Game", "PlayerPanel");
    }

    public List<ShopData> GetShopDataList()
    {
        List<ShopData> list = new List<ShopData>();
        AnalyzeJson("ShopData", (JsonData temp) =>
        {
            foreach (JsonData item in temp["data"])
            {
                Debug.Log("AllShopData itemStr:" + item.ToJson());
                ShopData data = new ShopData();
                data.ID = int.Parse(item["ID"].ToString());
                data.Name = item["Name"].ToString();
                data.Des = item["Des"].ToString();
                data.Type = int.Parse(item["Type"].ToString());
                data.ImgPath = item["ImgPath"].ToString();
                data.ImgName = item["ImgName"].ToString();
                list.Add(data);
                Debug.Log("AllShopData.count:" + list.Count);
            }
        });
        return list;
    }

    private void AnalyzeJson(string jsonName, Action<JsonData> callback)
    {
        string jsonPath = "Assets/GameData/Data/Json/" + jsonName + ".json";
        Debug.Log("json�ļ�·����" + jsonPath);
        string jsonStr = AssetDatabase.LoadAssetAtPath<TextAsset>(jsonPath).ToString();
        Debug.Log("json�ļ����ݣ�" + jsonStr);
        JsonData temp = JsonMapper.ToObject(jsonStr);
        callback(temp);
    }

    void Init()
    {
        gameObject.AddComponent<GameScenesManager>().Init(this);
        UIManager.Instance.Init(TipRoot, UIRoot, BackRoot);
    }

    public void ChangeAudio(AudioClip clip)
    {
        audio.clip = clip;
        audio.time = 0;
        audio.Play();
    }
}
