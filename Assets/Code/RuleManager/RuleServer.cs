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
        RegisterLobby,


        JoinLobby,
        LobbyEventPoll,
        UpdateLobbyStatus,
        SendLobbyEvent,
        ClientHandshake
    }
    [Serializable]
    public class ClientMessage : MBJson.JSONDeserializeable, MBJson.JSONTypeConverter
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
            if (SerializedType == ClientMessageType.GameAction)
            {
                ReturnValue = typeof(GameMessage);
            }
            else if (SerializedType == ClientMessageType.OpponentActionPoll)
            {
                ReturnValue = typeof(OpponentActionPoll);
            }
            else if(SerializedType == ClientMessageType.RegisterLobby)
            {
                ReturnValue = typeof(RegisterLobby);
            }
            else if(SerializedType == ClientMessageType.JoinLobby)
            {
                ReturnValue = typeof(JoinLobby);
            }
            else if(SerializedType == ClientMessageType.LobbyEventPoll)
            {
                ReturnValue = typeof(LobbyEventPoll);
            }
            else if (SerializedType == ClientMessageType.UpdateLobbyStatus)
            {
                ReturnValue = typeof(UpdateLobbyStatus);
            }
            else if (SerializedType == ClientMessageType.SendLobbyEvent)
            {
                ReturnValue = typeof(SendLobbyEvent);
            }
            else if(SerializedType == ClientMessageType.ClientHandshake)
            {
                ReturnValue = typeof(ClientHandshake);
            }
            else if(SerializedType == ClientMessageType.Null)
            {
                throw new Exception("Cant serialize null message");
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

    public class ClientHandshake : ClientMessage
    {
        public ClientHandshake() : base(ClientMessageType.ClientHandshake)
        {

        }
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

    public class LobbyMessage : ClientMessage
    {
        public LobbyMessage() { }
        public LobbyMessage(ClientMessageType NewType) { Type = NewType; }
        public string LobbyID = "";
    }
    public class RegisterLobby : LobbyMessage
    {
        public RegisterLobby() : base(ClientMessageType.RegisterLobby) { }
    }
    public class JoinLobby : LobbyMessage
    {
        public JoinLobby() : base(ClientMessageType.JoinLobby) { }
    }
    public class LobbyEventPoll : LobbyMessage
    {
        public LobbyEventPoll() : base(ClientMessageType.LobbyEventPoll) { }
    }
    public class SendLobbyEvent : LobbyMessage
    {
        public SendLobbyEvent() : base(ClientMessageType.SendLobbyEvent)
        {

        }
        public LobbyEvent EventToSend = new LobbyEvent();
    }
    public class UpdateLobbyStatus : LobbyMessage
    {
        public UpdateLobbyStatus() : base(ClientMessageType.UpdateLobbyStatus) { }
        public ConnectionLobbyStatus NewStatus = new ConnectionLobbyStatus();
    }
    public enum ServerMessageType
    {
        Null,
        OpponentAction,
        RequestStatusResponse,
        RegisterLobbyResponse,
        LobbyEventResponse,
        ServerHandshake,
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
            else if (SerializedType == ServerMessageType.RequestStatusResponse)
            {
                ReturnValue = typeof(RequestStatusResponse);
            }
            else if (SerializedType == ServerMessageType.RegisterLobbyResponse)
            {
                ReturnValue = typeof(RegisterLobbyResponse);
            }
            else if (SerializedType == ServerMessageType.LobbyEventResponse)
            {
                ReturnValue = typeof(LobbyEventResponse);
            }
            else if(SerializedType == ServerMessageType.ServerHandshake)
            {
                ReturnValue = typeof(ServerHandshake);
            }
            else if(SerializedType == ServerMessageType.Null)
            {
                throw new Exception("Cant deserizalize null message");
            }
            else
            {
                throw new Exception("Invalid Server type when deserializing client message");
            }
            return (ReturnValue);
        }
        public object Deserialize(MBJson.JSONObject ObjectToParse)
        {
            return (new MBJson.DynamicJSONDeserializer(this).Deserialize(ObjectToParse));
        }
    }
    public class ServerHandshake : ServerMessage
    {
        public ServerHandshake() : base(ServerMessageType.ServerHandshake)
        {

        }
        public int NewConnectionIdentifier = 0;
    }
    [Serializable]
    public class OpponentAction : ServerMessage
    {
        public OpponentAction() : base(ServerMessageType.OpponentAction) { }
        public List<RuleManager.Action> OpponentActions;
    }
    public class RequestStatusResponse : ServerMessage
    {
        public RequestStatusResponse(): base(ServerMessageType.RequestStatusResponse)
        {

        }
        public RequestStatusResponse(string Error) : base(ServerMessageType.RequestStatusResponse)
        {
            ErrorString = Error;
        }
        public string ErrorString = "";
    }
    public class RegisterLobbyResponse : ServerMessage
    {
        public RegisterLobbyResponse() : base(ServerMessageType.RegisterLobbyResponse) { }
        public string LobbyID = "";
    }
    //OK means that you sucesfully joined a lobby
    public enum LobbyEventType
    {
        Null,
        PlayerJoined,
        GameStart,
        PlayerStatusUpdated
    }

    public class LobbyEvent : MBJson.JSONDeserializeable, MBJson.JSONTypeConverter
    {
        public LobbyEventType Type = LobbyEventType.Null;
        public LobbyEvent()
        {

        }
        public LobbyEvent(LobbyEventType TypeToUse)
        {
            Type = TypeToUse;
        }
        public Type GetType(int IntegerToConvert)
        {
            Type ReturnValue = null;
            LobbyEventType SerializedType = (LobbyEventType)IntegerToConvert;
            if (SerializedType == LobbyEventType.GameStart)
            {
                ReturnValue = typeof(LobbyEvent_GameStart);
            }
            else if (SerializedType == LobbyEventType.PlayerJoined)
            {
                ReturnValue = typeof(LobbyEvent_PlayerJoined);
            }
            else if (SerializedType == LobbyEventType.PlayerStatusUpdated)
            {
                ReturnValue = typeof(LobbyEvent_StatusUpdated);
            }
            else
            {
                throw new Exception("Invalid LobbyEventType");
            }
            return (ReturnValue);
        }
        public object Deserialize(MBJson.JSONObject ObjectToParse)
        {
            return (new MBJson.DynamicJSONDeserializer(this).Deserialize(ObjectToParse));
        }
    }
    public class LobbyEvent_PlayerJoined : LobbyEvent
    {
        public LobbyEvent_PlayerJoined() : base(LobbyEventType.PlayerJoined) { }
    }

    public class LobbyEventResponse : ServerMessage
    {
        public LobbyEventResponse() : base(ServerMessageType.LobbyEventResponse) { }
        public List<LobbyEvent> Events = new List<LobbyEvent>();
    }
    public class LobbyEvent_GameStart : LobbyEvent
    {
        public LobbyEvent_GameStart() : base(LobbyEventType.GameStart)
        {

        }
        public int GameID = 0;
        public int PlayerIndex = 0;
    }
    public class LobbyEvent_StatusUpdated : LobbyEvent
    {
        public int ConnectionID = 0;
        public ConnectionLobbyStatus NewStatus = new ConnectionLobbyStatus();
        public LobbyEvent_StatusUpdated() : base(LobbyEventType.PlayerStatusUpdated)
        {

        }
    }
    public class ActiveGameInfo
    {
        Mutex m_InternalsMutex = new Mutex();
        private RuleManager.RuleManager m_GameRuleManager = new RuleManager.RuleManager(15,15);
        public List<List<RuleManager.Action>> m_PlayerActions = new List<List<RuleManager.Action>>();
        //Connection -> player index
        public Dictionary<int, int> ParticipatingPlayers = new Dictionary<int, int>();

        public ActiveGameInfo()
        {

        }
        private ServerMessage p_Handle_Action(GameAction ActionMessage)
        {
            ServerMessage ReturnValue = null;
            lock(m_InternalsMutex)
            {
                if (!ParticipatingPlayers.ContainsKey(ActionMessage.ConnectionIdentifier))
                {
                    ReturnValue = new RequestStatusResponse("Connection not present in game");
                    return (ReturnValue);
                }
                int PlayerIndex = ParticipatingPlayers[ActionMessage.ConnectionIdentifier];
                if (PlayerIndex != ActionMessage.ActionToExecute.PlayerIndex)
                {
                    ReturnValue = new RequestStatusResponse("Can't execute actions for your opponent");
                    return (ReturnValue);
                }
                string ErrorString;
                bool ActionIsValid = m_GameRuleManager.ActionIsValid(ActionMessage.ActionToExecute, out ErrorString);
                if (!ActionIsValid)
                {
                    ReturnValue = new RequestStatusResponse(ErrorString);
                    return (ReturnValue);
                }
                m_GameRuleManager.ExecuteAction(ActionMessage.ActionToExecute);
                ReturnValue = new RequestStatusResponse("Ok");
            }
            return (ReturnValue);
        }
        ServerMessage p_Handle_ActionPoll(OpponentActionPoll PollMessage)
        {
            ServerMessage ReturnValue = null;
            lock(m_InternalsMutex)
            {
                if (!ParticipatingPlayers.ContainsKey(PollMessage.ConnectionIdentifier))
                {
                    ReturnValue = new RequestStatusResponse("Connection not present in game");
                    return (ReturnValue);
                }
                int PlayerIndex = ParticipatingPlayers[PollMessage.ConnectionIdentifier];
                if (PlayerIndex == PollMessage.OpponentIndex)
                {
                    ReturnValue = new RequestStatusResponse("Can't pop your own actions");
                    return (ReturnValue);
                }
                if (PollMessage.OpponentIndex > m_PlayerActions.Count || PollMessage.OpponentIndex < 0)
                {
                    ReturnValue = new RequestStatusResponse("Invalid opponent index");
                    return (ReturnValue);
                }
                OpponentAction Response = new OpponentAction();
                Response.OpponentActions = new List<RuleManager.Action>(m_PlayerActions[PollMessage.OpponentIndex]);
                m_PlayerActions[PollMessage.OpponentIndex].Clear();
                ReturnValue = Response;
            }
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
            TotalDataStream.Write(TotalObjectData,0,TotalObjectData.Length);
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
        int m_ConnectionID = 0;

        public ClientConnection(string Adress,int Port)
        {
            TcpClient NewClient = new TcpClient(Adress, Port);
            m_AssociatedStream = NewClient.GetStream();

            //Handshake
            ClientHandshake HandshakeMessage = new ClientHandshake();
            ServerHandshake Response =(ServerHandshake) SendMessage(HandshakeMessage);
            m_ConnectionID = Response.NewConnectionIdentifier;
        }
        public ServerMessage SendMessage(ClientMessage MessageToSend)
        {
            ServerMessage ReturnValue = null;
            //throw new Exception(MessageToSend.ToString());
            MessageToSend.ConnectionIdentifier = m_ConnectionID;
            //throw new Exception(MBJson.JSONObject.SerializeObject(MessageToSend).ToString());
            byte[] BytesToSend = GetMessageData(MBJson.JSONObject.SerializeObject(MessageToSend));
            m_AssociatedStream.Write(BytesToSend,0,BytesToSend.Length);
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
            m_AssociatedStream.Write(BytesToSend,0,BytesToSend.Length);
        }
    }
    public class ConnectionLobbyStatus
    {
        public bool Ready = false;
    }
    public class ConnectionLobbyInfo
    {
        public List<LobbyEvent> StoredEvents = new List<LobbyEvent>();
        public ConnectionLobbyStatus Status = new ConnectionLobbyStatus();
    }
    class ActiveLobbyInfo
    {
        public Dictionary<int, ConnectionLobbyInfo> ConnectedUsers = new Dictionary<int, ConnectionLobbyInfo>();
    }
    public class RuleServer
    {
        Mutex m_InternalsMutex = new Mutex();
        int m_PortToUse = 0;
        private Dictionary<int, ActiveGameInfo> m_ActiveGames = new Dictionary<int, ActiveGameInfo>();
        private Dictionary<string, ActiveLobbyInfo> m_ActiveLobbys = new Dictionary<string, ActiveLobbyInfo>();

        int CurrentGameID = 1;
        int m_CurrentConnectionID = 0;
        int p_CreateNewGame(ActiveLobbyInfo AssociatedLobby)
        {
            CurrentGameID += 1;
            int NewGameID = CurrentGameID;
            ActiveGameInfo NewGame = new ActiveGameInfo();
            int CurrentPlayerIndex = 0;
            foreach(KeyValuePair<int, ConnectionLobbyInfo> Participant in  AssociatedLobby.ConnectedUsers)
            {
                NewGame.ParticipatingPlayers.Add(Participant.Key, CurrentPlayerIndex);
                CurrentPlayerIndex += 1;
                NewGame.m_PlayerActions.Add(new List<RuleManager.Action>());
            }
            m_ActiveGames.Add(NewGameID, NewGame);
            return (NewGameID);
        }
        private ServerMessage p_HandleLobbyMessage(LobbyMessage MessageToHandle)
        {
            ServerMessage ReturnValue = null;
            if(MessageToHandle is JoinLobby)
            {
                JoinLobby JoinMessageToHandle = (JoinLobby)MessageToHandle;
                m_InternalsMutex.WaitOne();
                if(!m_ActiveLobbys.ContainsKey(JoinMessageToHandle.LobbyID))
                {
                    ReturnValue = new RequestStatusResponse("No lobby exists with given LobbyID");
                    m_InternalsMutex.ReleaseMutex();
                    return (ReturnValue);
                }
                ActiveLobbyInfo Lobby = m_ActiveLobbys[JoinMessageToHandle.LobbyID];
                foreach(KeyValuePair<int, ConnectionLobbyInfo> Connection in Lobby.ConnectedUsers)
                {
                    Connection.Value.StoredEvents.Add(new LobbyEvent_PlayerJoined());
                }
                Lobby.ConnectedUsers.Add(JoinMessageToHandle.ConnectionIdentifier, new ConnectionLobbyInfo());
                ReturnValue = new RequestStatusResponse("Ok");
            }
            else if(MessageToHandle is RegisterLobby)
            {
                byte[] RandomBytes = new byte[4];
                new System.Random().NextBytes(RandomBytes);
                string LobbyID = System.Convert.ToBase64String(RandomBytes);
                RegisterLobbyResponse Response = new RegisterLobbyResponse();
                Response.LobbyID = LobbyID;
                ReturnValue = Response;
                m_InternalsMutex.WaitOne();
                ActiveLobbyInfo NewLobby = new ActiveLobbyInfo();
                NewLobby.ConnectedUsers.Add(MessageToHandle.ConnectionIdentifier, new ConnectionLobbyInfo());
                m_ActiveLobbys.Add(LobbyID, NewLobby);
            }
            else if(MessageToHandle is SendLobbyEvent)
            {
                SendLobbyEvent ClientLobbyEvent = (SendLobbyEvent)MessageToHandle;
                m_InternalsMutex.WaitOne();
                if (!m_ActiveLobbys.ContainsKey(ClientLobbyEvent.LobbyID))
                {
                    ReturnValue = new RequestStatusResponse("No lobby exists with given LobbyID");
                    m_InternalsMutex.ReleaseMutex();
                    return (ReturnValue);
                }
                if(ClientLobbyEvent.EventToSend is LobbyEvent_GameStart)
                {
                    ActiveLobbyInfo Lobby = m_ActiveLobbys[ClientLobbyEvent.LobbyID];
                    ReturnValue = new RequestStatusResponse("Ok");
                    foreach (KeyValuePair<int, ConnectionLobbyInfo> Statuses in Lobby.ConnectedUsers)
                    {
                        if(Statuses.Value.Status.Ready == false)
                        {
                            ReturnValue = new RequestStatusResponse("Can only start if all players are ready");
                            m_InternalsMutex.ReleaseMutex();
                            return (ReturnValue);
                        }
                    }
                    int NewGameID = p_CreateNewGame(Lobby);

                    //arbitrary algorithm to determine who starts
                    int CurrentPlayerIndex = 0;
                    foreach (KeyValuePair<int, ConnectionLobbyInfo> Statuses in Lobby.ConnectedUsers)
                    {
                        LobbyEvent_GameStart GameStartEvent = new LobbyEvent_GameStart();
                        GameStartEvent.GameID = NewGameID;
                        GameStartEvent.PlayerIndex = CurrentPlayerIndex;
                        Statuses.Value.StoredEvents.Add(GameStartEvent);
                        CurrentPlayerIndex++;
                    }
                    
                }
                else
                {
                    ReturnValue = new RequestStatusResponse("Invalid client lobby event");
                    m_InternalsMutex.ReleaseMutex();
                    return (ReturnValue);
                }
            }
            else if(MessageToHandle is UpdateLobbyStatus)
            {
                UpdateLobbyStatus UpdateStatusMessage = (UpdateLobbyStatus)MessageToHandle;
                m_InternalsMutex.WaitOne();
                if (!m_ActiveLobbys.ContainsKey(UpdateStatusMessage.LobbyID))
                {
                    ReturnValue = new RequestStatusResponse("No lobby exists with given LobbyID");
                    m_InternalsMutex.ReleaseMutex();
                    return (ReturnValue);
                }
                ActiveLobbyInfo Lobby = m_ActiveLobbys[UpdateStatusMessage.LobbyID];
                if(!Lobby.ConnectedUsers.ContainsKey(UpdateStatusMessage.ConnectionIdentifier))
                {
                    ReturnValue = new RequestStatusResponse("Connection not in lobby");
                    m_InternalsMutex.ReleaseMutex();
                    return (ReturnValue);
                }
                Lobby.ConnectedUsers[UpdateStatusMessage.ConnectionIdentifier].Status = UpdateStatusMessage.NewStatus;
                foreach(KeyValuePair<int,ConnectionLobbyInfo> Connection in Lobby.ConnectedUsers)
                {
                    if(Connection.Key != UpdateStatusMessage.ConnectionIdentifier)
                    {
                        LobbyEvent_StatusUpdated NewEvent = new LobbyEvent_StatusUpdated();
                        NewEvent.ConnectionID = UpdateStatusMessage.ConnectionIdentifier;
                        NewEvent.NewStatus = UpdateStatusMessage.NewStatus;
                        Connection.Value.StoredEvents.Add(NewEvent);
                    }
                }
                ReturnValue = new RequestStatusResponse();
            }
            else if(MessageToHandle is LobbyEventPoll)
            {
                LobbyEventPoll JoinMessageToHandle = (LobbyEventPoll)MessageToHandle;
                m_InternalsMutex.WaitOne();
                if (!m_ActiveLobbys.ContainsKey(JoinMessageToHandle.LobbyID))
                {
                    ReturnValue = new RequestStatusResponse("No lobby exists with given LobbyID");
                    m_InternalsMutex.ReleaseMutex();
                    return (ReturnValue);
                }
                ActiveLobbyInfo Lobby = m_ActiveLobbys[JoinMessageToHandle.LobbyID];
                //foreach (KeyValuePair<int, ConnectionLobbyStatus> Connection in Lobby.ConnectedUsers)
                //{
                //    Connection.Value.StoredEvents.Add(new LobbyEvent_PlayerJoined());
                //}

                if(!Lobby.ConnectedUsers.ContainsKey(JoinMessageToHandle.ConnectionIdentifier))
                {
                    ReturnValue = new RequestStatusResponse("Need to join lobby before you can poll events");
                    m_InternalsMutex.ReleaseMutex();
                    return (ReturnValue);
                }
                ConnectionLobbyInfo LobbyInfo = Lobby.ConnectedUsers[MessageToHandle.ConnectionIdentifier];
                LobbyEventResponse Response = new LobbyEventResponse();
                Response.Events = new List<LobbyEvent>(LobbyInfo.StoredEvents);
                LobbyInfo.StoredEvents.Clear();
                ReturnValue = Response;

            }

            m_InternalsMutex.ReleaseMutex();
            return (ReturnValue);
        }
        private ServerMessage p_HandleGameMessage(GameMessage ClientMessage)
        {
            ServerMessage ReturnValue = null;
            lock(m_InternalsMutex)
            {
                if (!m_ActiveGames.ContainsKey(ClientMessage.GameIdentifier))
                {
                    ReturnValue = new RequestStatusResponse("Invalid game ID");
                    return (ReturnValue);
                }
                ActiveGameInfo GameInfo = m_ActiveGames[ClientMessage.GameIdentifier];
                ReturnValue = GameInfo.HandleMessage(ClientMessage);
            }
            return (ReturnValue);
        }

        ServerMessage p_HandleMessage(ClientMessage MessageToHandle)
        {
            ServerMessage ReturnValue = null;
            if(MessageToHandle is GameMessage)
            {
                ReturnValue = p_HandleGameMessage((GameMessage) MessageToHandle);
            }
            else if(MessageToHandle is LobbyMessage)
            {
                ReturnValue = p_HandleLobbyMessage((LobbyMessage) MessageToHandle);
            }
            else if(MessageToHandle is ClientHandshake)
            {
                m_CurrentConnectionID++;
                ServerHandshake Response = new ServerHandshake();
                Response.NewConnectionIdentifier = m_CurrentConnectionID;
                ReturnValue = Response;
            }
            else
            {
                ReturnValue = new RequestStatusResponse("unsupported message type");
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