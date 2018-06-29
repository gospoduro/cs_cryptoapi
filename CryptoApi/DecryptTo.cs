using System;
using System.IO;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;

namespace JavaScience {

//--- P/Invoke CryptoAPI wrapper classes -----
public class Win32 
{

 [DllImport("crypt32.dll")]
    public static extern bool CryptDecodeObject(
	uint CertEncodingType,
	uint lpszStructType,
	byte[] pbEncoded,
	uint cbEncoded,
	uint flags,
	[In, Out] byte[] pvStructInfo,
	ref uint cbStructInfo);


 [DllImport("crypt32.dll", SetLastError=true)]
    public static extern IntPtr CertFindCertificateInStore(
	IntPtr hCertStore,
	uint dwCertEncodingType,
	uint dwFindFlags,
	uint dwFindType,
	[In, MarshalAs(UnmanagedType.LPWStr)]String pszFindString,
	IntPtr pPrevCertCntxt) ;

 [DllImport("crypt32.dll", SetLastError=true)]
    public static extern bool CertFreeCertificateContext(
	IntPtr hCertStore) ;


 [DllImport("crypt32.dll", SetLastError=true)]	
  public static extern bool CertGetCertificateContextProperty(
  IntPtr pCertContext, uint dwPropId, IntPtr pvData, ref uint pcbData);



 [DllImport("crypt32.dll", CharSet=CharSet.Auto, SetLastError=true)] //overloaded
    public static extern IntPtr CertOpenStore(
	[MarshalAs(UnmanagedType.LPStr)] String storeProvider,
	uint dwMsgAndCertEncodingType,
	IntPtr hCryptProv,
	uint dwFlags,
	String cchNameString) ;


 [DllImport("crypt32.dll", SetLastError=true)]
    public static extern bool CertCloseStore(
	IntPtr hCertStore,
	uint dwFlags) ;

}


 [StructLayout(LayoutKind.Sequential)]
  public struct PUBKEYBLOBHEADERS {
	public byte bType;	//BLOBHEADER
	public byte bVersion;	//BLOBHEADER
	public short reserved;	//BLOBHEADER
	public uint aiKeyAlg;	//BLOBHEADER
	public uint magic;	//RSAPUBKEY
	public uint bitlen;	//RSAPUBKEY
	public uint pubexp;	//RSAPUBKEY
 }


 [StructLayout(LayoutKind.Sequential)]
  public struct CRYPT_KEY_PROV_INFO
  {
	[MarshalAs(UnmanagedType.LPWStr)] public String ContainerName;
	[MarshalAs(UnmanagedType.LPWStr)] public String ProvName;
	public uint ProvType;
	public uint Flags;
	public uint ProvParam;
	public IntPtr rgProvParam;
	public uint KeySpec;
  }


public class DecryptTo{

 const uint CERT_SYSTEM_STORE_CURRENT_USER	= 0x00010000;
 const uint CERT_STORE_READONLY_FLAG		= 0x00008000;
 const uint CERT_STORE_OPEN_EXISTING_FLAG	= 0x00004000;
 const uint CERT_KEY_PROV_INFO_PROP_ID	= 0x00000002;
 const uint CERT_FIND_SUBJECT_STR	= 0x00080007;
 const uint X509_ASN_ENCODING 		= 0x00000001;
 const uint PKCS_7_ASN_ENCODING 	= 0x00010000;
 const uint RSA_CSP_PUBLICKEYBLOB	= 19;
 const int  AT_KEYEXCHANGE		= 1;  //keyspec values
 const int  AT_SIGNATURE		= 2;
 String[] keyspecs = {"", "AT_KEYEXCHANGE", "AT_SIGNATURE"};
 static uint ENCODING_TYPE 		= PKCS_7_ASN_ENCODING | X509_ASN_ENCODING ;

 private X509Certificate recipcert;
 private String keycontainer;
 private int RSAkeytype;
 private byte[] certkeymodulus;
 private byte[] certkeyexponent;
 private uint certkeysize;
 private bool verbose = false;

