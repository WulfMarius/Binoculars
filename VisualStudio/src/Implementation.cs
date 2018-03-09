using ModComponentMapper;
using UnityEngine;

namespace Binoculars
{
    public class Implementation
    {
        private float originalFOV;
        private UITexture texture;

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

        private static void ShowButtonPopups()
        {
            EquipItemPopupUtils.ShowItemPopups(string.Empty, Localization.Get("GAMEPLAY_Use"), false, false, false, true);
        }

        private void EndZoom()
        {
            if (!GameManager.GetVpFPSCamera().IsZoomed)
            {
                return;
            }

            ModUtils.UnfreezePlayer();
            RestoreCamera();
            HideOverlay();
            GameManager.GetWeaponCamera().gameObject.SetActive(true);
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

        private void ShowOverlay()
        {
            texture = UIUtils.CreateOverlay((Texture2D)Resources.Load("Binoculars_Overlay"));
        }

        private void StartZoom()
        {
            ModUtils.FreezePlayer();
            ZoomCamera();
            ShowOverlay();
            GameManager.GetWeaponCamera().gameObject.SetActive(false);
        }

        private void ZoomCamera()
        {
            vp_FPSCamera camera = GameManager.GetVpFPSCamera();
            originalFOV = ModUtils.GetFieldValue<float>(camera, "m_RenderingFieldOfView");
            camera.ToggleZoom(true);
            camera.SetFOVFromOptions(originalFOV * 0.1f);
        }
    }
}