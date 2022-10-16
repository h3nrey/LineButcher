using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
public class SceneCaller : MonoBehaviour
{
    public void CallScene(string sceneName) {
        Coroutines.DoAfter(() => SceneManager.LoadScene(sceneName),
            0, this);
    }
}

public class CallSceneProp {
    public float sceneCallcooldown;
    public string sceneName {  get; set; }

    public CallSceneProp(float cooldown, string name) {
        sceneCallcooldown = cooldown;
        sceneName = name;
    }
}
