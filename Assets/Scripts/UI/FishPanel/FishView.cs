using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using Random = System.Random;

public class FishItem
{
    public void Init(GameObject fish,int scale,Vector3 pos)
    {
        fish.transform.localScale = new Vector3(scale, scale, scale);
    }
}
public class FishView : ViewBase
{
    public float[] FishScale = new float[]
    {
        0.5f,1.5f,2.5f,3.5f
    };
    public GameObject player;
    public GameObject fish;
    public Transform posParent;
    public Vector3[] posArray;
    Random random = new Random();
    public override void Init(UIWindow uiWindow)
    {
        base.Init(uiWindow);
        player = uiWindow.transform.Find("Player").gameObject;
        fish = uiWindow.transform.Find("Fish").gameObject;
        fish.SetActive(false);
        posParent = uiWindow.transform.Find("CreatPos");
        posArray = new Vector3[posParent.childCount];
        for (int i = 0; i < posParent.childCount; i++)
        {
            posArray[i] = posParent.GetChild(i).position;
        }
        var triggerPlayer = player.AddComponent<TriggerEvent>();
        triggerPlayer.BeginDragAction = BeginAction;
        triggerPlayer.DragAction = DragAction;
        triggerPlayer.EndDragAction = EndAction;
        triggerPlayer.TiggerStay2D = StayAction;
    }

    public override void OnEnable()
    {
        base.OnEnable();
    }

    private void StayAction(Collider2D collision)
    {
        var fishObj = collision.gameObject;
        if (fishObj.name.Contains("Fish"))
        {
            if (fishObj.transform.localScale.x > player.transform.localScale.x)
            {
                Debug.Log("比我大");
            }
            else
            {
                Debug.Log("比我小");
                player.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
                GameObject.Destroy(fishObj);
            }
        }
    }

    bool isDrag;

    private void EndAction()
    {
        isDrag = false;
    }

    private void DragAction()
    {
        // 获取鼠标的屏幕位置
        Vector3 mousePosition = Input.mousePosition;

        // 将鼠标的屏幕坐标转换为视口坐标
        Vector3 viewportPosition = Camera.main.ScreenToViewportPoint(mousePosition);

        // 将视口坐标转换为世界坐标
        Vector3 worldPosition = Camera.main.ViewportToWorldPoint(viewportPosition);

        // 只设置x, y轴位置，z轴根据需要调整
        worldPosition.z = player.transform.position.z;
        player.transform.position = worldPosition;
    }

    private void BeginAction()
    {
        isDrag = true;
    }

    RaycastHit hit;
    float countDownTime = 1;
    float time = 0;
    public override void Update()
    {
        base.Update();
        if(Input.GetKeyDown(KeyCode.A))
        {
            uiWindow.transform.DOShakePosition(1f, new Vector3(10, 10, 0), 10, 90, false);
        }
        time += Time.deltaTime;
        if(time>countDownTime)
        {
            time = 0;
            var localFish = GameObject.Instantiate(fish,uiWindow.transform);
            localFish.SetActive(true);
            Vector3 pos = posArray[random.Next(0, posParent.childCount)];
            float scale = FishScale[random.Next(0,FishScale.Length)];
            localFish.transform.localScale = new Vector3(scale,scale,scale);
            localFish.transform.position = pos;
            localFish.transform.DOMove(new Vector3(-pos.x, pos.y, pos.z), 10f,false);
        }
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray ray = Boot.Instance.uiCamera.ScreenPointToRay(Input.mousePosition);

        //    // 发射射线并检查是否碰撞
        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        // 如果射线击中物体，打印出该物体的名字
        //        Debug.Log("Hit " + hit.transform.name);
        //        if (hit.collider.gameObject == player)
        //        { 
        //            // 获取鼠标的屏幕位置
        //            Vector3 mousePosition = Input.mousePosition;

        //            // 将鼠标的屏幕坐标转换为视口坐标
        //            Vector3 viewportPosition = Camera.main.ScreenToViewportPoint(mousePosition);

        //            // 将视口坐标转换为世界坐标
        //            Vector3 worldPosition = Camera.main.ViewportToWorldPoint(viewportPosition);

        //            // 只设置x, y轴位置，z轴根据需要调整
        //            worldPosition.z = player.transform.position.z;
        //        }
        //    }
        //}
    }
}
