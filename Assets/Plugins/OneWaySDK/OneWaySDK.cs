using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Threading; 

public class OneWaySDK : MonoBehaviour
{
	public static AndroidJavaObject _plugin;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	public enum OneWaySDKPlacementState
	{

     //A state that indicates that the placement is ready to show an ad. The `show:` selector can be called.
	kOneWaySDKPlacementStateReady = 0,
	
     // A state that indicates that no state is information is available.
     // @warning This state can that OneWaySDK is not initialized or that the placement is not correctly configured in the oneway ads admin tool.
	kOneWaySDKPlacementStateNotAvailable,
	
	//  A state that indicates that the placement is currently disabled. The placement can be enabled in the oneway ads admin tools.
	kOneWaySDKPlacementStateDisabled,
	
	//  A state that indicates that the placement is not currently ready, but will be in the future.
	//  @note This state most likely indicates that the ad content is currently caching.
	kOneWaySDKPlacementStateWaiting,
	
	// A state that indicates that the placement is properly configured, but there are currently no ads available for the placement.
	kOneWaySDKPlacementStateNoFill
	};

	public enum OneWaySDKFinishState
	{
		kOneWaySDKFinishStateError = 0,

		kOneWaySDKFinishStateSkipped ,
	
		kOneWaySDKFinishStateCompleted
	};

	public enum OneWaySDKError
	{
	
     // An error that indicates failure due to `OneWaySDK` currently being uninitialized.
	kOneWaySDKErrorNotInitialized = 0,
	
     // An error that indicates failure due to a failure in the initialization process.
	kOneWaySDKErrorInitializedFailed,
	
     // An error that indicates failure due to attempting to initialize `OneWaySDK` with invalid parameters.
	kOneWaySDKErrorInvalidArgument,
	
     // An error that indicates failure of the video player.
	kOneWaySDKErrorVideoPlayerError,
	
     // An error that indicates failure due to having attempted to initialize the `OneWaySDK` class in an invalid environment.
	kOneWaySDKErrorInitSanityCheckFail,
	
     // An error that indicates failure due to the presence of an ad blocker.
	kOneWaySDKErrorAdBlockerDetected,
	
     // An error that indicates failure due to inability to read or write a file.
	kOneWaySDKErrorFileIoError,
	
     // An error that indicates failure due to a bad device identifier.
	kOneWaySDKErrorDeviceIdError,

	// An error that indicates a failure when attempting to show an ad.
	kOneWaySDKErrorShowError,
	
	// An error that indicates an internal failure in `OneWaySDK`.
	kOneWaySDKErrorInternalError,
	
	// A state that indicates that the SDK is properly configured, but there are currently no ads available.
	kOneWaySDKCampaignNoFill
	}

	#region Constructor and Lifecycle

	static OneWaySDK()
	{
//		 try/catch this so that we can warn users if they try to stick this script on a GO manually

		try
		{
			// create a new obj for our manager
			var obj = new GameObject( "OneWaySDK" );
			obj.AddComponent<OneWaySDK>();
			DontDestroyOnLoad(obj);
		}
		catch( UnityException ) {
			Debug.LogWarning ("It looks like you have the OneWaySDK on a GameObject in your scene. Please remove the script from your scene.");
		}



		#if UNITY_ANDROID

		using( var pluginClass = new AndroidJavaClass( "mobi.oneway.OneWaySdkPlugin" ) )
		_plugin = pluginClass.CallStatic<AndroidJavaObject>( "instance" );
		#endif


	}


	// used to ensure the VungleManager will always be in the scene to avoid SendMessage logs if the user isn't using any events

	public static void noop(){}

	#endregion



	#region Events
	//RewardedAd
	public static event Action onRewardedAdReadyEvent;
	public static event Action <string> onRewardedAdShowEvent;
	public static event Action <string> onRewardedAdClickEvent;
	public static event Action <string,string> onRewardedAdCloseEvent;
    public static event Action <string,string,string> onRewardedAdFinishEvent;

	//InterstitialAd
	public static event Action onInterstitialAdReadyEvent;
	public static event Action <string> onInterstitialAdShowEvent;
	public static event Action <string> onInterstitialAdClickEvent;
	public static event Action <string,string> onInterstitialAdCloseEvent;

	//InterstitialImageAd
	public static event Action onInterstitialImageAdReadyEvent;
	public static event Action <string> onInterstitialImageAdShowEvent;
	public static event Action <string> onInterstitialImageAdClickEvent;
	public static event Action <string,string> onInterstitialImageAdCloseEvent;

	//error
	public static event Action <string,string> onOneWaySDKDidErrorEvent;

	#endregion

