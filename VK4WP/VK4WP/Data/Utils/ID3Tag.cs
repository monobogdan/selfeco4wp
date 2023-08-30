using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VK4WP.Data.Utils
{
    public static class ID3Tag
    {
        static byte[] Tag = { (byte)'T', (byte)'A', (byte)'G'};

        private static void WriteFixedString(BinaryWriter writer, string str, int len)
        {
            if(str.Length > 30)
                str = str.Substring(0, 30);

            for(int i = 0; i < len; i++)
            {
                if (i >= str.Length)
                    writer.Write((byte)0);
                else
                    writer.Write((byte)str[i]);
                
            }
        }

        public static byte[] Fill(string artist, string trackName, string album)
        {
            MemoryStream stream = new MemoryStream(128); // ID3v1 are fixed to1 128 bytes
            BinaryWriter writer = new BinaryWriter(stream);

            writer.Write(Tag);
            WriteFixedString(writer, trackName, 30);
            WriteFixedString(writer, artist, 30);
            WriteFixedString(writer, album, 30);
            WriteFixedString(writer, "0000", 4);
            WriteFixedString(writer, "Downloaded by VK4WP", 28);
            writer.Write((byte)0);

            return stream.ToArray();
        }
    }
}
