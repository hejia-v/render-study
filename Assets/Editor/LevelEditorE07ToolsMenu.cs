using UnityEngine;
using UnityEditor;
using System.Collections;

[InitializeOnLoad]
public class LevelEditorE07ToolsMenu : Editor
{
    //This is a public variable that gets or sets which of our custom tools we are currently using
    //0 - No tool selected
    //1 - The block eraser tool is selected
    //2 - The "Add block" tool is selected
    public static int SelectedTool
    {
        get
        {
            return EditorPrefs.GetInt("SelectedEditorTool", 0);
        }
        set
        {
            if (value == SelectedTool)
            {
                return;
            }

            EditorPrefs.SetInt("SelectedEditorTool", value);

            switch (value)
            {
                case 0:
                    EditorPrefs.SetBool("IsLevelEditorEnabled", false);

                    Tools.hidden = false;
                    break;
                case 1:
                    EditorPrefs.SetBool("IsLevelEditorEnabled", true);
                    EditorPrefs.SetBool("SelectBlockNextToMousePosition", false);
                    EditorPrefs.SetFloat("CubeHandleColorR", Color.magenta.r);
                    EditorPrefs.SetFloat("CubeHandleColorG", Color.magenta.g);
                    EditorPrefs.SetFloat("CubeHandleColorB", Color.magenta.b);

                    //Hide Unitys Tool handles (like the move tool) while we draw our own stuff
                    Tools.hidden = true;
                    break;
                default:
                    EditorPrefs.SetBool("IsLevelEditorEnabled", true);
                    EditorPrefs.SetBool("SelectBlockNextToMousePosition", true);
                    EditorPrefs.SetFloat("CubeHandleColorR", Color.yellow.r);
                    EditorPrefs.SetFloat("CubeHandleColorG", Color.yellow.g);
                    EditorPrefs.SetFloat("CubeHandleColorB", Color.yellow.b);

                    //Hide Unitys Tool handles (like the move tool) while we draw our own stuff
                    Tools.hidden = true;
                    break;
            }
        }
    }

    static LevelEditorE07ToolsMenu()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
        SceneView.onSceneGUIDelegate += OnSceneGUI;

        // EditorApplication.hierarchyWindowChanged可以让我们知道是否在编辑器加载了一个新的场景
        EditorApplication.hierarchyWindowChanged -= OnSceneChanged;
        EditorApplication.hierarchyWindowChanged += OnSceneChanged;
    }

    void OnDestroy()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;

        EditorApplication.hierarchyWindowChanged -= OnSceneChanged;
    }

    static void OnSceneChanged()
    {
        if (IsInCorrectLevel() == true)
        {
            Tools.hidden = LevelEditorE07ToolsMenu.SelectedTool != 0;
        }
        else
        {
            Tools.hidden = false;
        }
    }

    static void OnSceneGUI(SceneView sceneView)
    {
        if (IsInCorrectLevel() == false)
        {
            return;
        }

        DrawToolsMenu(sceneView.position);
    }

    static bool IsInCorrectLevel()
    {
        return UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name == "GameE07"
            || UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name == "GameE08"
            || UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name == "GameE09";
    }

    static void DrawToolsMenu(Rect position)
    {
        // 通过使用Handles.BeginGUI()，我们可以开启绘制Scene视图的GUI元素
        Handles.BeginGUI();

        //Here we draw a toolbar at the bottom edge of the SceneView
        // 这里我们在Scene视图的底部绘制了一个工具条
        GUILayout.BeginArea(new Rect(0, position.height - 35, position.width, 20), EditorStyles.toolbar);
        {
            string[] buttonLabels = new string[] { "None", "Erase", "Paint" };

            // GUILayout.SelectionGrid提供了一个按钮工具条
            // 通过把它的返回值存储在SelectedTool里可以让我们根据不同的按钮来实现不同的行为
            SelectedTool = GUILayout.SelectionGrid(
                SelectedTool,
                buttonLabels,
                3,
                EditorStyles.toolbarButton,
                GUILayout.Width(300));
        }
        GUILayout.EndArea();

        Handles.EndGUI();
    }
}
