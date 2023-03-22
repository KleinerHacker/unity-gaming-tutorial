using System;
using System.Linq;
using UnityEditor;
using UnityEditorEx.Runtime.editor_ex.Scripts.Runtime.Assets;
using UnityEditorEx.Runtime.editor_ex.Scripts.Runtime.Extra;
using UnityEngine;
using UnityGamingTutorial.Runtime.gaming.tutorial.Scripts.Runtime.Components;

namespace UnityGamingTutorial.Runtime.gaming.tutorial.Scripts.Runtime.Assets
{
    public sealed class TutorialSettings : ProviderAsset<TutorialSettings>
    {
        #region Static Access

        public static TutorialSettings Singleton => GetSingleton("Tutorial", "tutorial.asset");

#if UNITY_EDITOR
        public static SerializedObject SerializedSingleton => GetSerializedSingleton("Tutorial", "tutorial.asset");
#endif

        #endregion

        #region Inspector Data

        [SerializeField]
        private bool orderedTutorial;

        [SerializeField]
        private string playerPrefsPrefix = "tutorial";

        [SerializeField]
        [Tag]
        private string targetCanvasTag = "Untagged";

        [SerializeField]
        private TutorialStep[] steps = Array.Empty<TutorialStep>();

        #endregion

        #region Properties

        public bool OrderedTutorial => orderedTutorial;

        public string PlayerPrefsPrefix => playerPrefsPrefix;

        public string TargetCanvasTag => targetCanvasTag;

        public TutorialStep[] Steps => steps;

        #endregion

        public bool TryFindStep(string identifier, out TutorialStep step)
        {
            step = null;
            if (Steps.All(x => x.Identifier != identifier))
                return false;

            step = Steps.First(x => x.Identifier == identifier);
            return true;
        }
    }

    [Serializable]
    public sealed class TutorialStep
    {
        #region Inspector Data

        [SerializeField]
        private string identifier;

        [SerializeField]
        private UITutorialDialog dialog;

        [SerializeField]
        private Vector2 relativePosition = Vector2.zero;

        #endregion

        #region Properties

        public string Identifier => identifier;

        public GameObject Dialog => dialog.gameObject;

        public Vector2 RelativePosition => relativePosition;

        #endregion
    }
}