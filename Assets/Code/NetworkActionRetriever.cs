using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class NetworkActionRetriever : ActionRetriever
{
    // Start is called before the first frame update

    private Semaphore m_ConnectionSemaphore;
    private Queue<RuleManager.Action> m_RecievedActions = new Queue<RuleManager.Action>();
    private RuleServer.ClientConnection m_Connection;
    private Thread m_EventThread;

    int m_GameIdentifier = 0;
    bool m_ShouldStop = false;
    int m_OpponentPlayerIndex = 0;
    
    public NetworkActionRetriever(RuleServer.ClientConnection Connection,int GameIdentifier,int OpponentIndex)
    {
        m_GameIdentifier = GameIdentifier;
        m_OpponentPlayerIndex = OpponentIndex;
        m_Connection = Connection;
        m_EventThread = new Thread(p_RecieveEventThread);
        m_EventThread.Start();
        Thread SendThread = new Thread(p_SendEventThread);
        SendThread.Start();
    }

    ~NetworkActionRetriever()
    {
        m_ShouldStop = true;
    }

    void p_RecieveEventThread()
    {
        while(!m_ShouldStop)
        {
            RuleServer.OpponentActionPoll Message = new RuleServer.OpponentActionPoll();
            Message.GameIdentifier = m_GameIdentifier;
            Message.OpponentIndex = m_OpponentPlayerIndex;
            RuleServer.ServerMessage Response = null;
            lock(m_Connection)
            {
                Response = m_Connection.SendMessage(Message);
            }
            if(Response is RuleServer.RequestStatusResponse)
            {
                //yikes
                //print("Yikes");
            }
            else if(Response is RuleServer.OpponentAction)
            {
                RuleServer.OpponentAction ActionResponse = (RuleServer.OpponentAction)Response;
                lock(m_RecievedActions)
                {
                    foreach (RuleManager.Action OpponentAction in ActionResponse.OpponentActions)
                    {
                        m_RecievedActions.Enqueue(OpponentAction);
                    }
                }
            }
            Thread.Sleep(200);
        }
    }
    void p_SendEventThread()
    {
        while(!m_ShouldStop)
        {
            m_SemaphoreToUse.WaitOne();
            lock(m_ActionsToSend)
            {
                if(m_ActionsToSend.Count > 0)
                {
                    lock(m_Connection)
                    {
                        RuleServer.GameAction ActionToSend = m_ActionsToSend.Pop();
                        RuleServer.ServerMessage Response = m_Connection.SendMessage(ActionToSend);
                        if(!(Response is RuleServer.RequestStatusResponse))
                        {
                            //Yikes
                            throw new System.Exception("Invalid server response type: " + Response == null ? "response was null" : Response.GetType().Name);
                        }
                        RuleServer.RequestStatusResponse StatusResponse = (RuleServer.RequestStatusResponse)Response;
                        if(StatusResponse.ErrorString != "Ok")
                        {
                            m_ShouldStop = true;
                            throw new System.Exception("Error sending action: "+StatusResponse.ErrorString);
                        }
                    }
                }
            }
        }
    }
    public int getAvailableActions()
    {
        lock(m_RecievedActions)
        {
           return (m_RecievedActions.Count);
        }
    }
    public RuleManager.Action PopAction()
    {
        lock(m_RecievedActions)
        {
            RuleManager.Action ReturnValue = m_RecievedActions.Dequeue();
            return (ReturnValue);
        }
    }
    
    Semaphore m_SemaphoreToUse = new Semaphore(0,100);
    Stack<RuleServer.GameAction> m_ActionsToSend = new Stack<RuleServer.GameAction>();

    public void SendAction(RuleManager.Action ActionToSend)
    {
        RuleServer.GameAction Message = new RuleServer.GameAction();
        Message.ActionToExecute = ActionToSend;
        Message.GameIdentifier = m_GameIdentifier;

        lock(m_ActionsToSend)
        {
            m_ActionsToSend.Push(Message);
            m_SemaphoreToUse.Release();
        }
    }
}
