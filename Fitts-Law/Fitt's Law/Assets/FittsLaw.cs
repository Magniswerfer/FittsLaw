using UnityEngine;
using System.Collections;

public class FittsLaw : MonoBehaviour
{
	public enum InputDevice
	{
		Mouse,
		Joystick
	}

	/*
	 * 
	 * 
	 * 
	 * 
	 * 
	 */

	public InputDevice inputDevice;
	private float clicks;
	private int totalClicks;
	private float misclicks;
	private int x;
	public int clicksPerTurn = 5;
	public int maxClicks;

	public Material activeMaterial;
	public Material inactiveMaterial;

	private bool lClicked = true;
	private bool rClicked = true;
	private bool printed = false;

	private bool startClick = false;
	private bool endClick = false;
	private bool firstClick = false;

	private bool firstTransform = false;
	private bool secondTransform = false;
	private bool thirdTransform = false;
	private bool fourthTransform = false;

	private float xCursor;
	private float xStartValCursor;
	private float xLeftTarget;
	private float xRightTarget;

	private float ID;
	private float amplitude;
	private float cursorPos = 0f;
	private float targetWidth;

	private float startClickVal;
	private float endClickVal;

	private float totalTime;
	private float Timer = 0;

	private string results = "";

	GameObject rightTarget;
	GameObject leftTarget;
	GameObject cursor;

	Vector3 startPos;

	// Use this for initialization
	void Start (){
		//Gemmer positionen på Cursor, LeftTarrget og RightTarget i en variabel.
		xCursor = GameObject.Find ("Cursor").transform.position.x;
		xStartValCursor = GameObject.Find ("Cursor").transform.position.x;
		leftTarget = GameObject.Find ("LeftTarget");
		rightTarget = GameObject.Find ("RightTarget");

		cursor = GameObject.Find ("Cursor");
		startPos = new Vector3(0, 0, 0);
		cursor.transform.position = startPos;
		targetWidth = rightTarget.transform.lossyScale.x;

		//Gør musen usynlig og låser den.
		Cursor.lockState = CursorLockMode.Locked;

		SetSettings ();

		//debug
		print (amplitude);
		print (xRightTarget);

		GameObject.Find ("LeftTarget").transform.localScale = new Vector3 (targetWidth, 20, 0);
		GameObject.Find ("RightTarget").transform.localScale = new Vector3 (targetWidth, 20, 0);	
		GameObject.Find ("Cursor").GetComponent<MeshRenderer> ().material = activeMaterial;

		maxClicks = 3 * clicksPerTurn;
	
	}

	void SetSettings(){

		amplitude = xRightTarget - xStartValCursor;
		xLeftTarget = GameObject.Find ("LeftTarget").transform.position.x;
		xRightTarget = GameObject.Find ("RightTarget").transform.position.x;
		targetWidth = rightTarget.transform.lossyScale.x;

		cursor.gameObject.transform.position = startPos;
		cursorPos = 0;
		print (startPos);
		startClick = false;
		endClick = false;
		lClicked = true;
		rClicked = true;
	}

	// Update is called once per frame
	void Update (){


		//Gemmer positionen på Cursor i en variabel.
		 
		xCursor = GameObject.Find ("Cursor").transform.position.x;

		//Gør musen usynlig og låser den.
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		//tæller tiden der er gået fra spillet startede.
		Timer += Time.deltaTime;

		//Bestemmer musens hastighed(i dette tilfælde musens position). Der skal ændres på float-værdien for at gøre den mere/mindre følsom.
		if (inputDevice == InputDevice.Mouse) {
			float movement = Input.GetAxis ("Mouse X");
			cursorPos += movement * 0.25f;
		}

		//Startposition for Cursor, LeftTarget og RightTarget + Ændrer Cursors farve til at være grøn når spillet startes.
		GameObject.Find ("Cursor").transform.position = new Vector3 (cursorPos, 0, 0);
		 
		ChangeTestParams ();
		HoverAndClick ();
		Misclicks();
		Click();

	}

	void HoverAndClick ()
	{
		//checker om vi lige har klikket på samme knap ellers - Checker om musen holder over LeftTarget, hvis ja, clicks++. 
		if (lClicked) {
			if (xCursor < xLeftTarget + targetWidth / 2 && xCursor > xLeftTarget - targetWidth / 2) {
				if (Input.GetButtonDown ("Fire1")) {
					clicks++;
					misclicks--;
					totalClicks++;
					print ("Clicks: " + clicks);
					firstClick = true;
					lClicked = false;
					rClicked = true;
				}
			}
		}

		//checker om vi lige har klikket på samme knap ellers - Checker om musen holder over RightTarget, hvis ja, clicks++.
		if (rClicked) {
			if (xCursor < xRightTarget + targetWidth / 2 && xCursor > xRightTarget - targetWidth / 2) {
				if (Input.GetButtonDown ("Fire1")) {
					clicks++;
					misclicks--;
					totalClicks++;
					print ("Clicks: " + clicks);
					firstClick = true;
					rClicked = false;
					lClicked = true;
				}
			}
		}
	}

