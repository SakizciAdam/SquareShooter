using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareShooter.Network
{
    public class ByteWrapper
    {
        public byte[] data;
        public int index = 0;
        public ByteWrapper(byte[] data) { 
            this.data = data;

        }

        public void WriteByte(byte value)
        {
            if (this.data.Length == index)
            {
                Array.Resize<byte>(ref this.data, this.data.Length+1);
            }
            this.data[index++] = value; 
        }

        public void WriteBytes(byte[] value)
        {
            foreach(byte x in value)
            {
                
                WriteByte(x);
            }
        }

        public void WriteString(string value)
        {
            WriteInt(Encoding.UTF8.GetBytes(value).Length);
            WriteBytes(Encoding.UTF8.GetBytes(value));


        }

        public void WriteInt(int value) {
            WriteBytes(BitConverter.GetBytes(value));
        }

        public void WriteFloat(float value)
        {
            WriteBytes(BitConverter.GetBytes(value));
        }


        public int ReadInt()
        {
            return BitConverter.ToInt32(ReadBytes(4), 0);
        }

        public float ReadFloat()
        {
            return BitConverter.ToSingle(ReadBytes(4), 0);
        }

        public double ReadDouble()
        {
            return BitConverter.ToDouble(ReadBytes(8), 0);
        }

        public char ReadChar()
        {
            return BitConverter.ToChar(ReadBytes(2), 0);
        }

        public byte ReadByte()
        {
            return data[index++];
        }

        public string ReadString()
        {
            int length=ReadInt();

            return Encoding.UTF8.GetString(ReadBytes(length), 0, length);
        }

        public byte[] ReadBytes(int n)
        {
            byte[] v=new byte[n];
            Array.Copy(data, index, v, 0, n);
            index += n;
            return v;
        }
    }
}
