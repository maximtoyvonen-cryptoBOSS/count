namespace CounterApp;

public partial class MainPage : ContentPage
{
    private int count = 0;

    public MainPage()
    {
        InitializeComponent();
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
        CounterLabel.Text = count.ToString();
        SemanticScreenReader.Announce(CounterLabel.Text);
    }
}
