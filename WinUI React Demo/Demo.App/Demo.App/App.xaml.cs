using Microsoft.UI.Xaml;

namespace Demo.App;

public partial class App : Application
{
    private Window? _mainWindow;
    private SimpleHttpServer? _httpServer;
    private SimpleApiServer? _apiServer;

    public App()
    {
        InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        _mainWindow = new MainWindow();
        _mainWindow.Activate();

        _httpServer = new SimpleHttpServer();
        _httpServer.Start();

        _apiServer = new SimpleApiServer();

        _mainWindow.Closed += (_, _) =>
        {
            _httpServer.Dispose();
            _apiServer.Dispose();
        };
    }
}
