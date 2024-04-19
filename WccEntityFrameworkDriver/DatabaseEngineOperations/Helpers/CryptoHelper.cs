using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto.Generators;
using System.Security.Cryptography;
using System.Security;
using System.Diagnostics;

namespace WccEntityFrameworkDriver.DatabaseEngineOperations.Helpers
{
    public delegate void CryptoDebugEvent(string x);
    public class CryptoHelper
    {
        private int _cost = 9;
        public int Cost { 
            get { return _cost; }
            set { }
        }   
        
        public event CryptoDebugEvent CryptoDebugEvent;

        private RNGCryptoServiceProvider _rng = new RNGCryptoServiceProvider();
        public byte[] BcryptPasswordHash(string password,out byte[]salt)
        {
            salt = GenerateSalt();
            return BCrypt.Generate(Encoding.UTF8.GetBytes(password), salt,_cost);
                
        }

        public bool BcryptVerifyPassword(string pass, byte[] hash, byte[] salt)
        {
            return hash.SequenceEqual(BCrypt.Generate(Encoding.UTF8.GetBytes(pass), salt, _cost));
        }

        /// <summary>
        /// determines speed of hashing for provided cost
        /// </summary>
        /// <returns></returns>
        public long TestBcryptSpeed(string password,int cost)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            BCrypt.Generate(Encoding.UTF8.GetBytes(password), GenerateSalt(16), cost);
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }


        public byte[] GenerateSalt(int SaltLength=16)
        {
            byte[] salt = new byte[SaltLength];
            _rng.GetBytes(salt);
            return salt;
        }


    }
}
