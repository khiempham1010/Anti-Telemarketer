using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using AntiTelemarketer.AdapterManager;
using AntiTelemarketer.Droid.ContactApdapter;
using Android.Content;
using AntiTelemarketer.Droid.ContactService;
using AntiTelemarketer.Droid.BroadcastReceiverService;

namespace AntiTelemarketer.Droid
{
    [Activity(Label = "AntiTelemarketer", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            try
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                {
                    Xamarin.Essentials.Platform.Init(this, savedInstanceState);
                    RequestPermissions(Permission, requestID);
                }
            }
            catch { }

            // KẾT NỐI GIỮA TẦNG SPECIFIC VỚI PLC ĐỂ GỌI SPECIFIC Ở PLC
            ContactsAdapterManager.Current = new ContactsAdapter(this, Application);
            ContactService_AndroidManager.Current = new ContactService_Android(this, Application);
            ToastManager.Current = new ToastDependency(this);
            base.OnCreate(savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);




            LoadApplication(new App());

            // LẤY PERMISSION KHI KHỞI ĐỘNG APPS

            long repeatEveryday = (60 * 1000) * 60 * 24; // => run at 2:00 AM every day

            Intent intentAlarm = new Intent(this, typeof(MyReceiver));

            // create the object
            AlarmManager alarmManager = (AlarmManager)GetSystemService(Context.AlarmService);

            // define hours to starts => after 6 PM 
            Java.Util.Calendar calendarStart = Java.Util.Calendar.GetInstance(Java.Util.TimeZone.Default);
            calendarStart.Set(Java.Util.CalendarField.HourOfDay, 02);
            calendarStart.Set(Java.Util.CalendarField.Minute, 00);
            calendarStart.Set(Java.Util.CalendarField.Second, 00);

            //
            alarmManager.SetInexactRepeating(AlarmType.RtcWakeup, calendarStart.TimeInMillis, repeatEveryday, PendingIntent.GetBroadcast(this, 1, intentAlarm, PendingIntentFlags.UpdateCurrent));


        }

        #region PERMISSION
        const int requestID = 0;
        readonly string[] Permission =
       {
            Android.Manifest.Permission.ModifyPhoneState,
            Android.Manifest.Permission.CallPhone,
            Android.Manifest.Permission.ReadPhoneState,
            Android.Manifest.Permission.ReadPhoneNumbers,
            Android.Manifest.Permission.AnswerPhoneCalls,
            Android.Manifest.Permission.ReadExternalStorage,
            Android.Manifest.Permission.WriteExternalStorage,
            Android.Manifest.Permission.Internet,
            Android.Manifest.Permission.ReadContacts,
            Android.Manifest.Permission.WriteContacts,
            Android.Manifest.Permission.ReadCallLog,
            Android.Manifest.Permission.WriteCallLog,
        };

       
        #endregion 
    }
}