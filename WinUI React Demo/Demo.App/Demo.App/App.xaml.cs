﻿using Microsoft.UI.Xaml;

namespace Demo.App;

public partial class App : Application
{
    private Window? _mainWindow;

    public App()
    {
        InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        _mainWindow = new MainWindow();
        _mainWindow.Activate();
    }
}
