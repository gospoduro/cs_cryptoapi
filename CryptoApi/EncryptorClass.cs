// Tasks: implement
// CALG_3DES_112
// CALG_AES_192
// flgorithms

using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace CryptoApi
{    
    public class SymEncryptor
    {
        const int BUFFER_SIZE = 1024;
        
        public SymEncryptor(SymmetricAlgorithm sma, string filefrom, string fileto, int BlockSizeBits, int KeySizeBits, CipherMode CipherModeSMA, string Password)
        {
            byte[] buffer = new byte[BUFFER_SIZE];
            FileStream fileFrom = File.OpenRead(filefrom);
            FileStream fileTo = File.OpenWrite(fileto);
            sma.BlockSize = BlockSizeBits;
            sma.KeySize = KeySizeBits;
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password, UnicodeEncoding.Unicode.GetBytes("super secret key@@!@#$%^&*987987"), "SHA512", 100);
            
            sma.Key = pdb.GetBytes(KeySizeBits/8);
            sma.IV = pdb.GetBytes(BlockSizeBits/8);
            sma.Mode = CipherModeSMA;
            
            CryptoStream cs = new CryptoStream(fileTo, sma.CreateEncryptor(), CryptoStreamMode.Write);
            
            int ReadBytes = -1;
            do
            {
                ReadBytes = fileFrom.Read(buffer,0, BUFFER_SIZE);
                if (ReadBytes > 0)
                {
                    cs.Write(buffer, 0, ReadBytes);
                }
            }
            while(ReadBytes > 0);
            
            fileFrom.Close();
            cs.Close();
            fileTo.Close();
        }    
    }    
    // have to incrypt all file per once
    // Windows XP ++ required
    public class AsymEncryptor
    {
        public AsymEncryptor(string filefrom, string fileto)
        {
            byte[] buffer = File.ReadAllBytes(filefrom);
            Cryptography.AssignParameter();
            File.WriteAllBytes(fileto, Cryptography.EncryptData(buffer));
        }
    }
    public class SymDecryptor
    {
        const int BUFFER_SIZE = 1024;
        
        public SymDecryptor(SymmetricAlgorithm sma, string filefrom, string fileto, int BlockSizeBits, int KeySizeBits, CipherMode CipherModeSMA, string Password)
        {
            byte[] buffer = new byte[BUFFER_SIZE];
            FileStream fileFrom = File.OpenRead(filefrom);
            FileStream fileTo = File.OpenWrite(fileto);
            sma.BlockSize = BlockSizeBits;
            sma.KeySize = KeySizeBits;
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password, UnicodeEncoding.Unicode.GetBytes("super secret key@@!@#$%^&*987987"), "SHA512", 100);
            
            sma.Key = pdb.GetBytes(KeySizeBits/8);
            sma.IV = pdb.GetBytes(BlockSizeBits/8);
            sma.Mode = CipherModeSMA;
            
            CryptoStream cs = new CryptoStream(fileFrom, sma.CreateDecryptor(), CryptoStreamMode.Read);
            
            int ReadBytes = -1;
            do
            {
                ReadBytes = cs.Read(buffer,0, BUFFER_SIZE);
                if (ReadBytes > 0)
                {
                    fileTo.Write(buffer, 0, ReadBytes);
                }
            }
            while(ReadBytes > 0);
            
            cs.Close();
            fileFrom.Close();
            fileTo.Close();
        }    
    }
    //have to incrypt all file per once
    //Windows XP ++ required
    public class AsymDecryptor
    {
        public AsymDecryptor(string filefrom, string fileto)
        {
            byte[] buffer = File.ReadAllBytes(filefrom);
            Cryptography.AssignParameter();

            File.WriteAllBytes(fileto, Cryptography.DecryptData(buffer));
        }    
    }
    public class CryptoProviderInfo
    {
        object algImpl = null;
        int[] acceptedKeySizes = null;
        int[] acceptedDataSizes = null;
        
        public int[] KeySizes
        {
            get
            {
                return acceptedKeySizes;
            }
        }
        
        public int[] DataSizes
        {
            get
            {
                return acceptedDataSizes;
            }
        }
        
        public SymmetricAlgorithm SymAlgorithm
        {
            get
            {
                return (SymmetricAlgorithm)algImpl;
            }
        }
        
        public object Algorithm
        {
            get{return algImpl;}
        }
        
        public CryptoProviderInfo(string AlgorithmName)
        {
            algImpl = CryptoConfig.CreateFromName(AlgorithmName);
            try
                {
                    SymmetricAlgorithm ob = (SymmetricAlgorithm)algImpl;                
                    // Writing all acceptable keys
                    int maxKeys;
                    if (ob.LegalKeySizes[0].SkipSize > 0) maxKeys = (ob.LegalKeySizes[0].MaxSize - ob.LegalKeySizes[0].MinSize)/ob.LegalKeySizes[0].SkipSize + 1;
                    else maxKeys = 1;
                    acceptedKeySizes = new int[maxKeys];
                    for (int i = 0; i < maxKeys; i++)
                        acceptedKeySizes[i] = ob.LegalKeySizes[0].MinSize + i * ob.LegalKeySizes[0].SkipSize;
                    // Writing all acceptable data sizes
                    int dataSizes;
                    if (ob.LegalBlockSizes[0].SkipSize > 0) dataSizes = (ob.LegalBlockSizes[0].MaxSize - ob.LegalBlockSizes[0].MinSize)/ob.LegalBlockSizes[0].SkipSize + 1;
                    else dataSizes = 1;
                    acceptedDataSizes = new int[dataSizes];
                    for (int i = 0; i < dataSizes; i++)
                        acceptedDataSizes[i] = ob.LegalBlockSizes[0].MinSize + i * ob.LegalBlockSizes[0].SkipSize;
                }
                catch(Exception e)
                {
                    throw new Exception("Not sym algorithm!" + e.Message);
                }
        }
    }
}
