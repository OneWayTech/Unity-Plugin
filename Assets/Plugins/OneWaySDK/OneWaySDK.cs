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
			// create a new GO for our manager
			var go = new GameObject( "OneWaySDK" );
			go.AddComponent<OneWaySDK>();
			DontDestroyOnLoad( go );
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

	// Fired when a Vungle ad starts
	public static event Action <string> onOneWaySDKReadyEvent;

	public static event Action <string> onOneWaySDKDidStartEvent;

	public static event Action <string,string> onOneWaySDKDidFinishEvent;

	public static event Action <string,string> onOneWaySDKDidErrorEvent;

	#endregion

	void onOneWaySDKReady (string placementID) {
		 onOneWaySDKReadyEvent (placementID);
	}

	void onOneWaySDKDidStart (string placementID) {
		 onOneWaySDKDidStartEvent (placementID);
	}

	void onOneWaySDKDidFinish (string msg) {

		Dictionary<string,object> attrs = (Dictionary<string,object>) MiniJSONV.Json.Deserialize( msg );

		onOneWaySDKDidFinishEvent (attrs["placementId"].ToString(),attrs["state"].ToString());
	}

	void onOneWaySDKDidError (string msg) {

		Dictionary<string,object> attrs = (Dictionary<string,object>) MiniJSONV.Json.Deserialize( msg );

		onOneWaySDKDidErrorEvent (attrs["error"].ToString(),attrs["message"].ToString());

	}


	//  init
	[DllImport ("__Internal")]
	private static extern void _OneWaySDKInit(string PId,bool debugMode);
	public static void init(string iOSPId , string androidPId, bool debugMode)
	{
		#if UNITY_IPHONE
		_OneWaySDKInit( iOSPId, debugMode);
		#elif UNITY_ANDROID
		_plugin.Call("init",androidPId,debugMode);
		#endif
	}


	// show
	[DllImport ("__Internal")]
	private static extern void _OneWaySDKShow();
	public static void show()
	{
		#if UNITY_IPHONE
		_OneWaySDKShow();
		#elif UNITY_ANDROID
		_plugin.Call("showOneWayAd");
		#endif

	}


	//ShowPlacementID
	[DllImport ("__Internal")]
	private static extern void _OneWaySDKShowPlacementID(string placementID);
	public static void showPlacementID(string placementID)
	{
		#if UNITY_IPHONE
		_OneWaySDKShowPlacementID(placementID);
		#elif UNITY_ANDROID
		_plugin.Call("showPlacementID",placementID);
		#endif


	}

	//isReady
	[DllImport ("__Internal")]
	private static extern bool _OneWaySDKIsReady();
	public static bool IsReady()
	{
		#if UNITY_IPHONE
		return _OneWaySDKIsReady();
		#elif UNITY_ANDROID
		return _plugin.Call<bool>("isVideoAvailable");
		#endif
	}

}
