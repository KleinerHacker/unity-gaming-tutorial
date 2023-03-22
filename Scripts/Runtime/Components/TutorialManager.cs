using System;
using System.Collections.Generic;
using System.Linq;
using UnityBase.Runtime.@base.Scripts.Runtime.Components.Singleton;
using UnityBase.Runtime.@base.Scripts.Runtime.Components.Singleton.Attributes;
using UnityCommonEx.Runtime.common_ex.Scripts.Runtime.Utils.Extensions;
using UnityEngine;
using UnityGamingTutorial.Runtime.gaming.tutorial.Scripts.Runtime.Assets;
using UnityPrefsEx.Runtime.prefs_ex.Scripts.Runtime.Utils;

namespace UnityGamingTutorial.Runtime.gaming.tutorial.Scripts.Runtime.Components
{
#if PCSOFT_TUTORIAL
    [Singleton(Instance = SingletonInstance.RequiresNewInstance, Scope = SingletonScope.Application, CreationTime = SingletonCreationTime.Loading, ObjectName = "Tutorial Manager")]
    public sealed class TutorialManager : SingletonBehavior<TutorialManager>
    {
        private TutorialSettings _settings;
        private readonly IDictionary<string, UITutorialDialog> _tutorialDialogs = new Dictionary<string, UITutorialDialog>();

        #region Builtin Methods

        protected override void DoAwake()
        {
            _settings = TutorialSettings.Singleton;

            _tutorialDialogs.Clear();
            foreach (var step in _settings.Steps)
            {
                var canvas = new Func<Canvas>(() =>
                {
                    var canvasGo = FindObjectsOfType<Canvas>().FirstOrDefault(x => x.gameObject.CompareTag(_settings.TargetCanvasTag));
                    if (canvasGo == null)
                    {
                        Debug.LogWarning("[TUTORIAL] Unable to find canvas tagged with " + _settings.TargetCanvasTag);
                    }

                    return canvasGo == null ? FindObjectOfType<Canvas>() : canvasGo.GetComponent<Canvas>();
                }).Invoke();

                if (canvas == null)
                    throw new InvalidOperationException("Cannot find any target canvas, tag was " + _settings.TargetCanvasTag);

                var go = Instantiate(step.Dialog, Vector3.zero, Quaternion.identity, canvas.transform);
                ((RectTransform)go.transform).anchoredPosition += step.RelativePosition;
                _tutorialDialogs.Add(step.Identifier, go.GetComponent<UITutorialDialog>());
            }
        }

        #endregion

        internal void SkipTutorial()
        {
#if PCSOFT_TUTORIAL_LOGGING
            Debug.Log("[TUTORIAL] Skip tutorial");
#endif
            foreach (var key in _tutorialDialogs.Keys)
            {
                PlayerPrefsEx.SetBool(BuildKey(key), true, true);
            }
        }

        internal void HideCurrent()
        {
            if (!_tutorialDialogs.Any(x => x.Value.IsShown))
            {
#if PCSOFT_TUTORIAL_LOGGING
                Debug.Log("[TUTORIAL] No dialog is shown currently, nothing to hide");
#endif
                return;
            }

            _tutorialDialogs.Values
                .Where(x => x.IsShown)
                .ForEach(x => x.Hide());
        }

        internal void ResetTutorial()
        {
#if PCSOFT_TUTORIAL_LOGGING
            Debug.Log("[TUTORIAL] Reset tutorial");
#endif
            foreach (var key in _tutorialDialogs.Keys)
            {
                PlayerPrefsEx.SetBool(BuildKey(key), false, true);
            }

            PlayerPrefsEx.SetInt(CounterKey, 0, true);
        }

        internal void HandleEvent(string identifier, Action onFinished)
        {
            if (PlayerPrefsEx.GetBool(BuildKey(identifier), false))
            {
#if PCSOFT_TUTORIAL_LOGGING
                Debug.Log("[TUTORIAL] Tutorial already shown: " + identifier);
#endif

                onFinished?.Invoke();
                return;
            }

            if (!_settings.TryFindStep(identifier, out var step))
            {
                Debug.LogError("[TUTORIAL] Unable to find tutorial step " + identifier);

                onFinished?.Invoke();
                return;
            }

            if (_settings.OrderedTutorial && PlayerPrefsEx.GetInt(CounterKey, 0) != _tutorialDialogs.Keys.IndexOf(x => x == identifier))
            {
#if PCSOFT_TUTORIAL_LOGGING
                Debug.Log("[TUTORIAL] Tutorial on wrong step index. Requires other tutorial steps before: " + identifier);
#endif

                onFinished?.Invoke();
                return;
            }

#if PCSOFT_TUTORIAL_LOGGING
            Debug.Log("[TUTORIAL] Show tutorial: " + identifier);
#endif
            _tutorialDialogs[identifier].Show();
            _tutorialDialogs[identifier].Hidden += Hidden;
            PlayerPrefsEx.SetBool(BuildKey(identifier), true, true);
            PlayerPrefsEx.SetInt(CounterKey, _tutorialDialogs.Keys.IndexOf(x => x == identifier) + 1);

            void Hidden(object sender, EventArgs e)
            {
                onFinished?.Invoke();
                _tutorialDialogs[identifier].Hidden -= Hidden;
            }
        }

        private string BuildKey(string identifier) => _settings.PlayerPrefsPrefix + "." + identifier;
        private string CounterKey => BuildKey("counter");
    }
#endif
}