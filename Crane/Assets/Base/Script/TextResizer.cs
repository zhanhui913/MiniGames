using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextResizer : MonoBehaviour {

	private Text text;
	private TextGenerationSettings settings;
	private TextGenerator generator;
	private bool canStillFit = true;
	private int currentFontSize;

	// Use this for initialization
	void Start () {
		text = this.GetComponent<Text> ();
		generator = new TextGenerator ();

		Vector2 boxSize = new Vector2 (text.rectTransform.rect.width, text.rectTransform.rect.height);

		settings = text.GetGenerationSettings(boxSize);
		currentFontSize = settings.fontSize;

		//In order for this script to work, resizeTextForBestFit must be turned off
		if(settings.resizeTextForBestFit){
			settings.resizeTextForBestFit = false;
		}

		text.fontSize = Resize (text.text);
	}

	private int Resize(string textString){
		while(canStillFit){
			settings.fontSize = currentFontSize;
			generator.Populate(textString, settings);

			if(generator.characterCount - 1 == generator.characterCountVisible){
				canStillFit = false;
				break;
			}else{
				currentFontSize --;
			}
		}
		return currentFontSize;
	}
}
