using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Threading;
public class ClientConnectionHandler : MonoBehaviour
{
    // Start is called before the first frame update

    RuleServer.ClientConnection m_Connection = null;

    Queue<Tuple<RuleServer.ClientMessage,Action<RuleServer.ServerMessage>>> m_MessagesToSend = new Queue<Tuple<RuleServer.ClientMessage, Action<RuleServer.ServerMessage>>>();

    bool m_IsConnected = true;
    bool m_Stopping = false;

    Semaphore m_MessageAddedSemaphore = new Semaphore(0,1000000);

    void p_CommunicationThread()
    {
        print("Communicating");
        try
        {
            m_Connection = new RuleServer.ClientConnection("mrboboget.se", 80);
            print("Connection established");
            while(!m_Stopping)
            {
                m_MessageAddedSemaphore.WaitOne();
                Tuple<RuleServer.ClientMessage, Action<RuleServer.ServerMessage>> CurrentRequest = m_MessagesToSend.Dequeue();
                RuleServer.ServerMessage Response = m_Connection.SendMessage(CurrentRequest.Item1);
                print("Executing callback");
                CurrentRequest.Item2(Response);
            }
        }
        catch(Exception e)
        {
            m_IsConnected = false;
            print("Error connecting to server: " + e.Message);
        }
        lock(m_MessagesToSend)
        {
            foreach(Tuple<RuleServer.ClientMessage, Action<RuleServer.ServerMessage>> Request in m_MessagesToSend)
            {
                Request.Item2(null);
            }
            m_MessagesToSend.Clear();
        }
    }

    public bool IsConnected()
    {
        return (m_IsConnected);
    }
    public RuleServer.ClientConnection GetUnderlyingConnection()
    {
        lock(m_MessagesToSend)
        {
            m_MessagesToSend.Clear();
            m_IsConnected = false;
            m_Stopping = true;
        }
        return (m_Connection);
    }
    public void SendMessage(RuleServer.ClientMessage MessageToSend,Action<RuleServer.ServerMessage> Callback)
    {
        if(!m_IsConnected)
        {
            Callback(null);
            return;
        }
        lock (m_MessagesToSend)
        {
            m_MessagesToSend.Enqueue(new Tuple<RuleServer.ClientMessage, Action<RuleServer.ServerMessage>>(MessageToSend, Callback));
        }
        m_MessageAddedSemaphore.Release();
    }

    void Start()
    {
        Thread HandleMessageThread = new Thread(p_CommunicationThread);
        HandleMessageThread.Start();
    }
    private void OnDestroy()
    {
        m_Stopping = true;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
