using Schuldenbuch.Desktop;

namespace Schuldenbuch.Desktop;

public static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        MauiProgram
            .CreateMauiApp()
            .Run();
    }
}