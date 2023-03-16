using System.Numerics;
namespace RSAAlgorithm;

public class KeyGeneration
{
    public BigInteger p,q,n,f,e,d;

    public KeyGeneration() // eine public klasse names "KeyGeneration" um keys zu generieren
    {
        // zwei große Primzahlen von PrimeRandNum.cs
        PrimeRandNume primeRandNume = new PrimeRandNume();
        p = primeRandNume.generate(1024);
        q = primeRandNume.generate(1024);

        // die Rechnungen vom blatt
        n = p * q;
        f=(p-1)*(q-1);

        // Generiert zwei keys mit dem nahmen und gibt die im Program.cs als "key.d" und "key.e"
        e = generatePublicKey();
        d= generatePrivateKey();
        Console.WriteLine("publick Key \n" + e);
        Console.WriteLine("private Key \n" + d);
    }

    public BigInteger generatePublicKey() // Methoden deklaration und gibt ein "BigInteger".
    {
        BigInteger result=0; // initialisiert ein BigInteger und setzt den als 0
        BigIntegerPrimeTest BIPT = new BigIntegerPrimeTest(); // um zu gucken ob der gegeben Integer ist Primzahl ist und name ist "BIPT"

        for (BigInteger i = f / 2; i < f; i++) // Started ein loop das initialisiert die BigInteger variable "i" mit der value von "f / 2".
                                               // geht so lange "i" kleiner als "f", und "i" geht hoch jedesmal.
        {
            if (BIPT.IsProbablePrime(i, 10) == true) // testet "i" ist probable zu prime benutzt "IsProbablePrime" methode.
                                                     // zeite argument ist ein parameter das kontroliert die genauigkeit des Priemzahles.
            {
                if (BigInteger.GreatestCommonDivisor(i, f) == 1) // Damit wird geprüft, ob "i" und "f" relativ prim sind,
                                                                 // indem ihr größter gemeinsamer Teiler mit der Methode "GreatestCommonDivisor" der Klasse BigInteger berechnet wird.
                                                                 // Wenn der größte gemeinsame Teiler 1 ist, dann sind "i" und "f" relativ prim.
                {
                    result = i; // Wenn "i" sowohl wahrscheinlich prim als auch relativ prim zu "f" ist,
                                // wird "i" der "result" variable zugewiesen
                    break; // und die Schleife wird mit der Break-Anweisung beendet.
                }
            }
         }

        return result; // gibt "result" aus
    }

    // Diese Methode erzeugt einen privaten Schlüssel d.
    // Sie verwendet die Methode ModInverse (unten),
    // um die modulare Umkehrung von "e" modulo "f" zu berechnen.
    public BigInteger generatePrivateKey()
    {
        BigInteger result = ModInverse(e, f);
        return result;
    }

    public static BigInteger ModInverse(BigInteger a, BigInteger m) 
    {
        if (m == 1) return 0; // ist "m" gleich 1, dann, gibt die Methode 0 zurück, da die modulare Inverse nicht definiert ist, wenn der Modulus 1 ist.
        BigInteger m0 = m; // neue BigInteger-Variable mit dem Namen "m0" erstellen und mit dem Wert von "m" gleichsetzten.
        (BigInteger x, BigInteger y) = (1, 0); // Dadurch werden zwei BigInteger-Variablen, "x" und "y", initialisiert und ihre Anfangswerte auf 1 bzw. 0 gesetzt.
                                               // Dies wird im erweiterten euklidischen Algorithmus verwendet, um die modulare Inverse zu berechnen.

        // Damit beginnt eine Schleife, die so lange fortgesetzt wird, wenn "a" größer als 1 ist.
        // Innerhalb der Schleife berechnet der Algorithmus den Quotienten aus "a" / "m" und weist ihn einer neuen BigInteger-Variablen namens "q" zu.
        while (a > 1)
        {
            BigInteger q = a / m;
            (a, m) = (m, a % m); // Die Werte von "a" und "m" werden dann unter Verwendung der Tupel-Notation aktualisiert, wobei der neue Wert von "a" "m" ist und der neue Wert von "m" der Rest von "a" geteilt durch "m".
            (x, y) = (y, x - q * y); // Die Werte von "x" und "y" werden ebenfalls unter Verwendung der Tupel-Notation auf der Grundlage des erweiterten euklidischen Algorithmus aktualisiert.
        }
        return x < 0 ? x + m0 : x; // Schließlich gibt der Algorithmus "x" zurück, wenn er größer oder gleich 0 ist, oder "x" plus "m0", wenn er negativ ist.
                                   // Dadurch wird sichergestellt, dass das Ergebnis ein positiver Wert innerhalb des Bereichs des Modulus ist.
    }

}