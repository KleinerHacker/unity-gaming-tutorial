using UnityEditor;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor;
using UnityEngine;

namespace UnityGamingTutorial.Editor.gaming.tutorial.Scripts.Editor.Provider
{
    public sealed class TutorialStepList : TableReorderableList
    {
        public TutorialStepList(SerializedObject serializedObject, SerializedProperty elements) : base(serializedObject, elements)
        {
            Columns.Add(new FixedColumn { HeaderText = "Identifier", AbsoluteWidth = 150f, MaxHeight = 20f, ElementCallback = IdentifierElementCallback });
            Columns.Add(new FlexibleColumn { HeaderText = "Tutorial Dialog", MaxHeight = 20f, ElementCallback = TutorialDialogElementCallback });
            Columns.Add(new FixedColumn { HeaderText = "Relative Position", AbsoluteWidth = 150f, MaxHeight = 20f, ElementCallback = RelativePositionElementCallback });
        }

        private void IdentifierElementCallback(Rect rect, int i, bool isactive, bool isfocused)
        {
            var property = serializedProperty.GetArrayElementAtIndex(i);
            var identifierProperty = property.FindPropertyRelative("identifier");

            EditorGUI.PropertyField(rect, identifierProperty, GUIContent.none);
        }

        private void TutorialDialogElementCallback(Rect rect, int i, bool isactive, bool isfocused)
        {
            var property = serializedProperty.GetArrayElementAtIndex(i);
            var dialogProperty = property.FindPropertyRelative("dialog");

            EditorGUI.PropertyField(rect, dialogProperty, GUIContent.none);
        }

        private void RelativePositionElementCallback(Rect rect, int i, bool isactive, bool isfocused)
        {
            var property = serializedProperty.GetArrayElementAtIndex(i);
            var posProperty = property.FindPropertyRelative("relativePosition");

            EditorGUI.PropertyField(rect, posProperty, GUIContent.none);
        }
    }
}