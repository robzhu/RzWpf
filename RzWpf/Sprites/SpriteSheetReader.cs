using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace RzWpf
{
    public static class SpriteSheetReader
    {
        public static string QueryString = "/Text/Description";

        public static SpriteSheetMetadata ReadMetadata( string imageUri )
        {
            PngBitmapDecoder pngDecoder = new PngBitmapDecoder( 
                new Uri( imageUri, UriKind.RelativeOrAbsolute ),
                BitmapCreateOptions.PreservePixelFormat,
                BitmapCacheOption.Default );

            BitmapFrame pngFrame = pngDecoder.Frames[ 0 ];
            BitmapMetadata metadata = pngFrame.Metadata as BitmapMetadata;
            string serializedMetadata = metadata.GetQuery( QueryString ) as string;

            //InPlaceBitmapMetadataWriter pngInplace = pngFrame.CreateInPlaceBitmapMetadataWriter();
            //string serializedMetadata = pngInplace.GetQuery( QueryString ) as string;

            return new SpriteSheetMetadata( serializedMetadata );
        }
    }

    public static class SpriteSheetWriter
    {
        public const string QueryKey = "/Text/Description";

        public static void Save( string filePath, BitmapImage bitmap, SpriteSheetMetadata metadata )
        {
            File.Delete( filePath );

            FileStream stream = new FileStream( filePath, FileMode.Create );
            PngBitmapEncoder encoder = new PngBitmapEncoder();

            string spriteSheetMD = metadata.Serialize();

            //Initialize and set the metadata on the frame.
            var bitmapMetadata = new BitmapMetadata( "png" );
            bitmapMetadata.SetQuery( QueryKey, spriteSheetMD );

            BitmapFrame frame = BitmapFrame.Create( bitmap, null, bitmapMetadata, null );

            encoder.Frames.Add( frame );
            encoder.Save( stream );

            stream.Flush();
            stream.Close();
        }
    }
}
