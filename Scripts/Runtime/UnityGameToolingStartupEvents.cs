using UnityAssetLoader.Runtime.asset_loader.Scripts.Runtime;
using UnityEngine;
using UnityGamingTutorial.Runtime.gaming.tutorial.Scripts.Runtime.Assets;

namespace UnityGamingTutorial.Runtime.gaming.tutorial.Scripts.Runtime
{
    public static class UnityGameToolingStartupEvents
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        public static void Initialize()
        {
#if PCSOFT_TUTORIAL
            Debug.Log("[TUTORIAL] Initialize tutorial system");
            AssetResourcesLoader.LoadFromResources<TutorialSettings>("");
#endif
        }
    }
}