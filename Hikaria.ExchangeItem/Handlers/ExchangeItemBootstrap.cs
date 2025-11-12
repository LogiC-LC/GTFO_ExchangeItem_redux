// ==========================================================
// ExchangeItemBootstrap.cs — 初始化器 (安全注册)
// ==========================================================
using UnityEngine;

namespace Hikaria.ExchangeItem.Handlers
{
    internal static class ExchangeItemBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            if (ExchangeItemUpdater.Instance != null)
                return;

            var host = new GameObject("ExchangeItemUpdaterHost");
            Object.DontDestroyOnLoad(host);
            host.hideFlags = HideFlags.HideAndDontSave;
            host.AddComponent<ExchangeItemUpdater>();
        }
    }
}

