#import <AdSupport/AdSupport.h>
#import <AppTrackingTransparency/AppTrackingTransparency.h>

extern "C" {
    void RequestTrackingAuthorization()
    {
        if (@available(iOS 14, *)) {
            [ATTrackingManager requestTrackingAuthorizationWithCompletionHandler:^(ATTrackingManagerAuthorizationStatus status) {
                // 必要であれば Unity に通知可能
                // 例: UnitySendMessage("ATTManager", "OnATTResult", [@(status) stringValue].UTF8String);
            }];
        }
    }
}
