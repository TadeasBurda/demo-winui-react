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

        _httpServer = new SimpleHttpServer(["http://localhost:8080/"]);
        _httpServer.Start();

        _apiServer = new SimpleApiServer();
        _apiServer.Start();

        _mainWindow.Closed += (_, _) => _httpServer.Dispose();
        _mainWindow.Closed += (_, _) => _apiServer.Dispose();
    }
}
