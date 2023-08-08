using System;
using System.IO;
using System.Text;

public class Program
{
    const int ALPH_LEN = 26; // Define uma constante com o numero de letras no alfabeto
    // Define o Alfabeto em um vetor de char
    static char[] ALPHABET = new char[ALPH_LEN] { 'A', 'B', 'C', 'D',
        'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O',
        'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
    };

    static void Main(string[] args)
    {
        var rot = 12; // Rotacao escolhida para a cifra [negativa - vai para esquerda no alfabeto e positiva para direita]
        var filePath = ""; // Variavel do caminho do arquivo a ser Criptografado/Descriptografado
        StreamReader? file = null;

        try 
        {
            //Solicita o arquivo ao cliente e verifica se o mesmo existe
            Console.WriteLine("Seja bem vindo ao programa da Cifra de Cesar!!!");
            Console.Write("Informe o arquivo a ser alterado: ");
            filePath = Console.ReadLine();

            if(!File.Exists(filePath))
            {
                throw new Exception("Erro - Caminho nao encontrado");
            }

            file = new StreamReader(filePath); //Abre arquivo para leitura

            // Define se arquivo sera criptografado ou descriptografado
            Console.WriteLine("Digite [ C ] caso deseje criptografar o arquivo ou [ D ] caso queira descriptografar a cifra!");
            var opt = char.ToUpper((char) Console.Read());
            while(opt != 'C' && opt != 'D')
            {
                Console.WriteLine("Entrada invalida, digite [ C ] caso deseje criptografar o arquivo ou [ D ] caso queira descriptografa-lo! ");
                opt = char.ToUpper((char) Console.Read());
            }

            // Gera novo arquivo
            var splittedFilePath = filePath.Split(@"\");
            splittedFilePath[splittedFilePath.GetUpperBound(0)] = splittedFilePath[splittedFilePath.GetUpperBound(0)].Replace(".txt", String.Format("-{0}.txt", opt == 'C' ? "CRIPT" : "DEC"));
            var newFilePath = string.Join(@"\", splittedFilePath);
            File.Create(newFilePath).Close();

            var newFile = new StreamWriter(newFilePath);
            var line = file.ReadLine();

            // Le linha por linha do arquivo base e gera a linha coincidente cirptografada ou descriptografada no novo arquivo
            while(line != null)
            {
                var newLine = new StringBuilder();
                foreach(var c in line)
                {
                    var actualChar = opt == 'C'? EncryptChar(rot, c) : DecryptChar(rot, c);
                    newLine.Append(actualChar);
                }
                newFile.WriteLine(newLine);
                line = file.ReadLine();
            }

            newFile.Close();
            Console.WriteLine(String.Format("Arquivo gerado com sucesso, segue caminho para o seu novo arquivo apos a {0}: {1}"
                , opt == 'C' ? "criptografia" : "descriptografia",
                newFilePath));
            
        }
        catch (Exception ex) //Trata as excecoes
        {
            Console.Error.WriteLine(ex.Message);
            Console.Error.WriteLine("Finalizando programa devido ao erro, tente novamente mais tarde!");
        }
        finally 
        { 
            if(file != null)
            {
                file.Close();
            }
        }

        Console.WriteLine("FIM DO PROGRAMA");
    }

    // Funcao que recebe o "coeficiente de rotacao" da cifra e um caracter e o retorna criptografado
    static char EncryptChar(int rot, char cur)
    {
        if(char.IsLetter(cur))
        {
            var lower = char.IsLower(cur);

            cur = char.ToUpper(cur);

            var rotPos = Array.IndexOf(ALPHABET, cur) + rot;

            if (rotPos > ALPH_LEN - 1 )
            {
                rotPos -= ALPH_LEN;
            }

            if (rotPos < 0)
            {
                rotPos += ALPH_LEN;
            }

            cur = ALPHABET[rotPos];

            if(lower)
            {
                cur = char.ToLower(cur);
            }
        }
            
        return cur;
    }

    // Funcao que recebe o "coeficiente de rotacao" da cifra e um caracter e o retorna descriptografado [Reversao da criptografia]
    static char DecryptChar(int rot, char cur) => EncryptChar(-rot, cur);
}