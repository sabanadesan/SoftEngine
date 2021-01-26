using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

using SharpDX;
using Windows.UI.Xaml.Media.Imaging;

namespace App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Device device;
        Mesh[] meshes;
        Camera mera = new Camera();

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += Page_Loaded;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Choose the back buffer resolution here
            WriteableBitmap bmp = new WriteableBitmap(640, 480);

            device = new Device(bmp);

            // Our Image XAML control
            frontBuffer.Source = bmp;

            meshes = await device.LoadJSONFileAsync("monkey.babylon");

            mera.Position = new Vector3(0, 0, 10.0f);
            mera.Target = Vector3.Zero;

            // Registering to the XAML rendering loop
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        // Rendering loop handler
        void CompositionTarget_Rendering(object sender, object e)
        {
            device.Clear(0, 0, 0, 255);

            foreach (var mesh in meshes)
            {
                // rotating slightly the meshes during each frame rendered
                mesh.Rotation = new Vector3(mesh.Rotation.X + 0.01f, mesh.Rotation.Y + 0.01f, mesh.Rotation.Z);
            }

            // Doing the various matrix operations
            device.Render(mera, meshes);
            // Flushing the back buffer into the front buffer
            device.Present();
        }
    }
}
