using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static Unity.VisualScripting.Member;
using UnityEngine.UI;
public enum EditorEnum
{
    None = 0,
    First,
    Second,
    Third,
    Fourth,
}
public class ToolEditor : EditorWindow
{
    static List<int> intList = new List<int>();
    [MenuItem("Tools/ToolEditor")]
    static void ToolEditorFunc()
    {
        var window = GetWindow<ToolEditor>("工具编辑器");
        intList = new List<int>();
        window.Show();
    }
    Vector2 v2 = Vector2.zero;
    #region
    float sliderValue = 0;

    EditorEnum enumData = EditorEnum.None;

    string name = "";
    bool bl = false;
    GameObject go = null;
    #endregion
    private void OnGUI()
    {
        if (GUILayout.Button("添加"))
        {
            intList.Add(intList.Count + 1);
        }
        #region

        GUILayout.BeginHorizontal();
        sliderValue = GUILayout.HorizontalSlider(sliderValue, 0, 1);
        GUILayout.Label(sliderValue.ToString());
        if (Selection.gameObjects.Length > 0)
            Selection.gameObjects[0].transform.localScale = new Vector3(sliderValue, sliderValue, sliderValue);
        GUILayout.EndHorizontal();

        //enumData = (EditorEnum)EditorGUILayout.EnumPopup(enumData);

        //GUILayout.BeginHorizontal();
        //GUILayout.Label("姓名：");
        //name = GUILayout.TextField(name);
        //GUILayout.EndHorizontal();

        //GUILayout.BeginHorizontal();
        //GUILayout.Label("bool:");
        //bl = EditorGUILayout.Toggle(bl);
        //GUILayout.EndHorizontal();

        //GUILayout.BeginHorizontal();
        //go = (GameObject)EditorGUILayout.ObjectField("GameObject", go, typeof(GameObject), true);
        //GUILayout.EndHorizontal();
        #endregion

        v2 = GUILayout.BeginScrollView(v2);

        for (int i = 0; i < intList.Count; i++)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("第" + intList[i] + "个按钮"))
            {
                var win = GetWindow<EditorTestWindow>();
                win.thisName = "2408A";
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
    }
}
