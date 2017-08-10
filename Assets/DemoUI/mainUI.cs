using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class mainUI : MonoBehaviour {

	// Use this for initialization
	void Start () {


		GameObject btnObj = GameObject.Find("Canvas/initBtn");

		Button initBtn = btnObj.GetComponent<Button>();


	




		initBtn.onClick.AddListener(delegate() {


			Dictionary<string, string> metaData = new Dictionary<string, string>();
			metaData.Add("Name", "alice");
			metaData.Add("gameId", "12340");
			metaData.Add("level", "23");
		
			OneWaySDK.commitMetaData (metaData);

			OneWaySDK.init ("yd9npds4pfb0qufi", "k0tl9yuqpx6p31v2", false);

		});



		GameObject btnObj2 = GameObject.Find("Canvas/showBtn");

		Button showBtn = btnObj2.GetComponent<Button>();
		showBtn.enabled = false;

		showBtn.onClick.AddListener(delegate() {
			OneWaySDK.showPlacementID("2");
		});




		OneWaySDK.onOneWaySDKReadyEvent += ( placement ) =>{
			Debug.Log ("OneWaySDK is Ready for placement: " + placement);

			showBtn.enabled = true;

		};

		OneWaySDK.onOneWaySDKDidStartEvent += ( placement ) =>{
			Debug.Log ("OneWaySDK start to show placement: " + placement);

			showBtn.enabled = false;

		};

		OneWaySDK.onOneWaySDKDidFinishEvent += ( placement ,state) =>{
			Debug.Log ("OneWaySDK Finished placement " + placement +"finish state is :" + state);
		};

		OneWaySDK.onOneWaySDKDidErrorEvent += ( err, msg) =>{
			Debug.Log ("OneWaySDK is err: " + err + ",message:" + msg);
		};

	}

	void Update () {
		
	}
		

	void OnGUI() {
	}
}
