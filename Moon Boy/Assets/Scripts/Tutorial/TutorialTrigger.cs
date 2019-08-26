using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public Tutorial Tutorial;
    public Tutorial.Area Area;

    private readonly float cameraTwoHeight = 16.2f;
    private readonly float cameraThreeHeight = 16.2f;
    private readonly float cameraFourHeight = 15.9f;

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Player player = hitInfo.GetComponent<Player>();
        if (player != null)
        {
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            playerMovement.animator.SetFloat("Speed", 0f);
            playerMovement.animator.SetFloat("VerticalSpeed", 0f);
            playerMovement.enabled = false;

            float cameraHeight = Mathf.Infinity;
            switch (Area)
            {
                case Tutorial.Area.Shoot:
                    Tutorial.ActivateCamera(1);
                    cameraHeight = cameraTwoHeight;
                    break;

                case Tutorial.Area.Swap:
                    ExpandWeaponSpawn();
                    Tutorial.ActivateCamera(2);
                    cameraHeight = cameraThreeHeight;
                    break;

                case Tutorial.Area.Toggle:
                    Tutorial.ActivateCamera(3);
                    cameraHeight = cameraFourHeight;
                    Tutorial.WaitingForGrenade = true;
                    break;
            }

            StartCoroutine(Tutorial.WaitToStartAnimation(cameraHeight, Area, Tutorial.StartTapAnimation));
        }
    }

    private void ExpandWeaponSpawn()
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
                    circleCollider.radius = 0.8f;
                }
            }
        }
    }
}
