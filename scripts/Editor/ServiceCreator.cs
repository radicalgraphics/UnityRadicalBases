using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


namespace RadicalGraphics.Editor
{
    public partial class ServiceGenesis : EditorWindow
    {
        private static ServiceGenesis m_window;

        private const string MENU_ITEM_PATH = "Tools/RadicalGraphics/Create Service %#G";
        private const int WINDOW_WIDTH = 300;
        private const int WINDOW_HEIGHT = 125;
        private const int GUI_SPACING = 10;
        private const int TEXT_WIDTH = 150;


        [MenuItem(MENU_ITEM_PATH, false, 51)]
        public static void OpenEditorWindow()
        {
            m_window = EditorWindow.GetWindow<ServiceGenesis>(true, "Service Genesis");
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

                DrawField("Service name", ref m_serviceName);

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                GUI.enabled = !string.IsNullOrEmpty(m_serviceName);
                ButtonElement("Create", CreateNewService);
                GUI.enabled = true;

                EditorGUILayout.Space();
            }
            EditorGUILayout.EndVertical();
        }
    }

    public partial class ServiceGenesis
    {
        private string m_serviceName;

        private const string TEMPLATE_PATH = "Editor/Templates/";
        private const string OUTPUT_PATH = "Scripts/Core/Services/";

        private const string SERVICE_FILENAME = "CONST_ServiceName.cs.template";
        private const string SERVICE_MOBILE_FILENAME = "CONST_ServiceNameMobile.cs.template";

        private void CreateNewService()
        {
            #region Duplicate templates in new folder
            string _folderPath = Path.Combine(Application.dataPath, OUTPUT_PATH);
            DirectoryInfo _newDirectory = Directory.CreateDirectory(Path.Combine(_folderPath, m_serviceName));
            DirectoryInfo _newMobileDirectory = Directory.CreateDirectory(Path.Combine(_newDirectory.FullName, "Mobile"));

            string _templatePath = Path.Combine(Application.dataPath, TEMPLATE_PATH);

            string _sourceService = string.Concat(_templatePath, SERVICE_FILENAME);
            string _sourceMobile = string.Concat(_templatePath, SERVICE_MOBILE_FILENAME);

            string _outpoutService = string.Concat(_newDirectory.FullName, "/", SERVICE_FILENAME.Replace("CONST_ServiceName", string.Concat(m_serviceName, "Service")).Replace(".template", string.Empty));
            string _outputMobile = string.Concat(_newMobileDirectory.FullName, "/", SERVICE_MOBILE_FILENAME.Replace("CONST_ServiceName", string.Concat(m_serviceName, "Service")).Replace(".template", string.Empty));

            File.Copy(_sourceService, _outpoutService);
            File.Copy(_sourceMobile, _outputMobile);
            #endregion

            #region Open duplicated file and replace constants with module name data
            string _serviceText = ReplaceConstantsInFiles(_outpoutService);
            string _mobileText = ReplaceConstantsInFiles(_outputMobile);

            File.Delete(_outpoutService);
            File.Delete(_outputMobile);

            File.WriteAllText(_outpoutService, _serviceText);
            File.WriteAllText(_outputMobile, _mobileText);
            #endregion

            AssetDatabase.Refresh();
        }

        private string ReplaceConstantsInFiles(string pPath)
        {
            string _result = File.ReadAllText(pPath);

            _result = _result.Replace("CONST_ServiceName", m_serviceName);

            return _result;
        }
    }

    public partial class ServiceGenesis
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

