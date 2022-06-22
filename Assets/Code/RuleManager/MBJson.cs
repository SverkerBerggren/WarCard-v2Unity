using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace MBJson
{
    public enum JSONType
    {
        Integer,
        Float,
        Array,
        Aggregate,
        String,
        Boolean,
        Null
    }

    public class JSONObject
    {
        JSONType m_Type = JSONType.Null;
        object m_InternalData = null;

        public JSONType GetJSONType()
        {
            return (m_Type);
        }
        public JSONObject()
        {

        }
        public JSONObject(string StringData)
        {
            m_Type = JSONType.String;
            m_InternalData = StringData;
        }
        public JSONObject(int IntegerData)
        {
            m_Type = JSONType.Integer;
            m_InternalData = IntegerData;
        }
        public JSONObject(List<JSONObject> Contents)
        {
            m_Type = JSONType.Array;
            m_InternalData = Contents;
        }
        public JSONObject(Dictionary<string,JSONObject> Contents)
        {
            m_Type = JSONType.Aggregate;
            m_InternalData = Contents;
        }
        public JSONObject(bool BoolData)
        {
            m_Type = JSONType.Boolean;
            m_InternalData = BoolData;
        }
        

        public string GetStringData()
        {
            if(m_Type != JSONType.String)
            {
                throw new System.Exception("Object not of string type");
            }
            return ((string)m_InternalData);
        }
        public int GetIntegerData()
        {
            if (m_Type != JSONType.Integer)
            {
                throw new System.Exception("Object not of string type");
            }
            return ((int)m_InternalData);
        }
        public bool GetBooleanData()
        {
            if(m_Type != JSONType.Boolean)
            {
                throw new System.Exception("Object not of boolean type");
            }
            return ((bool)m_InternalData);
        }
        public List<JSONObject> GetArrayData()
        {
            if (m_Type != JSONType.Array)
            {
                throw new System.Exception("Object not of string type");
            }
            return ((List<JSONObject>)m_InternalData);
        }
        public Dictionary<string,JSONObject> GetAggregateData()
        {
            if (m_Type != JSONType.Array)
            {
                throw new System.Exception("Object not of string type");
            }
            return ((Dictionary<string,JSONObject>)m_InternalData);
        }
        public JSONObject this[string MemberName]
        {
            get
            {
                if(m_Type != JSONType.Array)
                {
                    throw new System.Exception("Object not of aggregate type");
                }
                Dictionary<string, JSONObject> AggregateData = (Dictionary<string, JSONObject>)m_InternalData;
                return (AggregateData[MemberName]);
            }
            set
            {
                if (m_Type != JSONType.Array)
                {
                    throw new System.Exception("Object not of aggregate type");
                }
                Dictionary<string, JSONObject> AggregateData = (Dictionary<string, JSONObject>)m_InternalData;
                AggregateData[MemberName] = value;
            }
        }

        public JSONObject Copy()
        {
            JSONObject ReturnValue = null;
            return (ReturnValue);            
        }
        static string ParseQuotedString(byte[] Buffer,int Offset,out int OutOffset)
        {
            string ReturnValue = "";
            int ParseOffset = Offset;
            //skipping "
            ParseOffset += 1;
            int NextQuote = FindCharacter(Buffer, ParseOffset, '\"');
            ReturnValue = System.Text.Encoding.UTF8.GetString(Buffer, ParseOffset, NextQuote - ParseOffset);
            ParseOffset = NextQuote + 1;
            OutOffset = ParseOffset;
            return (ReturnValue);
        }


        static void SkipWhiteSpace(byte[] Buffer,int Offset,out int OutOffset)
        {
            int ParseOffset = Offset;
            while(ParseOffset < Buffer.Length)
            {
                if(Buffer[ParseOffset] == ' ' || Buffer[ParseOffset] == '\t' || Buffer[ParseOffset] == '\n')
                {
                    ParseOffset += 1;
                }
                else
                {
                    break;
                }
            }
            OutOffset = ParseOffset;
        }
        static int FindCharacter(byte[] Buffer,int Offset,char CharacterToFind)
        {
            int ReturnValue = Offset;
            while(ReturnValue < Buffer.Length)
            {
                if(Buffer[Offset] == CharacterToFind)
                {
                    break;
                }
                ReturnValue++;
            }

            return (ReturnValue);
        }
        static JSONObject Parse_Boolean(byte[] Buffer, int Offset, out int OutOffset)
        {
            bool ReturnValue = false;
            int ParseOffset = Offset;
            if(Buffer[ParseOffset] == 't')
            {
                if(System.Text.Encoding.UTF8.GetString(Buffer,Offset,4) != "true")
                {
                    throw new System.Exception("Invalid true data");
                }
                ReturnValue = true;
                ParseOffset += 4;
            }
            else if(Buffer[ParseOffset] == 'f')
            {
                if (System.Text.Encoding.UTF8.GetString(Buffer, Offset, 5) != "false")
                {
                    throw new System.Exception("Invalid true data");
                }
                ParseOffset += 5;
            }
            else
            {
                throw new System.Exception("Invalid begin of boolean type");
            }
            OutOffset = ParseOffset;
            return (new JSONObject(ReturnValue));
        }
        static JSONObject Parse_Integer(byte[] Buffer, int Offset, out int OutOffset)
        {
            int ReturnValue = 0;
            int ParseOffset = Offset;
            int IntegerBegin = ParseOffset;
            int IntEnd = IntegerBegin;
            while(IntEnd < Buffer.Length)
            {
                if(!(Buffer[IntEnd] >= '0' && Buffer[IntEnd] <= '9'))
                {
                    break;
                }
                IntEnd += 1;
            }
            ReturnValue = int.Parse(System.Text.Encoding.UTF8.GetString(Buffer, IntegerBegin, IntEnd - IntegerBegin));

            OutOffset = IntEnd;
            return (new JSONObject(ReturnValue));
        }
        static JSONObject Parse_Null(byte[] Buffer, int Offset, out int OutOffset)
        {
            JSONObject ReturnValue = new JSONObject();
            int ParseOffset = Offset;
            if (System.Text.Encoding.UTF8.GetString(Buffer, Offset, 4) != "null")
            {
                throw new System.Exception("Invalid null data");
            }
            ParseOffset += 4;
            OutOffset = ParseOffset;
            return ReturnValue;
        }
        static JSONObject Parse_String(byte[] Buffer,int Offset,out int OutOffset)
        {
            string ReturnValue = null;
            int ParseOffset = Offset;
            ParseQuotedString(Buffer, ParseOffset, out ParseOffset);
            OutOffset = ParseOffset;
            return (new JSONObject(ReturnValue));
        }
        static JSONObject Parse_Aggregate(byte[] Buffer,int Offset, out int OutOffset)
        {
            Dictionary<string,JSONObject> Contents = new Dictionary<string,JSONObject>();
            int ParseOffset = Offset;
            //skipping {
            ParseOffset += 1;
            bool EndReached = false;
            while (ParseOffset < Buffer.Length)
            {
                SkipWhiteSpace(Buffer, ParseOffset, out ParseOffset);
                if (ParseOffset >= Buffer.Length)
                {
                    throw new System.Exception("Early end of file reached when parsing json object");
                }
                if (Buffer[ParseOffset] == '}')
                {
                    EndReached = true;
                    ParseOffset += 1;
                    break;
                }
                if(Buffer[ParseOffset] != '\"')
                {
                    throw new System.Exception("invalid begin of object member name");
                }
                string MemberName = ParseQuotedString(Buffer, ParseOffset, out ParseOffset);
                SkipWhiteSpace(Buffer, ParseOffset, out ParseOffset);
                if (ParseOffset >= Buffer.Length)
                {
                    throw new System.Exception("Early end of file reached when parsing json object");
                }
                if(Buffer[ParseOffset] != ':')
                {
                    throw new System.Exception("Invalid value delimiter in json object");
                }
                JSONObject Value = ParseJSONObject(Buffer, ParseOffset, out ParseOffset);
                Contents.Add(MemberName, Value);
                SkipWhiteSpace(Buffer, ParseOffset, out ParseOffset);
                if (ParseOffset >= Buffer.Length)
                {
                    throw new System.Exception("Early end of file reached when parsing json object");
                }
                if (Buffer[ParseOffset] == '}')
                {
                    EndReached = true;
                    ParseOffset += 1;
                    break;
                }
                if (Buffer[ParseOffset] != ',')
                {
                    throw new System.Exception("Invalid array delimiter");
                }
                ParseOffset += 1;
            }
            if (!EndReached)
            {
                throw new System.Exception("Early end of file reached when parsing json object");
            }
            OutOffset = ParseOffset;
            return (new JSONObject(Contents));
        }
        static JSONObject Parse_Array(byte[] ByteBuffer,int Offset,out int OutOffset)
        {
            List<JSONObject> Contents = new List<JSONObject>();
            int ParseOffset = Offset;
            //skipping [
            ParseOffset += 1;
            bool EndReached = false;
            SkipWhiteSpace(ByteBuffer, ParseOffset, out ParseOffset);
            while (ParseOffset < ByteBuffer.Length)
            {
                if(ParseOffset >= ByteBuffer.Length)
                {
                    throw new System.Exception("Early end of file reached when parsing json object");
                }
                if(ByteBuffer[ParseOffset] == ']')
                {
                    EndReached = true;
                    ParseOffset += 1;
                    break;
                }
                Contents.Add(ParseJSONObject(ByteBuffer, ParseOffset, out ParseOffset));
                SkipWhiteSpace(ByteBuffer, ParseOffset, out ParseOffset);
                if(ParseOffset >= ByteBuffer.Length)
                {
                    throw new System.Exception("Early end of file reached when parsing json object");
                }
                if (ByteBuffer[ParseOffset] == ']')
                {
                    EndReached = true;
                    ParseOffset += 1;
                    break;
                }
                if(ByteBuffer[ParseOffset]  != ',')
                {
                    throw new System.Exception("Invalid array delimiter");
                }
                ParseOffset += 1;
                SkipWhiteSpace(ByteBuffer, ParseOffset, out ParseOffset);
            }
            if (!EndReached)
            {
                throw new System.Exception("Early end of file reached when parsing json object");
            }
            OutOffset = ParseOffset;
            return (new JSONObject(Contents));
        }

        public static JSONObject ParseJSONObject(byte[] ByteBuffer,int Offset,out int OutOffset)
        {
            JSONObject ReturnValue = null;
            int ParseOffset = Offset;
            SkipWhiteSpace(ByteBuffer, ParseOffset, out ParseOffset);
            if(ParseOffset >= ByteBuffer.Length)
            {
                throw new System.Exception("Unexpected end of file reached when parsing JSON");
            }
            if(ByteBuffer[ParseOffset] == '{')
            {
                ReturnValue = Parse_Aggregate(ByteBuffer, ParseOffset, out ParseOffset);
            }
            else if(ByteBuffer[ParseOffset] == '[')
            {
                ReturnValue =  Parse_Array(ByteBuffer, ParseOffset, out ParseOffset);
            }
            else if(ByteBuffer[ParseOffset] == '\"')
            {
                ReturnValue = Parse_String(ByteBuffer, ParseOffset,out OutOffset);
            }
            else if(ByteBuffer[ParseOffset] == 't' || ByteBuffer[ParseOffset] == 'f')
            {
                ReturnValue = Parse_Boolean(ByteBuffer, ParseOffset, out ParseOffset);
            }
            else if(ByteBuffer[ParseOffset] == 'n')
            {
                ReturnValue = Parse_Null(ByteBuffer, ParseOffset, out ParseOffset);
            }
            else
            {
                ReturnValue = Parse_Integer(ByteBuffer, ParseOffset, out ParseOffset);
                //floats are not supported
            }
            OutOffset = ParseOffset;
            return (ReturnValue);
        }
    
        public static JSONObject SerializeObject<T,U>(T ObjectToSerialize)
        {
            JSONObject ReturnValue = null;
            Type ObjectType = ObjectToSerialize.GetType();
            if(ObjectToSerialize is string)
            {
                ReturnValue = new JSONObject((string)(object)ObjectToSerialize);
            }
            else if(ObjectToSerialize is int)
            {
                ReturnValue = new JSONObject((int)(object)ObjectToSerialize);
            }
            else if(ObjectToSerialize is List<U>)
            {

            }
            //else if(ObjectToSerialize is Dictionary<string,)
            return (ReturnValue);
        }
        public static T DeserializeObject<T>(JSONObject ObjectToParse) where T : new()
        {
            T ReturnValue = new T();

            return (ReturnValue);
        }
    }

    

}
