using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Foundation;
using UIKit;

namespace Nihol.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            LoadApplication(new App());
            CopyFiles("En01");
            CopyFiles("En02");
            CopyFiles("En03");
            CopyFiles("En04");
            return base.FinishedLaunching(app, options);
        }
        
        public void CopyFiles(string fileName)
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var inDocuments = NSBundle.MainBundle.BundlePath;
            var fileNameInDocuments = Path.Combine(documents, fileName + ".pdf");
            //Directory.CreateDirectory(directoryName);
            if (!File.Exists(fileNameInDocuments))
            {
                File.Copy(Path.Combine(inDocuments, "Documents" ,fileName + ".pdf"), fileNameInDocuments);
            }
        }
    }
}
