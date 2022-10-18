using System.Reflection;
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
            var eosManager = EOSManager.Instance;

            SaveManager.AccountLoginStatus = EOSManager.AccountLoginStatus.LoggedIn;
            SaveManager.ChatModeType = QuickChatModes.FreeChatOrQuickChat;
            SaveManager.AcceptedPrivacyPolicy = 2;

            eosManager.userId = new ProductUserId();

            eosManager.hasRunLoginFlow = true;
            eosManager.loginFlowFinished = true;

            AccountManager.Instance.privacyPolicyBg.gameObject.SetActive(false);
            AccountManager.Instance.waitingText.gameObject.SetActive(false);
            eosManager.HideCallbackWaitAnim();
            eosManager.IsAllowedOnline(true);

            __result = false;
            return false;
        }
    }

    [HarmonyPatch(typeof(EOSManager), nameof(EOSManager.Awake))]
    public static class InitializePlatformInterfacePatch
    {
        public static bool Prefix(EOSManager __instance)
        {
            new DestroyableSingleton<EOSManager>(__instance.Pointer).Awake();

            __instance.platformInitialized = true;
            SaveManager.LoadPlayerPrefs(true);
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
            matchmakerToken = "RemoveAccounts";
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
}
