using System.Collections;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
	[SerializeField] Transform camTransform;

	private Vector3 originalPos;

	void OnEnable()
	{
		originalPos = camTransform.localPosition;
	}
	public IEnumerator Shake(float shakeDuration, float decreaseFactor, float shakeAmount)
    {
		while(true)
        {
			if (shakeDuration > 0)
			{
				gameObject.transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
				shakeDuration -= Time.deltaTime * decreaseFactor;
				shakeAmount -= Time.deltaTime * decreaseFactor;
				if (shakeAmount <= 0) shakeAmount = 0;
				yield return new WaitForFixedUpdate();
			}
			else
			{
				gameObject.transform.localPosition = originalPos;
				break;
			}
			yield return null;
		}
	}
}
