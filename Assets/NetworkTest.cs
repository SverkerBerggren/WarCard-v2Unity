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
    static void WriteBigEndianInteger(UInt64 IntegerToWrite, int IntegerSize, Stream OutStream)
    {
        byte[] ArrayToWrite = new byte[IntegerSize];
        for (int i = 0; i < IntegerSize; i++)
        {
            ArrayToWrite[i] = (byte)(IntegerToWrite >> (IntegerSize * 8 - ((i + 1) * 8)));
        }
        OutStream.Write(ArrayToWrite, 0, IntegerSize);
    }
    static UInt64 ReadBigEndianInteger(int IntegerSize, Stream InStream)
    {
        UInt64 ReturnValue = 0;
        byte[] IntegerBytes = new byte[IntegerSize];
        int ReadBytes = InStream.Read(IntegerBytes, 0, IntegerSize);
        if (ReadBytes < IntegerSize)
        {
            throw new Exception("Early end of stream reached");
        }
        for (int i = 0; i < IntegerSize; i++)
        {
            ReturnValue <<= 8;
            ReturnValue += IntegerBytes[i];
        }
        return (ReturnValue);
    }

    public static void ClientPart()
    {
        RuleServer.ClientConnection NewConnection = new RuleServer.ClientConnection("127.0.0.1", 21337);
        RuleServer.ServerMessage Response = NewConnection.SendMessage(new RuleServer.OpponentActionPoll());
        print(MBJson.JSONObject.SerializeObject(Response).ToString());
        Response = NewConnection.SendMessage(new RuleServer.OpponentActionPoll());
        print(MBJson.JSONObject.SerializeObject(Response).ToString());
    }
    public static void ServerPart()
    {
        RuleServer.RuleServer LocalServer = new RuleServer.RuleServer(21337);
        LocalServer.Run();
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
        Thread ClientThread = new Thread(new ThreadStart(ClientPart));
        ClientThread.Start();
        Thread ServerThread = new Thread(new ThreadStart(ServerPart));
        ServerThread.Start();
        //int tempint;
        //print(MBJson.JSONObject.ParseJSONObject(Encoding.UTF8.GetBytes("{\"OpponentIndex\":0,\"GameIdentifier\":0,\"Type\":2,\"ConnectionIdentifier\":0}"), 0, out tempint).ToString());
        
        //byte[] BufferData = Encoding.UTF8.GetBytes("{\"123321\":123,\"array\":[123,true]}");
        //int OutInt;
        //print(MBJson.JSONObject.ParseJSONObject(BufferData, 0, out OutInt).ToString());
        //print(MBJson.JSONObject.SerializeObject(new SerializeTest()).ToString());
        //SerializeTest DeserializedData = MBJson.JSONObject.DeserializeObject<SerializeTest>(MBJson.JSONObject.SerializeObject(new SerializeTest()));
        //print(DeserializedData.TestInt);
        //print(DeserializedData.TestString);
        //foreach(int integer in DeserializedData.TestArray)
        //{
        //    print(integer);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
