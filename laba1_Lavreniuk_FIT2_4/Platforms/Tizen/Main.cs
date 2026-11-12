using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using System;

namespace laba1_Lavreniuk_FIT2_4
{
    internal class Program : MauiApplication
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        static void Main(string[] args)
        {
            var app = new Program();
            app.Run(args);
        }
    }
}
