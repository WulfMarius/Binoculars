using UnityEngine;

using AssetLoader;

using ModComponentMapper;

namespace Binoculars
{
    internal class InitializeAssetBundle
    {
        private const string GEAR_BINOCULARS = "GEAR_Binoculars";

        public static void OnLoad()
        {
            ModAssetBundleManager.RegisterAssetBundle("binoculars");

            ConsoleManager.Initialize();
            ModUtils.ExecuteStaticMethod(typeof(ConsoleManager), "AddCustomGearName", new object[] { "binoculars", GEAR_BINOCULARS });
        }

        private static void SpawnBinoculars()
        {
            Object prefab = Resources.Load(GEAR_BINOCULARS);

            Transform playerTransform = GameManager.GetPlayerTransform();
            Vector3 targetPosition = playerTransform.position + playerTransform.forward * 2;

            AssetUtils.instantiatePrefab((GameObject)prefab, targetPosition);
        }
    }
}
