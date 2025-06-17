using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorTestWindow : EditorWindow
{
    static List<GameObject> cubes = new List<GameObject>();
    [MenuItem("Test/TestWindow")]
    static void OpenWindow()
    {
        cubes.Clear();
        var window = EditorWindow.GetWindow<EditorTestWindow>("测试窗口");
        window.minSize = new Vector2(500, 500);
        window.Show(true);
    }
    string name = "";
    string name2 = "";
    private void OnGUI()
    {
        if (EditorApplication.isPlaying)
        {
            if (GUILayout.Button("按钮", GUILayout.Width(500)))
            {
                Debug.Log("你输入的名字是" + name);
                var gameobj = GameObject.Instantiate(Resources.Load("Cube"));
                /// cube1|0|0|0|type
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label("名字：");
            name = GUILayout.TextField(name);
            GUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.LabelField("Not playing");
        }
    }
}
