using Colossal.IO.AssetDatabase;
using Game.Input;
using Game.Modding;
using Game.Settings;

namespace ToggleEdgeScrolling
{
    public enum ButtonLocation
    {
        Off,
        MainUI,
        UniversalModMenu,
    }

    [FileLocation(nameof(ToggleEdgeScrolling))]
    [SettingsUIGroupOrder(BehaviourGroup, ButtonGroup, KeybindingGroup)]
    [SettingsUIShowGroupName(BehaviourGroup, ButtonGroup, KeybindingGroup)]
    [SettingsUIKeyboardAction(Mod.ToggleActionName, ActionType.Button)]
    public class Setting : ModSetting
    {
        public const string MainSection = "Main";

        public const string BehaviourGroup = "Behaviour";
        public const string ButtonGroup = "Button";
        public const string KeybindingGroup = "Keybinding";

        public Setting(IMod mod) : base(mod)
        {
        }

        [SettingsUISection(MainSection, BehaviourGroup)]
        public bool DisableOnStartup { get; set; }

        [SettingsUISection(MainSection, BehaviourGroup)]
        public bool DisableInBackground { get; set; }

        [SettingsUISection(MainSection, ButtonGroup)]
        public ButtonLocation Button
        {
            get => m_Button;
            set
            {
                m_Button = value;
                Mod.OnButtonLocationChanged();
            }
        }

        private ButtonLocation m_Button = ButtonLocation.MainUI;

        [SettingsUIKeyboardBinding(BindingKeyboard.E, Mod.ToggleActionName, ctrl: true, shift: true)]
        [SettingsUISection(MainSection, KeybindingGroup)]
        public ProxyBinding ToggleBinding { get; set; }

        [SettingsUIButton]
        [SettingsUIConfirmation]
        [SettingsUISection(MainSection, KeybindingGroup)]
        public bool ResetBindings
        {
            set
            {
                ResetKeyBindings();
            }
        }

        public override void SetDefaults()
        {
            DisableOnStartup = false;
            DisableInBackground = true;
            Button = ButtonLocation.MainUI;
        }
    }
}
