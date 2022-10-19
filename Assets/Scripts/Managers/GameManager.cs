using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Game;
    [SerializeField] public UIController UI;
    private PlayerBehaviour _player;

    private void Awake() {
        Game = this;
    }
    public void TooglePause() {
        if (Time.timeScale == 0) {
            Time.timeScale = 1;
            return;
        }
        else {
            Time.timeScale = 0;
        }
    }

    public void EndGame() {
        TooglePause();
        UI.ToggleElement(UI.resetButton);
    }
    
}
