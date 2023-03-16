using System.Text;
using System.Numerics;

namespace RSAAlgorithm
{
    internal class EncryptDecrypt
    {
        private List<BigInteger> Tochunks(string s, BigInteger kn) // nimmt die zwei Parameter ann
                                                                   // eine Zeichenkette "s" und eine BigInteger "kn".
                                                                   // Die Methode teilt die Zeichenkette "s" in BigInteger-Blöcke auf,
                                                                   // deren maximaler Wert "kn" ist.
        {

            // Die Methode initialisiert zwei Variablen "chunck" und "chunck_", die den aktuellen Ziffernchunk bzw.
            // eine Liste aller Chunks speichern.
            string chunck = "";
            List<string> chunck_ = new List<string>();

            // Die Methode durchläuft jedes Zeichen der Eingabezeichenkette "s" in einer Schleife und fügt jedes Zeichen dem aktuellen Chunk "chunck" hinzu.
            for (int i = 0; i < s.Length; i++)
            {
                chunck += s[i];

                // Die Methode prüft, ob der aktuelle Chunk "chunck" plus das nächste Zeichen in "s" einen BigInteger größer oder gleich "kn" bilden.
                // Wenn die Bedingung erfüllt ist, ist der aktuelle Chunk vollständig und muss zur Liste der Chunks "chunck_" hinzugefügt werden.
                if (i + 1 != s.Length)
                {
                    string next = chunck + s[i + 1];
                    if (BigInteger.Parse(next) >= kn)
                    {

                        // Wenn das nächste Zeichen in "s" 0 ist, findet der Code das letzte Nicht-Null-Zeichen im aktuellen Chunk und teilt den Chunk vor diesem Nicht-Null-Zeichen auf.
                        // Der Chunk vor dem Nicht-Null-Zeichen wird der Liste der Chunks "chunck_" hinzugefügt, und der verbleibende Chunk nach dem Nicht-Null-Zeichen wird in "chunck" gespeichert.
                        if (s[i + 1] == '0')
                        {

                            // find the first non zero 
                            // sting c is x000.. 
                            int index = chunck.Length - 1;
                            for (int j = chunck.Length - 1; j > 0; j--)
                            {
                                if (chunck[j] != '0')
                                {
                                    index = j;
                                    break;
                                }
                            }
                            string c = "";
                            for (int j = index; j < chunck.Length; j++)
                            {
                                c += chunck[j];
                            }

                            chunck = chunck.Remove(index);

                          
                            chunck_.Add(chunck);
                            chunck = "" + c;

                        }

                        // Wenn das aktuelle Zeichen in "s" das letzte Zeichen ist oder der aktuelle Chunk plus das nächste Zeichen in "s" eine BigInteger-Zahl kleiner als "kn" bilden,
                        // ist der aktuelle Chunk vollständig, und er muss der Liste der Chunks "chunck_" hinzugefügt werden.
                        else
                        {
                            
                            chunck_.Add(chunck);
                            chunck = "";
                        }

                    }
                }
                else
                {
             
                    chunck_.Add(chunck);
                    chunck = "";
                }

            }

            // Schließlich wandelt die Methode jedes Ziffernpaket aus der List<string> chunck_ in eine BigInteger um und gibt eine List<BigInteger> zurück, die alle Pakete enthält.
            List<BigInteger> list = new List<BigInteger>();
            for (int i = 0; i < chunck_.Count; i++)
            {
                list.Add(BigInteger.Parse(chunck_[i]));
            }
            return list;
        }

        //  hier wird encrypted
        public List<BigInteger> EncryptString(string message, BigInteger e, BigInteger n)
        {

            byte[] byt = Encoding.ASCII.GetBytes(message);// konvertiert die "message" in ein array als ASCII
            BigInteger number = new BigInteger(byt); // konvertirt byt array in ein großen BigInteger
            message = number.ToString(); // konvertiert BigInteger zurück in String und weist sie der Nachricht zu.
            Console.WriteLine("\necrypted");
            Console.WriteLine(message);// gibt text aus

            List<BigInteger> m =this.Tochunks(message, n); // konvertiert die Klartextnachricht in eine Liste von BigInteger um, wobei jeder BigInteger kleiner oder gleich "n" ist.
                                                           // Dies geschieht mit der Methode Tochunks (anfang).

            List<BigInteger> cypher = new List<BigInteger>(); // initialisiert leere Liste, die die verschlüsselte Nachricht enthält.

            for (int i = 0; i < m.Count; i++) // Die for-Schleife durchläuft jede BigInteger-Zahl in "m" und verschlüsselt sie mit dem öffentlichen Schlüssel "e" und dem Modulus "n".
                                              // Der verschlüsselte Wert wird dann der "cypher" hinzugefügt.
            {
     
                BigInteger result = BigInteger.ModPow(m[i], e, n);  // "n" verschlüsselt die BigInteger-Zahl m[i] mit der Formel C = m^e mod n, "C" ist der verschlüsselte Wert,
                                                                    // "m" der Klartextwert,
                                                                    // "e" der Exponent des öffentlichen Schlüssels und
                                                                    // "n" der Modulus ist.
                cypher.Add(result); // fügt die verschlüsselte BigInteger-Zahl der "cypher" liste hinzu.
            }
       
            return cypher; // gibt die "cypher" liste aus
        }

