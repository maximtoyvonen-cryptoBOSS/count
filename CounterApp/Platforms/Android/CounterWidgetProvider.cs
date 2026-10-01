using Android.App;
using Android.AppWidget;
using Android.Content;
using Android.Widget;
using Microsoft.Maui.Storage;

namespace CounterApp
{
    [BroadcastReceiver(Exported = true, Label = "Counter Widget")]
    [IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
    [MetaData("android.appwidget.provider", Resource = "@xml/widget_info")]
    public class CounterWidgetProvider : AppWidgetProvider
    {
        public const string ACTION_INCREMENT = "CounterApp.ACTION_INCREMENT";
        public const string ACTION_DECREMENT = "CounterApp.ACTION_DECREMENT";
        public const string ACTION_UPDATE = "CounterApp.ACTION_UPDATE";

        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            foreach (var widgetId in appWidgetIds)
            {
                UpdateAppWidget(context, appWidgetManager, widgetId);
            }
        }

        public override void OnReceive(Context context, Intent intent)
        {
            base.OnReceive(context, intent);

            if (intent.Action == ACTION_INCREMENT || intent.Action == ACTION_DECREMENT || intent.Action == ACTION_UPDATE)
            {
                if (intent.Action == ACTION_INCREMENT)
                {
                    int count = Preferences.Get("counter_value", 0);
                    count++;
                    Preferences.Set("counter_value", count);
                }
                else if (intent.Action == ACTION_DECREMENT)
                {
                    int count = Preferences.Get("counter_value", 0);
                    count--;
                    Preferences.Set("counter_value", count);
                }

                AppWidgetManager appWidgetManager = AppWidgetManager.GetInstance(context);
                ComponentName widgetComponent = new ComponentName(context, Java.Lang.Class.FromType(typeof(CounterWidgetProvider)));
                int[] widgetIds = appWidgetManager.GetAppWidgetIds(widgetComponent);

                foreach (var widgetId in widgetIds)
                {
                    UpdateAppWidget(context, appWidgetManager, widgetId);
                }
            }
        }

        private void UpdateAppWidget(Context context, AppWidgetManager appWidgetManager, int appWidgetId)
        {
            int currentCount = Preferences.Get("counter_value", 0);

            RemoteViews views = new RemoteViews(context.PackageName, Resource.Layout.widget_layout);
            views.SetTextViewText(Resource.Id.widget_counter_text, currentCount.ToString());

            views.SetOnClickPendingIntent(Resource.Id.widget_btn_plus, GetPendingIntent(context, ACTION_INCREMENT, appWidgetId * 10 + 1));
            views.SetOnClickPendingIntent(Resource.Id.widget_btn_minus, GetPendingIntent(context, ACTION_DECREMENT, appWidgetId * 10 + 2));

            appWidgetManager.UpdateAppWidget(appWidgetId, views);
        }

        private PendingIntent GetPendingIntent(Context context, string action, int requestCode)
        {
            Intent intent = new Intent(context, typeof(CounterWidgetProvider));
            intent.SetAction(action);
            return PendingIntent.GetBroadcast(context, requestCode, intent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);
        }
    }
}