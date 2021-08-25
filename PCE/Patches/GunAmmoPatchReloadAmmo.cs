using System;
using HarmonyLib;


namespace PCE.Patches
{
    // postfix to make sure ammo is drawn correctly if maxAmmo is modified while the player is reloading
    [Serializable]
    [HarmonyPatch(typeof(GunAmmo), "ReloadAmmo")]
    class GunAmmoPatchReloadAmmo
    {
        private static void Postfix(GunAmmo __instance)
        {
            __instance.ReDrawTotalBullets();
        }
    }
}
