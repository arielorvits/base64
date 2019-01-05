using System;
using System.Collections;
using System.Text;

namespace base64
{
    class Base64Enc
    {
        byte[] output;
        int inputIndex, outputIndex;
        byte first6bits, second6bits, third6bits, forth6bits;
        static string value2char = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";


        private byte SixBitsToBase64(byte val) { 
            return (byte)value2char[val];
        } 
        
        public Base64Enc(byte[] output)
        {
            this.output = output;
            inputIndex = 0;
            outputIndex = 0;
        }

        public void PutByte(byte b)
        {
            inputIndex++;
            if(inputIndex % 3 == 1){
                first6bits = (byte)(0xfc & b);
                output[outputIndex++] = SixBitsToBase64(first6bits);
                second6bits = (byte)((0x03 & b) << 4);
            }
            else if(inputIndex % 3 == 2){
                second6bits = (byte)(second6bits | (0xf0 & b) >> 4);
                output[outputIndex++] = SixBitsToBase64(second6bits);
                third6bits = (byte)((0x0f & b) << 2);
            }
            else if(inputIndex % 3 == 0){
                third6bits = (byte)(third6bits | ((0xc0 & b) >> 6));
                output[outputIndex++] = SixBitsToBase64(third6bits);
                forth6bits = (byte)(0x3f & b);
                output[outputIndex++] = SixBitsToBase64(forth6bits);
            }
        }

        /// Flush returns the total length of the output string
        public int Flush()
        {
            if(inputIndex%3 == 1){
                output[outputIndex++] = SixBitsToBase64(second6bits);
                output[outputIndex++] = (byte)'=';
                output[outputIndex++] = (byte)'=';
            }
            else if(inputIndex % 3 == 2){
                output[outputIndex++] = SixBitsToBase64(third6bits);
                output[outputIndex++] = (byte)'=';
            }
            return outputIndex;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            byte[] output = new byte[1000];
            var base64enc = new Base64Enc(output);

            base64enc.PutByte((byte)1);
            base64enc.PutByte((byte)1);
            base64enc.PutByte((byte)0);
            base64enc.PutByte((byte)1);
            //base64enc.PutByte((byte)1);
            //base64enc.PutByte((byte)0);
            int len = base64enc.Flush();
            Console.WriteLine(Encoding.ASCII.GetString(output, 0, len));
        }
    }
}