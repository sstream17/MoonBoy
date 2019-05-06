using UnityEngine;

namespace ActionCode.ColorPalettes {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ColorPaletteSwapper))]
    public sealed class ColorPaletteSwapperCycle : MonoBehaviour {
        public ColorPaletteSwapper swapper;
        public ColorPalette[] palettes;

        static public int current_palette_index = 0;

        private void Reset() {
            swapper = GetComponent<ColorPaletteSwapper>();
        }

        private void Start()
        {
            swapper.SwitchPalette(palettes[current_palette_index]);
        }

        public void SwapPalette(int palette_index) {
            if (palettes.Length == 0) {
                return;
            }

            swapper.SwitchPalette(palettes[palette_index]);
            current_palette_index = palette_index;
        }
    }
}
