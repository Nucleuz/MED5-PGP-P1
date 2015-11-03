using UnityEngine;
using System.Collections;

public class FogScript : MonoBehaviour {

    Color defaultCol;
    float defaultDensity;

    public float lerpLength;

    void Start() {
        defaultCol = RenderSettings.fogColor;
        defaultDensity = RenderSettings.fogDensity;
    }
    
    public void startSetFogColorToDefault() {
        StartCoroutine(setFogColToDefault());
    }

    public void startSetFogColor(Color newCol) {
        StartCoroutine(setFogColor(newCol));
    }

    public void startSetFogDensityToDefault() {
        StartCoroutine(setFogDensToDefault());
    }

    public void startSetFogDensity(float newDensity) {
        StartCoroutine(setFogDensity(newDensity));
    }

    IEnumerator setFogColor(Color newCol) {

        float startTime = Time.time;
        float endTime = startTime + lerpLength;

        while (Time.time < endTime) {
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, newCol, (Time.time - startTime) / lerpLength);
            yield return null;
        }

    }

    IEnumerator setFogColToDefault() {

        float startTime = Time.time;
        float endTime = startTime + lerpLength;

        Color oldCol = RenderSettings.fogColor;

        while (Time.time < endTime) {
            RenderSettings.fogColor = Color.Lerp(oldCol, defaultCol, (Time.time - startTime) / lerpLength);
            yield return null;
        }
    }

    IEnumerator setFogDensity(float newDensity) {

        float startTime = Time.time;
        float endTime = startTime + lerpLength;

        while (Time.time < endTime) {
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, newDensity, (Time.time - startTime) / lerpLength);
            yield return null;
        }

    }

    IEnumerator setFogDensToDefault() {

        float startTime = Time.time;
        float endTime = startTime + lerpLength;

        float oldDensity = RenderSettings.fogDensity;

        while (Time.time < endTime) {
            RenderSettings.fogDensity = Mathf.Lerp(oldDensity, defaultDensity, (Time.time - startTime) / lerpLength);
            yield return null;
        }

    }

}
