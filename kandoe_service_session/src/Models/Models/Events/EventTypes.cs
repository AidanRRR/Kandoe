using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.Models.Events
{
    public static class EventTypes
    {
        public static string GetChatEventType()
        {
            return "CHAT_MESSAGE";
        }
        public static string GetConnectEventType()
        {
            return "CONNECT";
        }
        public static string GetMoveEventType()
        {
            return "MOVE";
        }
        public static string GetTurnStartEventType()
        {
            return "TURN_START";
        }
    }
}
