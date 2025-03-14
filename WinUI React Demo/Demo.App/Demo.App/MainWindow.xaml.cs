using Microsoft.UI.Xaml;

namespace Demo.App;

public sealed partial class MainWindow : Window
{
    private readonly SimpleHttpServer _httpServer;
    private readonly SimpleApiServer _apiServer;

    public MainWindow()
    {
        InitializeComponent();

        _httpServer = new SimpleHttpServer();
        _httpServer.Start();

        _apiServer = new SimpleApiServer();
    }

    private void MainWindow_Closed(object sender, WindowEventArgs e)
    {
        _httpServer.Dispose();
        _apiServer.Dispose();
    }
}
