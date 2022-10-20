using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIController : MonoBehaviour
{
    [SerializeField] Image heartImage;
    [SerializeField] Transform heartsPanel;


    [Header("blood bar")]
    [SerializeField] Transform bloodBarContainer;
    [SerializeField] Image bloodBar;
    [SerializeField] Color bloodBarColor;
    [SerializeField] Color disabledBloodBarColor;
    [SerializeField] Image bloodBarSlash;


    public GameObject resetButton;
    public GameObject LooseText;
    public TMP_Text waveText;

    private void Start() {
        for (int i = 0; i < PlayerBehaviour.Player.maxBlood - 1; i++) {
            Instantiate(bloodBarSlash, bloodBarContainer);
        }
    }

    private void Update() {
        if(PlayerBehaviour.Player.currentBlood >= PlayerBehaviour.Player.currentAttackMode.bloodCost) {
            bloodBar.color = bloodBarColor;
        } else {
            bloodBar.color = disabledBloodBarColor;
        }
    }

    public void RemoveLastHeart() {
        int index = PlayerBehaviour.Player.playerLife - 1;
        heartsPanel.GetChild(index).gameObject.SetActive(false);
    }

    public void ToggleElement(GameObject obj) {
        obj.SetActive(!obj.activeSelf);
    }

    public void UpdateWavetext(int wave) {
        waveText.text = $"wave {wave}";
    } 

    public void ChangeBloodBarFillAmount(float amount) {
        print(HandleBloodFillReducer(amount));
        bloodBar.fillAmount += HandleBloodFillReducer(amount);
    }

    public float HandleBloodFillReducer(float amount) {
        float value = amount / PlayerBehaviour.Player.maxBlood;
        return value;
    }
}