 public static void Main(String[] args)
	{
	String encryptedContent, contentFile, encryptedKey, encryptedIV;
	String subjectstr;

	DecryptTo oDec = new DecryptTo();

	if(args.Length<4 || args.Length>5){
	 DecryptTo.usage();
	 return;
	 }

	if(args.Length==5)  //any 5th argument enables verbose mode
		oDec.verbose = true;

	encryptedContent = args[0] ;
	if (!File.Exists(encryptedContent)){
	 Console.WriteLine("File '{0}' not found.", encryptedContent);
	 return;
	}

	encryptedKey = args[2] ;
	if (!File.Exists(encryptedKey)){
	 Console.WriteLine("File '{0}' not found.", encryptedKey);
	 return;
	}

	encryptedIV = args[3] ;
	if (!File.Exists(encryptedIV)){
	 Console.WriteLine("File '{0}' not found.", encryptedIV);
	 return;
	}

	contentFile	= args[1];

	Console.Write("Enter SubjectName of recipient: ");
	subjectstr = Console.ReadLine();


	// ---- get recipient certificate and associated keycontainer ----
	if(!oDec.GetRecipientPVKProps(subjectstr)){
	 Console.WriteLine("Couldn't find recipient MY store certificate with private key");
	 return;
	 }
	 
	Console.WriteLine("\nFound certificate in MY store with SubjectName string \"{0}\"", subjectstr); 
	Console.WriteLine("SubjectName:\t{0}", oDec.recipcert.GetName());
	Console.WriteLine("Keycontainer: {0}", oDec.keycontainer);
	Console.WriteLine("Key type: {0}", oDec.keyspecs[oDec.RSAkeytype]);
	
	if(oDec.RSAkeytype == 2)
	{
	 Console.WriteLine("\n**** Decryption Failed! ****\nRSA decryption only supported by {0} key type.", oDec.keyspecs[1]);
	 return;
	}

	//----- get recipient certificate public key parameters (only used to get key size info) ----
	if(!oDec.GetCertPublicKey(oDec.recipcert)){
	 Console.WriteLine("Couldn't get recipient certificate public key");
	 return;
	}
	Console.WriteLine("Public key size:   {0} bits", oDec.certkeysize);

	//--- Decrypt the 3DES Key and IV files with RSA; decrypt content with 3DES----
	 if(oDec.TripleDESDecrypt(encryptedContent, contentFile, encryptedKey, encryptedIV))
	   Console.WriteLine("\nSuccessfully decrypted '{0}' to '{1}'", encryptedContent, contentFile);
	 else
	   Console.WriteLine("\n*** Failed to decrypt file ****") ;
  }



