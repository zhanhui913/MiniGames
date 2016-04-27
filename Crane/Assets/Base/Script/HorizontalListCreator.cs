using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Padding{
	[Range(0.0F, 1.0F)]
	public float top, bottom, middle, left, right;
}

public class HorizontalListCreator : MonoBehaviour {

	/**
	 * Padding class
	 */ 
	public Padding padding;

	/**
	 * Item to instantiate
	 */ 
	public Transform item;

	[Range(0.0F, 1.0F)]
	public float itemWidthPercent;

	protected int currentIndex = 0;

	private int itemWidth, itemHeight;
	private float currentX, currentY;

	public RectTransform parentRect;

	public Canvas rootCanvas;

	[HideInInspector]
	public float scaleFactor;

	public virtual void Awake(){
		this.transform.GetComponent<RectTransform>().anchorMin = Vector2.zero;
		this.transform.GetComponent<RectTransform>().anchorMax = Vector2.up;

		//Change the container's pivot so that the child would be instantiated in the correct position
		this.transform.GetComponent<RectTransform> ().pivot = Vector2.up;
	}

	public virtual void Start () {
		GetCanvasRoot ();
	}

	public void GetCanvasRoot(){
		//rootCanvas = AODManager.instance.currentScreenUICanvas;
		if (rootCanvas != null)
			scaleFactor = rootCanvas.scaleFactor;
	}

	/// <summary>
	/// Instatiate and position the object based on index.
	/// </summary>
	/// <param name="obj">Object.</param>
	/// <param name="thisIndex">Current Index.</param>
	/// <param name="lastIndex">Last index.</param>
	private void PositionThisObject(Transform obj, int thisIndex, int lastIndex){ 
		obj.localScale = Vector3.one;
		obj.GetComponent<RectTransform> ().anchorMin = obj.GetComponent<RectTransform> ().anchorMax = Vector2.up;
		ChangeSize (obj);
		UpdatePosition (obj, thisIndex, lastIndex);
	}

	private void UpdatePosition(Transform obj, int index, int lastIndex){
		float topPadding = padding.top * parentRect.rect.height;
		float bottomPadding = padding.bottom * parentRect.rect.height;
		float leftPadding = padding.left * parentRect.rect.width;
		float middleVerticalPadding = padding.middle * parentRect.rect.width;

		currentY = -topPadding;

		if (index == 0) {
			currentX = leftPadding;	
		} else {
			currentX += (itemWidth + middleVerticalPadding);
		}

		obj.GetComponent<RectTransform> ().localPosition = new Vector3 (currentX, currentY, 1);

		//After adding all elements, make the container envelope all items (for scrolling purposes)
		if(index == lastIndex - 1){
			float containerHeight = parentRect.sizeDelta.y;
			float containerWidth;

			containerWidth = Mathf.Abs(currentX + itemWidth + leftPadding); //left padding to be same value as right padding

			parentRect.sizeDelta = new Vector2(containerWidth, containerHeight);
		}
	}

	//Change the size of the object while keeping its aspect ratio
	private void ChangeSize(Transform obj){
		RectTransform rt = obj.GetComponent<RectTransform> ();

		float itemRelHeight = (1 - padding.top - padding.bottom);

		if (parentRect != null) {
			itemWidth = (int)(itemWidthPercent * parentRect.rect.width );
			itemHeight = (int)(itemRelHeight * parentRect.rect.height);
		} else {
			itemWidth = (int)(itemWidthPercent * (Screen.height * itemWidthPercent));
			itemHeight = (int)(itemRelHeight * (Screen.height * itemWidthPercent));
		}

		rt.sizeDelta = new Vector2 (itemWidth, itemHeight);
	}

	public void Reposition(List<GameObject> list = null){
		GetCanvasRoot ();
		if(list != null){
			for(int i = 0 ; i < list.Count; i++){
				PositionThisObject(list[i].transform,i, list.Count);
			}
		}else{ 
			for(int i = 0 ; i < this.transform.childCount; i++){
				PositionThisObject(this.transform.GetChild(i), i, this.transform.childCount);
			}
		}
	}

	public void ReDraw(){
		currentIndex = 0;
		currentX = currentY = 0.0f;
	}
}