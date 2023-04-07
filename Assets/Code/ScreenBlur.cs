using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBlur : MonoBehaviour {

	public GameObject UIHolder;
	public GameObject shadeObject;
	public GameObject blurObject;
	public TextMesh[] textObjects;
	public Material[] textMats;
	public Material blurMat;
	public Material shadeMat;

	public void SetupBlurSystem(){
		if (!UIHolder.activeSelf) {UIHolder.SetActive (true);}
		blurMat = blurObject.GetComponent<Renderer> ().material;
		shadeMat = shadeObject.GetComponent<Renderer> ().material;
		textMats = new Material[textObjects.Length];
		for (int i = 0; i < textObjects.Length; i++) {
			textMats [i] = textObjects [i].GetComponent<Renderer> ().material;
		}
	}

	public void ToggleBlur(bool blurOn, bool doFade = false, bool doText = false, float blurTime = 1f){
		StartCoroutine (BlurTiming (blurOn,doFade,doText,blurTime));
	}

	IEnumerator BlurTiming(bool blurOn, bool doFade, bool doText, float blurTime){

		float i = 0.0f;
		float rate = 1.0f / blurTime;

		float startAlpha = 1;
		float endAlpha = 0;

		if (blurOn) { startAlpha = 0; endAlpha = 1;	}

		while (i < 1.0f) {
			i += Time.deltaTime * rate;
			float alphaValue = Mathf.Lerp (startAlpha, endAlpha, i);
			blurMat.SetFloat ("_Size", alphaValue);
			if (doFade) {shadeMat.color = new Color (shadeMat.color.r, shadeMat.color.g, shadeMat.color.b, alphaValue);}
			if(doText){
			foreach (Material textColor in textMats) {
				textColor.color = new Color (textColor.color.r, textColor.color.g, textColor.color.b, alphaValue);
				}
			}
			yield return null;
		}
		blurMat.SetFloat ("_Size", endAlpha);
		shadeMat.color = new Color (shadeMat.color.r, shadeMat.color.g, shadeMat.color.b, endAlpha);
		foreach (Material textColor in textMats) {textColor.color = new Color (shadeMat.color.r, shadeMat.color.g, shadeMat.color.b, endAlpha);	}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
