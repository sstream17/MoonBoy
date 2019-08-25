using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public LevelTransition LevelTransition;

    void Awake()
    {
        var tutorialPref = PlayerPrefs.GetInt(PlayerPrefConstants.Tutorial);

        if (tutorialPref != -1)
        {
            SceneManager.LoadScene(1);
        }
    }
}
