namespace ControleDeProdutosAula
{
    public static class Util
    {
        public static async Task<byte[]> ToArrayAsync(this Stream stream)
        {
            var memoryStream = new MemoryStream();
            await memoryStream.CopyToAsync(stream);
            return await Task.FromResult(memoryStream.ToArray());
        }

        public static byte[] ToByteArray(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public static byte[] StreamToByteArray(Stream stream)
        {
            if (stream is MemoryStream)
            {
                return ((MemoryStream)stream).ToArray();
            }
            else
            {
                // Jon Skeet's accepted answer 
                return ReadFully(stream);
            }
        }

        public static byte[] ReadFully2(string input)
        {
            FileStream sourceFile = new FileStream(input, FileMode.Open); //Open streamer
            BinaryReader binReader = new BinaryReader(sourceFile);
            byte[] output = new byte[sourceFile.Length]; //create byte array of size file
            for (long i = 0; i < sourceFile.Length; i++)
                output[i] = binReader.ReadByte(); //read until done
            sourceFile.Close(); //dispose streamer
            binReader.Close(); //dispose reader
            return output;
        }

        internal static byte[]? ReadFully2(Func<string?> toString)
        {
            throw new NotImplementedException();
        }
    }
}
