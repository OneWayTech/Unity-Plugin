#import "OneWaySDKBridge.h"

static OneWaySDKBridge *_oneWaySDKBridgeSingleton;

//初始化
void _oneWaySDKConfigure(char *pID){
    NSString *pIDString = [NSString stringWithUTF8String:pID];
    [OneWaySDK configure:pIDString];
}

#pragma mark - 激励视频

void _oneWaySDKInitRewardedAd(){
    if (_oneWaySDKBridgeSingleton == nil) {
        _oneWaySDKBridgeSingleton = [[OneWaySDKBridge alloc] init];
    }
    [OWRewardedAd initWithDelegate:_oneWaySDKBridgeSingleton];
}
void _oneWaySDKShowRewardedAd(char *tag){
    NSString *tagString = [NSString stringWithUTF8String:tag];
    if ([OWRewardedAd isReady]) {
        [OWRewardedAd show:UnityGetMainWindow().rootViewController tag:tagString];
    }
}

BOOL _oneWaySDKRewardedAdIsReady(){
    return [OWRewardedAd isReady];
}

#pragma mark - 插屏
void _oneWaySDKInitInterstitialAd(){
    if (_oneWaySDKBridgeSingleton == nil) {
        _oneWaySDKBridgeSingleton = [[OneWaySDKBridge alloc] init];
    }
    
    [OWInterstitialAd initWithDelegate:_oneWaySDKBridgeSingleton];
}

void _oneWaySDKShowInterstitialAd(char *tag){
    if ([OWInterstitialAd isReady]) {
        NSString *tagString = [NSString stringWithUTF8String:tag];
        [OWInterstitialAd show:UnityGetMainWindow().rootViewController tag:tagString];
    }
}

BOOL _oneWaySDKInterstitialAdIsReady(){
    return [OWInterstitialAd isReady];
}


#pragma mark - 插屏图片
void _oneWaySDKInitInterstitialImageAd(){
    if (_oneWaySDKBridgeSingleton == nil) {
        _oneWaySDKBridgeSingleton = [[OneWaySDKBridge alloc] init];
    }
    
    [OWInterstitialImageAd initWithDelegate:_oneWaySDKBridgeSingleton];
}

void _oneWaySDKShowInterstitialImageAd(char *tag){
    if ([OWInterstitialImageAd isReady]) {
        NSString *tagString = [NSString stringWithUTF8String:tag];
        [OWInterstitialImageAd show:UnityGetMainWindow().rootViewController tag:tagString];
    }
}

BOOL _oneWaySDKInterstitialImageAdIsReady(){
    return [OWInterstitialImageAd isReady];
}

#pragma mark - other

void _debugLog(int isDebug){
    [OneWaySDK debugLog:isDebug];
}

void _commitMetaData(char *msg){
    
    NSData *data = [NSData dataWithBytes:msg length:strlen(msg)];
    
    NSError *err = nil;
    NSDictionary *dict = [NSJSONSerialization JSONObjectWithData:data options:kNilOptions error:&err];
    
    if (err) {
        NSLog( @"OneWaySDK JSONObjectWithData : ERROR MetaData Type");
        return;
    }
    
    OWUserMetaData *metaData = [[OWUserMetaData alloc] init];
    
    [dict enumerateKeysAndObjectsUsingBlock:^(id key, id obj, BOOL *stop) {
        [metaData set:key value:obj];
    }];
    
    [metaData commit];

}

#pragma mark - Delegate
@implementation OneWaySDKBridge : NSObject

- (void)sendToUnity:(NSString *)method withMessage:(NSString *)message{
    if (!message) {
        message = @"";
    }
    UnitySendMessage([@"OneWaySDK" UTF8String], [method UTF8String], [message UTF8String]);
}

- (NSString *)dictionaryToString:(NSDictionary *)dict{
    NSError *error;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dict options:kNilOptions error:&error];
    if (error) {
        return nil;//@{@"error":@"9",@"message":@"OneWaySDK Error : Unity DictionaryToString Error"};
    }
    
    return [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
}
#pragma mark - RewardedAd Delegate
- (void)oneWaySDKRewardedAdReady{
    [self sendToUnity:@"onRewardedAdReady" withMessage:nil];
}
- (void)oneWaySDKRewardedAdDidShow:(NSString *)tag {
    [self sendToUnity:@"onRewardedAdShow" withMessage:tag];
}
- (void)oneWaySDKRewardedAdDidClick:(NSString *)tag {
    [self sendToUnity:@"onRewardedAdClick" withMessage:tag];
}
- (void)oneWaySDKRewardedAdDidClose:(NSString *)tag withState:(NSString *)state{
    NSString *jsonStr = [self dictionaryToString:@{@"tag":tag,@"state":state}];
    [self sendToUnity:@"onRewardedAdClose" withMessage:jsonStr];
}
- (void)oneWaySDKRewardedAdDidFinish:(NSString *)tag state:(NSNumber *)state sessionId:(NSString *)sessionId{
    NSString *jsonStr = [self dictionaryToString:@{@"tag":tag,@"state":state,@"sessionId":sessionId}];
    [self sendToUnity:@"onRewardedAdFinish" withMessage:jsonStr];
}

#pragma mark - Interstitial Delegate
- (void)oneWaySDKInterstitialAdReady{
    [self sendToUnity:@"onInterstitialAdReady" withMessage:nil];
}
- (void)oneWaySDKInterstitialAdDidShow:(NSString *)tag {
    [self sendToUnity:@"onInterstitialAdShow" withMessage:tag];
}
- (void)oneWaySDKInterstitialAdDidClick:(NSString *)tag {
    [self sendToUnity:@"onInterstitialAdClick" withMessage:tag];
}
- (void)oneWaySDKInterstitialAdDidClose:(NSString *)tag withState:(NSString *)state {
    NSString *jsonStr = [self dictionaryToString:@{@"tag":tag,@"state":state}];
    [self sendToUnity:@"onInterstitialAdClose" withMessage:jsonStr];
}

#pragma mark - InterstitialImage Delegate
- (void)oneWaySDKInterstitialImageAdReady{
    [self sendToUnity:@"onInterstitialImageAdReady" withMessage:nil];
}
- (void)oneWaySDKInterstitialImageAdDidShow:(NSString *)tag {
    [self sendToUnity:@"onInterstitialImageAdShow" withMessage:tag];
}
- (void)oneWaySDKInterstitialImageAdDidClick:(NSString *)tag {
    [self sendToUnity:@"onInterstitialImageAdClick" withMessage:tag];
}
- (void)oneWaySDKInterstitialImageAdDidClose:(NSString *)tag withState:(NSString *)state {
    NSString *jsonStr = [self dictionaryToString:@{@"tag":tag,@"state":state}];
    [self sendToUnity:@"onInterstitialImageAdClose" withMessage:jsonStr];
}

#pragma mark - Total Error Delegate
- (void)oneWaySDKDidError:(OneWaySDKError)error withMessage:(NSString *)message{
    
    NSString *jsonStr = [self dictionaryToString:@{@"error":[NSString stringWithFormat:@"%ld",(long)error],@"message":message}];
    [self sendToUnity:@"onOneWaySDKDidError" withMessage:jsonStr];
    
}


@end
