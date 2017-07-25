using System;
using System.Collections.Generic;
using System.Text;

namespace zpgServer
{
    public static class Authorization
    {
        static bool autosaveEnabled = true;
        static List<Player> _playerLibrary = new List<Player>();
        public static List<Player> playerList { get { return _playerLibrary; } }
        public static void LoadAccounts()
        {
            List<Player> parsedList = XmlManager.ParseAccounts();
            foreach (Player player in parsedList)
            {
                _playerLibrary.Add(player);
            }
            if (parsedList.Count > 0)
                ConsoleEx.Log("Successfully loaded " + parsedList.Count + " account(s).");
            else
                ConsoleEx.Log("No account database found or it is corrupted.");
        }
        public static bool CreateAccount(string username, string password, string passwordSalt = null)
        {
            username = username.Replace(" ", "");
            foreach (Player p in _playerLibrary)
            {
                if (p.username == username)
                {
                    ConsoleEx.Error("Account creation error. Username taken: " + username);
                    return false;
                }
            }
            Player player = new Player(username, password, passwordSalt);
            _playerLibrary.Add(player);
            ConsoleEx.Log("Account " + username + " created successfully.");
            if (autosaveEnabled) { DiskCore.Enqueue(DiskAction.UpdateAccountDatabase); }
            return true;
        }

        public static string Login(string username, string password)
        {
            username = username.Replace(" ", "");
            for (int i = 0; i < _playerLibrary.Count; i++)
            {
                if (username == _playerLibrary[i].username)
                {
                    if (_playerLibrary[i].CheckPassword(password))
                    {
                        ConsoleEx.Log("Player " + _playerLibrary[i].username + " logged in.");
                        return _playerLibrary[i].Login();
                    }
                    else
                    {
                        ConsoleEx.Error("Login failed. Password mismatch.");
                        return "";
                    }
                }
            }
            ConsoleEx.Error("Login failed. User not found.");
            return "";
        }
        public static void Logout(Player player, string reason)
        {
            if (player == null)
                return;

            player.Logout();
            ConsoleEx.Log("Player " + player.username + " disconnected. Reason: " + reason + ".");
        }

        public static Player FindBySession(string sessionKey)
        {
            foreach (Player player in _playerLibrary)
            {
                if (player.sessionKey == sessionKey) { return player; }
            }
            return null;
        }
        public static Player FindByUsername(string username)
        {
            foreach (Player player in _playerLibrary)
            {
                if (player.username == username) { return player; }
            }
            return null;
        }
        public static void EnableAccountAutosave() { autosaveEnabled = true; }
        public static void DisableAccountAutosave() { autosaveEnabled = false; }
    }
}