	//RewardedAd
	//Ready
	void onRewardedAdReady () {
		try{
			onRewardedAdReadyEvent();
		}catch(Exception e){
			Debug.LogWarning("-------------- OneWaySDK Warning : Method 'onRewardedAdReadyEvent' not implemented --------------\n" + e.Message);
		}
	}
	//Show
	void onRewardedAdShow (string tag) {
		try{
			onRewardedAdShowEvent(tag);
		}catch(Exception e){
			Debug.LogWarning("-------------- OneWaySDK Warning : Method 'onRewardedAdShowEvent' not implemented --------------\n" + e.Message);
		}
	}
	//Click
	void onRewardedAdClick (string tag) {
		try{
			onRewardedAdClickEvent(tag);
		}catch(Exception e){
			Debug.LogWarning("-------------- OneWaySDK Warning : Method 'onRewardedAdClickEvent' not implemented --------------\n" + e.Message);
		}
	}
	//close
	void onRewardedAdClose (string info) {
		Dictionary<string,object> attrs = (Dictionary<string,object>) MiniJSONV.Json.Deserialize(info);
		try{
			onRewardedAdCloseEvent(attrs["tag"].ToString(),attrs["state"].ToString());
		}catch(Exception e){
			Debug.LogWarning("-------------- OneWaySDK Warning : Method 'onRewardedAdCloseEvent' not implemented --------------\n" + e.Message);
		}
	}

    //finish
        void onRewardedAdFinish (string info) {
        Dictionary<string,object> attrs = (Dictionary<string,object>) MiniJSONV.Json.Deserialize(info);
        try{
        onRewardedAdFinishEvent(attrs["tag"].ToString(),attrs["state"].ToString(),attrs["sessionId"].ToString());
        }catch(Exception e){
        Debug.LogWarning("-------------- OneWaySDK Warning : Method 'onRewardedAdFinishEvent' not implemented --------------\n" + e.Message);
        }
    }



	//InterstitialAd
	//Ready
	void onInterstitialAdReady () {
		try{
			onInterstitialAdReadyEvent();
		}catch(Exception e){
			Debug.LogWarning("-------------- OneWaySDK Warning : Method 'onInterstitialAdReadyEvent' not implemented --------------\n" + e.Message);
		}
	}
	//Show
	void onInterstitialAdShow (string tag) {
		try{
			onInterstitialAdShowEvent(tag);
		}catch(Exception e){
			Debug.LogWarning("-------------- OneWaySDK Warning : Method 'onInterstitialAdShowEvent' not implemented --------------\n" + e.Message);
		}
	}
	//Click
	void onInterstitialAdClick (string tag) {
		try{
			onInterstitialAdClickEvent(tag);
		}catch(Exception e){
			Debug.LogWarning("-------------- OneWaySDK Warning : Method 'onInterstitialAdClickEvent' not implemented --------------\n" + e.Message);
		}
	}
	//close
	void onInterstitialAdClose (string info) {
		Dictionary<string,object> attrs = (Dictionary<string,object>)MiniJSONV.Json.Deserialize (info);
		try {
			onInterstitialAdCloseEvent (attrs["tag"].ToString (), attrs ["state"].ToString ());
		} catch (Exception e) {
			Debug.LogWarning ("-------------- OneWaySDK Warning : Method 'onInterstitialAdCloseEvent' not implemented --------------\n" + e.Message);
		}
	}

	//InterstitialImageAd
	//Ready
	void onInterstitialImageAdReady () {
		try{
			onInterstitialImageAdReadyEvent();
		}catch(Exception e){
			Debug.LogWarning("-------------- OneWaySDK Warning : Method 'onInterstitialImageAdReadyEvent' not implemented --------------\n" + e.Message);
		}
	}
	//Show
	void onInterstitialImageAdShow (string tag) {
		try{
			onInterstitialImageAdShowEvent(tag);
		}catch(Exception e){
			Debug.LogWarning("-------------- OneWaySDK Warning : Method 'onInterstitialImageAdShowEvent' not implemented --------------\n" + e.Message);
		}
	}
	//Click
	void onInterstitialImageAdClick (string tag) {
		try{
			onInterstitialImageAdClickEvent(tag);
		}catch(Exception e){
			Debug.LogWarning("-------------- OneWaySDK Warning : Method 'onInterstitialImageAdClickEvent' not implemented --------------\n" + e.Message);
		}
	}
	//close
	void onInterstitialImageAdClose (string info) {
		Dictionary<string,object> attrs = (Dictionary<string,object>)MiniJSONV.Json.Deserialize (info);
		try {
			onInterstitialImageAdCloseEvent (attrs["tag"].ToString (), attrs ["state"].ToString ());
		} catch (Exception e) {
			Debug.LogWarning ("-------------- OneWaySDK Warning : Method 'onInterstitialImageAdCloseEvent' not implemented --------------\n" + e.Message);
		}
	}


	//Error 
	void onOneWaySDKDidError (string msg) {

		Dictionary<string,object> attrs = (Dictionary<string,object>) MiniJSONV.Json.Deserialize( msg );

		try{
			onOneWaySDKDidErrorEvent (attrs["error"].ToString(),attrs["message"].ToString());
		}catch(Exception e){
			Debug.LogWarning ("-------------- OneWaySDK Warning : Method 'onOneWaySDKDidErrorEvent ' not implemented --------------\n" + e.Message);
		}
	}


