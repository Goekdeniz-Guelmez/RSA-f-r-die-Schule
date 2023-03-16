using System.Numerics;
using System.Security.Cryptography;

namespace RSAAlgorithm;

class PrimeRandNume {
    public BigInteger generate(int size)
    {
        while (true)
        {
            int bitlen = size;
            RandomBigIntegerGenerator RBI = new RandomBigIntegerGenerator(); 
            BigInteger RandomNumber = RBI.NextBigInteger(bitlen); // Dies erzeugt ein zufälliges BigInteger-Objekt
                                                                  // mit "bitlen" Bits unter Verwendung der "NextBigInteger" Methode
                                                                  // des in der vorherigen Zeile erstellten RandomBigIntegerGenerator-Objekts.

            BigIntegerPrimeTest BIPT = new BigIntegerPrimeTest();

            // Diese prüft, ob RandomNumber eine wahrscheinliche Primzahl ist,
            // indem sie die "IsProbablePrime" Methode des "BIPT" Objekts mit Gewissheitsgrad "10" verwendet.
            // Wenn RandomNumber eine wahrscheinliche Primzahl ist,
            // dann wird sie von der "generate" Methode mit der "return" Anweisung zurückgegeben.
            if (BIPT.IsProbablePrime(RandomNumber, 10) == true)
            {
                return RandomNumber;

            }
            // Ist die if-Bedingung nicht erfüllt, wird die Schleife fortgesetzt und eine neue Zufallszahl erzeugt,
            // bis eine wahrscheinliche Primzahl gefunden ist.
        }
    }
}
    

// hier wird ein neuer BigInteder generiert
class RandomBigIntegerGenerator
{
    public BigInteger NextBigInteger(int bitLength) // nimmt einen bitLength-Parameter an und gibt einen BigInteger zurück.
    {
        if (bitLength < 1) return BigInteger.Zero; // wenn bitLength ist wehniger als 1, gib BigInteger als 0 zurück

        // Dies berechnet die Anzahl der Bytes und die Anzahl der verbleibenden Bits,
        // die erforderlich sind, um eine BigInteger-Zahl mit der angegebenen "BitLength" zu erzeugen.
        int bytes = bitLength / 8;
        int bits = bitLength % 8;

        // ein Byte-Array mit einer Länge wird erstellt,
        // die der Anzahl der Bytes entspricht, die benötigt werden,
        // um eine BigInteger-Zahl mit der angegebenen Länge zu erzeugen, plus ein zusätzliches Byte.
        // Also generiert es genug bytes um die bits abzu decken
        Random rnd = new Random();
        byte[] bs = new byte[bytes + 1];
        rnd.NextBytes(bs);

        // es wird eine Maske erstellt,
        // um die zusätzlichen Bits zu eliminieren, die durch das letzte Byte erzeugt werden.
        // Anschließend wird die Maske mit einer bitweisen UND-Operation auf das letzte Byte des Byte-Arrays angewendet.
        byte mask = (byte)(0xFF >> (8 - bits));
        bs[bs.Length - 1] &= mask;

        return new BigInteger(bs); // Damit wird ein neues BigInteger-Objekt mit dem zuvor erzeugten Byte-Array erstellt und zurückgegeben. Der resultierende BigInteger hat die angegebene Anzahl von Bits.
    }


}


// prüft, ob eine gegebene BigInteger-Zahl eine wahrscheinliche Primzahl ist oder nicht, und zwar mit einem bestimmten Grad an Gewissheit.
class BigIntegerPrimeTest
{
    public bool IsProbablePrime(BigInteger source, int certainty)
    {
        // Diese Zeilen prüfen, ob die source gleich 2 oder 3 ist,
        // in diesem Fall wird true zurückgegeben,
        // da es sich um Primzahlen handelt,
        // oder ob die Quelle kleiner als 2 oder eine gerade Zahl ist,
        // in diesem Fall wird false zurückgegeben, da es sich nicht um Primzahlen handelt.
        if (source == 2 || source == 3)
            return true;
        if (source < 2 || source % 2 == 0)
            return false;

        // Diese Zeilen berechnen die Werte von "d" und "s", wobei "d" ein solcher Wert ist,
        // dass Quelle-1 als 2^s * d geschrieben werden kann,
        // und "s" die größte nichtnegative ganze Zahl ist, so dass "d" eine ungerade Zahl ist.
        BigInteger d = source - 1;
        int s = 0;

        while (d % 2 == 0)
        {
            d /= 2;
            s += 1;
        }

        // There is no built-in method for generating random BigInteger values.
        // Instead, random BigIntegers are constructed from randomly generated
        // byte arrays of the same length as the source.

        // Diese Zeilen erstellen ein RandomNumberGenerator Objekt namens "rng" und ein Byte-Array namens "bytes",
        // das die gleiche Länge hat wie die Byte-Array-Darstellung von "source".
        // Anschließend werden in einer do-while-Schleife zufällige BigInteger-Werte im Bereich [2, source - 2] erzeugt,
        // bis ein geeignetes "a" gefunden wird. (hab ich vom internet koppiert)
        RandomNumberGenerator rng = RandomNumberGenerator.Create();
        byte[] bytes = new byte[source.ToByteArray().LongLength];
        BigInteger a;

        for (int i = 0; i < certainty; i++)
        {
            do
            {
                // This may raise an exception in Mono 2.10.8 and earlier.
                rng.GetBytes(bytes);
                a = new BigInteger(bytes);
            }
            while (a < 2 || a >= source - 2);

            // Diese Zeilen führen den Miller-Rabin-Primatitätstest für die gegebene Quelle unter Verwendung der zufällig erzeugten "a", "d" und "s" durch.
            // Es wird geprüft, ob a^d mod source gleich 1 oder source - 1 ist, und wenn nicht,
            // wird wiederholt x quadriert und geprüft, ob es gleich source - 1 ist oder nicht,
            // bis die Schleife endet oder festgestellt wird, dass x nicht gleich source - 1 ist.
            // Wenn x am Ende der Schleife nicht gleich source - 1 ist, wird false zurückgegeben,
            // da die angegebene source keine wahrscheinliche Primzahl ist.
            BigInteger x = BigInteger.ModPow(a, d, source);
            if (x == 1 || x == source - 1)
                continue;

            for (int r = 1; r < s; r++)
            {
                x = BigInteger.ModPow(x, 2, source);
                if (x == 1)
                    return false;
                if (x == source - 1)
                    break;
            }

            if (x != source - 1)
                return false;
        }

        return true; // und, wenn die Tests certainty Anzahl von times durch kommt, dan gibt es wahr aus
    }
}