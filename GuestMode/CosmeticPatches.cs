using HarmonyLib;
using Il2CppSystem;
using Reactor.Extensions;

namespace GuestMode;

public class CosmeticPatches
{
    [HarmonyPatch(typeof(FriendsListButton), nameof(FriendsListButton.Awake))]
    public static class FriendsListDestroy
    {
        public static void Prefix(FriendsListButton __instance)
        {
            __instance.gameObject.Destroy();
        }
    }
    
    [HarmonyPatch(typeof(AccountTab), nameof(AccountTab.Awake))]
    public static class AccountTabHide
    {
        public static void Prefix(AccountTab __instance)
        {
            __instance.transform.FindChild("AccountWindow").gameObject.SetActive(false);
        }
    }
    
    [HarmonyPatch(typeof(AccountManager), nameof(AccountManager.SignInFail))]
    public static class AccountErrorHide
    {
        public static bool Prefix(ref Action callback)
        {
            callback.Invoke();
            return false;
        }
    }
}