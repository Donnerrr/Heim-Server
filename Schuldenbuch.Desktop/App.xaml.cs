namespace Schuldenbuch.Desktop;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new MainPage()) { Title = "Schuldenbuch"};


        window.HandlerChanged += (sender, e) =>
        {
#if WINDOWS
            var nativeWindow = window.Handler?.PlatformView as Microsoft.UI.Xaml.Window;
            if (nativeWindow != null)
            {
                // Holt sich das Handle des Fensters
                var windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
                var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(windowHandle);
                var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
                
                // Setzt das Icon für die Titelleiste oben links
                // Wichtig: Die Datei muss im Build-Ordner ankommen (z.B. aus wwwroot kopiert)
               var iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "meinappicon.ico");

// DEBUG: Wenn das Fenster kurz aufblinkt und den Pfad in der Konsole ausgibt,
// wissen wir, dass der Code hier wirklich hinkommt.
System.Diagnostics.Debug.WriteLine($"Versuche Icon zu laden: {iconPath} - Datei vorhanden: {File.Exists(iconPath)}");

if (File.Exists(iconPath))
{
    appWindow.SetIcon("wwwroot\\meinappicon.ico");
}
            }
#endif
        };

        return window;
    }
}