	#if UNITY_IPHONE
	//  Configure
	[DllImport ("__Internal")]
	private static extern void _oneWaySDKConfigure(string PId);

	// RewardedAd
	[DllImport ("__Internal")]
	private static extern void _oneWaySDKInitRewardedAd();

	[DllImport ("__Internal")]
	private static extern bool _oneWaySDKRewardedAdIsReady();

	[DllImport ("__Internal")]
	private static extern void _oneWaySDKShowRewardedAd(string tag);




	//InterstitialAd
	[DllImport ("__Internal")]
	private static extern void _oneWaySDKInitInterstitialAd();

	[DllImport ("__Internal")]
	private static extern bool _oneWaySDKInterstitialAdIsReady();

	[DllImport ("__Internal")]
	private static extern void _oneWaySDKShowInterstitialAd(string tag);




	//InterstitialImageAd
	[DllImport ("__Internal")]
	private static extern void _oneWaySDKInitInterstitialImageAd();

	[DllImport ("__Internal")]
	private static extern bool _oneWaySDKInterstitialImageAdIsReady();

	[DllImport ("__Internal")]
	private static extern void _oneWaySDKShowInterstitialImageAd(string tag);



	//MetaData
	[DllImport ("__Internal")]
	private static extern void _debugLog(bool isDebug);

	//MetaData
	[DllImport ("__Internal")]
	private static extern void _commitMetaData(string metaData);
	#endif

	// configure
	public static void configure(string iOSPId , string androidPId)
	{
		#if UNITY_IPHONE
		_oneWaySDKConfigure(iOSPId);
		#elif UNITY_ANDROID
		_plugin.Call("configure",androidPId);
		#endif
	}

	// RewardedAd
	public static void initRewardedAd() 
	{
		#if UNITY_IPHONE
		_oneWaySDKInitRewardedAd();
		#elif UNITY_ANDROID
		_plugin.Call("initRewardedAd");
		#endif
	}
	public static bool isRewardedAdReady()
	{
		#if UNITY_IPHONE
		return _oneWaySDKRewardedAdIsReady();
		#elif UNITY_ANDROID
		return _plugin.Call<bool>("isRewardedAdReady");
		#else
		return false;
		#endif
	}
	public static void showRewardedAd(string tag = "default")
	{
		#if UNITY_IPHONE
		_oneWaySDKShowRewardedAd(tag);
		#elif UNITY_ANDROID
		_plugin.Call("showRewardedAd",tag);
		#endif
	}

		// InterstitialAd
		public static void initInterstitialAd()
		{
		#if UNITY_IPHONE
		_oneWaySDKInitInterstitialAd();
		#elif UNITY_ANDROID
		_plugin.Call("initInterstitialAd");
		#endif
		}
		public static bool isInterstitialAdReady()
		{
		#if UNITY_IPHONE
		return _oneWaySDKInterstitialAdIsReady();
		#elif UNITY_ANDROID
		return _plugin.Call<bool>("isInterstitialAdReady");
		#else
		return false;
		#endif
		}
		public static void showInterstitialAd(string tag = "default")
		{
		#if UNITY_IPHONE
		_oneWaySDKShowInterstitialAd(tag);
		#elif UNITY_ANDROID
		_plugin.Call("showInterstitialAd",tag);
		#endif
		}

		// InterstitialImageAd
		public static void initInterstitialImageAd()
		{
		#if UNITY_IPHONE
		_oneWaySDKInitInterstitialImageAd();
		#elif UNITY_ANDROID
		_plugin.Call("initInterstitialImageAd");
		#endif
		}
		public static bool isInterstitialImageAdReady()
		{
		#if UNITY_IPHONE
		return _oneWaySDKInterstitialImageAdIsReady();
		#elif UNITY_ANDROID
		return _plugin.Call<bool>("isInterstitialImageAdReady");
		#else
		return false;
		#endif
		}
		public static void showInterstitialImageAd(string tag = "default")
		{
		#if UNITY_IPHONE
		_oneWaySDKShowInterstitialImageAd(tag);
		#elif UNITY_ANDROID
		_plugin.Call("showInterstitialImageAd",tag);
		#endif
		}


	public static void commitMetaData(Dictionary<string,string> metaData )
	{
		string data = MiniJSONV.Json.Serialize(metaData);

		#if UNITY_IPHONE
		_commitMetaData(data);
		#elif UNITY_ANDROID
		_plugin.Call("commitMetaData",data);
		#endif
	}

	public static void setDebug(bool isDebug)
	{

		#if UNITY_IPHONE
		_debugLog(isDebug);
		#elif UNITY_ANDROID
		_plugin.Call("debugLog",isDebug);
		#endif
	}
		

}


