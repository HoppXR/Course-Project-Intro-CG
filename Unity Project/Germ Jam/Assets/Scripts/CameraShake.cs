using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake(float duration, float amplitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            float x = Random.Range(-1.0f, 1.0f) * amplitude;
            float y = Random.Range(-1.0f, 1.0f) * amplitude;
            
            transform.localPosition = new Vector3(x, y, originalPos.z);
            
            elapsedTime += Time.deltaTime;
            
            yield return null;
        }
        
        transform.localPosition = originalPos;
    }
}
