using System.Numerics;
using System.Security.Cryptography;
namespace RSAAlgorithm;
class RSA
{

    static public void Main(String[] args)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        KeyGeneration key = new KeyGeneration(); // erschaft neue instanz wo der Public und Private key generiert wird
        RSA rsa = new RSA(); // erschaft neue istanz der klasse RSA, aber weil die klasse leer ist ist diese line auch leer
        Console.WriteLine("\nschreib deine Nachricht");
        String ins = Console.ReadLine(); // user input als "ins" deklariert


        // erschaffen eine neue instanz der "RandomNumberGenerator" klasse und nutzt diese 256 Bytes zu generieren.
        // Diese Bytes werden dan in BidInteger konvertiert und diese Objekt hat den namen "a".
        RandomNumberGenerator rng = RandomNumberGenerator.Create();
         byte[] bytes = new byte[128];
          BigInteger a;
          rng.GetBytes(bytes);
          a = new BigInteger(bytes);
          a = BigInteger.Abs(a); // hier mache ich das um auf nummer sicher zu gehen ob es positiv ist weil manchmal ist in minus bereich


        EncryptDecrypt encryptDecrypt = new EncryptDecrypt(); // instanz wo alles ab geht

        List<BigInteger> cypher = encryptDecrypt.EncryptString(ins, key.e, key.n); // hier wird user input encrypted im encryptDecrypt (oben).
                                                                                   // "key.e" und "key.n" zum rechnen.

        string plain = encryptDecrypt.DecryptString(cypher, key.d, key.n); // hier wird decrypted mit der "DecryptString" methode des "encryptDecrypt" objektes.
                                                                           // "key.d" and "key.n" zum rechnen.
        Console.WriteLine("\ndecrypted");

        Console.WriteLine(plain);
    }
}