 private bool GetRecipientPVKProps(String searchstr) {
   IntPtr hSysStore	= IntPtr.Zero;  
   IntPtr hCertCntxt	= IntPtr.Zero;
   IntPtr pProvInfo	= IntPtr.Zero;
   uint provinfosize	= 0;
   string searchstore = "MY";  //only include MY store 

   bool gotpvkprops	= false;
   uint openflags = 	CERT_SYSTEM_STORE_CURRENT_USER	| 
			CERT_STORE_READONLY_FLAG 	| 
			CERT_STORE_OPEN_EXISTING_FLAG;

   hSysStore = Win32.CertOpenStore("System", ENCODING_TYPE, IntPtr.Zero, openflags, searchstore );
	if(hSysStore == IntPtr.Zero){
	   Console.WriteLine("Failed to open system store {0}", searchstore);
	   return false;
	  }
   //--- only accept the first matching certificate ----
   hCertCntxt=Win32.CertFindCertificateInStore(
      hSysStore,
      ENCODING_TYPE,
      0, 
      CERT_FIND_SUBJECT_STR,
      searchstr ,
      IntPtr.Zero) ;

  if(hCertCntxt == IntPtr.Zero){
	Win32.CertCloseStore(hSysStore, 0) ;
	return gotpvkprops;
	}
  if(!Win32.CertGetCertificateContextProperty(hCertCntxt, CERT_KEY_PROV_INFO_PROP_ID, IntPtr.Zero, ref provinfosize))
	{
	 if(hCertCntxt != IntPtr.Zero)
	  Win32.CertFreeCertificateContext(hCertCntxt);
	 Win32.CertCloseStore(hSysStore, 0) ;
	 return gotpvkprops;
	 }
  pProvInfo = Marshal.AllocHGlobal((int)provinfosize);
  if(Win32.CertGetCertificateContextProperty(hCertCntxt, CERT_KEY_PROV_INFO_PROP_ID, pProvInfo, ref provinfosize))
	{
	CRYPT_KEY_PROV_INFO ckinfo = (CRYPT_KEY_PROV_INFO)Marshal.PtrToStructure(pProvInfo, typeof(CRYPT_KEY_PROV_INFO));
	Marshal.FreeHGlobal(pProvInfo);
	this.recipcert = new X509Certificate(hCertCntxt);
	this.keycontainer = ckinfo.ContainerName;
	this.RSAkeytype = (int)ckinfo.KeySpec;
	gotpvkprops = true ;  // only way for valid return
	}
  else
	 Marshal.FreeHGlobal(pProvInfo);


//-------  Clean Up  -----------
  if(hCertCntxt != IntPtr.Zero)
	Win32.CertFreeCertificateContext(hCertCntxt);
  if(hSysStore != IntPtr.Zero)
	Win32.CertCloseStore(hSysStore, 0) ;
  return gotpvkprops;
 }




//--- Decrypt the encrypted key and IV; decrypt encrypted content ---
 private bool TripleDESDecrypt(String encContent, String content, String encKeyfile, String encIVfile)
 {    
    FileStream fin  = new FileStream(encContent, FileMode.Open, FileAccess.Read);
    FileStream fout = new FileStream(content, FileMode.OpenOrCreate, FileAccess.Write);
      
    byte[] buff = new byte[1000]; //decryption buffer.
    int lenread;                

   //--- Recover the RSA-encrypted DES key and IV ---
    byte[] encKey = GetFileBytes(encKeyfile);
    byte[] encIV  = GetFileBytes(encIVfile);
    Console.WriteLine("\nDecrypting 3DES Key and IV ...");
    byte[] key = DoRSADecrypt(encKey, this.keycontainer, this.RSAkeytype);
    byte[] IV  = DoRSADecrypt(encIV,  this.keycontainer, this.RSAkeytype);

    if(key == null || IV == null)
	return false;

   try
    {
    TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();          
    CryptoStream decStream = new CryptoStream(fout, tdes.CreateDecryptor(key, IV), CryptoStreamMode.Write);
    Console.WriteLine("Decrypting content ...");
    //do the decryption ...
    while( (lenread = fin.Read(buff, 0, 1000))>0)
        decStream.Write(buff, 0, lenread);
    decStream.Close(); 
    return true;
    }
   catch (Exception)
	{
	return false;
	}                    
}



