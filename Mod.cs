using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Game;
using Game.Input;
using Game.Modding;
using Game.SceneFlow;
using Game.Settings;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ToggleEdgeScrolling
{
    public class Mod : IMod
    {
        public const string ToggleActionName = "ToggleEdgeScrolling";

        public static ILog log = LogManager.GetLogger($"{nameof(ToggleEdgeScrolling)}.{nameof(Mod)}").SetShowsErrorsInUI(false);

        public static Setting Setting;

        private static ToggleUISystem s_UISystem;

        private ProxyAction m_ToggleAction;

        public void OnLoad(UpdateSystem updateSystem)
        {
            log.Info(nameof(OnLoad));

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
                log.Info($"Current mod asset at {asset.path}");

            Setting = new Setting(this);
            Setting.RegisterInOptionsUI();
            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(Setting));
            AssetDatabase.global.LoadSettings(nameof(ToggleEdgeScrolling), Setting, new Setting(this));

            Setting.RegisterKeyBindings();

            m_ToggleAction = Setting.GetAction(ToggleActionName);
            if (m_ToggleAction != null)
            {
                m_ToggleAction.shouldBeEnabled = true;
                m_ToggleAction.onInteraction += OnToggleInteraction;
            }
            else
            {
                log.Warn($"Could not resolve input action '{ToggleActionName}'.");
            }

            // Establish the initial intended state before the UI system reads it.
            EdgeScrollingController.Initialize();

            s_UISystem = updateSystem.World.GetOrCreateSystemManaged<ToggleUISystem>();
            updateSystem.UpdateAt<ToggleUISystem>(SystemUpdatePhase.UIUpdate);

            Application.focusChanged += OnApplicationFocusChanged;
        }

        public void OnDispose()
        {
            log.Info(nameof(OnDispose));

            Application.focusChanged -= OnApplicationFocusChanged;

            if (m_ToggleAction != null)
            {
                m_ToggleAction.onInteraction -= OnToggleInteraction;
                m_ToggleAction.shouldBeEnabled = false;
                m_ToggleAction = null;
            }

            s_UISystem = null;

            if (Setting != null)
            {
                Setting.UnregisterInOptionsUI();
                Setting = null;
            }
        }

        // Called by the Setting when the button placement option changes.
        public static void OnButtonLocationChanged()
        {
            s_UISystem?.RefreshLocation();
        }

        private void OnToggleInteraction(ProxyAction action, InputActionPhase phase)
        {
            if (phase != InputActionPhase.Performed)
                return;

            EdgeScrollingController.Toggle();
        }

        private void OnApplicationFocusChanged(bool focused)
        {
            EdgeScrollingController.OnFocusChanged(focused);
        }
    }
}
