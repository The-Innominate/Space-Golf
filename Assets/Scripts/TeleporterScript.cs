using UnityEngine;

public class TeleporterScript : MonoBehaviour
{
    [SerializeField] private GameObject ConnectedTeleporter1;
    [SerializeField] private GameObject ConnectedTeleporter2;
    [SerializeField, Min(1)] private int teleporterID;

    private bool canTeleport = true;
    private float cooldownTime = 0.5f; // half-second cooldown to avoid loop

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canTeleport) return;

        if (collision.CompareTag("Player"))
        {
            Transform destination = GetDestination(teleporterID);
            if (destination != null)
            {
                // Start cooldown on both teleporters
                StartCoroutine(TeleportWithCooldown(collision.gameObject, destination));
            }
        }
    }

    private System.Collections.IEnumerator TeleportWithCooldown(GameObject player, Transform destination)
    {
        // Prevent re-entry during teleport
        canTeleport = false;

        // Move the player
        player.transform.position = destination.position;

        // Find the destination's script and disable teleport briefly there too
        TeleporterScript destScript = destination.GetComponent<TeleporterScript>();
        if (destScript != null)
        {
            destScript.canTeleport = false;
            yield return new WaitForSeconds(cooldownTime);
            destScript.canTeleport = true;
        }

        // Re-enable this portal too
        yield return new WaitForSeconds(cooldownTime);
        canTeleport = true;
    }

    private void OnValidate()
    {
        teleporterID = Mathf.Clamp(teleporterID, 1, 2);
    }

    private Transform GetDestination(int telePosition)
    {
        switch (telePosition)
        {
            case 1:
                return ConnectedTeleporter1?.transform;
            case 2:
                return ConnectedTeleporter2?.transform;
            default:
                return null;
        }
    }
}
