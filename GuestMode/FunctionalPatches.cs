using HarmonyLib;
using InnerNet;

namespace GuestMode;

public class FunctionalPatches
{
    [HarmonyPatch(typeof(AccountManager), nameof(AccountManager.CanPlayOnline))]
    public static class CanPlayOnlinePatch
    {
        public static void Postfix(ref bool __result)
        {
            __result = true;
        }
    }

    [HarmonyPatch(typeof(EOSManager), nameof(EOSManager.IsAllowedOnline))]
    public static class IsAllowedOnlinePatch
    {
        public static void Prefix(ref bool canOnline)
        {
            canOnline = true;
        }
    }
    
    [HarmonyPatch(typeof(EOSManager), nameof(EOSManager.ProductUserId), MethodType.Getter)]
    public static class ProductUserIdOverride
    {
        public static void Postfix(ref string __result)
        {
            __result = "amogus";
        }
    }
    
    [HarmonyPatch(typeof(EOSManager), nameof(EOSManager.UserIDToken), MethodType.Getter)]
    public static class UserIDTokenOverride
    {
        public static void Postfix(ref string __result)
        {
            __result = "amogus";
        }
    }
    
    [HarmonyPatch(typeof(InnerNetClient), nameof(InnerNetClient.JoinGame))]
    public static class JoinGamePatch
    {
        public static void Prefix()
        {
            SaveManager.AccountLoginStatus = EOSManager.AccountLoginStatus.LoggedIn;
        }
    }
}