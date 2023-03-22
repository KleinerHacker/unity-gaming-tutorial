using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityUIEx.Runtime.ui_ex.Scripts.Runtime.Components.UI.Window;

namespace UnityGamingTutorial.Runtime.gaming.tutorial.Scripts.Runtime.Components
{
    [AddComponentMenu(UnityGamingTutorialConstants.Root + "/Tutorial Dialog")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(UIDialog))]
    public sealed class UITutorialDialog : UIBehaviour
    {
        #region Inspector Data

        [Header("References")]
        [SerializeField]
        private Toggle skipToggle;

        #endregion

        #region Properties

        public bool IsShown => _dialog.State == ViewableState.Shown;

        #endregion

        #region Events

        public event EventHandler Shown
        {
            add => _dialog.Shown += value;
            remove => _dialog.Shown -= value;
        }

        public event EventHandler Hidden
        {
            add => _dialog.Hidden += value;
            remove => _dialog.Hidden -= value;
        }

        #endregion

        private UIDialog _dialog;

        #region Builtin Methods

        protected override void Awake()
        {
            _dialog = GetComponent<UIDialog>();
        }

        #endregion

        public void Show() => _dialog.Show();

        public void Hide() => _dialog.Hide();

        public void Submit()
        {
            _dialog.Hide();
            if (skipToggle != null && skipToggle.isOn)
            {
#if PCSOFT_TUTORIAL
                TutorialManager.Singleton.SkipTutorial();
#endif
            }
        }

        public void SkipTutorial()
        {
#if PCSOFT_TUTORIAL
            TutorialManager.Singleton.SkipTutorial();
#endif
            _dialog.Hide();
        }
    }
}