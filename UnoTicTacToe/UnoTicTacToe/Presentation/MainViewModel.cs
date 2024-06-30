namespace UnoTicTacToe.Presentation;

public partial class MainViewModel : ObservableObject
{
    private INavigator _navigator;

    [ObservableProperty]
    private string? name;

    public MainViewModel(
        IStringLocalizer localizer,
        IOptions<AppConfig> appInfo,
        INavigator navigator)
    {
        _navigator = navigator;
        Title = "Main";
        Title += $" - {localizer["ApplicationName"]}";
        Title += $" - {appInfo?.Value?.Environment}";
        GoToSecond = new AsyncRelayCommand<bool>(GoToSecondView);
    }
    public string? Title { get; }

    public ICommand GoToSecond { get; }

    private async Task GoToSecondView(bool parameter)
    {
        await _navigator.NavigateViewModelAsync<SecondViewModel>(this, data: new Entity(parameter));
    }

}
