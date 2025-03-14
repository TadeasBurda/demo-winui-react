using Microsoft.UI.Xaml;
using System;

namespace Demo.App;

public sealed partial class MainWindow : Window
{
    private readonly SimpleHttpServer _httpServer = new();
    private readonly SimpleApiServer _apiServer = new();

    public MainWindow()
    {
        InitializeComponent();

        _webView2.Source = new Uri(SimpleHttpServer.BASE_URL);
    }

    private void MainWindow_Closed(object _, WindowEventArgs __)
    {
        _httpServer.Dispose();
        _apiServer.Dispose();
    }
}
