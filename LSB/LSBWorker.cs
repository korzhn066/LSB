using Microsoft.Win32;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows;

namespace LSB
{
    internal class LSBWorker
    {
        public void Hide(
            string filePath,
            string text = "text for hidding",
            int emptyPixels = 0,
            ColorType colorType = ColorType.Red,
            BitPlaneType bitPlaneType = BitPlaneType.Zero)
        {
            var bitmap = new Bitmap(filePath);
            var resultBitmap = new Bitmap(bitmap);

            SetMessageLendth(
                resultBitmap,
                text.Length,
                emptyPixels,
                colorType,
                bitPlaneType);

            var offset = 8 +  8 * emptyPixels;

            foreach (var symbol in text)
            {
                var symbolbits = ByteConverter.ByteToBits((byte)symbol);

                var number = 0;

                for (int i = 0; i < 8; i++)
                {
                    number = offset + i + i * emptyPixels;

                    var x = number % bitmap.Width;
                    var y = number / bitmap.Width;

                    
                    if (y > bitmap.Height)
                    {
                        throw new Exception("Text so long");
                    }

                    var pixel = bitmap.GetPixel(x, y);

                    var newColor = GetColorWithInformation(
                        pixel,
                        colorType,
                        symbolbits[i],
                        bitPlaneType);

                    resultBitmap.SetPixel(x, y, newColor);
                }

                offset = number + 1;
            }

            var saveFileDialog = new SaveFileDialog();
            
            if (saveFileDialog.ShowDialog() == true)
            {
                resultBitmap.Save(saveFileDialog.FileName, ImageFormat.Bmp);
            }
        }

        private static void SetMessageLendth(
            Bitmap resultBitmap,
            int length,
            int emptyPixels = 0,
            ColorType colorType = ColorType.Red,
            BitPlaneType bitPlaneType = BitPlaneType.Zero)
        {
            var temp = (byte)length;

            var symbolbits = ByteConverter.ByteToBits((byte)length);

            for (int i = 0; i < 8; i++)
            {
                var number = i + i * emptyPixels;

                var x = number % resultBitmap.Width;
                var y = number / resultBitmap.Width;


                if (y > resultBitmap.Height)
                {
                    throw new Exception("Text so long");
                }

                var pixel = resultBitmap.GetPixel(x, y);

                var newColor = GetColorWithInformation(
                    pixel,
                    colorType,
                    symbolbits[i],
                    bitPlaneType);

                resultBitmap.SetPixel(x, y, newColor);
            }
        }

        private static byte GetMessageLendth(
            Bitmap bitmap,
            int emptyPixels = 0,
            ColorType colorType = ColorType.Red,
            BitPlaneType bitPlaneType = BitPlaneType.Zero)
        {
            var symbolbits = new BitArray(8);

            for (int j = 0; j < 8; j++)
            {
                var number = j + j * emptyPixels;

                var x = number % bitmap.Width;
                var y = number / bitmap.Width;

                if (y > bitmap.Height)
                {
                    throw new Exception("Text so long");
                }

                var pixel = bitmap.GetPixel(x, y);

                symbolbits[j] = GetInformationFromColor(
                    pixel,
                    colorType,
                    bitPlaneType);
            }

            return ByteConverter.BitsToByte(symbolbits);
        }

        public void Show(
            string filePath,
            int emptyPixels = 0,
            ColorType colorType = ColorType.Red,
            BitPlaneType bitPlaneType = BitPlaneType.Zero)
        {
            var bitmap = new Bitmap(filePath);
            var offset = 8 + 8 * emptyPixels;

            var messageLength = GetMessageLendth(
                bitmap,
                emptyPixels,
                colorType,
                bitPlaneType);

            var result = new StringBuilder("");

            for (int i = 0; i < messageLength; i++)
            {
                var symbolbits = new BitArray(8);

                int number = 0;

                for (int j = 0; j < 8; j++)
                {
                    number = offset + j + j * emptyPixels;

                    var x = number % bitmap.Width;
                    var y = number / bitmap.Width;

                    if (y > bitmap.Height)
                    {
                        throw new Exception("Text so long");
                    }

                    var pixel = bitmap.GetPixel(x, y);

                    symbolbits[j] = GetInformationFromColor(
                        pixel,
                        colorType,
                        bitPlaneType);
                }

                offset = number + 1;

                result.Append((char)ByteConverter.BitsToByte(symbolbits));
            }

            MessageBox.Show(result.ToString());
        }





        private static bool GetInformationFromColor(
            Color color,
            ColorType colorType,
            BitPlaneType bitPlaneType)
        {
            var byteOfColor = colorType switch
            {
                ColorType.Red => color.R,
                ColorType.Green => color.G,
                ColorType.Blue => color.B,
                _ => throw new NotImplementedException()
            };

            var bitsOfColor = ByteConverter.ByteToBits(byteOfColor);

            return bitsOfColor[(int)bitPlaneType];
        }
        

        private static Color GetColorWithInformation(
            Color color, 
            ColorType colorType, 
            bool bit, 
            BitPlaneType bitPlaneType)
        {
            var byteOfColor = colorType switch
            {
                ColorType.Red => color.R,
                ColorType.Green => color.G,
                ColorType.Blue => color.B,
                _ => throw new NotImplementedException()
            };

            var bitsOfColor = ByteConverter.ByteToBits(byteOfColor);

            bitsOfColor[(int)bitPlaneType] = bit;

            var r = 1;
            var g = 1;
            var b = 1;

            switch (colorType)
            {
                case ColorType.Red:
                    r = ByteConverter.BitsToByte(bitsOfColor);
                    g = color.G;
                    b = color.B;
                    break;
                case ColorType.Green:
                    r = color.R;
                    g = ByteConverter.BitsToByte(bitsOfColor);
                    b = color.B;
                    break;
                case ColorType.Blue:
                    r = color.R;
                    g = color.G;
                    b = ByteConverter.BitsToByte(bitsOfColor);
                    break;
            }

            return Color.FromArgb(r, g, b);
        }
    }
}
