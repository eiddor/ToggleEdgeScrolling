import { ModRegistrar } from "cs2/modding";
import { MainUIButton, UniversalMenuButton } from "mods/edge-scrolling-button";

const register: ModRegistrar = (moduleRegistry) => {
    // Append to both hook points; each component decides whether to render
    // based on the placement chosen in the mod settings.
    moduleRegistry.append("GameTopRight", MainUIButton);
    moduleRegistry.append("UniversalModMenu", UniversalMenuButton);
};

export default register;