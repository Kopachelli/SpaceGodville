using System;
using System.Collections.Generic;
using System.Text;

namespace zpgServer
{
    public class Player
    {
        string _username;
        string _passwordHash;
        string _passwordSalt;
        string _sessionKey;

        bool _isOnline = false;
        bool _isWaitingForUpdate = false;
        Timer _connectionTimer;

        Ship _ship;

        public int loginCounter = 0;
        public string startingShipName = "";

        public string username { get { return _username; } }
        public string password { get { return _passwordHash; } }
        public string passwordSalt { get { return _passwordSalt; } }
        public string sessionKey { get { return _sessionKey; } }
        public Ship ship { get { return _ship; } set { _ship = value; } }
        public bool isWaitingForUpdate { get { return _isWaitingForUpdate; } }

        public Player(string username, string password, string passwordSalt = null, bool hashProvided = false)
        {
            _username = username;
            if (!hashProvided)
            {
                if (passwordSalt != null) { _passwordSalt = passwordSalt; }
                else { _passwordSalt = PasswordHashing.CreateSalt(8); }
                _passwordHash = PasswordHashing.CreatePasswordHash(password, _passwordSalt);
            }
            else
            {
                _passwordSalt = passwordSalt;
                _passwordHash = password;
            }
        }

        public string Login()
        {
            _isOnline = true;
            _sessionKey = Guid.NewGuid().ToString().Replace("-", "");
            loginCounter += 1;
            DiskCore.Enqueue(DiskAction.UpdateAccountDatabase);
            _connectionTimer = new Timer(Settings.playerDisconnectTimer, false);
            return _sessionKey;
        }
        public void Logout()
        {
            _isOnline = false;
        }
        public bool CheckPassword(string password)
        {
            string newPassHash = PasswordHashing.CreatePasswordHash(password, _passwordSalt);
            return _passwordHash == newPassHash;
        }
        public void NotifyOnUpdate() { _isWaitingForUpdate = true; }
        public void NotifyOnFlush() { _isWaitingForUpdate = false; }
        public void Tick()
        {
            if (_isOnline && _connectionTimer.Tick())
            {
                Authorization.Logout(this, "Connection timeout");
            }
        }
        public void KeepAlive()
        {
            _connectionTimer.Reset();
        }
    }
}
