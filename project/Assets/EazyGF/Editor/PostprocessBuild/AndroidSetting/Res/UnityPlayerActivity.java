package com.acoingames.idle.drive.in ;

//import com.tencent.bugly.crashreport.CrashReport;
import com.facebook.FacebookSdk;
import com.flurry.android.FlurryAgent;
import com.unity3d.player.*;
import android.app.Activity;
import android.content.Intent;
import android.content.res.Configuration;
import android.net.Uri;
import android.os.Bundle;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.ViewGroup;
import android.view.Window;
import android.widget.RelativeLayout;

import com.sequence.analytics.AnalyticsControl;
import com.sequence.illegal.Tools;
import com.auto.adListener.RewardedVideoListener;
import com.auto.adListener.BannerAdListener;
import com.auto.control.AdControler;

import java.util.Locale;

public class UnityPlayerActivity extends Activity implements
        RewardedVideoListener,
        BannerAdListener
{
    protected UnityPlayer mUnityPlayer; // don't change the name of this variable; referenced from native code
    private RelativeLayout mSDKRelativeLayout;
    public static final String mLogTag = "drivein-android-log";

    // Setup activity layout
    @Override protected void onCreate(Bundle savedInstanceState)
    {
        requestWindowFeature(Window.FEATURE_NO_TITLE);
        super.onCreate(savedInstanceState);

        mUnityPlayer = new UnityPlayer(this);
        setContentView(mUnityPlayer);
        mUnityPlayer.requestFocus();


        new FlurryAgent.Builder()
                .withLogEnabled(true)
                .build(this, "5T89GQFD7DWJQSQDNDWX");
        FacebookSdk.setApplicationId("2361998500685151");
        AnalyticsControl.initAnalytics(getApplication());
        AnalyticsControl.setUserId(this, Tools.getUserId(this));
        AnalyticsControl.activeThinkingData( this, "60b6b5f0d20348d4bc85dc52f883ba8a");

        mSDKRelativeLayout = new RelativeLayout(this);
        this.addContentView(mSDKRelativeLayout, new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WRAP_CONTENT,ViewGroup.LayoutParams.WRAP_CONTENT));

        InitAdsSDK();
    }

    @Override protected void onNewIntent(Intent intent)
    {
        // To support deep linking, we need to make sure that the client can get access to
        // the last sent intent. The clients access this through a JNI api that allows them
        // to get the intent set on launch. To update that after launch we have to manually
        // replace the intent with the one caught here.
        setIntent(intent);
    }

    public void SendEmail(String PackageName,String AppVersion)//
    {
        String ID=Tools.getUserId(mUnityPlayer.getContext());
        String DeviceModel = android.os.Build.MODEL;
        String DeviceVersion=android.os.Build.VERSION.RELEASE;
        String Region=  Locale.getDefault().getCountry();
        String FinalValue="My suggestion/feedback is "+'\n'+'\n'+'\n'+"Please don't delete the information below."+'\n'+
                "######################################"+'\n'+
                "UserId:"+ID+'\n'+
                "Version:"+AppVersion+'\n'+
                "PackageName:"+PackageName+'\n'+
                "ADVersion:"+"4.4.6"+'\n'+
                "DeviceModel:"+DeviceModel+'\n'+
                "OSVersion:"+DeviceVersion+'\n'+
                "Region:"+Region+'\n'+
                "######################################"+'\n'+
                "Please don't delete the information above.";

        Uri uri = Uri.parse("mailto:monniemaxwell@gmail.com");
        String[] email = {"monniemaxwell@gmail.com"};
        // Uri uri = Uri.parse("mailto:feedback@acoingames.com");
        //  String[] email = {"feedback@acoingames.com"};
        Intent intent = new Intent(Intent.ACTION_SENDTO, uri);
        intent.putExtra(Intent.EXTRA_CC, email); // 抄送人
        intent.putExtra(Intent.EXTRA_SUBJECT, "Player Feedback"); // 主题
        intent.putExtra(Intent.EXTRA_TEXT, FinalValue); // 正文
        startActivity(Intent.createChooser(intent, "Select Mail Application"));
    }

    //初始化SDK
    public void InitAdsSDK()
    {
        //广告系统
        try {
            AdControler.init(this, mSDKRelativeLayout, true, this);
            AdControler.start();
            AdControler.setRewardedAdListener(this);
        }
        catch (Exception e)
        {
            // android.util.Log.i(mLogTag, "init ad error: " + e.toString());
        }
    }

    //插屏广告的展示
    public void ShowInterstitialAD()
    {

        AdControler.showInterstitialAD();
        // android.util.Log.i(mLogTag, "Show InterstitialAD!");
    }

    //插屏广告是否加载成功
    public boolean IsInterstitialADSuc()
    {
        boolean bRet = false;
        bRet = AdControler.isInterstitialReady();

        return bRet;
    }

    //banner的展示
    public void ShowBottomADBannar(String pos)
    {
        AdControler.showBottomADBannar(pos);
        //android.util.Log.i(mLogTag, "Show Banner!");
    }
    //banner的隐藏
    public void HiddenBottomADBannar()
    {
        AdControler.hiddenBottomADBannar();
    }
    //判断banner是否加载成功
    public boolean IsBannerSuc()
    {
        return AdControler.isBannerReady();
    }

    //nativebanner的展示
    public void ShowNativeADBannar(String pos)
    {
        AdControler.showNativeADBannar(pos);
        //android.util.Log.i(mLogTag, "Show NativeBanner!");
    }
    //nativebanner的隐藏
    public void HiddenNativeADBannar()
    {
        AdControler.hiddenNativeADBannar();
    }
    //判断nativebanner是否加载成功
    public boolean IsNativeBannerSuc()
    {
        return AdControler.isNativeBannerReady();
    }

    //播放奖励视频广告
    public void ShowRewardVideo()
    {
        AdControler.showRewardVideo();
    }
    //判断奖励视频是否加载成功
    private int rewardTest = 0;
    public boolean IsRewardVideoSuc()
    {
        boolean re = AdControler.isRewardVideoReady();
        return re;
    }
    //加载奖励视频成功
    @Override
    public void rewaredVideoReady() {
        //  mUnityPlayer.UnitySendMessage("(singleton)SDKManager", "OnRewaredVideoCompleted", "");
        //android.util.Log.i(mLogTag, "load rewared video suc");
    }
    //播放奖励视频成功
    @Override
    public void rewaredVideoCompleted() {
        mUnityPlayer.UnitySendMessage("ADMgr", "RewardCallbackSuccess", "");
        //android.util.Log.i(mLogTag, "play rewared video complete");
    }

    @Override
    public void rewardVideoFailed() {
        // mUnityPlayer.UnitySendMessage("(singleton)SDKManager", "onVideoFailed", "");
    }

    //隐藏所有banner和nativebanner广告
    public void HiddenAllAds()
    {
        AdControler.hiddenAllAds();
    }

    //Banner load suc callback
    @Override
    public void adLoadSuccess() {
        //android.util.Log.i(mLogTag, "banner suc~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~`");
    }
    //Banner load failed callback
    @Override
    public void adLoadFailed() {
        //android.util.Log.i(mLogTag, "banner failed~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~`");
    }


    //打点事件
    public  void MySendGameEven(String key,String json)
    {
        AnalyticsControl.sendEvent(key, json);
        //json格式如下：
        // AnalyticsControl.sendEvent("Level", "{\"HightestLevel\":99}");
    }

    //用户属性
    public void MySetAddedProperty(String key, float addValue)
    {
        AnalyticsControl.setAddedProperty(mUnityPlayer.getContext(),key,addValue);
        // android.util.Log.i(mLogTag, "调用Add属性");
    }

    // Quit Unity
    @Override protected void onDestroy ()
    {
        mUnityPlayer.quit();
        super.onDestroy();
    }

    // Pause Unity
    @Override protected void onPause()
    {
        super.onPause();
        mUnityPlayer.pause();
    }

    // Resume Unity
    @Override protected void onResume()
    {
        super.onResume();
        mUnityPlayer.resume();
    }

    @Override protected void onStart()
    {
        super.onStart();
        mUnityPlayer.start();
    }

    @Override protected void onStop()
    {
        super.onStop();
        mUnityPlayer.stop();
    }

    // Low Memory Unity
    @Override public void onLowMemory()
    {
        super.onLowMemory();
        mUnityPlayer.lowMemory();
    }

    // Trim Memory Unity
    @Override public void onTrimMemory(int level)
    {
        super.onTrimMemory(level);
        if (level == TRIM_MEMORY_RUNNING_CRITICAL)
        {
            mUnityPlayer.lowMemory();
        }
    }

    // This ensures the layout will be correct.
    @Override public void onConfigurationChanged(Configuration newConfig)
    {
        super.onConfigurationChanged(newConfig);
        mUnityPlayer.configurationChanged(newConfig);
    }

    // Notify Unity of the focus change.
    @Override public void onWindowFocusChanged(boolean hasFocus)
    {
        super.onWindowFocusChanged(hasFocus);
        mUnityPlayer.windowFocusChanged(hasFocus);
    }

    // For some reason the multiple keyevent type is not supported by the ndk.
    // Force event injection by overriding dispatchKeyEvent().
    @Override public boolean dispatchKeyEvent(KeyEvent event)
    {
        if (event.getAction() == KeyEvent.ACTION_MULTIPLE)
            return mUnityPlayer.injectEvent(event);
        return super.dispatchKeyEvent(event);
    }

    // Pass any events not handled by (unfocused) views straight to UnityPlayer
    @Override public boolean onKeyUp(int keyCode, KeyEvent event)     { return mUnityPlayer.injectEvent(event); }
    @Override public boolean onKeyDown(int keyCode, KeyEvent event)   { return mUnityPlayer.injectEvent(event); }
    @Override public boolean onTouchEvent(MotionEvent event)          { return mUnityPlayer.injectEvent(event); }
    /*API12*/ public boolean onGenericMotionEvent(MotionEvent event)  { return mUnityPlayer.injectEvent(event); }
}
