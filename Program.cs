﻿using Avalonia;
using Avalonia.ReactiveUI;
using System;

namespace MyApp;

class Program
{
    
    public static void Main(string[] args)
     => BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();

}
