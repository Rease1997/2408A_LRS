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
                Debug.Log("���Ҵ�");
            }
            else
            {
                Debug.Log("����С");
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
        // ��ȡ������Ļλ��
        Vector3 mousePosition = Input.mousePosition;

        // ��������Ļ����ת��Ϊ�ӿ�����
        Vector3 viewportPosition = Camera.main.ScreenToViewportPoint(mousePosition);

        // ���ӿ�����ת��Ϊ��������
        Vector3 worldPosition = Camera.main.ViewportToWorldPoint(viewportPosition);

        // ֻ����x, y��λ�ã�z�������Ҫ����
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

        //    // �������߲�����Ƿ���ײ
        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        // ������߻������壬��ӡ�������������
        //        Debug.Log("Hit " + hit.transform.name);
        //        if (hit.collider.gameObject == player)
        //        { 
        //            // ��ȡ������Ļλ��
        //            Vector3 mousePosition = Input.mousePosition;

        //            // ��������Ļ����ת��Ϊ�ӿ�����
        //            Vector3 viewportPosition = Camera.main.ScreenToViewportPoint(mousePosition);

        //            // ���ӿ�����ת��Ϊ��������
        //            Vector3 worldPosition = Camera.main.ViewportToWorldPoint(viewportPosition);

        //            // ֻ����x, y��λ�ã�z�������Ҫ����
        //            worldPosition.z = player.transform.position.z;
        //        }
        //    }
        //}
    }
}