 private byte[] DoRSADecrypt(byte[] encdata, String container, int keyspec)
 {
   if(encdata==null ||container==null || (keyspec !=1 && keyspec !=2) )
	return null;

   byte[] clearkey = null;
   try
    {
	//Construct RSA with keycontainer associated with certificate found
	CspParameters cp = new CspParameters();
	cp.KeyContainerName = container;
	cp.KeyNumber = keyspec;
	RSACryptoServiceProvider oRSA = new RSACryptoServiceProvider(cp);
	clearkey = oRSA.Decrypt(encdata, false);
    }

   catch(CryptographicException cexc)
	{
		Console.WriteLine("Error in DoRSAKeyDecrypt\n{0}", cexc.Message);
		return null ;
	}
    return clearkey;
  }


//----- decode public key and extract modulus and exponent ----
 private bool GetCertPublicKey(X509Certificate cert)
 {
	byte[] publickeyblob ;
        byte[] encodedpubkey = cert.GetPublicKey(); //asn.1 encoded public key

       	uint blobbytes=0;
	if(verbose) {
	  Console.WriteLine();
	  showBytes("Encoded publickey", encodedpubkey);
	  Console.WriteLine();
	 }
	if(Win32.CryptDecodeObject(ENCODING_TYPE, RSA_CSP_PUBLICKEYBLOB, encodedpubkey, (uint)encodedpubkey.Length, 0, null, ref blobbytes))
	 {
	  publickeyblob = new byte[blobbytes];
	   if(Win32.CryptDecodeObject(ENCODING_TYPE, RSA_CSP_PUBLICKEYBLOB, encodedpubkey, (uint)encodedpubkey.Length, 0, publickeyblob, ref blobbytes))
		if(verbose)
		  showBytes("CryptoAPI publickeyblob", publickeyblob);
	 }
	else{
	  Console.WriteLine("Couldn't decode publickeyblob from certificate publickey") ;
	  return false;
	}
	 
	PUBKEYBLOBHEADERS pkheaders = new PUBKEYBLOBHEADERS() ;
	int headerslength = Marshal.SizeOf(pkheaders);
	IntPtr buffer = Marshal.AllocHGlobal( headerslength);
	Marshal.Copy( publickeyblob, 0, buffer, headerslength );
	pkheaders = (PUBKEYBLOBHEADERS) Marshal.PtrToStructure( buffer, typeof(PUBKEYBLOBHEADERS) );
	Marshal.FreeHGlobal( buffer );

 // --- note that CryptDecodeObject on PUBLICKEYBLOB will always return aiKeyAlg 0x0000a400 (CALG_RSA_KEYX)
	if(verbose) {
	 Console.WriteLine("\n ---- PUBLICKEYBLOB headers ------");
	 Console.WriteLine("  btype     {0}", pkheaders.bType);
	 Console.WriteLine("  bversion  {0}", pkheaders.bVersion);
	 Console.WriteLine("  reserved  {0}", pkheaders.reserved);
	 Console.WriteLine("  aiKeyAlg  0x{0:x8}", pkheaders.aiKeyAlg);
	 String magicstring = (new ASCIIEncoding()).GetString(BitConverter.GetBytes(pkheaders.magic)) ;
	 Console.WriteLine("  magic     0x{0:x8}     '{1}'", pkheaders.magic, magicstring);
	 Console.WriteLine("  bitlen    {0}", pkheaders.bitlen);
	 Console.WriteLine("  pubexp    {0}", pkheaders.pubexp);
	 Console.WriteLine(" --------------------------------");
	}
	//-----  Get public key size in bits -------------
	this.certkeysize = pkheaders.bitlen;

	//-----  Get public exponent -------------
	byte[] exponent = BitConverter.GetBytes(pkheaders.pubexp); //little-endian ordered
	Array.Reverse(exponent);    //convert to big-endian order
	this.certkeyexponent = exponent;
	if(verbose)
	 showBytes("\nPublic key exponent (big-endian order):", exponent);

	//-----  Get modulus  -------------
	int modulusbytes = (int)pkheaders.bitlen/8 ;
	byte[] modulus = new byte[modulusbytes];
	try{
		Array.Copy(publickeyblob, headerslength, modulus, 0, modulusbytes);
		Array.Reverse(modulus);   //convert from little to big-endian ordering.
		this.certkeymodulus = modulus;
		if(verbose)
		 showBytes("\nPublic key modulus  (big-endian order):", modulus);
	}
	catch(Exception){
		Console.WriteLine("Problem getting modulus from publickeyblob");
		return false;
	}
   return true;
 }



 private byte[] GetFileBytes(String filename){
	if(!File.Exists(filename))
	 return null;
	Stream stream=new FileStream(filename,FileMode.Open);
	int datalen = (int)stream.Length;
	byte[] filebytes =new byte[datalen];
	stream.Seek(0,SeekOrigin.Begin);
	stream.Read(filebytes,0,datalen);
	stream.Close();
	return filebytes;
  }


 private void PutFileBytes(String outfile, byte[] data, int bytes) {
  FileStream fs = null;
	if(bytes > data.Length) {
	   Console.WriteLine("Too many bytes");
	   return;
	  }
	try{
	 fs = new FileStream(outfile, FileMode.Create);
	 fs.Write(data, 0, bytes);
	 Console.WriteLine("Wrote file '{0}'", outfile) ;
	}
	catch(Exception e) {
	 Console.WriteLine(e.Message) ; 
	}
	finally {
	 fs.Close();
	}
 }




 private static void showBytes(String info, byte[] data){
  Console.WriteLine("{0}  [{1} bytes]", info, data.Length);
  for(int i=1; i<=data.Length; i++){	
	Console.Write("{0:X2}  ", data[i-1]) ;
	if(i%16 == 0)
	 Console.WriteLine();
	}
  Console.WriteLine();
  }


 private static void usage()
  {
   Console.WriteLine("\nUsage:\nDecryptTo.exe [EncryptedFile] [outContentFile] [inKeyfile] [inIVfile]");
  }


 }
}
