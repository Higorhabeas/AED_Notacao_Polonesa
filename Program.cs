using System;
using System.ComponentModel.Design.Serialization;


class Program
{
    static void Main(string[] args)
    {   
        //Console.WriteLine("Digite a expressão que você deseja calcular: ");
        //string expressao = Console.ReadLine();
        string expressao = "(35+15)/(12-10)*2";
        /*Definindo um vetor que irá armazenar a expressao Polonesa reversa*/
        string[] vetorPolonesa = new string[expressao.Length];
        /*Lista para tratar cada elemento e string expressão*/
        List<string> elementos = SepararElementos(expressao);
        /*criando a pilha*/
        Stack<string> pilha = new Stack<string>();
        /*vetor de operadores*/
        string[] vetorOperadores = new string[expressao.Length];
        int indexPolonesa = -1;
        int indexOperadores = -1;
        /*lendo os elementos da lista baseada na expressão*/
        for (int i=0; i < elementos.Count; i++){
           
            //verificando se é número
            if( int.TryParse(elementos[i],out _)){
                indexPolonesa++;
                vetorPolonesa[indexPolonesa]=elementos[i];
                
            }else if(elementos[i] == "("){
                pilha.Push(elementos[i]);
                Console.WriteLine("Elemento empilhado:" + pilha.Peek());
            }
            else if( elementos[i] == "+" || elementos[i] == "-" || elementos[i] == "*" || elementos[i] == "/"){
                /*se for operador e a prioridade do topo da pilha for maior igual a prioridade 
                do elemento lido vou desenpilhar e na sequencio empilhar o operador*/
                while(pilha.Count !=0 && Prioridade(pilha.Peek()) >= Prioridade(elementos[i])){
                    indexOperadores++;
                    vetorOperadores[indexOperadores] = pilha.Pop(); 
                    indexPolonesa++;
                    vetorPolonesa[indexPolonesa] = vetorOperadores[indexOperadores];
                }
                pilha.Push(elementos[i]);
                Console.WriteLine("Elemento empilhado:" + pilha.Peek());

            }else if(elementos[i] == ")"){
                /*encontrado o fechamento de parenteses seginifica que tem que desempilhar até 
                achar o outro '('*/
                while(pilha.Count != 0 && pilha.Peek() != "("){
                    
                        indexOperadores++;
                        vetorOperadores[indexOperadores] = pilha.Pop(); 
                        indexPolonesa++;
                        vetorPolonesa[indexPolonesa] = vetorOperadores[indexOperadores];
                     
                }
                if (pilha.Peek()=="("){
                    pilha.Pop();
                }    
            }  
            
        }
        /*descarregando o restante dos operadores da pilha no vetor de notação Polonesa*/
        while( pilha.Count != 0){
            if(pilha.Peek() =="("){
                pilha.Pop();
            }else{
                indexOperadores++;
                vetorOperadores[indexOperadores] = pilha.Pop(); 
                indexPolonesa++;
                vetorPolonesa[indexPolonesa] = vetorOperadores[indexOperadores];
            }
            
        }        

        pilha.Clear();
        /*lendo a notação polonesa e fazendo as oparções*/
        for (int j=0; j < vetorPolonesa.Length; j++){ 

            /*se for número empilha*/
            if(float.TryParse(vetorPolonesa[j],out _)){

                pilha.Push(vetorPolonesa[j]);

            }else{
                /*verificando se a pilha está vazia*/
                if (pilha.Count ==0)
                {
                    Console.WriteLine("Pilha vazia");
                }else{
                    float y = float.Parse(pilha.Pop());

                    if(pilha.Count ==0 ){
                        Console.WriteLine("Pilha vazia");
                    }else{
                        float x = float.Parse(pilha.Pop());

                        switch (vetorPolonesa[j])
                        {   
                            case "+": 
                                pilha.Push(Convert.ToString(x + y));
                                break;
                            case "-":
                                pilha.Push(Convert.ToString(x-y));
                                break;
                            case "*":
                                pilha.Push(Convert.ToString(x*y));
                                break;
                            case "/":
                                pilha.Push(Convert.ToString(x/y));
                                break;

                            default:
                                Console.WriteLine("Número inválido!");
                                break;
                        }
                        Console.WriteLine("O resultado da expressão digitada é :" + pilha.Peek());
                    }
                }   
            }   
        }   
        
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

        foreach (char caractere in expressao)
        {
            /*verificando se é número*/
            if (char.IsDigit(caractere))
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

        // Adiciona o último número à lista, se não estiver vazio
        if (!string.IsNullOrEmpty(numeroAtual))
        {
            elementos.Add(numeroAtual);
        }

        return elementos;
    }

}

