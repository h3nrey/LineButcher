using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{
    [SerializeField] Image heartImage;
    [SerializeField] Transform heartsPanel;

    public GameObject resetButton;

    public void RemoveLastHeart() {
        int index = PlayerBehaviour.Player.playerLife - 1;
        heartsPanel.GetChild(index).gameObject.SetActive(false);
    }

    public void ToggleElement(GameObject obj) {
        obj.SetActive(!obj.activeSelf);
    }
}
