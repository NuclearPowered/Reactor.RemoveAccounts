using System;
using System.Reflection;
using System.Text.Json;
using AmongUs.Data;
using Epic.OnlineServices;
using HarmonyLib;
using InnerNet;
using Reactor.Utilities.Extensions;

namespace Reactor.RemoveAccounts;

internal class FunctionalPatches
{
    [HarmonyPatch]
    public static class RunLoginPatch
    {
        public static MethodBase TargetMethod()
        {
            return ReflectionExtensions.EnumeratorMoveNext(typeof(EOSManager), nameof(EOSManager.RunLogin));
        }

        public static bool Prefix(ref bool __result)
        {
            EOSManager.Instance.IsAllowedOnline(true);

            __result = false;
            return false;
        }
    }

    [HarmonyPatch(typeof(EOSManager), nameof(EOSManager.Awake))]
    public static class InitializePlatformInterfacePatch
    {
        private static readonly PropertyInfo _localUserIdProperty = typeof(EpicManager).GetProperty("localUserId", BindingFlags.Static | BindingFlags.Public);

        public static bool Prefix(EOSManager __instance)
        {
            DestroyableSingleton<EOSManager>._instance = __instance;
            if (__instance.DontDestroy) __instance.gameObject.DontDestroyOnLoad();

            __instance.platformInitialized = true;
            _localUserIdProperty?.SetValue(null, new EpicAccountId());

            DataManager.Player.Account.LoginStatus = EOSManager.AccountLoginStatus.LoggedIn;
            DataManager.Settings.Multiplayer.ChatMode = QuickChatModes.FreeChatOrQuickChat;

            __instance.userId = new ProductUserId();

            __instance.hasRunLoginFlow = true;
            __instance.loginFlowFinished = true;

            return false;
        }
    }

    [HarmonyPatch(typeof(AccountManager), nameof(AccountManager.OnSceneLoaded))]
    public static class AccountManagerOnSceneLoadedPatch
    {
        public static void Postfix(AccountManager __instance)
        {
            __instance.privacyPolicyBg.gameObject.SetActive(false);
            __instance.waitingText.gameObject.SetActive(false);
            __instance.postLoadWaiting.gameObject.SetActive(false);
        }
    }

    [HarmonyPatch(typeof(EOSManager), nameof(EOSManager.ContinueInOfflineMode))]
    public static class ContinueInOfflineModePatch
    {
        public static bool Prefix()
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(EOSManager), nameof(EOSManager.LoginWithCorrectPlatform))]
    public static class LoginWithCorrectPlatformPatch
    {
        public static bool Prefix()
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(AccountManager), nameof(AccountManager.CanPlayOnline))]
    public static class CanPlayOnlinePatch
    {
        public static bool Prefix(out bool __result)
        {
            __result = true;
            return false;
        }
    }

    [HarmonyPatch(typeof(EOSManager), nameof(EOSManager.ProductUserId), MethodType.Getter)]
    public static class ProductUserIdOverride
    {
        public static bool Prefix(out string __result)
        {
            __result = string.Empty;
            return false;
        }
    }

    [HarmonyPatch(typeof(EOSManager), nameof(EOSManager.UserIDToken), MethodType.Getter)]
    public static class UserIDTokenOverride
    {
        public static bool Prefix(out string __result)
        {
            __result = null;
            return false;
        }
    }

    [HarmonyPatch(typeof(HttpMatchmakerManager), nameof(HttpMatchmakerManager.TryReadCachedToken))]
    public static class CoGetOrRefreshTokenPatch
    {
        public static bool Prefix(out bool __result, out string matchmakerToken)
        {
            __result = true;
            matchmakerToken = Convert.ToBase64String(JsonSerializer.SerializeToUtf8Bytes(new
            {
                Content = new
                {
                    Puid = "RemoveAccounts",
                    ClientVersion = Constants.GetBroadcastVersion(),
                    ExpiresAt = DateTime.UtcNow.AddHours(1),
                },
                Hash = "RemoveAccounts",
            }));
            return false;
        }
    }

    [HarmonyPatch(typeof(StoreManager), nameof(StoreManager.Initialize))]
    public static class StoreManagerPatch
    {
        public static bool Prefix()
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(AchievementManager), nameof(AchievementManager.UpdateAchievementProgress))]
    public static class UpdateAchievementProgressPatch
    {
        public static bool Prefix()
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(AchievementManager), nameof(AchievementManager.UnlockAchievement))]
    public static class UnlockAchievementPatch
    {
        public static bool Prefix()
        {
            return false;
        }
    }
}
