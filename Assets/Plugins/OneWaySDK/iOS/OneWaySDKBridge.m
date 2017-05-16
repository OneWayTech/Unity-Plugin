#import "OneWaySDKBridge.h"

static OneWaySDKBridge *_OneWaySDKBridgeSingleton;

void _OneWaySDKInit(char *pID,bool debugMode)
{
    if (_OneWaySDKBridgeSingleton == nil) {
        _OneWaySDKBridgeSingleton = [[OneWaySDKBridge alloc] init];
    }
    NSString *pIDString = [NSString stringWithUTF8String:pID];
    
    [OneWaySDK initialize:pIDString delegate:_OneWaySDKBridgeSingleton testMode:debugMode];
}

void _OneWaySDKShowPlacementID(char *PlacementId)
{
    NSString *Placement = [NSString stringWithUTF8String:PlacementId];
    
    if ([OneWaySDK isReady:Placement]) {
        [OneWaySDK show:UnityGetMainWindow().rootViewController placementId:Placement];
    }
}


void _OneWaySDKShow(void)
{
    [OneWaySDK show:UnityGetMainWindow().rootViewController];
}

BOOL _OneWaySDKIsReady(void)
{
    return [OneWaySDK isReady];
}




@implementation OneWaySDKBridge :NSObject

- (void)oneWaySDKReady:(NSString *)placementId{
    UnitySendMessage([@"OneWaySDK" UTF8String], [@"onOneWaySDKReady" UTF8String],[placementId UTF8String]);
}

- (void)oneWaySDKDidStart:(NSString *)placementId{
    UnitySendMessage([@"OneWaySDK" UTF8String], [@"onOneWaySDKDidStart" UTF8String],[placementId UTF8String]);
}

- (void)oneWaySDKDidFinish:(NSString *)placementId withFinishState:(OneWaySDKFinishState)state{
    
    NSDictionary *dict = @{@"placementId":placementId,@"state":[NSString stringWithFormat:@"%ld",(long)state]};
    
    NSError *error;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dict options:kNilOptions error:&error];
    NSString *jsonStr =[[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    
    UnitySendMessage([@"OneWaySDK" UTF8String], [@"onOneWaySDKDidFinish" UTF8String],[jsonStr UTF8String]);
}

- (void)oneWaySDKDidError:(OneWaySDKError)error withMessage:(NSString *)message{
    
    NSDictionary *dict = @{@"error":[NSString stringWithFormat:@"%ld",(long)error],@"message":message};
    
    NSError *err;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dict options:kNilOptions error:&err];
    NSString *jsonStr =[[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    UnitySendMessage([@"OneWaySDK" UTF8String], [@"onOneWaySDKDidError" UTF8String],[jsonStr UTF8String]);
}


@end
