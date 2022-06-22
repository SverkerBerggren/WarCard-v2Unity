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

    static void WriteBigEndianInteger(UInt64 IntegerToWrite, int IntegerSize,Stream OutStream)
    {
        byte[] ArrayToWrite = new byte[IntegerSize];
        for(int i = 0; i < IntegerSize;i++)
        {
            ArrayToWrite[i] = (byte)(IntegerToWrite >> (IntegerSize * 8 - ((i + 1) * 8)));
        }
        OutStream.Write(ArrayToWrite, 0, IntegerSize);
    }
    static UInt64 ReadBigEndianInteger(int IntegerSize,Stream InStream)
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

        TcpClient Connecterino = new TcpClient("mrboboget.se", 443);
        NetworkStream Stream = Connecterino.GetStream();

        MemoryStream MemoryStreamToUse = new MemoryStream();
        //byte[] Buffer = new byte[4];
        RuleManager.MoveAction TestMove = new RuleManager.MoveAction();
        TestMove.NewPosition = new RuleManager.Coordinate(1, 1);
        TestMove.PlayerIndex = 1;
        TestMove.UnitID = 2;
        print("Network Test");
        byte[] MoveData = Encoding.UTF8.GetBytes(MBJson.JSONObject.SerializeObject(TestMove).ToString());
        print(System.Text.Encoding.UTF8.GetString(MoveData));
        WriteBigEndianInteger((ulong) MoveData.Length, 4, MemoryStreamToUse);
        MemoryStreamToUse.Write(MoveData, 0, MoveData.Length);

        RuleManager.AttackAction TestAttack = new RuleManager.AttackAction();
        TestAttack.PlayerIndex = 1;
        TestAttack.AttackerID = 2;
        TestAttack.DefenderID = 2;
        byte[] AttackData = Encoding.UTF8.GetBytes(MBJson.JSONObject.SerializeObject(TestAttack).ToString());
        print(System.Text.Encoding.UTF8.GetString(AttackData));
        WriteBigEndianInteger((ulong)AttackData.Length, 4, MemoryStreamToUse);
        MemoryStreamToUse.Write(AttackData, 0, AttackData.Length);

        
        print(MBJson.JSONObject.SerializeObject(TestMove).ToString());
        print(MBJson.JSONObject.SerializeObject(TestAttack).ToString());

        byte[] BytesToSend = MemoryStreamToUse.GetBuffer();
        print(BytesToSend.Length);
        Stream.Write(BytesToSend, 0, BytesToSend.Length);


        Connecterino.Close();
        return;
        for (int i = 0; i < 2; i++)
        {
            int ObjectSize = (int)ReadBigEndianInteger(4, Stream);
            byte[] ByteBuffer = new byte[ObjectSize];
            Stream.Read(ByteBuffer, 0, ObjectSize);
            print(MBJson.JSONObject.ParseJSONObject(ByteBuffer, 0, out ObjectSize));
        }
    }
    [Serializable]
    class SerializeTest
    {
        public int TestInt = 123;
        public string TestString = "StringData";
        public List<int> TestArray = new List<int>{1, 2, 3, 4};
    }
    void Start()
    {
        Thread TestThread = new Thread(new ThreadStart(TestSendStuff));
        TestThread.Start();
        byte[] BufferData = Encoding.UTF8.GetBytes("{\"123321\":123,\"array\":[123,true]}");
        int OutInt;
        print(MBJson.JSONObject.ParseJSONObject(BufferData, 0, out OutInt).ToString());
        print(MBJson.JSONObject.SerializeObject(new SerializeTest()).ToString());
        SerializeTest DeserializedData = MBJson.JSONObject.DeserializeObject<SerializeTest>(MBJson.JSONObject.SerializeObject(new SerializeTest()));
        print(DeserializedData.TestInt);
        print(DeserializedData.TestString);
        foreach(int integer in DeserializedData.TestArray)
        {
            print(integer);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
