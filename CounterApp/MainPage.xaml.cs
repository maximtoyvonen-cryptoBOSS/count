namespace CounterApp;

public partial class MainPage : ContentPage
{
    private int count = 0;

    public MainPage()
    {
        InitializeComponent();

        count = Preferences.Get("counter_value", 0);
        UpdateCounterLabel();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        count = Preferences.Get("counter_value", 0);
        CounterLabel.Text = count.ToString();
    }

    private void OnIncrementClicked(object sender, EventArgs e)
    {
        count++;
        UpdateCounterLabel();
    }

    private void OnDecrementClicked(object sender, EventArgs e)
    {
        count--;
        UpdateCounterLabel();
    }

    private void OnResetClicked(object sender, EventArgs e)
    {
        count = 0;
        UpdateCounterLabel();
    }

    private void UpdateCounterLabel()
    {
        Preferences.Set("counter_value", count);
        CounterLabel.Text = count.ToString();
        SemanticScreenReader.Announce(CounterLabel.Text);
        UpdateAndroidWidget();
    }

    private void UpdateAndroidWidget()
    {
#if ANDROID
        var context = Android.App.Application.Context;
        var intent = new Android.Content.Intent(context, typeof(CounterWidgetProvider));
        intent.SetAction(CounterWidgetProvider.ACTION_UPDATE);
        context.SendBroadcast(intent);
#endif
    }
}
