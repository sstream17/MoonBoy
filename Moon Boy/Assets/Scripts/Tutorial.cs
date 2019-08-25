using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public LevelTransition LevelTransition;

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Player player = hitInfo.GetComponent<Player>();
        if (player != null)
        {
            PlayerPrefs.SetInt(PlayerPrefConstants.Tutorial, -1);
            LevelTransition.FadeToLevel(0);
        }
    }
}
