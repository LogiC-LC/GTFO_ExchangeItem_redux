// ==========================================================
// ExchangeItem.cs — 主功能模块 (Feature)
// ==========================================================
using Hikaria.ExchangeItem.Handlers;
using Hikaria.ExchangeItem.Managers;
using Player;
using TheArchive.Core.Attributes.Feature;
using TheArchive.Core.Attributes.Feature.Patches;
using TheArchive.Core.FeaturesAPI;
using TheArchive.Core.FeaturesAPI.Settings;
using TheArchive.Core.Localization;
using UnityEngine;

namespace Hikaria.ExchangeItem.Features
{
    [EnableFeatureByDefault]
    [DisallowInGameToggle]
    public class ExchangeItem : Feature
    {
        public override string Name => "资源交换";
        public static new ILocalizationService Localization { get; set; }

        [FeatureConfig]
        public static ExchangeItemSetting Settings { get; set; }

        public class ExchangeItemSetting
        {
            public KeyCode ExchangeItemKey { get; set; } = KeyCode.T;

            [FSDisplayName("强制物品交换")]
            [FSDescription("由于网络同步原因，此选项启用后将可能导致资源余量异常，请谨慎开启！")]
            public bool ForceExchangeItem { get; set; } = false;
        }

        public override void OnFeatureSettingChanged(FeatureSetting setting)
        {
            ExchangeItemUpdater.Instance?.OnInteractionKeyChanged();
        }

        [ArchivePatch(typeof(LocalPlayerAgent), nameof(LocalPlayerAgent.Setup))]
        private class LocalPlayerAgent__Setup__Patch
        {
            private static void Postfix(LocalPlayerAgent __instance)
            {
                ExchangeItemUpdater.BindToLocalPlayer(__instance);
            }
        }

        [ArchivePatch(typeof(PlayerInventoryLocal), nameof(PlayerInventoryLocal.DoWieldItem))]
        private class PlayerInventoryLocal__DoWieldItem__Patch
        {
            private static void Postfix()
            {
                ExchangeItemUpdater.Instance?.OnWieldItemChanged();
            }
        }

        [ArchivePatch(typeof(PlayerAmmoStorage), nameof(PlayerAmmoStorage.SetStorageData))]
        private class PlayerAmmoStorage__SetStorageData__Patch
        {
            private static void Postfix(PlayerAmmoStorage __instance)
            {
                var agent = __instance.m_playerBackpack?.Owner?.PlayerAgent?.TryCast<PlayerAgent>();
                if (agent != null)
                    ExchangeItemUpdater.Instance?.OnAmmoStorageChanged(agent);
            }
        }

        public override void Init()
        {
            ExchangeItemManager.Setup();
        }
    }
}

