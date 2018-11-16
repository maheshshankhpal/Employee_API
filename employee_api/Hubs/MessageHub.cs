using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace DynamicReportAPI.Hubs
{
    public class joinUserDetails
    {
        public string gropname {get;set;}
        public string userid {get;set;}
        public string connectionId {get;set;}        
    }
    public class MessageHub : Hub
    {
        public Task Send(string message)
        {
            return Clients.All.SendAsync("Send", "Omi");
        }
        public void joinToGroup(string userid, List<GroupsList> groplist)
        {
            if (groplist.Count > 0)
            {
               var connectionuser = Context.ConnectionId;
                foreach (var grp in groplist)
                {
                    Groups.AddToGroupAsync(connectionuser, grp.GroupName);
                    joinUserDetails userJoined = new joinUserDetails();
                    userJoined.connectionId =connectionuser;
                    userJoined.gropname = grp.GroupName;
                    userJoined.userid = userid;
                    Clients.Group(grp.GroupName).SendAsync("onJoinGroup",userJoined);
                }
            }
        }

        public override Task OnConnectedAsync()
        {
           
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {

            return base.OnDisconnectedAsync(exception);

        }



    }
}