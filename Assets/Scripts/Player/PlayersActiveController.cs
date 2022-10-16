using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersActiveController : MonoBehaviour
{
    public static PlayersActiveController instance;
    public List<GameObject> playersActive;
    [SerializeField] GameObject firstPlayer;
    private void Awake() {
       instance = this;
        playersActive.Add(firstPlayer);

    }
}
