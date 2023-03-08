using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundScroller : MonoBehaviour
{
    [Range(-1f, 1f)]
    public float scrollSpeed = 0.5f;
    private float offset;
    private Material mat;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene scene)
    {
        // Destroy the game object if the scene is the menu scene
        if (scene.name == "Menu")
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        offset -= (Time.deltaTime * scrollSpeed) / 10f;
        mat.SetTextureOffset("_BaseMap", new Vector2(0f, offset));
    }
}
