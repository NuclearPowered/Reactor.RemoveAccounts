using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace Reactor.RemoveAccounts;

[BepInAutoPlugin("gg.reactor.RemoveAccounts")]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]
public partial class RemoveAccountsPlugin : BasePlugin
{
    public Harmony Harmony { get; } = new(Id);

    public override void Load()
    {
        Harmony.PatchAll();
    }
}
