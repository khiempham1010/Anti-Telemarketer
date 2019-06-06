using System;
using System.Collections.Generic;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Telecom;
using Android.Telephony;
using Android.Widget;
using AntiTelemarketer.API;
using AntiTelemarketer.DatabaseHelper;
using AntiTelemarketer.Droid.BroadcastReceiverService;
using AntiTelemarketer.Droid.ContactService;
using AntiTelemarketer.Droid.DatabaseHelper;
using AntiTelemarketer.Model;
using SQLite;

using static Android.Provider.Contacts;

[assembly: Xamarin.Forms.Dependency(typeof(IncomingCallReceiver))]
namespace AntiTelemarketer.Droid.BroadcastReceiverService
{
    //INTENT FILTER NÀY GIÚP HÀM OVERIDE NÀY CHỈ INVOKE KHI GÓ CUỘC GỌI ĐẾN
    //SỬ DỤNG PHONE_STATE CHÚ Ý XEM MANIFEST
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { "android.intent.action.PHONE_STATE" })]
    public class IncomingCallReceiver : BroadcastReceiver
    {

        #region declare
        //         
        private SQLiteConnection connection;


        // BIẾN XÁC ĐỊNH XEM CUỘC GỌI ĐẾN CÓ PHẢI BỊ BLOCK HAY KHÔNG
        static bool isBlocking = false;

        // BIẾN XAC ĐỊNH XEM CUỘC GỌI ĐẾN CÓ NẰM TRONG DANH BẠ HAY KHÔNG
        static bool isInPhonebook = false;

        // BIẾN GHI NHẬN LẠI SỐ ĐIỆN THOẠI ĐÃ GỌI ĐẾN (HOẶC GỌI ĐI) TÙY THEO CONFIG
        static string incomingNumber = String.Empty;

        // BIẾN PHÂN BIỆT GIỮA CUỘC GỌI ĐẾN VÀ CUỘC GỌI ĐI
        static bool isIcomingCall = false;

        // VARIABLE DETECT SETTING
        static bool isEnale = true;
        static bool isNotification = true;

        public static UserReportDatabaseHelper reportDatabaseHelper;
        public static TelemarketerDatabaseHelper telemarketerDatabaseHelper;
        public static SettingDatabaseHelper settingDatabaseHelper;


        static List<UserReport> reports;
        static List<Telemarketer> telemarketers;
        static ContactService_Android contactServiceAndroid;
        static List<PhoneContact> phoneContacts;
        #endregion





        private bool isTheSame(string incomingcall, string phoneContact)
        {
            incomingcall = incomingcall.Trim().Replace(" ", string.Empty).Replace("-", string.Empty).Replace("+", string.Empty);
            phoneContact = phoneContact.Trim().Replace(" ", string.Empty).Replace("-", string.Empty).Replace("+", string.Empty);

            if (incomingcall.Length == phoneContact.Length)
            {
                if (incomingcall.Equals(phoneContact))
                    return true;
                return false;
            }
            else
            {
                phoneContact = phoneContact.Substring(2, phoneContact.Length - 2);
                incomingcall = incomingcall.Substring(1, incomingcall.Length - 1);
                if (incomingcall.Equals(phoneContact))
                    return true;
                return false;

            }
        }



        // THƯƠNG THỨC OVERRIDE NÀY SẼ ĐƯỢC THỰC THI KỂ CẢ APP LÚC KHÔNG CHẠY
        public override void OnReceive(Context context, Intent intent)
        {
            try
            {
                // MANUAL CREATE CONNECTION FOR SETTING
                Android_SQLite android_SQLite = new Android_SQLite();
                connection = android_SQLite.GetConnection();
                settingDatabaseHelper = new SettingDatabaseHelper("" +
                    "IF_WE_ADD_THIS_TEXT_OUR_CLASS_WILL_" +
                    "USE_A_DEFALT_CONSTRUCTOR_WITHOUT_PLATFORM_ERROR" +
                    "WHEN_RUNNING_IN_BACKGROUND");
                // MANUAL ADD CONNECT FOR THIS CLASS
                settingDatabaseHelper.connection = connection;
                settingDatabaseHelper.CreateTable();
                isEnale = settingDatabaseHelper.GetKeyValue(SystemKey.IS_ENABLE);
                isNotification = settingDatabaseHelper.GetKeyValue(SystemKey.IS_NOTIFICATION);
            }
            catch
            {

            }


            // LẤY DANH SÁCH REPORT MỖI LẦN ĐƯỢC INVOKE
            try
            {
                // MANUAL CREATE CONNECTION
                Android_SQLite android_SQLite = new Android_SQLite();
                connection = android_SQLite.GetConnection();
                reportDatabaseHelper = new UserReportDatabaseHelper("" +
                    "IF_WE_ADD_THIS_TEXT_OUR_CLASS_WILL_" +
                    "USE_A_DEFALT_CONSTRUCTOR_WITHOUT_PLATFORM_ERROR" +
                    "WHEN_RUNNING_IN_BACKGROUND");

                // MANUAL ADD CONNECT FOR THIS CLASS
                reportDatabaseHelper.connection = connection;
                reportDatabaseHelper.CreateTable();
                reports = reportDatabaseHelper.GetAllReport();

                //MANUAL ADD CONNECT FOR THIS Telemarketer
                connection = android_SQLite.GetConnection();
                telemarketerDatabaseHelper = new TelemarketerDatabaseHelper("" +
                    "IF_WE_ADD_THIS_TEXT_OUR_CLASS_WILL_" +
                    "USE_A_DEFALT_CONSTRUCTOR_WITHOUT_PLATFORM_ERROR" +
                    "WHEN_RUNNING_IN_BACKGROUND");
                telemarketerDatabaseHelper.connection = connection;
                telemarketerDatabaseHelper.CreateTable();
                telemarketers = telemarketerDatabaseHelper.GetAllTelemarketer();

            }
            catch
            {

            }

            // Toast.MakeText(context, "EX: " + reportDatabaseHelper.exceptionError, ToastLength.Short).Show();



            // LẤY DANH BẠ LÊN
            // MỤC ĐÍCH ĐỂ SO SÁNH NÊU KHÔNG CÓ Ở TRONG DANH BẠ THÌ MỚI BLOCK
            try
            {
                contactServiceAndroid = new ContactService_Android();
                phoneContacts = contactServiceAndroid.GetAllContacts();
            }
            catch
            {
                Toast.MakeText(context, "ERROR WHEN GET CONTACT", ToastLength.Short).Show();
            }


            #region BLOCKING
            if (intent.Extras != null)
            {
                if (!isEnale)
                    return;
                string state = intent.GetStringExtra(TelephonyManager.ExtraState);
                // TRÍCH XUẤT SỐ ĐIỆN THOẠI GỌI ĐÊN (GỌI ĐI)
                incomingNumber = intent.GetStringExtra(TelephonyManager.ExtraIncomingNumber);
                //   incomingNumber = "6505551212";
                // CUỘC GỌI ĐẾN
                if (state.Equals(TelephonyManager.ExtraStateRinging))
                {
                    isIcomingCall = true;

                    //KIẾM TRA XEM CÓ PHẢI LÀ SỐ ĐIỆN THOẠI TRONG DANH BẠ KHÔNG?
                    foreach (PhoneContact phone in phoneContacts)
                    {
                        if (isTheSame(incomingNumber, phone.PhoneNumber))
                        {
                            isInPhonebook = true;
                            //NẰM TRONG PHONEBOOK THÌ KHÔNG CẦN KIỂM TRA NỮA.
                            return;
                        }
                    }
                    // KIỂM TRA XEM CÓ PHẢI LÀ SỐ TRONG DANH SÁCH BLOCK HAY KHÔNG?
                    foreach (UserReport report in reports)
                    {
                        if (incomingNumber.Equals(report.phoneNumber))
                        {
                            StopACall(context);
                            isBlocking = true;
                            return;
                        }
                    }

                    //Toast.MakeText(context, reports.Count, ToastLength.Long).Show();

                    // KIỂM TRA XEM CÓ PHẢI LÀ SỐ TRONG DANH SÁCH Telemarketer HAY KHÔNG?
                    foreach (Telemarketer telemarketer in telemarketers)
                    {
                        if (incomingNumber.Equals(telemarketer.phoneNumber))
                        {
                            StopACall(context);
                            isBlocking = true;
                            return;
                        }
                    }

                }
                if (state.Equals(TelephonyManager.ExtraStateOffhook))
                {
                    isIcomingCall = false;
                    return;
                }


                // CUỘC GỌI BỊ DẬP MÁY - KỂ CẢ VIỆC GỌI CHO NGƯỜI KHÁC TẮT
                if (state.Equals(TelephonyManager.ExtraStateIdle))
                {
                    if (isInPhonebook)
                        return;

                    if (isBlocking&&isIcomingCall)
                    {
                        CreateNotification(incomingNumber, context);
                        isBlocking = false;
                        return;
                    }

                    if (!isIcomingCall)
                        return;


                    // TẠO RA THÔNG BÁO REPORT NẾU NÓ LÀ MỘT SỐ KHÔNG BỊ BLOCK, KHÔNG NẰM TRONG DANH BẠ VÀ LÀ CUỘC GỌI ĐẾN
                    CreateNotification_Report(incomingNumber, context);
                    isBlocking = false;
                    isIcomingCall = false;
                    isInPhonebook = false;
                }
            }
            #endregion
        }


        public void AddReportList(string incommingNumber)
        {
            UserReport report = new UserReport();
            report.phoneNumber = incommingNumber;
            report.reportDate = DateTime.Now;
            report.IsSync = false;
            reportDatabaseHelper.AddReport(report);
        }


        #region Notification
        static int mark = 0;

        private void CreateNotification(String incommingNumber, Context context)
        {
            if (!isNotification)
                return;

            NotificationManager notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
            String NOTIFICATION_CHANNEL_ID = "my_channel_id_01";

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                NotificationChannel notificationChannel = new NotificationChannel(NOTIFICATION_CHANNEL_ID, "ATA", NotificationImportance.High);
                notificationChannel.EnableLights(true);
                notificationChannel.SetVibrationPattern(new long[] { 0, 1000, 500, 1000 });
                notificationChannel.EnableVibration(true);
                notificationManager.CreateNotificationChannel(notificationChannel);
            }

            var uiIntent = new Intent(context, typeof(MainActivity));
            uiIntent.PutExtra("FromNotification", true);
            var resultPendingIntent = PendingIntent.GetActivity(context, 0, uiIntent, PendingIntentFlags.UpdateCurrent);

            NotificationCompat.Builder notificationBuilder = new NotificationCompat.Builder(context, NOTIFICATION_CHANNEL_ID);
            NotificationCompat.BigTextStyle textStyle = new NotificationCompat.BigTextStyle();
            notificationBuilder.SetAutoCancel(true)
                    .SetDefaults(Notification.ColorDefault)
                    .SetSmallIcon(Resource.Drawable.myReportIcon)
                    .SetPriority((int)NotificationPriority.High)
                    .SetContentIntent(resultPendingIntent)
                    .SetContentTitle("ATA-Blocking Notification")
                    .SetGroupSummary(true)
                    .SetVibrate(new long[0])
                    .SetGroup("ATA_GROUP")
                    //.SetOngoing(true)
                    .SetContentText("Anti-Telemarketer blocked a phone call from " + incommingNumber);

            //Add intent filters for each action and register them on a broadcast receiver
            notificationBuilder.SetStyle(textStyle);
            notificationManager.Notify(mark, notificationBuilder.Build());
            mark++;
        }




        private void CreateNotification_Report(String incommingNumber, Context context)
        {
            if (!isNotification)
                return;
            NotificationManager notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
            String NOTIFICATION_CHANNEL_ID = "my_channel_id_01";

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                NotificationChannel notificationChannel = new NotificationChannel(NOTIFICATION_CHANNEL_ID, "ATA", NotificationImportance.High);
                // Configure the notification channel.
                notificationChannel.EnableLights(true);
                notificationChannel.SetVibrationPattern(new long[] { 0, 1000, 500, 1000 });
                notificationChannel.EnableVibration(true);
                notificationManager.CreateNotificationChannel(notificationChannel);

                NotificationCompat.Builder notificationBuilder = new NotificationCompat.Builder(context, NOTIFICATION_CHANNEL_ID);

                NotificationCompat.BigTextStyle textStyle = new NotificationCompat.BigTextStyle();
                var actionIntent1 = new Intent(context, typeof(CustomActionReceiver));
                actionIntent1.PutExtra("IncomingCall", incommingNumber);
                actionIntent1.SetAction("REPORT");
                actionIntent1.SetClass(context, typeof(CustomActionReceiver));
                var pIntent1 = PendingIntent.GetBroadcast(context, 0, actionIntent1, PendingIntentFlags.CancelCurrent);


                Intent resultIntent = context.PackageManager.GetLaunchIntentForPackage(context.PackageName);
                var contentIntent = PendingIntent.GetActivity(context, 0, resultIntent, PendingIntentFlags.CancelCurrent);

                var uiIntent = new Intent(context, typeof(MainActivity));
                uiIntent.PutExtra("FromNotification", true);
                var resultPendingIntent = PendingIntent.GetActivity(context, 0, uiIntent, PendingIntentFlags.UpdateCurrent);

                notificationBuilder.SetAutoCancel(true)
                        .SetDefaults(Notification.ColorDefault)
                        .SetSmallIcon(Resource.Drawable.recentIcon)
                        .SetPriority((int)NotificationPriority.High)
                        .SetContentTitle("ATA-Blocking Notification")
                        .SetContentIntent(resultPendingIntent)
                        .SetGroupSummary(true)
                        .SetVibrate(new long[0])
                        .AddAction(Resource.Drawable.recentIcon, "REPORT", pIntent1)
                        .SetGroup("ATA_GROUP")
                        .SetContentText("Do you want to report " + incommingNumber + " ?");

                var intentFilter = new IntentFilter();
                intentFilter.AddAction("REPORT");

                notificationBuilder.SetStyle(textStyle);
                notificationManager.Notify(mark, notificationBuilder.Build());
                mark++;

            }
            else
            {

                NotificationCompat.Builder notificationBuilder = new NotificationCompat.Builder(context, NOTIFICATION_CHANNEL_ID);


                NotificationCompat.BigTextStyle textStyle = new NotificationCompat.BigTextStyle();
                var actionIntent1 = new Intent();
                actionIntent1.PutExtra("IncomingCall", incommingNumber);
                actionIntent1.SetAction("REPORT");
                var pIntent1 = PendingIntent.GetBroadcast(context, 0, actionIntent1, PendingIntentFlags.CancelCurrent);


                Intent resultIntent = context.PackageManager.GetLaunchIntentForPackage(context.PackageName);
                var contentIntent = PendingIntent.GetActivity(context, 0, resultIntent, PendingIntentFlags.CancelCurrent);

                var uiIntent = new Intent(context, typeof(MainActivity));
                uiIntent.PutExtra("FromNotification", true);
                var resultPendingIntent = PendingIntent.GetActivity(context, 0, uiIntent, PendingIntentFlags.UpdateCurrent);
                notificationBuilder.SetAutoCancel(true)
                        .SetDefaults(Notification.ColorDefault)
                        .SetSmallIcon(Resource.Drawable.recentIcon)
                        .SetPriority((int)NotificationPriority.High)
                        .SetContentTitle("ATA-Blocking Notification")
                        .SetContentIntent(resultPendingIntent)
                        .SetGroupSummary(true)
                        .SetVibrate(new long[0])
                        .AddAction(Resource.Drawable.recentIcon, "REPORT", pIntent1)
                        .SetGroup("ATA_GROUP")
                        .SetContentText("Do you want to report " + incommingNumber + " ?");

                var intentFilter = new IntentFilter();
                intentFilter.AddAction("REPORT");
                //
                notificationBuilder.SetStyle(textStyle);
                notificationManager.Notify(mark, notificationBuilder.Build());
                mark++;
            }

        }


        #endregion


        #region OtherModule
        public void StopACall(Context context)
        {

            // THERE HAVE NO ANDROID 9 (ANDROID PIE)
            //   if( (Build.VERSION.SdkInt < BuildVersionCodes.))
            var manager = (TelephonyManager)context.GetSystemService(Context.TelephonyService);
            IntPtr TelephonyManager_getITelephony = JNIEnv.GetMethodID(
                    manager.Class.Handle,
                    "getITelephony",
                    "()Lcom/android/internal/telephony/ITelephony;");

            IntPtr telephony = JNIEnv.CallObjectMethod(manager.Handle, TelephonyManager_getITelephony);
            IntPtr ITelephony_class = JNIEnv.GetObjectClass(telephony);
            IntPtr ITelephony_endCall = JNIEnv.GetMethodID(
                    ITelephony_class,
                    "endCall",
                    "()Z");

            JNIEnv.CallBooleanMethod(telephony, ITelephony_endCall);
            JNIEnv.DeleteLocalRef(telephony);
            JNIEnv.DeleteLocalRef(ITelephony_class);

        }

        #endregion


    }



    [BroadcastReceiver(Enabled = true)]
    public class MyReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            Thread t1 = new Thread((obj) => {
                try
                {

                    SynchronizeTelemarketer.SyncUpAsync();
                }
                catch (Exception)
                {

                    throw;
                }
            });


            Thread t2 = new Thread(() => {
                try
                {
                    SynchronizeTelemarketer.SyncDown();
                }
                catch { }
            });

            t1.Start();
            t2.Start();
        }
    }
}


