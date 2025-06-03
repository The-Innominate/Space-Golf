using UnityEngine;

public static class TeleporterFactory
{
    public static Teleporter CreateBasicTeleporter(GameObject teleporterObject)
    {
        var teleporter = teleporterObject.AddComponent<BasicTeleporter>();
        teleporter.Initialize();
        return teleporter;
    }
}
