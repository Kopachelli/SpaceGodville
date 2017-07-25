using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace zpgServer
{
    static public class WebSecurity
    {
        static RSACryptoServiceProvider RSA;
        static RSAParameters RSAKeyInfo;

        public static void GenerateKeys()
        {
            //Generate a public/private key pair.  
            RSA = new RSACryptoServiceProvider();
            //Save the public key information to an RSAParameters structure.  
            RSAKeyInfo = RSA.ExportParameters(false);
        }

        
    }
}
