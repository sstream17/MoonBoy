using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActionCode.ColorPalettes;

public class Menu : MonoBehaviour
{
    public enum MenuScreen {Main, LevelSelect, Options, CharacterSelect};

    public int current_color_palette;

    private GameObject currentMenu;
    public GameObject mainMenu;
    public GameObject levelSelectMenu;
    public GameObject optionsMenu;
    public GameObject characterSelectMenu;

    public ColorPaletteSwapperCycle playerColorPalette;


    void Awake() {
        currentMenu = mainMenu;
        SwitchMenu(MenuScreen.Main);
    }

    void Start() {
        current_color_palette = GameControl.control.current_palette_index;
        GameControl.control.playerAmmo = 50;
        GameControl.control.playerLives = 3;
    }


    public void SwitchMenu(MenuScreen menuToOpen) {
        GameObject newMenu;
        switch(menuToOpen) {
            case MenuScreen.Main:
                newMenu = mainMenu;
                break;
            case MenuScreen.LevelSelect:
                newMenu = levelSelectMenu;
                break;
            case MenuScreen.Options:
                newMenu = optionsMenu;
                break;
            case MenuScreen.CharacterSelect:
                newMenu = characterSelectMenu;
                break;
            default:
                newMenu = mainMenu;
                break;
        }
        currentMenu.SetActive(false);
        currentMenu = newMenu;
        currentMenu.SetActive(true);
    }


    public void OpenMainMenu() {
        SwitchMenu(MenuScreen.Main);
    }


    public void OpenLevelSelectMenu() {
        SwitchMenu(MenuScreen.LevelSelect);
    }


    public void OpenOptionsMenu() {
        SwitchMenu(MenuScreen.Options);
    }


    public void OpenCharacterSelectMenu() {
        SwitchMenu(MenuScreen.CharacterSelect);
    }


    public void OnPaletteSave() {
        current_color_palette = GameControl.control.current_palette_index;
        OpenOptionsMenu();
    }


    public void OnPaletteExit() {
        GameControl.control.current_palette_index = current_color_palette;
        playerColorPalette.SwapPalette(GameControl.control.current_palette_index);
        OpenOptionsMenu();
    }
}
