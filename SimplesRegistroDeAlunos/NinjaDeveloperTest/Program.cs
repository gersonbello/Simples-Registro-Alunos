using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NinjaDeveloperTest
{
    class Program
    {
        static List<AlunoRegistro> Alunos = new List<AlunoRegistro>();
        static List<MateriaRegistro> Materias = new List<MateriaRegistro>();

        static string pageName = "Cadastro de aluno";
        static string avisoDeFalha;
        static int colunaDePreenchimentoAtual;
        static int linhaAluno, navegacao;

        static string caminhoArquivoAluno = AppDomain.CurrentDomain.BaseDirectory + "Alunos.txt";
        static string caminhoArquivoMateria = AppDomain.CurrentDomain.BaseDirectory + "Materias.txt";

        static void Main(string[] args)
        {
            LerOuCriarArquivo();

        Menu:

            pageName = ("Menu");
            PaginaDeNavegacao();
            Console.WriteLine("01 - Cadastro de aluno\n02 - Cadastro de materia\n03 - Cadastro de notas\n04 - Visualizar notas");
            if (int.TryParse(Console.ReadLine(), out navegacao))
            {
                switch (navegacao)
                {
                    case 1: Console.Clear(); goto CadastroDeAluno;
                    case 2: Console.Clear(); goto CadastroDeMateria;
                    case 3: Console.Clear(); goto CadastroDeNota;
                    case 4: Console.Clear(); goto VisualizarNotas;
                }
            }

            Console.WriteLine("Insira um número válido de navegação");
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();
            goto Menu;

        CadastroDeAluno:
            Console.Clear();
            pageName = ("Cadastro de Aluno");
            colunaDePreenchimentoAtual = 0;
            AlunoRegistro novoAluno = new AlunoRegistro();
            string[] colunasAluno = { "Nome: ", "Sobrenome: ", "Data nascimento: ", "Cpf: ", "Curso: "};
            string[] preenchimentoAluno = { "Nome", "Sobrenome", "01/01/2000", "00000000000", "Curso" , "Materia", "Nota"};

            while (colunaDePreenchimentoAtual <= colunasAluno.Length)
            {
                PaginaDeNavegacao();

                for (int coluna = 0; coluna < colunaDePreenchimentoAtual; coluna++)
                {
                   if(coluna < colunasAluno.Length) Console.WriteLine(colunasAluno[coluna] + preenchimentoAluno[coluna]);
                }

                switch (colunaDePreenchimentoAtual)
                {
                    case 0:
                        Console.WriteLine("Preencha somente com letras | 01 - Voltar");
                        if (avisoDeFalha != null) Console.WriteLine(avisoDeFalha);
                        Console.Write(colunasAluno[0]);
                        string nome = Console.ReadLine();
                        if (int.TryParse(nome, out navegacao))
                        {
                            if (navegacao == 1) goto Menu;
                        }
                        if(!SomenteLetras(nome) || nome == "") avisoDeFalha = ("Insira somente caracteres válidos");
                        else
                        {
                            avisoDeFalha = null;
                            preenchimentoAluno[0] = nome;
                            colunaDePreenchimentoAtual++;
                        }
                        break;

                    case 1:
                        Console.WriteLine("Preencha com qualquer caracter | 01 - Voltar | 02 - Anterior");
                        Console.Write(colunasAluno[1]);
                        string sobrenome = Console.ReadLine();
                        if (int.TryParse(sobrenome, out navegacao))
                        {
                            if (navegacao == 2)
                            {
                                avisoDeFalha = null;
                                colunaDePreenchimentoAtual --;
                                break;
                            }
                            if (navegacao == 1) goto Menu;
                        }
                        if (sobrenome != "")
                        {
                            preenchimentoAluno[1] = sobrenome;
                            colunaDePreenchimentoAtual++;
                        }
                        else avisoDeFalha = ("Insira ao menos um caractere");
                        break;

                    case 2:
                        Console.WriteLine("Preencha no formato: dia/mês/ano | 01 - Voltar | 02 - Anterior");
                        if (avisoDeFalha != null) Console.WriteLine(avisoDeFalha);
                        Console.Write(colunasAluno[2]);
                        string data = Console.ReadLine();
                        if (int.TryParse(data, out navegacao))
                        {
                            if (navegacao == 2)
                            {
                                avisoDeFalha = null;
                                colunaDePreenchimentoAtual --;
                                break;
                            }
                            if (navegacao == 1) goto Menu;
                        }
                        if (SomenteDatas(data))
                        {
                            DateTime limite = new DateTime(2002, 01, 01);
                            if (DateTime.Parse(data) < limite)
                            {
                                avisoDeFalha = null;
                                preenchimentoAluno[2] = data;
                                colunaDePreenchimentoAtual++;
                            }
                            else avisoDeFalha = ("Datas devem ser anteriores à 01/01/2002");
                        } else avisoDeFalha = ("Insira uma data válida");
                        break;

                    case 3:
                        Console.WriteLine("Preencha somente com números | 01 - Voltar | 02 - Anterior");
                        if (avisoDeFalha != null) Console.WriteLine(avisoDeFalha);
                        Console.Write(colunasAluno[3]);
                        string cpf = Console.ReadLine();
                        if (int.TryParse(cpf, out navegacao))
                        {
                            if (navegacao == 2)
                            {
                                avisoDeFalha = null;
                                colunaDePreenchimentoAtual --;
                                break;
                            }
                            if (navegacao == 1) goto Menu;
                        }
                        if (SomenteNumeros(cpf))
                        {
                            char[] numeros = cpf.ToCharArray();
                            if (numeros.Length == 11)
                            {
                                bool checarRegistro = false;
                                List<string> alunos = File.ReadAllLines(caminhoArquivoAluno).ToList();
                                foreach (var linha in alunos)
                                {
                                    string[] entradas = linha.Split(',');
                                    if (cpf == entradas[3]) checarRegistro = true;
                                }
                                if (!checarRegistro)
                                {
                                    avisoDeFalha = null;
                                    preenchimentoAluno[3] = cpf;
                                    colunaDePreenchimentoAtual++;
                                }
                                else avisoDeFalha = ("Cpf já registrado");
                            } else avisoDeFalha = ("Insira todos e somente os digitos de cpf");
                        }
                        else avisoDeFalha = ("Insira um total de 11 digitos");
                        break;

                    case 4:
                        Console.WriteLine("Preencha com qualquer caracter | 01 - Voltar | 02 - Anterior");
                        Console.Write(colunasAluno[4]);
                        string curso = Console.ReadLine();
                        if (int.TryParse(curso, out navegacao))
                        {
                            if (navegacao == 2)
                            {
                                avisoDeFalha = null;
                                colunaDePreenchimentoAtual --;
                                break;
                            }
                            if (navegacao == 1) goto Menu;
                        }
                        if (curso != "")
                        {
                            preenchimentoAluno[4] = curso;
                            colunaDePreenchimentoAtual++;
                        } else avisoDeFalha = ("Insira ao menos um caractere");
                        break;

                    case 5:
                        Console.WriteLine();
                        LinhaDeSeparacao();
                        Console.WriteLine("01 - Voltar | 02 - Salvar | 03 - Excluir");
                        if (int.TryParse(Console.ReadLine(), out navegacao))
                        {
                            if (navegacao == 1) goto Menu;
                            if(navegacao == 2)
                            {
                                novoAluno.Nome = preenchimentoAluno[0];
                                novoAluno.Sobrenome = preenchimentoAluno[1];
                                novoAluno.DataDeNascimento = preenchimentoAluno[2];
                                novoAluno.Cpf = preenchimentoAluno[3];
                                novoAluno.Curso = preenchimentoAluno[4];
                                Alunos.Add(novoAluno);
                                SalvarAlunos();
                                PaginaDeNavegacao();
                                Console.WriteLine("Registro de aluno salvo");
                                Console.WriteLine("\nPressione qualquer tecla para continuar....");
                                Console.ReadKey();
                                goto Menu;
                            }
                            if(navegacao == 3)
                            {
                                preenchimentoAluno = null;
                                PaginaDeNavegacao();
                                Console.WriteLine("Registro excluido");

                                Console.WriteLine("\nPressione qualquer tecla para continuar....");
                                Console.ReadKey();
                                goto Menu;
                            }
                        }
                        break;
                }
            }

        CadastroDeMateria:
            Console.Clear();
            pageName = ("Cadastro de Materia");
            colunaDePreenchimentoAtual = 0;
            MateriaRegistro novaMateria = new MateriaRegistro();
            string[] colunasMateria = { "Descrição: ", "Data cadastro: ","Situação: "};
            string[] preenchimentoMateria = { "Nome", "01/01/2000", "Ativo"};

            while (colunaDePreenchimentoAtual <= colunasMateria.Length)
            {
                PaginaDeNavegacao();

                for (int coluna = 0; coluna < colunaDePreenchimentoAtual; coluna++)
                {
                    if (coluna < colunasMateria.Length) Console.WriteLine(colunasMateria[coluna] + preenchimentoMateria[coluna]);
                }

                switch (colunaDePreenchimentoAtual)
                {
                    case 0:
                        Console.WriteLine("Preencha somente com letras | 01 - Voltar");
                        if (avisoDeFalha != null) Console.WriteLine(avisoDeFalha);
                        Console.Write(colunasMateria[0]);
                        string descricao = Console.ReadLine();
                        if (int.TryParse(descricao, out navegacao))
                        {
                            if (navegacao == 1) goto Menu;
                        }
                        if (!SomenteLetras(descricao) || descricao == "") avisoDeFalha = ("Insira somente caracteres válidos");
                        else
                        {
                            bool checarRegistro = false;
                            List<string> materias = File.ReadAllLines(caminhoArquivoMateria).ToList();
                            foreach (var linha in materias)
                            {
                                string[] entradas = linha.Split(',');
                                if (descricao == entradas[0]) checarRegistro = true;
                            }
                            if (!checarRegistro)
                            {
                                avisoDeFalha = null;
                                preenchimentoMateria[0] = descricao;
                                colunaDePreenchimentoAtual++;
                            }
                            else avisoDeFalha = ("Matéria já resgistrada");
                        }
                        break;

                    case 1:
                        Console.WriteLine("Preencha no formato: dia/mês/ano | 01 - Voltar | 02 - Anterior");
                        if (avisoDeFalha != null) Console.WriteLine(avisoDeFalha);
                        Console.Write(colunasMateria[1]);
                        string data = Console.ReadLine();
                        if (int.TryParse(data, out navegacao))
                        {
                            if (navegacao == 2)
                            {
                                avisoDeFalha = null;
                                colunaDePreenchimentoAtual--;
                                break;
                            }
                            if (navegacao == 1) goto Menu;
                        }
                        if (SomenteDatas(data))
                        {
                            if (DateTime.Parse(data) <= DateTime.Today)
                            {
                                avisoDeFalha = null;
                                preenchimentoMateria[1] = data;
                                colunaDePreenchimentoAtual++;
                            }
                            else avisoDeFalha = ("Datas não devem exeder a de hoje: " + DateTime.Today.ToString("dd/MM/yyyy"));
                        }
                        else avisoDeFalha = ("Insira uma data válida");
                        break;

                    case 2:
                        Console.WriteLine("Preencha somente com ativo ou inativo | 01 - Voltar | 02 - Anterior");
                        if (avisoDeFalha != null) Console.WriteLine(avisoDeFalha);
                        Console.Write(colunasMateria[2]);
                        string situacao = Console.ReadLine();
                        situacao = situacao.ToLower();
                        if (int.TryParse(situacao, out navegacao))
                        {
                            if (navegacao == 2)
                            {
                                avisoDeFalha = null;
                                colunaDePreenchimentoAtual--;
                                break;
                            }
                            if (navegacao == 1) goto Menu;
                        }
                        if (situacao != "ativo" && situacao != "inativo") avisoDeFalha = ("Insira somente valores válidos");
                        else
                        {
                            avisoDeFalha = null;
                            preenchimentoMateria[2] = situacao;
                            colunaDePreenchimentoAtual++;
                        }
                        break;

                    case 3:
                        Console.WriteLine();
                        LinhaDeSeparacao();
                        Console.WriteLine("01 - Voltar | 02 - Salvar | 03 - Excluir");
                        if (int.TryParse(Console.ReadLine(), out navegacao))
                        {
                            if (navegacao == 1) goto Menu;
                            if (navegacao == 2)
                            {
                                novaMateria.descricao = preenchimentoMateria[0];
                                novaMateria.dataCadastro = preenchimentoMateria[1];
                                novaMateria.situacao = preenchimentoMateria[2];
                                Materias.Add(novaMateria);
                                SalvarMaterias();
                                PaginaDeNavegacao();
                                Console.WriteLine("Registro de materia salvo");
                                Console.WriteLine("\nPressione qualquer tecla para continuar....");
                                Console.ReadKey();
                                goto Menu;
                            }
                            if (navegacao == 3)
                            {
                                preenchimentoAluno = null;
                                PaginaDeNavegacao();
                                Console.WriteLine("Registro excluido");

                                Console.WriteLine("\nPressione qualquer tecla para continuar....");
                                Console.ReadKey();
                                goto Menu;
                            }
                        }
                        break;
                }
            }

        CadastroDeNota:
            Console.Clear();
            pageName = ("Cadastro de Nota");
            colunaDePreenchimentoAtual = 0;
            string[] colunasNota = { "Aluno: ", "Materia: ", "Nota: " };
            string[] preenchimentoNota = { "Nome", "Materia", "100" };
            int novaNota = 0;
            string materia = " ";

            while (colunaDePreenchimentoAtual <= colunasNota.Length)
            {
                int navegacao;
                PaginaDeNavegacao();

                for (int coluna = 0; coluna < colunaDePreenchimentoAtual; coluna++)
                {
                    if (coluna < colunasNota.Length) Console.WriteLine(colunasNota[coluna] + preenchimentoNota[coluna]);
                }
                switch (colunaDePreenchimentoAtual)
                {
                    case 0:
                        Console.WriteLine("Preencha com um aluno registrado usando nome e sobrenome | 01 - Voltar");
                        if (avisoDeFalha != null) Console.WriteLine(avisoDeFalha);
                        Console.Write(colunasNota[0]);
                        string nomeProcura = Console.ReadLine();
                        int[] lugaresNaLista = new int[Alunos.Count + 1];
                        bool encontrado = false;
                        int numeroDeResultados = 0;
                        if (int.TryParse(nomeProcura, out navegacao))
                        {
                            if (navegacao == 1) goto Menu;
                        }
                        for(int procura = 0; procura < Alunos.Count; procura++)
                        {
                            if (nomeProcura.ToLower() == Alunos[procura].Nome.ToLower() 
                                || nomeProcura.ToLower() == (Alunos[procura].Nome + " " + Alunos[procura].Sobrenome).ToLower())
                            {
                                numeroDeResultados++;
                                lugaresNaLista[numeroDeResultados] = procura;
                                encontrado = true;
                            }
                        }
                        if (!encontrado) avisoDeFalha = ("Aluno não encontrado");
                        else
                        {
                            if (numeroDeResultados == 1)
                            {
                                linhaAluno = lugaresNaLista[numeroDeResultados];
                                avisoDeFalha = null;
                                preenchimentoNota[0] = Alunos[linhaAluno].Nome + " " + Alunos[linhaAluno].Sobrenome;
                                colunaDePreenchimentoAtual++;
                            }
                            else
                            {
                                int lugarEscolhido;
                                bool escolhido = false;
                                while (!escolhido)
                                {
                                    int listCounter = 0;
                                    PaginaDeNavegacao();
                                    if (avisoDeFalha != null) Console.WriteLine(avisoDeFalha);
                                    Console.WriteLine("Diversos resultados para sua pesquisa, digite o número do aluno");
                                    for (int i = 1; i <= numeroDeResultados; i++)
                                    {
                                        AlunoRegistro esteAluno = Alunos[lugaresNaLista[i]];
                                        Console.WriteLine(i + " - " + esteAluno.Nome + " " + esteAluno.Sobrenome +
                                            " - Data nascimento:" + esteAluno.DataDeNascimento + " - Curso" + esteAluno.Curso);
                                        listCounter++;
                                    }
                                    string escolha = Console.ReadLine();
                                    if (int.TryParse(escolha, out lugarEscolhido))
                                    {
                                        if (lugarEscolhido <= listCounter && lugarEscolhido > 0)
                                        {
                                            linhaAluno = lugaresNaLista[lugarEscolhido];
                                            colunaDePreenchimentoAtual++;
                                            preenchimentoNota[0] = Alunos[linhaAluno].Nome + " " + Alunos[linhaAluno].Sobrenome;
                                            escolhido = true;
                                        }
                                        else avisoDeFalha = ("Escolha um valor correspondente à tabela");
                                    }
                                }
                            }
                        }
                        break;

                    case 1:
                        Console.WriteLine("Preencha com uma matéria registrada | 01 - Voltar | 02 - Anterior");
                        if (avisoDeFalha != null) Console.WriteLine(avisoDeFalha);
                        Console.Write(colunasNota[1]);
                        string materiaProcura = Console.ReadLine();
                        int lugaresNaListaDeMateria = 0;
                        bool encontradoMateria = false;
                        if (int.TryParse(materiaProcura, out navegacao))
                        {
                            if (navegacao == 2)
                            {
                                avisoDeFalha = null;
                                colunaDePreenchimentoAtual--;
                                break;
                            }
                            if (navegacao == 1) goto Menu;
                        }
                        if (!encontradoMateria)
                        {
                            for (int procura = 0; procura < Materias.Count; procura++)
                            {
                                if (materiaProcura.ToLower() == Materias[procura].descricao.ToLower())
                                {
                                    lugaresNaListaDeMateria = procura;
                                    encontradoMateria = true;
                                }
                            }
                        }
                        if (!encontradoMateria) avisoDeFalha = ("Matéria não encontrado");
                        else
                        {
                            avisoDeFalha = null;
                            preenchimentoNota[1] = Materias[lugaresNaListaDeMateria].descricao;
                            colunaDePreenchimentoAtual++;
                        }
                        materia = preenchimentoNota[1];
                        break;

                    case 2:
                        Console.WriteLine("Preencha somente com letras | 01 - Voltar | 02 - Anterior");
                        if (avisoDeFalha != null) Console.WriteLine(avisoDeFalha);
                        Console.Write(colunasNota[2]);
                        string nota = Console.ReadLine();
                        nota.ToLower();
                        if (int.TryParse(nota, out navegacao))
                        {
                            if (navegacao >= 0 && navegacao <= 100)
                            {
                                avisoDeFalha = null;
                                novaNota = int.Parse(nota);
                                preenchimentoNota[2] = nota;
                                colunaDePreenchimentoAtual++;
                            }
                            else avisoDeFalha = ("Insira um número válido");
                        }
                        else avisoDeFalha = ("Insira somente valores válidos: 0-100");
                        break;

                    case 3:
                        Console.WriteLine();
                        LinhaDeSeparacao();
                        Console.WriteLine("01 - Voltar | 02 - Salvar | 03 - Excluir");
                        if (int.TryParse(Console.ReadLine(), out navegacao))
                        {
                            if (navegacao == 1) goto Menu;
                            if (navegacao == 2)
                            {
                                SalvarNota(novaNota,linhaAluno,materia);
                                PaginaDeNavegacao();
                                Console.WriteLine("Registro de nota salvo");
                                Console.WriteLine("\nPressione qualquer tecla para continuar....");
                                Console.ReadKey();
                                goto Menu;
                            }
                            if (navegacao == 3)
                            {
                                preenchimentoAluno = null;
                                PaginaDeNavegacao();
                                Console.WriteLine("Registro excluido");

                                Console.WriteLine("\nPressione qualquer tecla para continuar....");
                                Console.ReadKey();
                                goto Menu;
                            }
                        }
                        break;
                }
            }

        VisualizarNotas:
            pageName = ("Visualização de notas do aluno");
            colunaDePreenchimentoAtual = 0;
            string[] preenchimentoVisualizador = { "Nome"};
            bool alunoEncontrado = false;

            while (!alunoEncontrado)
            {
                    PaginaDeNavegacao();

                    Console.WriteLine("Preencha com um aluno registrado usando nome e sobrenome | 01 - Voltar");
                    if (avisoDeFalha != null) Console.WriteLine(avisoDeFalha);
                    Console.Write("Aluno: ");
                    string nomeProcura = Console.ReadLine();
                    int[] lugaresNaLista = new int[Alunos.Count + 1];
                    bool encontrado = false;
                    int numeroDeResultados = 0;
                    if (int.TryParse(nomeProcura, out navegacao))
                    {
                        if (navegacao == 1) goto Menu;
                    }
                    for (int procura = 0; procura < Alunos.Count; procura++)
                    {
                        if (nomeProcura.ToLower() == Alunos[procura].Nome.ToLower()
                            || nomeProcura.ToLower() == (Alunos[procura].Nome + " " + Alunos[procura].Sobrenome).ToLower())
                        {
                            numeroDeResultados++;
                            lugaresNaLista[numeroDeResultados] = procura;
                            encontrado = true;
                        }
                    }
                    if (!encontrado) avisoDeFalha = ("Aluno não encontrado");
                    else
                    {
                        if (numeroDeResultados == 1)
                        {
                            linhaAluno = lugaresNaLista[numeroDeResultados];
                            avisoDeFalha = null;
                            preenchimentoVisualizador[0] = Alunos[linhaAluno].Nome + " " + Alunos[linhaAluno].Sobrenome;
                            alunoEncontrado = true;
                        }
                        else
                        {
                            int lugarEscolhido;
                            bool escolhido = false;
                            while (!escolhido)
                            {
                                PaginaDeNavegacao();
                            int listCounter = 0;
                                Console.WriteLine("Diversos resultados para sua pesquisa, digite o número do aluno");
                             if (avisoDeFalha != null) Console.WriteLine(avisoDeFalha);
                            for (int i = 1; i <= numeroDeResultados; i++)
                                {
                                    AlunoRegistro esteAluno = Alunos[lugaresNaLista[i]];
                                    Console.WriteLine(i + " - " + esteAluno.Nome + " " + esteAluno.Sobrenome +
                                        " - Data nascimento:" + esteAluno.DataDeNascimento + " - Curso: " + esteAluno.Curso);
                                    listCounter++;
                                }
                                string escolha = Console.ReadLine();
                                if (int.TryParse(escolha, out lugarEscolhido))
                                {
                                    if (lugarEscolhido <= listCounter && lugarEscolhido > 0)
                                    {
                                        linhaAluno = lugaresNaLista[lugarEscolhido];
                                        preenchimentoVisualizador[0] = Alunos[linhaAluno].Nome + " " + Alunos[linhaAluno].Sobrenome;
                                        escolhido = true;
                                        alunoEncontrado = true;
                                    }
                                    else avisoDeFalha = ("Escolha um valor correspondente à tabela");
                                }
                            }
                        }
                    }
            }
            PaginaDeNavegacao();
            Console.WriteLine("Aluno: " + preenchimentoVisualizador[0]);
            Console.WriteLine();
            LinhaDeSeparacao();
            Console.WriteLine("01 - Voltar | 02 - Visualizar");
            if (int.TryParse(Console.ReadLine(), out navegacao))
            {
                if (navegacao == 1) goto Menu;
                if (navegacao == 2) goto ResumoDoAluno;
            }
        ResumoDoAluno:
            while (true) {
                PaginaDeNavegacao();
                LinhaDeSeparacao();
                List<string> alunos = File.ReadAllLines(caminhoArquivoAluno).ToList();
                string[] entradas = alunos[linhaAluno].Split(',');
                Console.WriteLine("Aluno \n" + preenchimentoVisualizador[0]);
                LinhaDeSeparacao();
                Console.WriteLine();
                for (int i = 5; i < entradas.Length; i++)
                {
                    PalavraCentralizadaComLinhaPontilhada(entradas[i], 40);
                    i++;
                    Console.WriteLine("Nota: " + entradas[i] + "\n");
                }
                LinhaDeSeparacao();
                Console.WriteLine("01 - Voltar");
                if (int.TryParse(Console.ReadLine(), out navegacao)) if (navegacao == 1) goto Menu;
            }
        }
        static bool SomenteLetras(string letras)
        {
            bool teste = true;
            foreach (char letra in letras) if (!char.IsLetter(letra)) teste = false;
            return teste;
        }
        static bool SomenteDatas(string data)
        {
            DateTime newdata;
            bool teste = true;
            char[] charcheck = data.ToCharArray();
            if (!(charcheck.Length == 10 && DateTime.TryParse(data, out newdata))) teste = false;
            return teste;
        }
        static bool SomenteNumeros(string numeros)
        {
            bool teste = true;
            foreach (char numero in numeros) if (!char.IsDigit(numero)) teste = false;
            return teste;
        }
        static void PaginaDeNavegacao()
        {
            Console.Clear();
            Console.WriteLine("Universidade Ecológica do Sitio do Caqui");
            LinhaDeSeparacao();
            Console.WriteLine("\n{0}\n", pageName);
            LinhaDeSeparacao();
            Console.WriteLine();
        } 
        static void LinhaDeSeparacao()
        {
            Console.WriteLine("----------------------------------------");
        } 
        static void SalvarAlunos()
        {
            List<string> saveFile = new List<string>();
            foreach (var aluno in Alunos)
            {
                saveFile.Add($"{aluno.Nome},{aluno.Sobrenome},{aluno.DataDeNascimento},{aluno.Cpf},{aluno.Curso}");
            }
            File.WriteAllLines(caminhoArquivoAluno, saveFile);
        }
        static void SalvarMaterias()
        {
            List<string> saveFile = new List<string>();
            foreach (var materia in Materias)
            {
                saveFile.Add($"{materia.descricao},{materia.dataCadastro},{materia.situacao}");
            }
            File.WriteAllLines(caminhoArquivoMateria, saveFile);
        }
        static void SalvarNota(int nota, int alunoId, string materia)
        {
            List<string> alunos = File.ReadAllLines(caminhoArquivoAluno).ToList();
            List<string> saveFile = new List<string>();
            int linhaParaSobrepor = 0;
            foreach (var linha in alunos)
            {
                string[] entradas = linha.Split(',');

                AlunoRegistro alunoRegistrado = new AlunoRegistro();
                alunoRegistrado.Nome = entradas[0];
                alunoRegistrado.Sobrenome = "," + entradas[1];
                alunoRegistrado.DataDeNascimento = "," + entradas[2];
                alunoRegistrado.Cpf = "," + entradas[3];
                alunoRegistrado.Curso = "," + entradas[4];


                if (entradas.Length > 5)
                {
                    bool notaJaRegistrada = false;
                    for (int i = 5; i < entradas.Length; i++)
                    {
                        if (entradas[i] == materia && linhaParaSobrepor == alunoId)
                        {
                            int a = i + 1;
                            entradas[a] = nota.ToString();
                            notaJaRegistrada = true;
                        }
                    }
                    if (notaJaRegistrada)
                    {
                        saveFile.Add(String.Join(",", entradas));

                    }
                    else
                    {
                        if (linhaParaSobrepor == alunoId)
                        {
                            string notaMaisMateria = ($",{materia},{nota.ToString()}");
                            saveFile.Add(String.Join(",", entradas) + notaMaisMateria);
                        }
                        else
                        {
                            saveFile.Add(String.Join(",", entradas));
                        }
                    }
                }
                else
                {
                    if (linhaParaSobrepor == alunoId)
                    {
                        string notaMaisMateria = ($",{materia},{nota.ToString()}");
                        saveFile.Add(String.Join(",", entradas) + notaMaisMateria);
                    }
                    else
                    {
                        saveFile.Add(String.Join(",", entradas));
                    }

                }
                linhaParaSobrepor++;
            }
            File.WriteAllLines(caminhoArquivoAluno, saveFile);
        }
        static void LerOuCriarArquivo()
        {
            if (File.Exists(caminhoArquivoAluno))
            {
                List<string> alunos = File.ReadAllLines(caminhoArquivoAluno).ToList();
                foreach (var linha in alunos)
                {
                    AlunoRegistro alunoRegistrado = new AlunoRegistro();
                    string[] entradas = linha.Split(',');
                    if (entradas.Length > 1)
                    {

                        alunoRegistrado.Nome = entradas[0];
                        alunoRegistrado.Sobrenome = entradas[1];
                        alunoRegistrado.DataDeNascimento = entradas[2];
                        alunoRegistrado.Cpf = entradas[3];
                        alunoRegistrado.Curso = entradas[4];
                        if (entradas.Length > 5)
                        {
                            for (int i = 6; i < entradas.Length; i++)
                            {
                                alunoRegistrado.materiasENotas.Add(entradas[i]);
                            }
                        }

                    }
                    Alunos.Add(alunoRegistrado);
                }
            }
            else File.CreateText(caminhoArquivoAluno).Dispose();

            if (File.Exists(caminhoArquivoMateria))
            {
                List<string> alunos = File.ReadAllLines(caminhoArquivoMateria).ToList();
                foreach (var linha in alunos)
                {
                    string[] entradas = linha.Split(',');

                    MateriaRegistro materiaRegistrada = new MateriaRegistro();

                    materiaRegistrada.descricao = entradas[0];
                    materiaRegistrada.dataCadastro = entradas[1];
                    materiaRegistrada.situacao = entradas[2];

                    Materias.Add(materiaRegistrada);
                }
            }
            else
            {
                File.CreateText(caminhoArquivoMateria).Dispose();
            }

        }
        static void PalavraCentralizadaComLinhaPontilhada(string palavra, int tamanhoLinha)
        {
            char[] linha = new char[tamanhoLinha];
            char[] letras = palavra.ToCharArray();
            int inicioPosicionamento = linha.Length/2 - letras.Length/ 2;
            int finalPosicionamento = inicioPosicionamento + letras.Length;
            int posicionamento = 0, letraPosicionamento = inicioPosicionamento;
            foreach (char i in linha)
            {
                
                if (posicionamento >= inicioPosicionamento && posicionamento < finalPosicionamento)
                {
                    linha[letraPosicionamento] = letras[posicionamento - inicioPosicionamento];
                    letraPosicionamento++;
                }
                else
                {
                    linha[posicionamento] = char.Parse("-");
                }
                posicionamento++;
            }
            string novaLinha = new string(linha);
            Console.WriteLine(novaLinha);
        }
    }
}
