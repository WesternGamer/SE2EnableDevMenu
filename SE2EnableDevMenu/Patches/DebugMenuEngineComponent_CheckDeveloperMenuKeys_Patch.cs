using HarmonyLib;
using Keen.VRage.Client.EngineComponents;
using Keen.VRage.Library.Diagnostics;
using System.Reflection.Emit;


namespace SE2EnableDevMenu.Patches
{
    [HarmonyPatch(typeof(DebugMenuEngineComponent), "CheckDeveloperMenuKeys")]
    internal class DebugMenuEngineComponent_CheckDeveloperMenuKeys_Patch
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            Label lab = generator.DefineLabel();

            List<CodeInstruction> il = 
                [
                new CodeInstruction(OpCodes.Ldarg_0), 
                new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(DebugMenuEngineComponent), "_debugMenu")),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter("Keen.VRage.Client.DebugMenu.DebugMenuController:IsEnabled")),
                new CodeInstruction(OpCodes.Brtrue_S, lab),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(DebugMenuEngineComponent), "_debugMenu")),
                new CodeInstruction(OpCodes.Ldc_I4_1),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertySetter("Keen.VRage.Client.DebugMenu.DebugMenuController:IsEnabled")),
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(Log), "Default")),
                new CodeInstruction(OpCodes.Ldstr, "[SE2EnableDevMenu] Enabled Dev Menu."),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(Log), "WriteLine", [typeof(string)])),
                new CodeInstruction(OpCodes.Ret)
                ];

            il[il.Count - 1].labels.Add(lab);

            return il;
        }
    }
}
