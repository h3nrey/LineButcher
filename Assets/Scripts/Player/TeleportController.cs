using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    private float[] verticalBounderies => PlayerBehaviour.Player.verticalBounderies;
    private Transform[] tpPoints => PlayerBehaviour.Player.tpPoints;

    private void Update() {
        HandleTeleport();
    }

    private void HandleTeleport() {
        if (!PlayerBehaviour.Player.canTeleport) return;
        Vector2 pos = transform.position;
        if (pos.y >= verticalBounderies[0]) {
            transform.position = new Vector2(pos.x, tpPoints[0].position.y);
        }
        else if (pos.y <= verticalBounderies[1]) {
            transform.position = new Vector2(pos.x, tpPoints[1].position.y);
        }
    }
}
