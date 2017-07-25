using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace zpgServer
{
    public class PlayerResponsePair
    {
        public Player player;
        public HttpListenerResponse response;
        public Timer timeout;

        public PlayerResponsePair(Player player, HttpListenerResponse response)
        {
            this.player = player;
            this.response = response;
        }
    }
}
