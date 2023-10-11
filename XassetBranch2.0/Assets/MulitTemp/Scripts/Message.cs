using Google.Protobuf;
using Im;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal class Message
{
    public Message()
    {

    }

    private byte[] buffer = new byte[1024];
    private int startIndex;

    public byte[] Buffer
    {
        get { return buffer; }
    }
    public int StartIndex
    {
        get { return startIndex; }
    }
    public int Remsize
    {
        get { return buffer.Length - startIndex; }
    }

    public void ReadBuffer(int len, Action<ImMsg> handle)
    {
        startIndex += len;

        while (true)
        {
            if (startIndex <= 4) return;
            int count = BitConverter.ToInt32(buffer, 0);
            if (startIndex >= count + 4)
            {
                ImMsg mainPack = (ImMsg)ImMsg.Descriptor.Parser.ParseFrom(buffer, 4, count);
                handle(mainPack);
                Array.Copy(buffer, count + 4, buffer, 0, startIndex - count - 4);
                startIndex -= (count + 4);
            }

            else
            {
                break;
            }
        }
    }

    public static byte[] PackData(ImMsg pack)
    {
        byte[] data = pack.ToByteArray(); //包体


        byte[] head = BitConverter.GetBytes(data.Length); //包头
        //foreach (byte b in head)
        //{
        //    UnityEngine.Debug.Log(b);
        //}
        return head.Concat(data).ToArray();

        
    }

}

