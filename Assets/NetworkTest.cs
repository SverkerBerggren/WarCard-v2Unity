using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;

//using System.Text.Json;

//using System.Text.Json;

public class NetworkTest : MonoBehaviour
{
    // Start is called before the first frame update

    void WriteBigEndianInteger(UInt64 IntegerToWrite, int IntegerSize,Stream OutStream)
    {
        byte[] ArrayToWrite = new byte[IntegerSize];
        for(int i = 0; i < IntegerSize;i++)
        {
            ArrayToWrite[i] = (byte)(IntegerToWrite >> (IntegerSize * 8 - ((i + 1) * 8)));
        }
        OutStream.Write(ArrayToWrite, 0, IntegerSize);
    }
    UInt64 ReadBigEndianInteger(int IntegerSize,Stream InStream)
    {
        UInt64 ReturnValue = 0;
        byte[] IntegerBytes = new byte[IntegerSize];
        int ReadBytes = InStream.Read(IntegerBytes, 0, IntegerSize);
        if(ReadBytes < IntegerSize)
        {
            throw new Exception("Early end of stream reached");
        }
        for(int i = 0; i < IntegerSize; i++)
        {
            ReturnValue <<= 8;
            ReturnValue += IntegerBytes[i];
        }
        return (ReturnValue);
    }

    
    public static void TestSendStuff()
    {
        //sprint("Kommisch hitisch")
        RuleManager.MoveAction MoveStuff = new RuleManager.MoveAction();
        MoveStuff.NewPosition = new RuleManager.Coordinate(1, 1);
        MoveStuff.PlayerIndex = 12;
        MoveStuff.UnitID = 1221;

        TcpClient Connecterino = new TcpClient("192.168.0.123", 13000);
        NetworkStream Stream = Connecterino.GetStream();
        byte[] Buffer;
        string TempTestData = JsonUtility.ToJson(MoveStuff);
        print(TempTestData);
        Buffer = Encoding.ASCII.GetBytes(TempTestData);
        Stream.Write(Buffer, 0, Buffer.Length);
        Buffer = new byte[4096];
        int ReadBytes = Stream.Read(Buffer, 0, Buffer.Length);
        //MoveAction NewAction = JsonUtility.FromJson<MoveAction>(Encoding.Default.GetString(Buffer,0,ReadBytes));
        RuleManager.Action NewAction = JsonUtility.FromJson<RuleManager.Action>(Encoding.Default.GetString(Buffer,0,ReadBytes));
        print(NewAction is RuleManager.MoveAction);
        //print(NewAction.NewPosition);
        //print(NewAction.PlayerIndex);
        //print(NewAction.UnitID);
    }

    void Start()
    {
        Thread TestThread = new Thread(new ThreadStart(TestSendStuff));
        TestThread.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