	void Click ()
	{
		//Gemmer Timer værdien fra første klik
		if (!startClick) {
			if (clicks == 1) {
				startClickVal = Timer;
				startClick = true;
				print ("DEBUG: TIMER RESET");
			}
		}
		
		//Gemmer Timer værdien fra sidste klik
		if (!endClick) {
			if (clicks == clicksPerTurn) {
				endClickVal = Timer;
				endClick = true;
			}
		}
		
		//printer en tid, når vi har klikket ti gange.
		if (!printed) {
			if (clicks == clicksPerTurn) {						
				print ("Du har klikket " + clicksPerTurn + " gange på " + (endClickVal - startClickVal) + " sekunder.");
				printed = true;

			}
		}
	}

	void ChangeTestParams(){

		//transform object
		if (totalClicks == clicksPerTurn) {

			if(firstTransform == false){

				rightTarget.gameObject.transform.position += new Vector3 (3, 0, 0);
				leftTarget.gameObject.transform.position += new Vector3 (-3, 0, 0);
				firstTransform = true;
				SetSettings();
				results += "Test 1:" +  "\n";
				SettingsUsed();

				ResetSettings();



			}
			//debug
		}
		
		if (totalClicks == clicksPerTurn*2) {
			if(secondTransform == false){

				rightTarget.gameObject.transform.localScale += new Vector3 (5, 0, 0);
				leftTarget.gameObject.transform.localScale += new Vector3 (5, 0, 0);
				secondTransform = true;
				SetSettings();
				results += "\n" + "Test 2:" +  "\n";
				SettingsUsed();
				ResetSettings();

			}
		}

		if (totalClicks == clicksPerTurn * 3) {
			if (thirdTransform == false) {
				
				rightTarget.gameObject.transform.position += new Vector3 (2, 0, 0);
				leftTarget.gameObject.transform.position += new Vector3 (-2, 0, 0);

				rightTarget.gameObject.transform.localScale += new Vector3 (-1, 0, 0);
				leftTarget.gameObject.transform.localScale += new Vector3 (-1, 0, 0);

				thirdTransform = true;
				SetSettings ();
				results += "\n" + "Test 3:" + "\n";
				SettingsUsed ();
				ResetSettings();
			}

		}
		if (totalClicks == clicksPerTurn*4) {
			if(fourthTransform == false){

				fourthTransform = true;
				SetSettings();
				results += "\n" + "Test 4:" +  "\n";
				SettingsUsed();
				print ("Result for all test: " + results);

				ResetSettings();
				Debug.Break();
			}
		}


	}

	void Misclicks(){
		if (firstClick == true) {

			if (lClicked == false) {
				if (xCursor > xRightTarget + targetWidth / 2 || xCursor < xRightTarget - targetWidth / 2) {
					
					if (Input.GetButtonDown ("Fire1")) {
						misclicks++;

						//DEBUG
						print ("KLIK ISTEDET PÅ: LEFT " + misclicks);
					}
				}
			}

			if (rClicked == false) {
				if (xCursor > xLeftTarget + targetWidth / 2 || xCursor < xLeftTarget - targetWidth / 2) {
					if (Input.GetButtonDown ("Fire1")) {
						misclicks++;
						//DEBUG
						print ("KLIK ISTEDET PÅ: RIGHT " + misclicks);
					}
				}
			}
		}
	}

	float IndexOfDifficulty(){
		ID = Mathf.Log(amplitude / (targetWidth + 1), 2);
		return ID;
	}
	
	float AverageTime(){
		totalTime = endClickVal - startClickVal;
		return totalTime / (maxClicks - 1);
	}

	float ErrorRate(){
		return misclicks / clicks;
	}

	void SettingsUsed(){
		print (
			"TargetWidth: " + targetWidth + "\n" +
			"Amplitude: " + amplitude + "\n" +
			"Index of Difficulty: " + IndexOfDifficulty () + "\n" +
			"Average Time: " + AverageTime () + "\n" +
		       "Misclicks: " + misclicks + "\n" +
			"Error rate: " + ErrorRate()+ "\n" +
			"Total time:" + totalTime + "\n" +
			"Debug: X start pos:" + xStartValCursor + "\n + " +
			"More debug: X pos:" + xRightTarget + "\n + "


 			);

		results += "TargetWidth: " + targetWidth + "\n" +
			"Amplitude: " + amplitude + "\n" +
			"Index of Difficulty: " + IndexOfDifficulty () + "\n" +
			"Average Time: " + AverageTime () + "\n" +
			"Misclicks: " + misclicks + "\n" +
				"Error rate: " + ErrorRate () + "\n" + 
				"Total time:" + totalTime + "\n" ;
	}

	void ResetSettings() {

		misclicks = 0;
		clicks = 0;
		firstClick = false;

	}
}

