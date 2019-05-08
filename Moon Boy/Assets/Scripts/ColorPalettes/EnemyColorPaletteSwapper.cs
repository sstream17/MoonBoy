using UnityEngine;

namespace ActionCode.ColorPalettes
{
    /// <summary>
    /// Use this component to swap sprite's texture palettes at runtime.
    /// To create a new Color Palette Asset go to Project Tab and Right Click > Create > Action Code > Color Palette.
    /// Important: The sprite texture asset should be marked Read/Write Enabled.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class EnemyColorPaletteSwapper : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        public ColorPalette defaultPalette;

        private int _lastPalletId;
        private Point2[][] _colorMatrix;


        private void Reset()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }


        private void Awake()
        {
            _colorMatrix = defaultPalette.GetColorPositionsMatrix();
            _lastPalletId = defaultPalette.Id;
        }


        private void OnDisable()
        {
            SwitchDefaultPalette();
        }


        public void SwitchDefaultPalette()
        {
            SwitchPalette(defaultPalette);
        }


        public void SwitchPalette(ColorPalette palette)
        {
            if (defaultPalette == null || palette == null)
            {
                Debug.LogErrorFormat("Default palette or palette are null.", defaultPalette.name, palette.name);
                return;
            }
            else if (_lastPalletId == palette.Id) return;
            else if (palette.colors.Length == 0)
            {
                Debug.LogErrorFormat("Palette {0} is empty.", palette.name);
                return;
            }
            else if (defaultPalette.colors.Length != palette.colors.Length)
            {
                Debug.LogErrorFormat("{0} size is different from {1} size.", defaultPalette.name, palette.name);
                return;
            }
            else if (_colorMatrix == null)
            {
                Debug.LogError("Colors Position Matrix is not initialized.");
                return;
            }

            Texture2D texture = _spriteRenderer.sprite.texture;
            if (texture.format == TextureFormat.ARGB32 ||
                texture.format == TextureFormat.RGBA32 ||
                texture.format == TextureFormat.RGB24 ||
                texture.format == TextureFormat.Alpha8 ||
                texture.format == TextureFormat.RFloat ||
                texture.format == TextureFormat.RGB9e5Float ||
                texture.format == TextureFormat.RGBAFloat ||
                texture.format == TextureFormat.RGFloat)
            {
                for (int i = 0; i < palette.colors.Length; i++)
                {
                    foreach (Point2 p in _colorMatrix[i])
                    {
                        texture.SetPixel(p.x, p.y, palette.colors[i]);
                    }
                }
                texture.Apply();
                _lastPalletId = palette.Id;
            }
            else Debug.LogErrorFormat("Unsupported texture format - texture {0} needs to be ARGB32, RGBA32, RGB24, Alpha8 or one of float formats. " +
                "Current value is {1}. Set texture {0} compression to None.",
                texture.name, texture.format);
        }
    }
}
