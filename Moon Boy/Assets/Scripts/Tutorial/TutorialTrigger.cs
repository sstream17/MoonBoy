using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public Tutorial Tutorial;
    public Tutorial.Area Area;

    private readonly Vector2 cameraTwoPosition = new Vector2(114f,16.2f);
    private readonly Vector2 cameraThreePosition = new Vector2(125f, 16.2f);
    private readonly Vector2 cameraFourPosition = new Vector2(152f, 15.9f);

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

            Vector2 cameraPosition = new Vector2(Mathf.Infinity, Mathf.Infinity);
            switch (Area)
            {
                case Tutorial.Area.Shoot:
                    Tutorial.ActivateCamera(1);
                    cameraPosition = cameraTwoPosition;
                    break;

                case Tutorial.Area.Swap:
                    ExpandWeaponSpawn();
                    Tutorial.ActivateCamera(2);
                    cameraPosition = cameraThreePosition;
                    break;

                case Tutorial.Area.Toggle:
                    Tutorial.ActivateCamera(3);
                    cameraPosition = cameraFourPosition;
                    break;
            }

            StartCoroutine(Tutorial.WaitToStartAnimation(cameraPosition, Area, Tutorial.StartTapAnimation));
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
