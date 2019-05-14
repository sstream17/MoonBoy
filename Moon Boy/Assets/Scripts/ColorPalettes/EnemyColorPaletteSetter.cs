using UnityEngine;

namespace ActionCode.ColorPalettes {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(EnemyColorPaletteSwapper))]
    public sealed class EnemyColorPaletteSetter : MonoBehaviour {
        public EnemyColorPaletteSwapper swapper;
        public ColorPalette[] palettes;

        private void Reset() {
            swapper = GetComponent<EnemyColorPaletteSwapper>();
        }

        private void Start() {
            swapper.SwitchPalette(palettes[0]);
        }
    }
}
