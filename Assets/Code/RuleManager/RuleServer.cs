using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace RuleServer
{



    public enum ClientMessageType
    {
        Null,
        GameAction,
        OpponentActionPoll,
    }
    [Serializable]
    public class ClientMessage : MBJson.JSONDeserializeable,MBJson.JSONTypeConverter
    {
        public ClientMessageType Type = ClientMessageType.Null;
        public ClientMessage()
        {

        }
        protected ClientMessage(ClientMessageType TypeToUse)
        {
            Type = TypeToUse;
        }
        public Type GetType(int MessageType)
        {
            Type ReturnValue = null;
            ClientMessageType SerializedType = (ClientMessageType)MessageType;
            if(SerializedType == ClientMessageType.GameAction)
            {
                ReturnValue = typeof(GameMessage);
            }
            else if(SerializedType == ClientMessageType.OpponentActionPoll)
            {
                ReturnValue = typeof(OpponentActionPoll);
            }
            else
            {
                throw new Exception("Invalid ClientMessage type when deserializing client message");
            }
            return (ReturnValue);
        }
        public object Deserialize(MBJson.JSONObject ObjectToParse)
        {
            return (new MBJson.DynamicJSONDeserializer(this).Deserialize(ObjectToParse));
        }
        public int ConnectionIdentifier = 0;
    }
    [Serializable]
    public class GameMessage : ClientMessage
    {
        protected GameMessage(ClientMessageType TypeToUse) : base(TypeToUse) { }
        public int GameIdentifier = 0;
    }
    public class GameAction : GameMessage
    {
        public GameAction() : base(ClientMessageType.GameAction) { }
        public RuleManager.Action ActionToExecute;
    }
    public class OpponentActionPoll : GameMessage
    {
        public OpponentActionPoll() : base(ClientMessageType.OpponentActionPoll) { }
        public int OpponentIndex = 0;
    }

    public enum ServerMessageType
    {
        Null,
        OpponentAction,
        Ok,
        Error,
    }
    [Serializable]
    public class ServerMessage : MBJson.JSONDeserializeable, MBJson.JSONTypeConverter
    {
        public ServerMessageType Type = ServerMessageType.Null;

        public ServerMessage()
        {

        }
        public ServerMessage(ServerMessageType TypeToUse)
        {
            Type = TypeToUse;
        }
        public Type GetType(int MessageType)
        {
            Type ReturnValue = null;
            ServerMessageType SerializedType = (ServerMessageType)MessageType;
            if (SerializedType == ServerMessageType.OpponentAction)
            {
                ReturnValue = typeof(OpponentAction);
            }
            else if(SerializedType == ServerMessageType.Error)
            {
                ReturnValue = typeof(ErrorMessage);
            }
            else if(SerializedType == ServerMessageType.Ok)
            {
                ReturnValue = typeof(ServerResponseOK);
            }
            else
            {
                throw new Exception("Invalid ClientMessage type when deserializing client message");
            }
            return (ReturnValue);
        }
        public object Deserialize(MBJson.JSONObject ObjectToParse)
        {
            return (new MBJson.DynamicJSONDeserializer(this).Deserialize(ObjectToParse));
        }
    }
    [Serializable]
    public class OpponentAction  : ServerMessage
    {
        public OpponentAction() : base(ServerMessageType.OpponentAction) { }
        public List<RuleManager.Action> OpponentActions;
    }
    public class ErrorMessage : ServerMessage
    {
        public ErrorMessage()
        {

        }
        public ErrorMessage(string Error) : base(ServerMessageType.Error) 
        {
            ErrorString = Error;
        }
        public string ErrorString = "";
    }
    public class ServerResponseOK : ServerMessage
    {
        public ServerResponseOK() : base(ServerMessageType.Ok) { }
    }



    public class ActiveGameInfo
    {
        Mutex m_InternalsMutex = new Mutex();
        private RuleManager.RuleManager m_GameRuleManager;
        List<List<RuleManager.Action>> m_PlayerActions = new List<List<RuleManager.Action>>();
        //Connection -> player index
        private Dictionary<int, int> ParticipatingPlayers = new Dictionary<int, int>();

        private ServerMessage p_Handle_Action(GameAction ActionMessage)
        {
            ServerMessage ReturnValue = null;
            m_InternalsMutex.WaitOne();
            if(!ParticipatingPlayers.ContainsKey(ActionMessage.ConnectionIdentifier))
            {
                ReturnValue =new ErrorMessage("Connection not present in game");
                m_InternalsMutex.ReleaseMutex();
                return (ReturnValue);
            }
            int PlayerIndex = ParticipatingPlayers[ActionMessage.ConnectionIdentifier];
            if(PlayerIndex != ActionMessage.ActionToExecute.PlayerIndex)
            {
                ReturnValue = new ErrorMessage("Can't execute actions for your opponent");
                m_InternalsMutex.ReleaseMutex();
                return (ReturnValue);
            }
            string ErrorString;
            bool ActionIsValid = m_GameRuleManager.ActionIsValid(ActionMessage.ActionToExecute,out ErrorString);
            if(!ActionIsValid)
            {
                ReturnValue = new ErrorMessage(ErrorString);
                m_InternalsMutex.ReleaseMutex();
                return (ReturnValue);
            }
            m_GameRuleManager.ExecuteAction(ActionMessage.ActionToExecute);
            m_InternalsMutex.ReleaseMutex();
            ReturnValue = new ServerResponseOK();
            return (ReturnValue);
        }
        ServerMessage p_Handle_ActionPoll(OpponentActionPoll PollMessage)
        {
            ServerMessage ReturnValue = null;
            m_InternalsMutex.WaitOne();
            if (!ParticipatingPlayers.ContainsKey(PollMessage.ConnectionIdentifier))
            {
                ReturnValue = new ErrorMessage("Connection not present in game");
                m_InternalsMutex.ReleaseMutex();
                return (ReturnValue);
            }
            int PlayerIndex = ParticipatingPlayers[PollMessage.ConnectionIdentifier];
            if(PlayerIndex == PollMessage.OpponentIndex)
            {
                ReturnValue = new ErrorMessage("Can't pop your own actions");
                m_InternalsMutex.ReleaseMutex();
                return (ReturnValue);
            }
            if(PollMessage.OpponentIndex > m_PlayerActions.Count || PollMessage.OpponentIndex < 0)
            {
                ReturnValue = new ErrorMessage("Invalid opponent index");
                m_InternalsMutex.ReleaseMutex();
                return (ReturnValue);
            }
            OpponentAction Response = new OpponentAction();
            Response.OpponentActions = new List<RuleManager.Action>(m_PlayerActions[PollMessage.OpponentIndex]);
            m_PlayerActions[PollMessage.OpponentIndex].Clear();
            m_InternalsMutex.WaitOne();
            return (ReturnValue);
        }
        public ServerMessage HandleMessage(GameMessage MessageToHandle)
        {
            ServerMessage ReturnValue = null;
            if(MessageToHandle is GameAction)
            {
                ReturnValue = p_Handle_Action((GameAction)MessageToHandle);
            }
            else if(MessageToHandle is OpponentActionPoll)
            {
                ReturnValue = p_Handle_ActionPoll((OpponentActionPoll)MessageToHandle);
            }
            return (ReturnValue);
        }
    }
    
    public class JSONConnection
    {
        protected static void WriteBigEndianInteger(UInt64 IntegerToWrite, int IntegerSize, Stream OutStream)
        {
            byte[] ArrayToWrite = new byte[IntegerSize];
            for (int i = 0; i < IntegerSize; i++)
            {
                ArrayToWrite[i] = (byte)(IntegerToWrite >> (IntegerSize * 8 - ((i + 1) * 8)));
            }
            OutStream.Write(ArrayToWrite, 0, IntegerSize);
        }
        protected static UInt64 ReadBigEndianInteger(int IntegerSize, Stream InStream)
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
        protected static byte[] GetMessageData(MBJson.JSONObject ObjectToSend)
        {
            MemoryStream TotalDataStream = new MemoryStream();
            byte[] TotalObjectData = System.Text.Encoding.UTF8.GetBytes(ObjectToSend.ToString());
            WriteBigEndianInteger((ulong)TotalObjectData.Length, 4, TotalDataStream);
            TotalDataStream.Write(TotalObjectData);
            return (TotalDataStream.ToArray());
        }
        protected static MBJson.JSONObject ParseJSONObject(Stream InStream)
        {
            MBJson.JSONObject ReturnValue = null;
            ulong MessageLength = ReadBigEndianInteger(4, InStream);
            byte[] MessageBuffer = new byte[MessageLength];
            ulong ReadBytes = 0;
            while (InStream.CanRead && ReadBytes < MessageLength)
            {
                ulong NewBytes = (ulong)InStream.Read(MessageBuffer, (int)ReadBytes, (int)(MessageLength - ReadBytes));
                if (NewBytes == 0)
                {
                    throw new Exception("End of data reached when reading JSON value");
                }
                ReadBytes += NewBytes;
            }
            int tempint;
            ReturnValue = MBJson.JSONObject.ParseJSONObject(MessageBuffer, 0, out tempint);
            //throw new Exception(System.Text.Encoding.UTF8.GetString(MessageBuffer));
            return (ReturnValue);
        }
    }

    public class ClientConnection : JSONConnection
    {

        NetworkStream m_AssociatedStream = null;

        public ClientConnection(string Adress,int Port)
        {
            TcpClient NewClient = new TcpClient(Adress, Port);
            m_AssociatedStream = NewClient.GetStream();
        }
        public ServerMessage SendMessage(ClientMessage MessageToSend)
        {
            ServerMessage ReturnValue = null;
            //throw new Exception(MessageToSend.ToString());
            //throw new Exception(MBJson.JSONObject.SerializeObject(MessageToSend).ToString());
            byte[] BytesToSend = GetMessageData(MBJson.JSONObject.SerializeObject(MessageToSend));
            m_AssociatedStream.Write(BytesToSend);
            ReturnValue = MBJson.JSONObject.DeserializeObject<ServerMessage>(ParseJSONObject(m_AssociatedStream));
            return (ReturnValue);
        }
    }

    public class ServerConnection : JSONConnection
    {
        NetworkStream m_AssociatedStream = null;
        public ServerConnection(NetworkStream AssociatedStream)
        {
            m_AssociatedStream = AssociatedStream;
        }
        public ClientMessage GetNextMessage()
        {
            ClientMessage ReturnValue =MBJson.JSONObject.DeserializeObject<ClientMessage>(ParseJSONObject(m_AssociatedStream));
            return (ReturnValue);
        }
        public void SendServerResponse(ServerMessage MessageToSend)
        {
            byte[] BytesToSend = GetMessageData(MBJson.JSONObject.SerializeObject(MessageToSend));
            m_AssociatedStream.Write(BytesToSend);
        }
    }
    public class RuleServer
    {
        Mutex m_InternalsMutex = new Mutex();
        int m_PortToUse = 0;
        private Dictionary<int, ActiveGameInfo> m_ActiveGames = new Dictionary<int, ActiveGameInfo>();
        
        private ServerMessage p_HandleGameMessage(GameMessage ClientMessage)
        {
            ServerMessage ReturnValue = null;
            m_InternalsMutex.WaitOne();
            if(!m_ActiveGames.ContainsKey(ClientMessage.GameIdentifier))
            {
                ReturnValue = new ErrorMessage("Invalid game ID");
                m_InternalsMutex.ReleaseMutex();
                return (ReturnValue);
            }
            ActiveGameInfo GameInfo = m_ActiveGames[ClientMessage.GameIdentifier];
            m_InternalsMutex.ReleaseMutex();
            GameInfo.HandleMessage(ClientMessage);
            return (ReturnValue);
        }

        ServerMessage p_HandleMessage(ClientMessage MessageToHandle)
        {
            ServerMessage ReturnValue = null;
            if(MessageToHandle is GameMessage)
            {
                ReturnValue = p_HandleGameMessage((GameMessage) MessageToHandle);
            }
            else
            {
                ReturnValue = new ErrorMessage("unsupported message type");
            }
            return (ReturnValue);
        }
        public void p_ConnectionHandler(object Data)
        {
            //Has the responsibility for handling initial handshakes, but otherwise just forwards the messages
            TcpClient TCPConnection = (TcpClient)Data;
            NetworkStream ClientStream = TCPConnection.GetStream();
            ServerConnection Connection = new ServerConnection(ClientStream);
            while(TCPConnection.Connected)
            {
                ClientMessage NewMessage = Connection.GetNextMessage();
                ServerMessage Response = p_HandleMessage(NewMessage);
                Connection.SendServerResponse(Response);
                //throw new Exception("First message recieved");
                //break;
            }
        }

        public RuleServer(int PortToUse)
        {
            m_PortToUse = PortToUse;
        }
        public void Run()
        {
            Int32 port = m_PortToUse;
            TcpListener Server = new TcpListener(port);
            Server.Start();
            while (true)
            {
                TcpClient Client = Server.AcceptTcpClient();
                Thread NewThread = new Thread(this.p_ConnectionHandler);
                NewThread.Start(Client);
            }
        }
    }
}