#region BUTTON_ACTION
[BroadcastReceiver]
[IntentFilter(new string[] { "REPORT" })]
public class CustomActionReceiver : BroadcastReceiver
{
    public UserReportDatabaseHelper reportDatabaseHelper;
    public override void OnReceive(Context context, Intent intent)
    {
        String str = intent.GetStringExtra("IncomingCall");
        ManualReport(str);
        int notificationId = intent.GetIntExtra("REPORT", 0);
        NotificationManager manager1 = (NotificationManager)context.GetSystemService(Context.NotificationService);
        manager1.Cancel(notificationId);
        Toast.MakeText(context, str + "was reported", ToastLength.Short).Show();
    }



    public void ManualReport(string incommingNumber)
    {
        // MANUAL CREATE CONNECTION
        Android_SQLite android_SQLite = new Android_SQLite();
        SQLiteConnection connection = android_SQLite.GetConnection();
        reportDatabaseHelper = new UserReportDatabaseHelper("" +
            "IF_WE_ADD_THIS_TEXT_OUR_CLASS_WILL_" +
            "USE_A_DEFALT_CONSTRUCTOR_WITHOUT_PLATFORM_ERROR" +
            "WHEN_RUNNING_IN_BACKGROUND");

        // MANUAL ADD CONNECT FOR THIS CLASS
        reportDatabaseHelper.connection = connection;
        reportDatabaseHelper.CreateTable();

        UserReport report = new UserReport();
        report.phoneNumber = incommingNumber;
        report.reportDate = DateTime.Now;
        report.IsSync = false;
        reportDatabaseHelper.AddReport(report);

    }

}

#endregion



