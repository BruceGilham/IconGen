using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using ImageProcessor;
using ImageProcessor.Imaging.Formats;

namespace IconGen
{
    internal class ProcessImage
    {
        public void Process(string sourceFile, string outputLocation, ObservableCollection<string> output)
        {
            if (sourceFile == null) return;
            if (outputLocation == null) return;

            if (!Directory.Exists(outputLocation))
                if (MessageBoxResult.Yes ==
                    MessageBox.Show("Output Directory doesn't exist, create it?", "Create Directory",
                        MessageBoxButton.YesNo))
                {
                    Directory.CreateDirectory(outputLocation);
                }
                else
                {
                    return;
                }
            else
            {
                //Check to see if there are files and if there are ask if we want to delete them
                var files = Directory.GetFiles(outputLocation);
                if (files.Any())
                {
                    var results = MessageBox.Show(
                        "Delete Existing Files?  Files with the same name will be overwritten", "Delete Files",
                        MessageBoxButton.YesNoCancel);

                    switch (results)
                    {
                        case MessageBoxResult.None:
                            return;
                        case MessageBoxResult.OK:
                            break;
                        case MessageBoxResult.Cancel:
                            return;
                        case MessageBoxResult.Yes:
                            foreach (var file in files)
                            {
                                File.Delete(file);
                            }
                            break;
                        case MessageBoxResult.No:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            //Run the code in a thread and update the status string
            var t = new Task(() =>
            {
                //Open the file
                SaveImage(sourceFile, outputLocation, 180, output: output);
                SaveImage(sourceFile, outputLocation, 76, output: output);
                SaveImage(sourceFile, outputLocation, 152, output: output);
                SaveImage(sourceFile, outputLocation, 40, output: output);
                SaveImage(sourceFile, outputLocation, 48, output: output);
                SaveImage(sourceFile, outputLocation, 58, output: output);
                SaveImage(sourceFile, outputLocation, 72, output: output);
                SaveImage(sourceFile, outputLocation, 96, output: output);
                SaveImage(sourceFile, outputLocation, 144, output: output);
                SaveImage(sourceFile, outputLocation, 192, output: output);
                SaveImage(sourceFile, outputLocation, 50, output: output);
                SaveImage(sourceFile, outputLocation, 100, output: output);
                SaveImage(sourceFile, outputLocation, 152, output: output);
                SaveImage(sourceFile, outputLocation, 87, output: output);
                SaveImage(sourceFile, outputLocation, 57, output: output);
                SaveImage(sourceFile, outputLocation, 114, output: output);
                SaveImage(sourceFile, outputLocation, 120, output: output);
                SaveImage(sourceFile, outputLocation, 29, output: output);
                SaveImage(sourceFile, outputLocation, 58, output: output);
                SaveImage(sourceFile, outputLocation, 80, output: output);
                SaveImage(sourceFile, outputLocation, 512, output: output);
                SaveImage(sourceFile, outputLocation, 1024, output: output);
                SaveImage(sourceFile, outputLocation, 320, 480, output);
                SaveImage(sourceFile, outputLocation, 640, 960, output);
                SaveImage(sourceFile, outputLocation, 640, 1136, output);
                SaveImage(sourceFile, outputLocation, 768, 1004, output);
                SaveImage(sourceFile, outputLocation, 1024, 748, output);
                SaveImage(sourceFile, outputLocation, 1536, 2008, output);
                SaveImage(sourceFile, outputLocation, 2048, 1496, output);
            });
            t.Start();
        }

        private static void SaveImage(string sourceFile, string outputLocation, int newWidth, int newHeight = 0, ObservableCollection<string> output = null)
        {
            try
            {
                var imageBytes = File.ReadAllBytes(sourceFile);
                var flieExt = Path.GetExtension(sourceFile);
                var size = new System.Drawing.Size(newWidth, newHeight);
                string newFileName;
                if (newHeight == 0)
                    newFileName = "icon-" + newWidth + "x" + newWidth;
                else
                    newFileName = "icon-" + newWidth + "x" + newHeight;

                using (var inStream = new MemoryStream(imageBytes))
                {
                    var outputFileName = Path.Combine(outputLocation, newFileName + flieExt);
                    Application.Current.Dispatcher.Invoke(() => output?.Insert(0, DateTime.Now + ": " + outputFileName));

                    using (var outStream = new FileStream(outputFileName, FileMode.OpenOrCreate))
                    {
                        // Initialize the ImageFactory using the overload to preserve EXIF metadata.
                        using (var imageFactory = new ImageFactory(preserveExifData: true))
                        {
                            ISupportedImageFormat format = new PngFormat();
                            // Load, resize, set the format and quality and save an image.
                            imageFactory.Load(inStream).Resize(size).Format(format).Save(outStream);
                            Application.Current.Dispatcher.Invoke(() => output?.Insert(0, DateTime.Now + ": " + outputFileName + " Saved"));
                        }
                        // Do something with the stream.
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
