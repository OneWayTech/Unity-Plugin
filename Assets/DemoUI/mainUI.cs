using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class mainUI : MonoBehaviour {

	// Use this for initialization
	void Start () {



		Button configureBtn = GameObject.Find("Canvas/configure").GetComponent<Button>();

		configureBtn.onClick.AddListener(delegate() {
			configureBtn.enabled = false;
			configureBtn.GetComponent<Image>().color = Color.gray;

			Dictionary<string, string> metaData = new Dictionary<string, string>();
			metaData.Add("Name", "unity");
			metaData.Add("gameId", "12340");
			metaData.Add("level", "23");
		
			OneWaySDK.commitMetaData (metaData);
			OneWaySDK.setDebug(true);
			OneWaySDK.configure("po4wvdsaaaygsc7t", "7ki7x0p42uah3qh1");

		});



		Button rewarded = GameObject.Find("Canvas/rewarded").GetComponent<Button>();
		rewarded.onClick.AddListener(delegate() {
			rewarded.enabled = false;
			rewarded.GetComponent<Image>().color = Color.gray;
			GameObject.Find("Canvas/rewarded/Text").GetComponent<Text>().text = "Please Wait";


			if(OneWaySDK.isRewardedAdReady()){
				OneWaySDK.showRewardedAd();
			}else{
				OneWaySDK.initRewardedAd();
			}

		});



		//RewardedAd Event
		OneWaySDK.onRewardedAdReadyEvent += () =>{
			Debug.Log ("OneWaySDK RewardedAd Ready");

			rewarded.enabled = true;
			rewarded.GetComponent<Image>().color = new Color(0.007F, 0.6F, 0.917F);
			GameObject.Find("Canvas/rewarded/Text").GetComponent<Text>().text = "Show Rewarded Ad";
		};

		OneWaySDK.onRewardedAdShowEvent += ( tag ) =>{
			Debug.Log ("OneWaySDK RewardedAd Show for tag: " + tag);

			rewarded.enabled = false;
		};

		OneWaySDK.onRewardedAdClickEvent += ( tag ) =>{
			Debug.Log ("OneWaySDK RewardedAd Click for tag: " + tag);
		};

		OneWaySDK.onRewardedAdCloseEvent += ( tag ,state) =>{
			Debug.Log ("OneWaySDK RewardedAd Close for tag: " + tag +" state is: " + state);
		};




		Button interstitial = GameObject.Find("Canvas/interstitial").GetComponent<Button>();

		interstitial.onClick.AddListener(delegate() {

			interstitial.enabled = false;
			interstitial.GetComponent<Image>().color = Color.gray;
			GameObject.Find("Canvas/interstitial/Text").GetComponent<Text>().text = "Please Wait";

			if(OneWaySDK.isInterstitialAdReady()){
				OneWaySDK.showInterstitialAd();
			}else{
				OneWaySDK.initInterstitialAd();
			}

		});

		//InterstitialAd Event
		OneWaySDK.onInterstitialAdReadyEvent += () =>{
			Debug.Log ("OneWaySDK InterstitialAd Ready ");

			interstitial.enabled = true;
			interstitial.GetComponent<Image>().color = new Color(0.007F, 0.6F, 0.917F);
			GameObject.Find("Canvas/interstitial/Text").GetComponent<Text>().text = "Show InitRewarded Ad";
		};

		OneWaySDK.onInterstitialAdShowEvent += ( tag ) =>{
			Debug.Log ("OneWaySDK InterstitialAd Show for tag: " + tag);
		};

		OneWaySDK.onInterstitialAdClickEvent += ( tag ) =>{
			Debug.Log ("OneWaySDK InterstitialAd Click for tag: " + tag);
		};

		OneWaySDK.onInterstitialAdCloseEvent += ( tag ,state) =>{
			Debug.Log ("OneWaySDK InterstitialAd Close for tag: " + tag +" state is: " + state);
		};


		//error
		OneWaySDK.onOneWaySDKDidErrorEvent += ( err, msg) =>{
			Debug.Log ("OneWaySDK is err: " + err + " , message: " + msg);
		};

	}

	void Update () {
		
	}
		

	void OnGUI() {
	}
}
