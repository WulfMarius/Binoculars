using UnityEngine;

using AssetLoader;

using ModComponentMapper;

namespace Binoculars
{
    internal class InitializeAssetBundle
    {
        private const string PREFAB_BINOCULARS = "GEAR_Binoculars";
        private const string DISPLAY_NAME_BINOCULARS = "Binoculars";

        public static void OnLoad()
        {
            ModAssetBundleManager.RegisterAssetBundle("binoculars.unity3d");

            ModUtils.RegisterConsoleGearName(DISPLAY_NAME_BINOCULARS, PREFAB_BINOCULARS);
            ModUtils.InsertIntoLootTable(LootTableName.LootTableSafe, (GameObject)Resources.Load(PREFAB_BINOCULARS), 2);

            GearSpawnInfo gearSpawnInfo = new GearSpawnInfo();
            gearSpawnInfo.PrefabName = PREFAB_BINOCULARS;
            gearSpawnInfo.Position = new Vector3(-1.072307f, 1.021434f, 4.15576f);
            gearSpawnInfo.Rotation = new Quaternion(0f, 0.3887139f, 0f, 0.9213586f);
            gearSpawnInfo.SpawnChance = 1;
            GearSpawner.AddGearSpawnInfo("SafeHouseA", gearSpawnInfo);
        }
    }
}
