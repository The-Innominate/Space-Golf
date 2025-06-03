using UnityEngine;
using System.Collections;

public class BasicTeleporter : Teleporter
{
    private GameObject connectedTeleporter; // Teleporter to send player to
    private bool canTeleport = true;        // Prevents teleport spam
    private float cooldownTime = 0.5f;      // Half-second cooldown

    public override void Initialize(GameObject connectedTeleporter)
    {
        this.connectedTeleporter = connectedTeleporter; // Set destination teleporter
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canTeleport) return;

        if (collision.CompareTag("Player") && connectedTeleporter != null)
        {
            StartCoroutine(TeleportWithCooldown(collision.gameObject));
        }
    }

    private IEnumerator TeleportWithCooldown(GameObject player)
    {
        canTeleport = false;

        // Move player to connected teleporter's position
        player.transform.position = connectedTeleporter.transform.position;

        // Disable teleport on connected teleporter to avoid loop
        var connectedScript = connectedTeleporter.GetComponent<BasicTeleporter>();
        if (connectedScript != null)
        {
            connectedScript.canTeleport = false;
            yield return new WaitForSeconds(cooldownTime);
            connectedScript.canTeleport = true;
        }

        // Re-enable this teleporter after cooldown
        yield return new WaitForSeconds(cooldownTime);
        canTeleport = true;
    }
}
