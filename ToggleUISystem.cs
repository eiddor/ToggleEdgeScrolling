using System;
using Colossal.UI.Binding;
using Game.Modding;
using Game.SceneFlow;
using Game.UI;
using Unity.Entities;

namespace ToggleEdgeScrolling
{
    // Exposes the mod's state to the in-game UI and receives the toggle action
    // from the floating / universal-mod-menu button.
    public partial class ToggleUISystem : UISystemBase
    {
        public const string kGroup = "toggleEdgeScrolling";

        private ValueBinding<bool> m_EnabledBinding;
        private ValueBinding<string> m_LocationBinding;
        private ValueBinding<bool> m_IconLibraryBinding;

        protected override void OnCreate()
        {
            base.OnCreate();

            m_EnabledBinding = new ValueBinding<bool>(kGroup, "enabled", EdgeScrollingController.Enabled);
            m_LocationBinding = new ValueBinding<string>(kGroup, "buttonLocation", GetLocationKey());
            m_IconLibraryBinding = new ValueBinding<bool>(kGroup, "iconLibraryAvailable", IsIconLibraryInstalled());

            AddBinding(m_EnabledBinding);
            AddBinding(m_LocationBinding);
            AddBinding(m_IconLibraryBinding);
            AddBinding(new TriggerBinding(kGroup, "toggle", OnToggle));

            EdgeScrollingController.StateChanged += OnStateChanged;
        }

        protected override void OnDestroy()
        {
            EdgeScrollingController.StateChanged -= OnStateChanged;
            base.OnDestroy();
        }

        // Called when the button placement setting changes so the UI updates live.
        public void RefreshLocation()
        {
            m_LocationBinding?.Update(GetLocationKey());
        }

        private void OnToggle()
        {
            EdgeScrollingController.Toggle();
        }

        private void OnStateChanged()
        {
            m_EnabledBinding?.Update(EdgeScrollingController.Enabled);
        }

        private static string GetLocationKey()
        {
            return Mod.Setting != null ? Mod.Setting.Button.ToString() : ButtonLocation.Off.ToString();
        }

        // Detects whether algernon's Unified Icon Library mod is installed so the UI
        // can use its icon. When it is absent we must never reference coui://uil/...
        // because requesting a missing coui resource crashes the UI resource handler.
        private static bool IsIconLibraryInstalled()
        {
            ModManager modManager = GameManager.instance?.modManager;
            if (modManager == null)
                return false;

            foreach (ModManager.ModInfo info in modManager)
            {
                if (info?.name != null && info.name.IndexOf("UnifiedIconLibrary", StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
            }

            return false;
        }
    }
}
