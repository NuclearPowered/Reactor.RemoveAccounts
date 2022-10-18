using HarmonyLib;
using Reactor.Utilities.Extensions;

namespace Reactor.RemoveAccounts;

internal class CosmeticPatches
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
}
