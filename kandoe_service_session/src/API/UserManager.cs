using System;
using Microsoft.AspNetCore.SignalR.Hubs;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace API
{
    // Extension methods die Context in SignalR hub uitbreiden
    public static class UserManager
    {

       // SignalR wijst een Connection ID toe aan elke verbonden gebruiker
       // Hier houden we een mapping bij van de SignalR connection ID naar de User ID
       private static ConcurrentDictionary<string, string> ConnectedUsers = new ConcurrentDictionary<string, string>();

       public static string UserId(this HubCallerContext ctx) 
       {
            try 
            {
                var userId = ConnectedUsers[ctx.ConnectionId];
                return userId;
            }
            catch (KeyNotFoundException ex) 
            {
                var e = ex;
                return null;
            }
       }

       public static bool IsInSession(this HubCallerContext ctx, Guid sessionId, bool manager = false)
       {
            try 
            {
                var userId = ConnectedUsers[ctx.ConnectionId];
                return true;
            }
            catch (KeyNotFoundException ex) 
            {
                var e = ex;
                return false;
            }
       }

       public static void SetUserId(this HubCallerContext ctx, string userId)
       {
           ConnectedUsers[ctx.ConnectionId] = userId;
       }
    }
}