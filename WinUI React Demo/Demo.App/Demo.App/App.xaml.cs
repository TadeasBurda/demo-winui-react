using Microsoft.UI.Xaml;

namespace Demo.App;

public partial class App : Application
{
    private Window? _mainWindow;
    private SimpleHttpServer? _server;

    public App()
    {
        InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        _mainWindow = new MainWindow();
        _mainWindow.Activate();

        _server = new SimpleHttpServer(["http://localhost:8080/"]);
        _server.Start();

        _mainWindow.Closed += (_, _) => _server.Dispose();
    }
}
