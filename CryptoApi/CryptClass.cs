using System;
using System.IO;
using System.Windows.Forms;
using System.Text;
using System.Security.Cryptography;

namespace CryptoApi
{
	public static class CryptClass
	{  
	    const int BufferSize = 1024;
	    const int OpenFile = 256;
	    static System.Net.Sockets.TcpListener server = null;
	    static string SecretKey = null;
	    static bool HavePassword = false;
	    static byte[] AsymEncrRemoteServerPublicKey = null;
	    
	    public struct SymEncryptAlgorithmOptions
	    {
	        public string Algoname;
	        public int BlockSizeBits;
	        public int KeySizeBits;
	        
	        public string ToRequest()
	        {
	            return Algoname + "*" + BlockSizeBits.ToString() + "*" + KeySizeBits.ToString();
	        }
	        
	        public void FromRequest(string request)
	        {
	            Algoname = request.Substring(0, request.IndexOf("*"));
	            request = request.Remove(0, request.IndexOf("*") + 1);
	            BlockSizeBits = Convert.ToInt32(request.Substring(0, request.IndexOf("*")));
	            request = request.Remove(0, request.IndexOf("*") + 1);
	            KeySizeBits = Convert.ToInt32(request);
	        }
	    }
	    
	    public static SymEncryptAlgorithmOptions symEncryptAlgorithmOptions; 
	    
	    static void GetEncryptionOptions(string formattedData)
	    {
	        symEncryptAlgorithmOptions.FromRequest(formattedData.Remove(0,2));
	    }
	}
}
