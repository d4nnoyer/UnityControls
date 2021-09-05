namespace UnityControls
{
    public class StreamString
    {
        private readonly System.IO.Stream _io;
        private readonly System.Text.UnicodeEncoding _strEncode;

        public StreamString(System.IO.Stream io)
        {
            this._io = io;
            _strEncode = new System.Text.UnicodeEncoding();
        }

        public string ReadString()
        {
            int len = _io.ReadByte() * 256;
            len += _io.ReadByte();
            byte[] inBuffer = new byte[len];
            _io.Read(inBuffer, 0, len);

            return _strEncode.GetString(inBuffer);
        }

        public void WriteString(string outString)
        {
            byte[] outBuffer = _strEncode.GetBytes(outString);
            int len = outBuffer.Length;
            if (len > ushort.MaxValue) len = ushort.MaxValue;
            _io.WriteByte((byte)(len / 256));
            _io.WriteByte((byte)(len & 255));
            _io.Write(outBuffer, 0, len);
            _io.Flush();
        }
    }
}