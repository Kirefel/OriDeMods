using HarmonyLib;

namespace Trainer
{
    // The race disables regular saves but leaves checkpoints alone
    // This means you can respawn during the race (for example as part of ghost door)
    //  but you can't quit to menu (for example for door warp)
    // TODO abuse backup saves to allow for quit + reload races?
    [HarmonyPatch(typeof(SaveGameController), nameof(SaveGameController.PerformSave))]
    class DisableSavesSave { static bool Prefix() => !TrainerRace.RaceActive; }
    [HarmonyPatch(typeof(SaveSlotBackupsManager), nameof(SaveSlotBackupsManager.CreateCurrentBackup))]
    class DisableSavesBackup { static bool Prefix() => !TrainerRace.RaceActive; }
}