        // hier wird decrypted
        // gibt eine Zeichenkette zurück, die die entschlüsselte Nachricht darstellt.
        // Hier wird eine neue Listennachricht erstellt, um die entschlüsselte Nachricht zu speichern.
        public string DecryptString(List<BigInteger> cypher, BigInteger d, BigInteger n)
        {
            List<BigInteger> message = new List<BigInteger>();

            // Diese Schleife durchläuft jedes Element der eingegebenen "cypher" liste und entschlüsselt es mit dem privaten Schlüssel "d" und "n".
            // Das Ergebnis jeder Entschlüsselung wird in der Nachrichtenliste gespeichert.
            for (int i = 0; i < cypher.Count; i++)
            {
                BigInteger result = BigInteger.ModPow(cypher[i], d, n); /// M = c^d mod n
                message.Add(result);
            }

            // wandelt die entschlüsselte Nachricht aus einer Liste von BigIntegern in eine Zeichenkette um.
            // Jedes Element der Nachrichtenliste wird an die Zeichenkette angehängt.
            string plain ="";
            for (int i = 0; i < message.Count; i++)
            {
                plain += message[i].ToString();
            }

            // wandelt die Zeichenkette plain mit der Methode "ToByteArray()" der Klasse BigInteger in ein Array von Bytes um.
            // Anschließend wird eine neue Instanz der Klasse "ASCIIEncoding" erstellt und die Methode "GetString()" verwendet,
            // um das Byte-Array wieder in eine Zeichenkette umzuwandeln.
            // Die resultierende Zeichenkette stellt die entschlüsselte Nachricht dar und wird von der Methode zurückgegeben.
            byte[] bytes = BigInteger.Parse(plain).ToByteArray();
            ASCIIEncoding ascii = new ASCIIEncoding();
            plain = ascii.GetString(bytes);
            return plain;
        }

        public List<BigInteger> Encrypt(BigInteger message, BigInteger e, BigInteger n)
        {
            string plain = message.ToString(); // wandelt die Nachricht in ein string um und speichert sie in der Variablen "plain".
            Console.WriteLine("plain ");
            Console.WriteLine(plain); // gibt aus

            List<BigInteger> chunks = new List<BigInteger>(); // neue leere liste namens "chucks"
            chunks = this.Tochunks(plain, n); // ruft die Methode "Tochunks" mit den Parametern "plain" und "n" auf,
                                              // um die Nachricht in Stücke geeigneter Größe zu zerlegen,
                                              // die verschlüsselt werden sollen.
                                              // output von "Tochuncks" ist in "chunks"

            List<BigInteger> cypher = new List<BigInteger>(); // neue leere liste namens "cypher"
            for (int i = 0; i < chunks.Count; i++) // durchläuft die Chunks-Liste.
            {

                BigInteger result = BigInteger.ModPow(chunks[i], e, n);  // Innerhalb der Schleife wird jedes Chunk mit der Methode BigInteger.
                                                                         // ModPow verschlüsselt, die die modulare Potenzierung des Chunk,
                                                                         // den Verschlüsselungsschlüssel "e" und den Modulus "n" berechnet (C = m^e mod n).
                cypher.Add(result); // der result kommt in die liste
            }
            Console.WriteLine("//////////////////");
            return cypher; // gibt die liste mit den encrypted chunks aus.
        }

        // jetzt gibt es aus
        public string Decrypt(List<BigInteger> cypher, BigInteger d, BigInteger n)
        {
            string message=""; // In dieser Zeile wird eine neues leeres String namens "message" deklariert,
                               // die zum Speichern der entschlüsselten Nachricht verwendet wird.

            for (int i = 0; i < cypher.Count; i++) // läuft jedes Element im string durch.
            {
                BigInteger result = BigInteger.ModPow(cypher[i], d, n); // berechnet das Ergebnis der Entschlüsselungsoperation mit Hilfe der "ModPow" Methode (M = c^d mod n).
                                                                        // erhöht den Wert von cypher[i] auf die Potenz von "d" und nimmt dann das Ergebnis modulo "n".
                message +=result.ToString(); // fügt den entschlüsselten Wert als String in die Nachrichtenvariable ein.
            }
            return message; // gibt aus
        }
    }
}
