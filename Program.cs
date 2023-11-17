using System;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;


class Program
{
    static void Main(string[] args)
    {
        string res = "s";
        do {
            Console.WriteLine("Digite a expressão que você deseja calcular: ");
            string expressao = Console.ReadLine();
            //string expressao = "(35+15)/(12-10)*2";
            /*Definindo um vetor que irá armazenar a expressao Polonesa reversa*/
            bool flag = true;
            string[] vetorPolonesa = new string[expressao.Length];
            /*Lista para tratar cada elemento e string expressão*/
            foreach (char valor in expressao)
            {
                if (!(char.IsDigit(valor) || valor == '(' || valor == ')' || valor == '+' || valor == '-' || valor == '*' || valor == '/' || valor == '.'))
                {
                    flag = false;
                }
            }
            if (flag)
            {
                List<string> elementos = SepararElementos(expressao);
                /*criando a pilha*/
                Stack<string> pilha = new Stack<string>();
                Stack<float> pilhaValores = new Stack<float>();
                /*vetor de operadores*/
                string[] vetorOperadores = new string[expressao.Length];
                int indexPolonesa = -1;
                int indexOperadores = -1;
                /*lendo os elementos da lista baseada na expressão*/
                for (int i = 0; i < elementos.Count; i++)
                {
                    //Console.WriteLine("Elemento: " + elementos[i]);
                    //verificando se é número
                    if (float.TryParse(elementos[i], out _))
                    {
                        indexPolonesa++;
                        vetorPolonesa[indexPolonesa] = elementos[i];

                    }
                    else if (elementos[i] == "(")
                    {
                        pilha.Push(elementos[i]);
                        Console.WriteLine("Elemento empilhado:" + pilha.Peek());
                    }
                    else if (elementos[i] == "+" || elementos[i] == "-" || elementos[i] == "*" || elementos[i] == "/")
                    {
                        /*se for operador e a prioridade do topo da pilha for maior igual a prioridade 
                        do elemento lido vou desenpilhar e na sequencio empilhar o operador*/
                        while (pilha.Count != 0 && Prioridade(pilha.Peek()) >= Prioridade(elementos[i]))
                        {
                            indexOperadores++;
                            vetorOperadores[indexOperadores] = pilha.Pop();
                            indexPolonesa++;
                            vetorPolonesa[indexPolonesa] = vetorOperadores[indexOperadores];
                        }
                        pilha.Push(elementos[i]);
                        Console.WriteLine("Elemento empilhado:" + pilha.Peek());

                    }
                    else if (elementos[i] == ")")
                    {
                        /*encontrado o fechamento de parenteses seginifica que tem que desempilhar até 
                        achar o outro '('*/
                        while (pilha.Count != 0 && pilha.Peek() != "(")
                        {

                            indexOperadores++;
                            vetorOperadores[indexOperadores] = pilha.Pop();
                            indexPolonesa++;
                            vetorPolonesa[indexPolonesa] = vetorOperadores[indexOperadores];

                        }
                        if (pilha.Peek() == "(")
                        {
                            pilha.Pop();
                        }
                    }

                }
                /*descarregando o restante dos operadores da pilha no vetor de notação Polonesa*/
                while (pilha.Count != 0)
                {
                    if (pilha.Peek() == "(")
                    {
                        pilha.Pop();
                    }
                    else
                    {
                        indexOperadores++;
                        vetorOperadores[indexOperadores] = pilha.Pop();
                        indexPolonesa++;
                        vetorPolonesa[indexPolonesa] = vetorOperadores[indexOperadores];
                    }

                }
                Console.WriteLine("Expressão polonesa: ");
                for(int i = 0; i < vetorPolonesa.Length; i++)
                {
                    Console.Write(" "+vetorPolonesa[i]);
                }
                
                pilha.Clear();
                /*lendo a notação polonesa e fazendo as oparções*/
                for (int j = 0; j < vetorPolonesa.Length; j++)
                {
                    
                    CultureInfo culture = CultureInfo.InvariantCulture;
                    /*se for número empilha*/
                    if (float.TryParse(vetorPolonesa[j], out _))
                    {

                        pilhaValores.Push(float.Parse(vetorPolonesa[j],culture));

                    }
                    else
                    {
                        /*verificando se a pilha está vazia*/
                        if (pilhaValores.Count == 0)
                        {
                            Console.WriteLine(" ");
                        }
                        else
                        {
                            float y = 0;
                            float x = 0;
                                //Console.WriteLine("\nConsultando antes de fazer operação y : " + pilhaValores.Peek());
                                y = pilhaValores.Pop();

                            
                            if (pilhaValores.Count == 0)
                            {
                                Console.WriteLine(" ");
                            }
                            else
                            {
                                //Console.WriteLine("Consultando antes de fazer operação x : " + pilhaValores.Peek());
                                x = pilhaValores.Pop();
                                

                                switch (vetorPolonesa[j])
                                {
                                    case "+":
                                        pilhaValores.Push(x + y);                                        
                                        break;
                                    case "-":
                                        pilhaValores.Push(x - y);                                        
                                        break;
                                    case "*":
                                        pilhaValores.Push(x * y);                                        
                                        break;
                                    case "/":
                                        pilhaValores.Push(x / y);                                        
                                        break;

                                    default:
                                        Console.WriteLine("Número inválido!");
                                        break;
                                }
                                Console.WriteLine("\nO resultado da operação empilhada é :" + pilhaValores.Peek());
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Digite uma expressão válida!");
            }
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("Deseja digitar uma nova expressao? s/n");
            res = Console.ReadLine();
        } while (res == "s" || res == "S");
        
    }
    static int Prioridade(string elemento){
        int retorno=0;
        if (elemento =="("){
            retorno=1;
        }else if(elemento =="+" || elemento =="-"){
            retorno =2;
        }else if(elemento == "*" || elemento == "/"){
            retorno=3;
        }
        return retorno;       
    }
    

     static List<string> SepararElementos(string expressao)
    {
        List<string> elementos = new List<string>();
        string numeroAtual = "";
        bool primeiroCiclo = true;
        

        foreach (char caractere in expressao)
        {
            if(primeiroCiclo)
            {
                //tratando número negativo
                if(caractere == '-')
                {
                    numeroAtual += caractere;
                    primeiroCiclo = false;
                }else{
                    Console.WriteLine(caractere);
                    /*verificando se é número*/
                    if (char.IsDigit(caractere) || caractere == '.')
                    {
                        numeroAtual += caractere;
                    }
                    else if (caractere == '+' || caractere == '-' || caractere == '*' || caractere == '/' || caractere == '(' || caractere == ')')
                    {
                        /*Adiciona o número atual à lista, se não estiver vazio*/
                        if (!string.IsNullOrEmpty(numeroAtual))
                        {
                            elementos.Add(numeroAtual);
                            numeroAtual = ""; // Reinicia o número atual
                        }

                        // Adiciona o operador ou parêntese à lista
                        elementos.Add(caractere.ToString());
                    }

                }
                
            }else{
                if (char.IsDigit(caractere) || caractere == '.')
                    {
                        numeroAtual += caractere;
                    }
                    else if (caractere == '+' || caractere == '-' || caractere == '*' || caractere == '/' || caractere == '(' || caractere == ')')
                    {
                        /*Adiciona o número atual à lista, se não estiver vazio*/
                        if (!string.IsNullOrEmpty(numeroAtual))
                        {
                            elementos.Add(numeroAtual);
                            numeroAtual = ""; // Reinicia o número atual
                        }

                        // Adiciona o operador ou parêntese à lista
                        elementos.Add(caractere.ToString());
                    }
            }
            
        }

        // Adiciona o último número à lista, se não estiver vazio
        if (!string.IsNullOrEmpty(numeroAtual))
        {
            elementos.Add(numeroAtual);
        }

        return elementos;
    }

}

