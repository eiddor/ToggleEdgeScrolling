using System.Collections.Generic;
using Colossal;

namespace ToggleEdgeScrolling
{
    public class LocaleEN : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocaleEN(Setting setting)
        {
            m_Setting = setting;
        }

        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                { m_Setting.GetSettingsLocaleID(), "Toggle Edge Scrolling" },
                { m_Setting.GetOptionTabLocaleID(Setting.MainSection), "Main" },

                { m_Setting.GetOptionGroupLocaleID(Setting.BehaviourGroup), "Behaviour" },
                { m_Setting.GetOptionGroupLocaleID(Setting.ButtonGroup), "Toggle button" },
                { m_Setting.GetOptionGroupLocaleID(Setting.KeybindingGroup), "Keybinding" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DisableOnStartup)), "Disable edge scrolling on startup" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DisableOnStartup)), "When enabled, mouse edge scrolling is turned off automatically when the game starts." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DisableInBackground)), "Disable edge scrolling when in background" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DisableInBackground)), "When enabled, mouse edge scrolling is turned off while the game window is not focused, and restored when it regains focus." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Button)), "Toggle button" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Button)), "Where to show a clickable button that toggles edge scrolling. 'Main UI' adds a floating button to the top-right of the screen. 'Universal mod menu' adds it to the game's mods button menu. 'Off' shows no button." },
                { m_Setting.GetEnumValueLocaleID(ButtonLocation.Off), "Off" },
                { m_Setting.GetEnumValueLocaleID(ButtonLocation.MainUI), "Main UI" },
                { m_Setting.GetEnumValueLocaleID(ButtonLocation.UniversalModMenu), "Universal mod menu" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ToggleBinding)), "Toggle edge scrolling" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ToggleBinding)), "Hotkey to enable or disable mouse edge scrolling." },
                { m_Setting.GetBindingKeyLocaleID(Mod.ToggleActionName), "Toggle edge scrolling" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetBindings)), "Reset key bindings" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetBindings)), "Reset the toggle hotkey back to its default (Ctrl + Shift + E)." },
                { m_Setting.GetOptionWarningLocaleID(nameof(Setting.ResetBindings)), "Reset the toggle hotkey to its default value?" },

                { m_Setting.GetBindingMapLocaleID(), "Toggle Edge Scrolling" },
            };
        }

        public void Unload()
        {
        }
    }
}
