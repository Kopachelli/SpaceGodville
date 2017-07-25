using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace zpgServer
{
    public static class WebRequest
    {
        //==============================================================================================================
        // Hero page
        //==============================================================================================================
        public static void OnHero(HttpListenerRequest request, HttpListenerResponse response)
        {
            string responseString = "";
            try
            {
                // Parsing arguments
                Dictionary<string, string> postArguments = WebReader.GetPostArgs(request);
                string sessionKey = postArguments["sessionKey"];
                // Looking for player session
                Player player = Authorization.FindBySession(sessionKey);
                // Session found - send the hero page
                if (player != null)
                {
                    responseString = WebConstructor.GetHeroPage(player.ship);
                }
                // Session not found
                else
                {
                    responseString = WebConstructor.defaultHeader;
                    responseString += "<h1>No session found. Session key invalid or expired.</h1>";
                    responseString += WebConstructor.defaultFooter;
                }
            }
            // Something went wrong (typically, argument parsing error)
            catch (Exception ex)
            {
                responseString = WebConstructor.defaultHeader;
                responseString += "<h1>Unknown error:</h1><h2>" + ex.Message + "</h2>";
                responseString += WebConstructor.defaultFooter;
            }
            // Send the message
            WebWriter.Reply(response, responseString);
        }
        //==============================================================================================================
        // Log update
        //==============================================================================================================
        public static void OnPilotLog(HttpListenerRequest request, HttpListenerResponse response)
        {
            string responseString = "";
            try
            {
                // Parsing arguments
                Dictionary<string, string> postArguments = WebReader.GetPostArgs(request);
                string sessionKey = postArguments["sessionKey"];
                // Looking for player session
                Player player = Authorization.FindBySession(sessionKey);
                // Session found - send the log
                if (player != null)
                {
                    responseString = WebConstructor.GetShipLog(player.ship);
                }
                // No session found
                else
                {
                    responseString = "<p>Authorization error.</p>";
                }
            }
            // Something went wrong (typically, argument parsing error)
            catch (Exception ex)
            {
                responseString = WebConstructor.defaultHeader;
                responseString += "<h1>Unknown error:</h1><h2>" + ex.Message + "</h2>";
                responseString += WebConstructor.defaultFooter;
            }
            // Send the message
            WebWriter.Reply(response, responseString);
        }
        //==============================================================================================================
        // Registration
        //==============================================================================================================
        public static void OnRegister(HttpListenerRequest request, HttpListenerResponse response)
        {
            string responseString = WebConstructor.defaultHeader;

            Dictionary<string, string> postArguments = WebReader.GetPostArgs(request);
            string username, password, passwordRepeat, shipName;
            try
            {
                // Parsing arguments
                username = postArguments["username"];
                password = postArguments["password"];
                passwordRepeat = postArguments["passwordRepeat"];
                shipName = postArguments["shipName"];
                // If the passwords do match, create an account
                if (password == passwordRepeat)
                {
                    bool accountCreated = Authorization.CreateAccount(username, password);
                    // If successful, login the player and create the ship
                    if (accountCreated)
                    {
                        string sessionKey = Authorization.Login(username, password);
                        Player player = Authorization.FindBySession(sessionKey);
                        Ship ship = Universe.AddNewPlayerShip(shipName, player);
                        player.startingShipName = shipName;
                        // Redirect player to hero page
                        responseString = WebConstructor.headOpener;
                        responseString += "</head><body onload=\"sendPostRequest('hero', {sessionKey: '"
                            + sessionKey + "'});\">";
                    }
                    // Something failed within authorization module (typically username already taken)
                    else { responseString += "<h1>" + ConsoleEx.lastErrorMessage + "</h1>"; }
                }
                // Password mismatch
                else if (password != passwordRepeat)
                    responseString += "<h1>Account creation failed. Passwords do not match.</h1>";
                // Finish the message
                responseString += WebConstructor.defaultFooter;
            }
            // Something went wrong (typically, argument parsing error)
            catch (Exception ex)
            {
                responseString = WebConstructor.defaultHeader;
                responseString += "<h1>Unknown error:</h1><h2>" + ex.Message + "</h2>";
                responseString += WebConstructor.defaultFooter;
            }
            WebWriter.Reply(response, responseString);
        }
        //==============================================================================================================
        // Login
        //==============================================================================================================
        public static void OnLogin(HttpListenerRequest request, HttpListenerResponse response)
        {
            string responseString = WebConstructor.defaultHeader;

            try
            {
                // Parsing arguments
                Dictionary<string, string> postArguments = WebReader.GetPostArgs(request);
                string username = postArguments["username"];
                string password = postArguments["password"];
                // A little easter egg
                if (username == "admin" && password == "admin")
                {
                    responseString = WebConstructor.defaultHeader;
                    responseString += "<h1>This is not a router bro.</h1>";
                }
                // Actual username and password
                else if (username != null && password != null)
                {
                    // Authorize a player
                    string sessionKey = Authorization.Login(username, password);
                    // Check if the authorization failed
                    if (sessionKey == "")
                    {
                        throw new Exception(ConsoleEx.lastErrorMessage);
                    }
                    // Check if the ship is fine
                    Player player = Authorization.FindBySession(sessionKey);
                    // If not - create a new one
                    if (player.ship == null)
                        Universe.AddNewPlayerShip(player.startingShipName, player);
                    // If authorization is successful, redirect to hero page
                    if (sessionKey != "")
                    {
                        responseString = WebConstructor.headOpener;
                        responseString += "</head><body onload=\"sendPostRequest('hero', {sessionKey: '"
                            + sessionKey + "'});\">";
                        //responseString += "<h1>Login successful. Session key: " + sessionKey + "</h1>";
                    }
                    // If not - show an error message
                    else
                    {
                        responseString += "<h1>" + ConsoleEx.lastErrorMessage + "</h1>";
                    }
                }
                // Finish the message
                responseString += WebConstructor.defaultFooter;
            }
            // Something went wrong (typically, argument parsing error)
            catch (Exception ex)
            {
                responseString = WebConstructor.defaultHeader;
                responseString += "<h1>An error occured:</h1><h2>";
                responseString += ex.Message + "</h2>";
                responseString += WebConstructor.defaultFooter;
            }
            // Send message
            WebWriter.Reply(response, responseString);
        }
        //==============================================================================================================
        // Update
        //==============================================================================================================
        public static void OnUpdate(HttpListenerRequest request, HttpListenerResponse response)
        {
            try
            {
                // Parsing arguments
                Dictionary<string, string> postArguments = WebReader.GetPostArgs(request);
                string sessionKey = postArguments["sessionKey"];
                Player player = Authorization.FindBySession(sessionKey);
                WebUpdaterCore.Enqueue(new PlayerResponsePair(player, response));
                player.KeepAlive();
            }
            catch (Exception) {}
        }
    }

}
