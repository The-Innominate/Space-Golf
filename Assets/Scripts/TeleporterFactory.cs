using UnityEngine;

public static class TeleporterFactory
{
    public static Teleporter CreateBasicTeleporter(GameObject teleporterObject, GameObject connectedTeleporter)
    {
        var teleporter = teleporterObject.AddComponent<BasicTeleporter>();
        teleporter.Initialize(connectedTeleporter);
        return teleporter;
    }

}
