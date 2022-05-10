using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace RadicalGraphics.Editor
{
    public partial class AppModuleGenesis : EditorWindow
    {
        private static AppModuleGenesis m_window;

        private const string MENU_ITEM_PATH = "Tools/RadicalGraphics/Create Module %#G";
        private const int WINDOW_WIDTH = 300;
        private const int WINDOW_HEIGHT = 125;
        private const int GUI_SPACING = 10;
        private const int TEXT_WIDTH = 150;


        [MenuItem(MENU_ITEM_PATH, false, 51)]
        public static void OpenEditorWindow()
        {
            m_window = EditorWindow.GetWindow<AppModuleGenesis>(true, "Module Genesis");
            m_window.minSize = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT);
            m_window.maxSize = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT);
        }

        private void OnGUI()
        {
            GUIStyle _style = new GUIStyle();
            _style.fontSize = 14;
            _style.normal.textColor = Color.white;
            _style.fontStyle = FontStyle.Bold;
            _style.margin = new RectOffset(10, 10, 10, 10);
            _style.padding = new RectOffset(10, 10, 10, 10);

            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 20, 20) });
            {
                DrawTitle("Configuration", true);

                DrawField("Module name", ref m_moduleName);

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                GUI.enabled = !string.IsNullOrEmpty(m_moduleName);
                ButtonElement("Create", CreateNewModule);
                GUI.enabled = true;

                EditorGUILayout.Space();
            }
            EditorGUILayout.EndVertical();
        }
    }

    public partial class AppModuleGenesis
    {
        private string m_moduleName;

        private const string TEMPLATE_PATH = "Editor/Templates/";
        private const string OUTPUT_PATH = "Modules/";

        private const string CONTROLLER_FILENAME = "CONST_ControllerName.cs.template";
        private const string VIEW_FILENAME = "CONST_ViewName.cs.template";
        private const string MODEL_FILENAME = "CONST_ModelName.cs.template";
        private const string SCENE_FILENAME = "CONST_SceneName.unity";

        private void CreateNewModule()
        {
            #region Duplicate templates in new folder
            string _folderPath = Path.Combine(Application.dataPath, OUTPUT_PATH);
            DirectoryInfo _newDirectory = Directory.CreateDirectory(Path.Combine(_folderPath, m_moduleName));
            DirectoryInfo _newScriptsDirectory = Directory.CreateDirectory(Path.Combine(_newDirectory.FullName, "Scripts"));

            string _templatePath = Path.Combine(Application.dataPath, TEMPLATE_PATH);

            string _sourceController = string.Concat(_templatePath, CONTROLLER_FILENAME);
            string _sourceView = string.Concat(_templatePath, VIEW_FILENAME);
            string _sourceModel = string.Concat(_templatePath, MODEL_FILENAME);
            string _sourceScene = string.Concat(_templatePath, SCENE_FILENAME);

            string _outputController = string.Concat(_newScriptsDirectory.FullName, "/", CONTROLLER_FILENAME.Replace("CONST_ControllerName", string.Concat(m_moduleName, "Controller")).Replace(".template", string.Empty));
            string _outputView = string.Concat(_newScriptsDirectory.FullName, "/", VIEW_FILENAME.Replace("CONST_ViewName", string.Concat(m_moduleName, "View")).Replace(".template", string.Empty));
            string _outputModel = string.Concat(_newScriptsDirectory.FullName, "/", MODEL_FILENAME.Replace("CONST_ModelName", string.Concat(m_moduleName, "Model")).Replace(".template", string.Empty));
            string _outputScene = string.Concat(_newDirectory.FullName, "/", SCENE_FILENAME.Replace("CONST_SceneName", string.Concat(m_moduleName, "Scene")));

            File.Copy(_sourceController, _outputController);
            File.Copy(_sourceView, _outputView);
            File.Copy(_sourceModel, _outputModel);
            File.Copy(_sourceScene, _outputScene);
            #endregion

            #region Open duplicated file and replace constants with module name data
            string _controllerText = ReplaceConstantsInFiles(_outputController);
            string _viewText = ReplaceConstantsInFiles(_outputView);
            string _modelText = ReplaceConstantsInFiles(_outputModel);
            string _sceneText = ReplaceConstantsInFiles(_outputScene);

            File.Delete(_outputController);
            File.Delete(_outputView);
            File.Delete(_outputModel);
            File.Delete(_outputScene);

            File.WriteAllText(_outputController, _controllerText);
            File.WriteAllText(_outputView, _viewText);
            File.WriteAllText(_outputModel, _modelText);
            File.WriteAllText(_outputScene, _sceneText);
            #endregion

            AssetDatabase.Refresh();
        }

        private string ReplaceConstantsInFiles(string pPath)
        {
            string _result = File.ReadAllText(pPath);

            _result = _result.Replace("CONST_ModuleName", m_moduleName);
            _result = _result.Replace("CONST_ControllerName", string.Concat(m_moduleName, "Controller"));
            _result = _result.Replace("CONST_ViewName", string.Concat(m_moduleName, "View"));
            _result = _result.Replace("CONST_ModelName", string.Concat(m_moduleName, "Model"));
            _result = _result.Replace("CONST_InterfaceName", string.Concat("I", m_moduleName, "View"));

            return _result;
        }
    }

    public partial class AppModuleGenesis
    {
        private void ButtonElement(string pButton, Action pMethod)
        {
            if (GUILayout.Button(pButton, new GUILayoutOption[] { }))
                pMethod.Invoke();

            GUILayout.Space(GUI_SPACING / 2);
        }

        private void DrawField(string pTitle, ref string pId)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(pTitle);
                GUILayout.Space(10f);
                pId = GUILayout.TextField(pId, GUILayout.MinWidth(TEXT_WIDTH), GUILayout.MaxWidth(TEXT_WIDTH));
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(GUI_SPACING / 4);
        }

        private void DrawField(string pTitle, ref int pValue)
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label(pTitle);
                GUILayout.Space(10f);
                pValue = EditorGUILayout.IntField(pValue, GUILayout.MinWidth(TEXT_WIDTH), GUILayout.MaxWidth(TEXT_WIDTH));
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(GUI_SPACING / 4);
        }

        private void DrawField(string pTitle, ref bool pActive)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(pTitle);
                GUILayout.Space(10f);

                pActive = GUILayout.Toggle(pActive, "", GUILayout.MinWidth(TEXT_WIDTH), GUILayout.MaxWidth(TEXT_WIDTH));
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(GUI_SPACING / 4);
        }

        private void DrawTitle(string pText, bool pBig = false)
        {
            if (pBig)
            {
                GUILayout.Label(pText, new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleLeft, fontSize = 18 });
                EditorGUILayout.Space();
            }
            else
                GUILayout.Label(pText, new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.BoldAndItalic, alignment = TextAnchor.MiddleLeft });
        }
    }
}