using UnityEditor;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor.Utils;
using UnityEngine;
using UnityEngine.UIElements;
using UnityGamingTutorial.Runtime.gaming.tutorial.Scripts.Runtime.Assets;

namespace UnityGamingTutorial.Editor.gaming.tutorial.Scripts.Editor.Provider
{
    public sealed class TutorialProvider : SettingsProvider
    {
        #region Static Area

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            return new TutorialProvider();
        }

        #endregion

        private SerializedObject _serializedObject;

        private SerializedProperty _orderedProperty;
        private SerializedProperty _playerPrefsPrefixProperty;
        private SerializedProperty _canvasTagProperty;
        private SerializedProperty _stepsProperty;

        private TutorialStepList _stepList;

        public TutorialProvider() : base("Project/Player/Tutorial System", SettingsScope.Project, new[] { "Tooling", "Tutorial", "Gaming" })
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            _serializedObject = TutorialSettings.SerializedSingleton;
            if (_serializedObject == null)
                return;

            _orderedProperty = _serializedObject.FindProperty("orderedTutorial");
            _playerPrefsPrefixProperty = _serializedObject.FindProperty("playerPrefsPrefix");
            _canvasTagProperty = _serializedObject.FindProperty("targetCanvasTag");
            _stepsProperty = _serializedObject.FindProperty("steps");

            _stepList = new TutorialStepList(_serializedObject, _stepsProperty);
        }

        public override void OnTitleBarGUI()
        {
            GUILayout.BeginVertical();
            {
                ExtendedEditorGUILayout.SymbolField("Activate System", "PCSOFT_TUTORIAL");
                EditorGUI.BeginDisabledGroup(
#if PCSOFT_TUTORIAL
                    false
#else
                    true
#endif
                );
                {
                    ExtendedEditorGUILayout.SymbolField("Verbose Logging", "PCSOFT_TUTORIAL_LOGGING");
                }
                EditorGUI.EndDisabledGroup();
            }
            GUILayout.EndVertical();
        }

        public override void OnGUI(string searchContext)
        {
            _serializedObject.Update();

            GUILayout.Space(15f);

#if PCSOFT_TUTORIAL
            EditorGUILayout.PropertyField(_canvasTagProperty, new GUIContent("Target Canvas for dialogs"));
            EditorGUILayout.PropertyField(_playerPrefsPrefixProperty, new GUIContent("Prefix for player prefs"));
            GUILayout.Space(15f);

            EditorGUILayout.PropertyField(_orderedProperty, new GUIContent("Ordered Tutorial Steps", null, "The tutorial steps are shown in defined order."));
            _stepList.DoLayoutList();
#else
            EditorGUILayout.HelpBox("Tutorial System deactivated", MessageType.Info);
#endif

            _serializedObject.ApplyModifiedProperties();
        }
    }
}