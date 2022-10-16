using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BlinkEffect : MonoBehaviour {

    [SerializeField] Color[] colors;
    [SerializeField] float blinkTime = 6f;


    public  void BlinkSprite() {
        StartCoroutine(BlinkCoroutine());
    }

    IEnumerator BlinkCoroutine() {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        for (int i = 0; i < blinkTime; i++) {
            foreach (Color color in colors) {
                sprite.color = color;
                yield return new WaitForSeconds(0.1f);
            }
        }

        sprite.color = Color.white;
    }
}

