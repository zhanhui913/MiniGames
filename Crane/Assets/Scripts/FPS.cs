﻿using UnityEngine;
using System.Collections;

// Attach this to a GUIText to make a frames/second indicator.
//
// It calculates frames/second over each updateInterval,
// so the display does not keep changing wildly.
//
// It is also fairly accurate at very low FPS counts (<10).
// We do this not by simply counting frames per interval, but
// by accumulating FPS for each frame. This way we end up with
// correct overall FPS even if the interval renders something like
// 5.5 frames.
public class FPS : MonoBehaviour {

	public  float updateInterval = 0.5F;
	
	private float accum   = 0; // FPS accumulated over the interval
	private int   frames  = 0; // Frames drawn over the interval
	private float timeleft; // Left time for current interval

	private UILabel fpsText;

	// Use this for initialization
	void Start () {
		/*if( !NGUIText ){
			Debug.Log("UtilityFramesPerSecond needs a NGUIText component!");
			enabled = false;
			return;
		}*/

		fpsText = GetComponent<UILabel> ();

		timeleft = updateInterval; 
	}
	
	// Update is called once per frame
	void Update () {
		timeleft -= Time.deltaTime;
		accum += Time.timeScale/Time.deltaTime;
		++frames;
		
		// Interval ended - update GUI text and start new interval
		if( timeleft <= 0.0 ){
			// display two fractional digits (f2 format)
			float fps = accum/frames;

			fpsText.text = Mathf.Floor(fps)+" FPS";

			timeleft = updateInterval;
			accum = 0.0F;
			frames = 0;
		}
	}
}
