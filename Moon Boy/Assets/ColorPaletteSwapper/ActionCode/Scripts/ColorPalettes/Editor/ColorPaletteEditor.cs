using System.IO;
using UnityEditor;
using UnityEngine;
using ActionCode.ColorPalettes;
using System.Collections.Generic;

namespace ActionCode.Editors
{
    [CustomEditor(typeof(ColorPalette))]
    public sealed class ColorPaletteEditor : Editor
    {
        private SerializedProperty _colorsProperty;
        private SerializedProperty _colorsPositionFilePathProperty;

        private ColorPalette _colorPalette;

        private readonly GUIContent _moveButtonContent = new GUIContent("\u21b4", "Move down");
        private readonly GUIContent _duplicateButtonContent = new GUIContent("+", "Duplicate");
        private readonly GUIContent _deleteButtonContent = new GUIContent("-", "Delete");
        private readonly GUIContent _addColorsManuallyButtonContent = new GUIContent("Add Colors Manually");
        private readonly GUILayoutOption _miniButtonWidth = GUILayout.Width(20f);
        private readonly GUILayoutOption _miniButtonHeight = GUILayout.Height(16f);

        private Sprite _paletteSprite;
        private Texture2D _texture;
        private bool _showColorPositionFileProps = false;

        private void OnEnable()
        {
            _colorPalette = target as ColorPalette;
            _colorsProperty = serializedObject.FindProperty("colors");
            _colorsPositionFilePathProperty = serializedObject.FindProperty("_colorsPositionFilePath");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();

            if (_colorsProperty.arraySize == 0)
            {
                EditorGUILayout.HelpBox("This Collor Palette is empty.", MessageType.Info);
                if (GUILayout.Button(_addColorsManuallyButtonContent))
                {
                    _colorsProperty.InsertArrayElementAtIndex(0);
                    _colorsProperty.GetArrayElementAtIndex(0).colorValue = Color.white;
                }
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                _paletteSprite = EditorGUILayout.ObjectField("From Palette", _paletteSprite, typeof(Sprite), false, _miniButtonHeight) as Sprite;
                EditorGUI.BeginDisabledGroup(_paletteSprite == null);
                if (GUILayout.Button("Add From Palette", EditorStyles.miniButtonRight))
                {
                    CreateFromSpritePalettte(true);
                }
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                for (int i = 0; i < _colorsProperty.arraySize; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(_colorsProperty.GetArrayElementAtIndex(i), new GUIContent("Color-" + i.ToString("d2")));

                    if (GUILayout.Button(_moveButtonContent, EditorStyles.miniButtonLeft, _miniButtonWidth))
                    {
                        _colorsProperty.MoveArrayElement(i, i + 1);
                    }
                    if (GUILayout.Button(_duplicateButtonContent, EditorStyles.miniButtonMid, _miniButtonWidth))
                    {
                        _colorsProperty.InsertArrayElementAtIndex(i);
                    }
                    if (GUILayout.Button(_deleteButtonContent, EditorStyles.miniButtonRight, _miniButtonWidth))
                    {
                        _colorsProperty.DeleteArrayElementAtIndex(i);
                    }
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.Space();

                if (GUILayout.Button("Remove All Colors") &&
                    EditorUtility.DisplayDialog("Attention", "All colors from this palette will be lost.\nAre you sure?", "Yes", "Cancel"))
                {
                    _colorsProperty.ClearArray();
                }

                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                _paletteSprite = EditorGUILayout.ObjectField("Sprite Palette", _paletteSprite, typeof(Sprite), false, _miniButtonHeight) as Sprite;
                EditorGUI.BeginDisabledGroup(_paletteSprite == null);
                if (GUILayout.Button("Replace", EditorStyles.miniButtonLeft))
                {
                    if (_colorsProperty.arraySize > 0)
                    {
                        if (EditorUtility.DisplayDialog("Attention", "This action will replace all current colors from this palette.\nDo you want to proceed?", "Yes", "No"))
                            CreateFromSpritePalettte(true);
                    }
                    else CreateFromSpritePalettte(true);
                }
                else if (GUILayout.Button("Add", EditorStyles.miniButtonRight))
                {
                    CreateFromSpritePalettte(false);
                }
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndHorizontal();

                if(_showColorPositionFileProps = EditorGUILayout.Foldout(_showColorPositionFileProps, "Color Position File"))
                {
                    EditorGUILayout.HelpBox("Color Position File is used to optimize the color swap. " +
                        "This file only need to be set on the palette attached to ColorPaletteSwapper component.", MessageType.Info);

                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.PropertyField(_colorsPositionFilePathProperty);
                    EditorGUI.EndDisabledGroup();

                    EditorGUILayout.BeginHorizontal();
                    _texture = EditorGUILayout.ObjectField("Texture", _texture, typeof(Texture2D), false, _miniButtonHeight) as Texture2D;
                    EditorGUI.BeginDisabledGroup(_texture == null);
                    if (GUILayout.Button("Calculate"))
                    {
                        string assetPath = AssetDatabase.GetAssetPath(_colorPalette);
                        string filePath = assetPath.Split('.')[0];
                        string posFilePath = filePath + "-positions.txt";

                        try
                        {
                            string positions = _colorPalette.GetColorPositionsContent(_texture);

                            StreamWriter writer = new StreamWriter(posFilePath, false);
                            writer.Write(positions);
                            writer.Close();

                            AssetDatabase.ImportAsset(posFilePath);
                            _colorsPositionFilePathProperty.stringValue = posFilePath;
                            Debug.Log("Position text file generate at " + posFilePath);
                        }
                        catch (UnityException e)
                        {
                            EditorUtility.DisplayDialog("Unity Exception", e.Message, "Ok");
                        }
                    }
                    EditorGUI.EndDisabledGroup();
                    EditorGUILayout.EndHorizontal();
                }
            }

            serializedObject.ApplyModifiedProperties();
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_colorPalette, "Changed Color Palette");
            }
        }

        private void CreateFromSpritePalettte(bool clear)
        {
            if (clear) _colorsProperty.ClearArray();
            List<Color> colorList = new List<Color>();

            try
            {
                for (float y = _paletteSprite.rect.y; y < _paletteSprite.rect.yMax; y++)
                {
                    for (float x = _paletteSprite.rect.x; x < _paletteSprite.rect.xMax; x++)
                    {
                        Color color = _paletteSprite.texture.GetPixel((int)x, (int)y);
                        if (!colorList.Contains(color)) colorList.Add(color);
                    }
                }
            }
            catch (UnityException e)
            {
                EditorUtility.DisplayDialog("Unity Exception", e.Message, "Ok");
            }

            for (int i = 0; i < colorList.Count; i++)
            {
                _colorsProperty.InsertArrayElementAtIndex(i);
                _colorsProperty.GetArrayElementAtIndex(i).colorValue = colorList[i];
            }
            _paletteSprite = null;
        }
    }
}