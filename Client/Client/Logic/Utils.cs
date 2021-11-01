using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;


namespace Client.Logic
{
    public static class Utils
    {
        public static string SpanToString(TimeSpan span)
        {
            return span.Hours > 0 ? span.ToString(@"hh\:mm\:ss") : span.ToString(@"mm\:ss");
        }

        public static BitmapImage ByteToImage(byte[] imageData)
        {
            if (imageData == null)
            {
                return null;
            }

            BitmapImage bmpImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            bmpImg.BeginInit();
            bmpImg.StreamSource = ms;
            bmpImg.EndInit();
            bmpImg.CacheOption = BitmapCacheOption.OnLoad;

            return bmpImg;
        }

        public static byte[] ImageToByte(ImageSource imageSource)
        {
            if (imageSource == null)
            {
                return null;
            }

            var encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageSource as BitmapSource));

            byte[] result;
            using (var stream = new MemoryStream())
            {
                encoder.Save(stream);
                result = stream.ToArray();
            }
            
            return result;
        }

        public static Playlist ConvertShD(this SharedData.Playlist playlist)
        {
            var image = playlist.Name + ".png";
            var imagePath = ConfigurationMaster.CachedImage(image, playlist.DataImage);

            return new Playlist(null, playlist.Name, imagePath, playlist.Author);
        }
    }
}
