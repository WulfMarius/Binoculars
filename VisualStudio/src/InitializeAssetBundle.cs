using AssetLoader;
using ModComponentMapper;
using UnityEngine;

using static ModComponentMapper.LootTableName;
using static ModComponentMapper.SceneName;

namespace Binoculars
{
    internal class InitializeAssetBundle
    {
        public static void OnLoad()
        {
            ModAssetBundleManager.RegisterAssetBundle("binoculars/binoculars.unity3d");

            InitBinoculars();
        }

        private static void InitBinoculars()
        {
            Mapper.Map("GEAR_Binoculars")
                .RegisterInConsole("Binoculars")
                .AddToLootTable(LootTableSafe, 2)
                .SpawnAt(SafeHouseA, new Vector3(-1.072307f, 1.021434f, 4.15576f), new Quaternion(0f, 0.3887139f, 0f, 0.9213586f), 1)
                .SpawnAt(CoastalRegion, new Vector3(356.4126f, 203.8332f, 1155.813f), new Quaternion(0.04814799f, 0.3441772f, 0.02169151f, -0.9374185f), 0.8f)
                .SpawnAt(RadioControlHut, new Vector3(1.576444f, 1.598085f, 3.757237f), new Quaternion(0f, 0.3848061f, 0f, -0.9229974f), 0.8f);
        }
    }
}
