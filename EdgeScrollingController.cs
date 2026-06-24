using System;
using Game.Settings;

namespace ToggleEdgeScrolling
{
    // Central owner of the edge-scrolling state. Keeps the user's intended
    // on/off choice separate from any temporary suspension caused by the game
    // running in the background, and pushes the resolved value to the game.
    public static class EdgeScrollingController
    {
        private static bool s_UserEnabled;
        private static bool s_BackgroundSuspended;

        // Raised whenever the user's intended state changes (hotkey or UI button).
        public static event Action StateChanged;

        // The user's intended edge-scrolling state (drives the button's selected look).
        public static bool Enabled => s_UserEnabled;

        public static void Initialize()
        {
            s_UserEnabled = Mod.Setting != null && Mod.Setting.DisableOnStartup ? false : GetGameEdgeScrolling();
            s_BackgroundSuspended = false;
            Apply();
        }

        public static void Toggle()
        {
            SetEnabled(!s_UserEnabled);
        }

        public static void SetEnabled(bool enabled)
        {
            if (s_UserEnabled == enabled)
                return;

            s_UserEnabled = enabled;
            Mod.log.Info($"Edge scrolling intent changed -> {s_UserEnabled}");
            Apply();
            StateChanged?.Invoke();
        }

        public static void OnFocusChanged(bool focused)
        {
            if (Mod.Setting == null)
                return;

            if (!Mod.Setting.DisableInBackground)
            {
                if (s_BackgroundSuspended)
                {
                    s_BackgroundSuspended = false;
                    Apply();
                }

                return;
            }

            s_BackgroundSuspended = !focused;
            Apply();
        }

        // Resolves the effective state from the user's choice and the
        // background-suspension rule, then pushes it to the game.
        private static void Apply()
        {
            bool enabled = s_UserEnabled && !(Mod.Setting != null && Mod.Setting.DisableInBackground && s_BackgroundSuspended);
            SetGameEdgeScrolling(enabled);
        }

        private static bool GetGameEdgeScrolling()
        {
            GameplaySettings gameplay = SharedSettings.instance?.gameplay;
            return gameplay != null && gameplay.edgeScrolling;
        }

        private static void SetGameEdgeScrolling(bool enabled)
        {
            GameplaySettings gameplay = SharedSettings.instance?.gameplay;
            if (gameplay == null || gameplay.edgeScrolling == enabled)
                return;

            gameplay.edgeScrolling = enabled;
            gameplay.Apply();
            Mod.log.Info($"Edge scrolling set to {enabled}.");
        }
    }
}
