using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBlur : MonoBehaviour {

	public GameObject UIHolder;
	public GameObject shadeObject;
	public GameObject blurObject;
	public GameObject fadeObject;
	public GameObject[] textHolders;
	public TextMesh[] textObjects;
	public TextMesh[] endTexts;
	public TextMesh gameplayText;
	public Material[] textMats;
	public Material blurMat;
	public Material shadeMat;
	public Material fadeMat;

	public void SetupBlurSystem(){
		if (!UIHolder.activeSelf) {UIHolder.SetActive (true);}
		blurMat = blurObject.GetComponent<Renderer> ().material;
		shadeMat = shadeObject.GetComponent<Renderer> ().material;
		fadeMat = fadeObject.GetComponent<Renderer> ().material;

		textMats = new Material[textObjects.Length];
		for (int i = 0; i < textObjects.Length; i++) {
			textMats [i] = textObjects [i].GetComponent<Renderer> ().material;
		}
		ToggleTextHolders (true);
		ToggleTextHolders (false, 1);
		ToggleTextHolders (false, 2);
	}

	public void ToggleTextHolders(bool turnOn, int toggleRef = -1){
		for (int i = 0; i < textHolders.Length; i++) {
			if (toggleRef == -1 || toggleRef == i) {
				textHolders [i].SetActive (turnOn);
			}
		}
	}

	public void ToggleBlur(bool blurOn, bool doFade = false, bool doText = false, float blurTime = 1f){
		StartCoroutine (BlurTiming (blurOn,doFade,doText,blurTime));
	}

	public void ToggleFade(bool fadeOn, float fadeTime){
		StartCoroutine (FadeTiming (fadeOn, fadeTime));
	}

	IEnumerator BlurTiming(bool blurOn, bool doFade, bool doText, float blurTime){
		blurObject.SetActive (true);
		float i = 0.0f;
		float rate = 1.0f / blurTime;

		float startAlpha = 1;
		float endAlpha = 0;

		if (blurOn) { startAlpha = 0; endAlpha = 1;	}

		while (i < 1.0f) {
			i += Time.deltaTime * rate;
			float alphaValue = Mathf.Lerp (startAlpha, endAlpha, i);
			blurMat.SetFloat ("_Size", alphaValue);
			if(doFade) {shadeMat.color = new Color (shadeMat.color.r, shadeMat.color.g, shadeMat.color.b, alphaValue);}
			if(doText){
			foreach (Material textColor in textMats) {
				textColor.color = new Color (textColor.color.r, textColor.color.g, textColor.color.b, alphaValue);
				}
			}
			yield return null;
		}
		blurMat.SetFloat ("_Size", endAlpha);
		shadeMat.color = new Color (shadeMat.color.r, shadeMat.color.g, shadeMat.color.b, endAlpha);
		foreach (Material textColor in textMats) {textColor.color = new Color (textColor.color.r, textColor.color.g, textColor.color.b, endAlpha);	}
		if (!blurOn) {blurObject.SetActive (false);	}
		Gameboss.isAnimating = false;
	}


	IEnumerator FadeTiming(bool fadeUp, float blurTime){
		float i = 0.0f;
		float rate = 1.0f / blurTime;

		float startAlpha = 1;
		float endAlpha = 0;

		if (fadeUp) { startAlpha = 0; endAlpha = 1;	}

		while (i < 1.0f) {
			i += Time.deltaTime * rate;
			float alphaValue = Mathf.Lerp (startAlpha, endAlpha, i);
			fadeMat.color = new Color (fadeMat.color.r, fadeMat.color.g, fadeMat.color.b, alphaValue);
			yield return null;
		}
		fadeMat.color = new Color (fadeMat.color.r, fadeMat.color.g, fadeMat.color.b, endAlpha);
		Gameboss.isAnimating = false;
	}

	public IEnumerator SeperateTextTiming(bool fadeUp, float blurTime, TextMesh textObject){
		float i = 0.0f;
		float rate = 1.0f / blurTime;

		float startAlpha = 1;
		float endAlpha = 0;

		if (fadeUp) { startAlpha = 0; endAlpha = 1;	}

		while (i < 1.0f) {
			i += Time.deltaTime * rate;
			float alphaValue = Mathf.Lerp (startAlpha, endAlpha, i);
			textObject.color = new Color (textObject.color.r, textObject.color.g, textObject.color.b, alphaValue);

			yield return null;
		}
		textObject.color = new Color (textObject.color.r, textObject.color.g, textObject.color.b, endAlpha);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
