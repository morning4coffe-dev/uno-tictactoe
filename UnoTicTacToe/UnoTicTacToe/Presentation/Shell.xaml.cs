namespace UnoTicTacToe.Presentation;

public sealed partial class Shell : UserControl, IContentControlProvider
{
    public Shell()
    {
        this.InitializeComponent();
    }
    public ContentControl ContentControl => Splash;

    private void ProgressRing_ActualThemeChanged(FrameworkElement sender, object args)
    {

    }
}
