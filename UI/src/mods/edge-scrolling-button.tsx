import { bindValue, trigger, useValue } from "cs2/api";
import { FloatingButton } from "cs2/ui";
import fallbackIcon from "../images/edge-scrolling.svg";
import styles from "./edge-scrolling-button.module.scss";

// Must match ToggleUISystem.kGroup and the binding names on the C# side.
const GROUP = "toggleEdgeScrolling";

const enabled$ = bindValue<boolean>(GROUP, "enabled", false);
const buttonLocation$ = bindValue<string>(GROUP, "buttonLocation", "Off");
const iconLibraryAvailable$ = bindValue<boolean>(GROUP, "iconLibraryAvailable", false);

// Preferred icon from the optional Unified Icon Library mod. We only reference
// this coui:// path when the C# side confirms the library is installed, because
// requesting a missing coui resource crashes the game's UI resource handler.
const UIL_ICON = "coui://uil/Standard/ArrowsHeightLocked.svg";

const ToggleButton = () => {
    const enabled = useValue(enabled$);
    const hasIconLibrary = useValue(iconLibraryAvailable$);
    const icon = hasIconLibrary ? UIL_ICON : fallbackIcon;

    return (
        <FloatingButton
            src={icon}
            className={enabled ? styles.enabled : styles.disabled}
            tooltipLabel={enabled ? "Edge scrolling: on" : "Edge scrolling: off"}
            onSelect={() => trigger(GROUP, "toggle")}
        />
    );
};

// Mounted in the main game UI (top-right floating area); only renders when the
// player has chosen that placement in the mod settings.
export const MainUIButton = () => {
    const location = useValue(buttonLocation$);
    return location === "MainUI" ? <ToggleButton /> : null;
};

// Mounted in the game's Universal Mod menu; only renders when chosen.
export const UniversalMenuButton = () => {
    const location = useValue(buttonLocation$);
    return location === "UniversalModMenu" ? <ToggleButton /> : null;
};
