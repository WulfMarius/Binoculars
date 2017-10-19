using Harmony;
using System.Reflection;
using UnityEngine;

namespace Binoculars
{
    public class Implementation
    {
        private GameObject equippedModel;

        private float originalFOV;
        private UITexture texture;

        public static void ShowItemPopups(string primaryAction, string secondaryAction, bool showAmmo, bool showDuration, bool showReload, bool showHolster)
        {
            EquipItemPopup equipItemPopup = InterfaceManager.m_Panel_HUD.m_EquipItemPopup;
            ShowItemIcons(equipItemPopup, primaryAction, secondaryAction, showAmmo, showDuration);

            if (Utils.IsGamepadActive())
            {
                equipItemPopup.m_ButtonPromptFire.ShowPromptForKey(primaryAction, "Fire");
                MaybeRepositionFireButtonPrompt(equipItemPopup, secondaryAction);
                equipItemPopup.m_ButtonPromptAltFire.ShowPromptForKey(secondaryAction, "AltFire");
                MaybeRepositionAltFireButtonPrompt(equipItemPopup, primaryAction);
            }
            else
            {
                equipItemPopup.m_ButtonPromptFire.ShowPromptForKey(secondaryAction, "AltFire");
                MaybeRepositionFireButtonPrompt(equipItemPopup, primaryAction);
                equipItemPopup.m_ButtonPromptAltFire.ShowPromptForKey(primaryAction, "Interact");
                MaybeRepositionAltFireButtonPrompt(equipItemPopup, secondaryAction);
            }

            string reloadText = showReload ? Localization.Get("GAMEPLAY_Reload") : string.Empty;
            equipItemPopup.m_ButtonPromptReload.ShowPromptForKey(reloadText, "Reload");

            string holsterText = showHolster ? Localization.Get("GAMEPLAY_HolsterPrompt") : string.Empty;
            equipItemPopup.m_ButtonPromptHolster.ShowPromptForKey(holsterText, "Holster");
        }

        public void OnControlModeChangedWhileEquipped()
        {
            EndZoom();
        }

        public void OnEquipped()
        {
            ShowButtonPopups();
        }

        public void OnSecondaryAction()
        {
            if (GameManager.GetVpFPSCamera().IsZoomed)
            {
                EndZoom();
            }
            else
            {
                StartZoom();
            }
        }

        public void OnUnequipped()
        {
            EndZoom();
        }

        private static UITexture CreateOverlay(Texture2D texture)
        {
            UIRoot root = UIRoot.list[0];
            UIPanel panel = NGUITools.AddChild<UIPanel>(root.gameObject);

            UITexture result = NGUITools.AddChild<UITexture>(panel.gameObject);
            result.mainTexture = texture;

            Vector2 windowSize = panel.GetWindowSize();
            result.width = (int)windowSize.x;
            result.height = (int)windowSize.y;

            return result;
        }

        private static void ExecuteMethod(object instance, string methodName, params object[] parameters)
        {
            MethodInfo methodInfo = AccessTools.Method(instance.GetType(), methodName, AccessTools.GetTypes(parameters));
            methodInfo.Invoke(instance, parameters);
        }

        private static void FreezePlayer()
        {
            GameManager.GetVpFPSPlayer().Controller.m_Controller.SimpleMove(Vector3.zero);
            GameManager.GetPlayerManagerComponent().DisableCharacterController();
        }

        private static T GetFieldValue<T>(object target, string fieldName)
        {
            FieldInfo fieldInfo = AccessTools.Field(target.GetType(), fieldName);
            if (fieldInfo != null)
            {
                return (T)fieldInfo.GetValue(target);
            }

            return default(T);
        }

        private static void MaybeRepositionAltFireButtonPrompt(EquipItemPopup __instance, string otherAction)
        {
            ExecuteMethod(__instance, "MaybeRepositionAltFireButtonPrompt", new object[] { otherAction, });
        }

        private static void MaybeRepositionFireButtonPrompt(EquipItemPopup equipItemPopup, string otherAction)
        {
            ExecuteMethod(equipItemPopup, "MaybeRepositionFireButtonPrompt", new object[] { otherAction, });
        }

        private static void ShowButtonPopups()
        {
            ShowItemPopups(string.Empty, Localization.Get("GAMEPLAY_Use"), false, false, false, true);
        }

        private static void ShowItemIcons(EquipItemPopup equipItemPopup, string primaryAction, string secondaryAction, bool showAmmo, bool showDuration)
        {
            ExecuteMethod(equipItemPopup, "ShowItemIcons", new object[] { primaryAction, secondaryAction, showAmmo, showDuration });
        }

        private static void UnfreezePlayer()
        {
            GameManager.GetPlayerManagerComponent().EnableCharacterController();
        }

        private void EndZoom()
        {
            if (!GameManager.GetVpFPSCamera().IsZoomed)
            {
                return;
            }

            UnfreezePlayer();
            RestoreCamera();
            HideOverlay();
            GameManager.GetWeaponCamera().gameObject.SetActive(true);
        }

        private void HideEquippedModel()
        {
            if (equippedModel == null)
            {
                return;
            }

            Object.Destroy(equippedModel);
            equippedModel = null;
        }

        private void HideOverlay()
        {
            if (texture == null)
            {
                return;
            }

            Object.Destroy(texture);
            texture = null;
        }

        private void RestoreCamera()
        {
            var camera = GameManager.GetVpFPSCamera();
            camera.ToggleZoom(false);
            camera.SetFOVFromOptions(originalFOV);
        }

        private void ShowEquippedModel()
        {
            GameObject prefab = (GameObject)Resources.Load("TOOL_Binoculars");

            this.equippedModel = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            this.equippedModel.transform.parent = GameManager.GetVpFPSCamera().transform;
            this.equippedModel.transform.localPosition = new Vector3(0, -0.25f, 0.25f);
            this.equippedModel.transform.localRotation = Quaternion.Euler(0, -90, -60);
        }

        private void ShowOverlay()
        {
            texture = CreateOverlay((Texture2D)Resources.Load("Binoculars_Overlay"));
        }

        private void StartZoom()
        {
            FreezePlayer();
            ZoomCamera();
            ShowOverlay();
            GameManager.GetWeaponCamera().gameObject.SetActive(false);
        }

        private void ZoomCamera()
        {
            vp_FPSCamera camera = GameManager.GetVpFPSCamera();
            originalFOV = GetFieldValue<float>(camera, "m_RenderingFieldOfView");
            camera.ToggleZoom(true);
            camera.SetFOVFromOptions(originalFOV * 0.1f);
        }
    }
}