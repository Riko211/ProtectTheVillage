using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour
{
    private float fps;
    [SerializeField] private Text fpstext;

    private void Start()
    {
        StartCoroutine(FpsUpdate());
    }
    private void Update()
    {
        fps = 1.0f / Time.unscaledDeltaTime;
    }

    IEnumerator FpsUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            fpstext.text = "FPS: " + Mathf.Round(fps).ToString();
        }
    }
}
