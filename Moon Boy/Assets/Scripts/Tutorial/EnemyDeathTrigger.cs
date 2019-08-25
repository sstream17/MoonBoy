using UnityEngine;

public class EnemyDeathTrigger : MonoBehaviour
{
    private void OnDestroy()
    {
        ShrinkWeaponSpawn();
    }

    private void ShrinkWeaponSpawn()
    {
        GameObject weaponSpawn = GameObject.FindGameObjectWithTag("Collectible");
        if (weaponSpawn != null)
        {
            GameObject trigger = weaponSpawn.transform.GetChild(0).gameObject;
            if (trigger != null)
            {
                CircleCollider2D circleCollider = trigger.GetComponent<CircleCollider2D>();
                if (circleCollider != null)
                {
                    circleCollider.radius = 0f;
                }
            }
        }
    }
}
