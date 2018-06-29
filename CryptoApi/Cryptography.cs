using System;
using System.Security;
using System.Security.Cryptography;
using System.IO;
using System.Windows.Forms;

namespace CryptoApi
{
	/// <summary>
	/// Description of Cryptography.
	/// </summary>
    public static class Cryptography
    {  
        public static RSACryptoServiceProvider rsa;
        
        const string publicxml = ".public.xml";
        const string privatexml = ".private.xml";
        
        static string CONTAINER_NAME = null;
        static string ContainerFileName = null;
        
        /// <summary>
        /// Initer
        /// </summary>
        /// <param name="ContainerName">Name of the container</param>
        /// <param name="FileWithContainer">Start of the two files with pravate and public keys</param>
        public static void Init(string ContainerName, string FileWithContainer)
        {
            CONTAINER_NAME = ContainerName;
            ContainerFileName = FileWithContainer;
        }
        
        public static void AssignParameter()  
        {	
            const int PROVIDER_RSA_FULL = 1; 	
            CspParameters cspParams;	
            cspParams = new CspParameters(PROVIDER_RSA_FULL);	
            cspParams.KeyContainerName = CONTAINER_NAME;	
            cspParams.Flags = CspProviderFlags.UseUserProtectedKey;
            cspParams.ProviderName = "Microsoft Strong Cryptographic Provider";	

            rsa = new RSACryptoServiceProvider(1024, cspParams);   
        }  
        public static string EncryptData(string data2Encrypt)  
        {	
            //AssignParameter();      
            StreamReader reader = new   StreamReader(ContainerFileName + publicxml);
            string publicOnlyKeyXML = reader.ReadToEnd();
            rsa.FromXmlString(publicOnlyKeyXML);	
            reader.Close();	//read plaintext, encrypt it to ciphertext	
            byte[] plainbytes =	System.Text.Encoding.UTF8.GetBytes(data2Encrypt);	
            byte[] cipherbytes = rsa.Encrypt(plainbytes,true);	
                        
            return Convert.ToBase64String(cipherbytes);  
        }  
        
        public static byte[] EncryptData(string data2Encrypt, byte[] CspBlob)
        {	
            byte[] cipherbytes;
            
            try
            {
                const int PROVIDER_RSA_FULL = 1; 	
                CspParameters cspParams;	
                cspParams = new CspParameters(PROVIDER_RSA_FULL);	
                cspParams.KeyContainerName = CONTAINER_NAME;	
                cspParams.Flags = CspProviderFlags.UseExistingKey;
                cspParams.ProviderName = "Microsoft Strong Cryptographic Provider";	

                RSACryptoServiceProvider rsa2 = new RSACryptoServiceProvider(1024, cspParams);

                rsa2.ImportCspBlob(CspBlob);
                byte[] plainbytes =	System.Text.Encoding.UTF8.GetBytes(data2Encrypt);	
                cipherbytes = rsa2.Encrypt(plainbytes,true);	//true-gag
            }
            catch(Exception e)
            {
                throw new Exception("EncryptData - " + e.Message);
            }
            
            return cipherbytes;  
        }  
 
        /// <summary>
        /// Returns byte[HowMuch] of encrypted data
        /// </summary>
        /// <param name="plainbytes"></param>
        /// <param name="HowMuch">in this parameter fill how much bytes in this massive are in use</param>
        /// <returns></returns>
        public static byte[] EncryptData(byte[] plainbytes)  
        {	
           // AssignParameter();      
            StreamReader reader = new   StreamReader(ContainerFileName + publicxml);
            string publicOnlyKeyXML = reader.ReadToEnd();
            rsa.FromXmlString(publicOnlyKeyXML);	
            reader.Close();
            byte[] plain = rsa.Encrypt(plainbytes,true);
            
            return plain;            
        }   
        
        public static byte[] GetPublicKey()
        {
            RSACryptoServiceProvider rsa2;
            try
            {
            const int PROVIDER_RSA_FULL = 1; 	
            CspParameters cspParams;	
            cspParams = new CspParameters(PROVIDER_RSA_FULL);	
            cspParams.KeyContainerName = CONTAINER_NAME;	
            cspParams.Flags = CspProviderFlags.UseExistingKey;
            cspParams.ProviderName = "Microsoft Strong Cryptographic Provider";	

            rsa2 = new RSACryptoServiceProvider(1024, cspParams);

            StreamReader reader = new   StreamReader(ContainerFileName + publicxml);
            string publicOnlyKeyXML = reader.ReadToEnd();
            rsa2.FromXmlString(publicOnlyKeyXML);	
            reader.Close();
            }
            catch(Exception ee)
            {
                throw new Exception("GetPublicKey - error - " + ee.Message);
            }
            return rsa2.ExportCspBlob(false);
        }

        public static byte[] DecryptData(byte[] Encrypted)
        {	
            //AssignParameter();	
            StreamReader reader = new StreamReader(ContainerFileName + privatexml);	
            string publicPrivateKeyXML = reader.ReadToEnd();	
            rsa.FromXmlString(publicPrivateKeyXML);	reader.Close();     //read ciphertext, decrypt it to plaintext	
            byte[] encr = rsa.Decrypt(Encrypted,true); 	
            return encr; 	
        }      
        
        public static void AssignNewKey()  
        {	
            AssignParameter();		//provide public and private RSA params	
            StreamWriter writer = new   StreamWriter(ContainerFileName + privatexml);	
            string publicPrivateKeyXML = rsa.ToXmlString(true);	
            writer.Write(publicPrivateKeyXML);	
            writer.Close();	//provide public only RSA params	
            writer = new StreamWriter(ContainerFileName + publicxml);	
            string publicOnlyKeyXML = rsa.ToXmlString(false);	
            writer.Write(publicOnlyKeyXML);	writer.Close();  
        }  
        public static string DecryptDataToString(byte[] getpassword)  
        {	
            AssignParameter();	
            StreamReader reader = new StreamReader(ContainerFileName + privatexml);	
            string publicPrivateKeyXML = reader.ReadToEnd();	
            rsa.FromXmlString(publicPrivateKeyXML);	
            reader.Close();
            byte[] plain =	rsa.Decrypt(getpassword,true); 	
            return System.Text.Encoding.UTF8.GetString(plain);
        }        
    }
}
