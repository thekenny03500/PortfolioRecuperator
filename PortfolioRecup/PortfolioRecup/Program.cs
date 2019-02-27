using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PortfolioRecup
{
    public class Program
    {
        public static string PathIn = ".";
        public static string PathOut = ".\\app";
        public static int codeError = 0;
        public static string[] tempoDirName = null;
        public static string[] tempofileName = null;

        public static void RecuperationPage(string path)
        {
            List<string> dirs = Directory.EnumerateDirectories(path).Where(d => !d.Contains("\\_")).ToList();
            foreach (string dir in dirs)
            {
                Console.WriteLine("---> Création \"" + dir + "\"");
                Directory.CreateDirectory(PathOut + dir);
                Console.WriteLine("----> Test si c'est un dossier page ou conteneur de pages");
                if (!File.Exists(dir + "/app.js"))
                {
                    Console.WriteLine("-----> Conteneur de pages");
                    RecuperationPage(dir);
                }
                else
                {
                    Console.WriteLine("----> Dossier page");
                    Console.WriteLine("-----> copie app.js");
                    File.Copy(dir + "/app.js", PathOut + dir + "/app.js");
                    Console.WriteLine("-----> copie style.css");
                    File.Copy(dir + "/app.js", PathOut + dir + "/style.css");

                    if (Directory.Exists(dir + "/Image"))
                    {
                        Console.WriteLine("-----> Création \"/Image\"");
                        Directory.CreateDirectory(PathOut + dir + "/Image");
                        List<string> fileName = Directory.GetFiles(dir + "/Image").Where(f => !f.Contains(".ini")).ToList();
                        foreach (string file in fileName)
                        {
                            Console.WriteLine("------> Copie \"" + file + "\"");
                            File.Copy(file, PathOut + file);
                        }
                    }

                    if (Directory.Exists(dir + "/Content_File"))
                    {
                        Console.WriteLine("-----> Création \"/Content_File\"");
                        Directory.CreateDirectory(PathOut + dir + "/Content_File");
                        List<string> fileName = Directory.GetFiles(dir + "/Content_File").Where(f => !f.Contains(".ini")).ToList();
                        foreach (string file in fileName)
                        {
                            Console.WriteLine("------> Copie \"" + file + "\"");
                            File.Copy(file, PathOut + file);
                        }
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            try
            {
                bool go = false;
                do
                {
                    Console.Clear();
                    Console.WriteLine("/--------------------------------------------------------------/");
                    Console.WriteLine("/              Debut de la recuperation du site ?              /");
                    Console.WriteLine("/--------------------------------------------------------------/");
                    Console.WriteLine("                            [Y,N]");
                    ConsoleKey? keyPress = null;
                    keyPress = Console.ReadKey().Key;
                    if (keyPress.HasValue)
                    {
                        if (keyPress.Value == ConsoleKey.Y)
                            go = true;
                        else if (keyPress.Value == ConsoleKey.N)
                            Environment.Exit(0);
                    }
                } while (!go);

                // Debut
                Console.Clear();
                codeError = 1;
                Console.WriteLine("-> Test si l'app existe");
                if (!File.Exists(PathIn + "/__init__.py"))
                {
                    throw new Exception("Le dossier courant ne contien pas le projet");
                }

                codeError = 2;
                Console.WriteLine("-> Debut de recuperation des noms de dossier");
                List<string> DirMain = Directory.EnumerateDirectories(PathIn).ToList();

                // Creation du dossier de sortie
                Console.WriteLine("-> Test existant dossier de sortie ");
                if (DirMain.Any(d => d == PathOut))
                {
                    codeError = 3;
                    Console.WriteLine("--> Dossier de sortie anterieur existant ");
                    Console.WriteLine("---> Suppression");
                    Directory.Delete(PathOut, true);
                }
                codeError = 4;
                Console.WriteLine("--> Création du dossier de sortie");
                Directory.CreateDirectory(PathOut);
                //codeError = 5;
                //Console.WriteLine("--> Création dossier \"static\"");
                //Directory.CreateDirectory(PathOut + "/static");

                // Recuperation de l'app
                codeError = 5;
                Console.WriteLine("-> Copie __init__");
                File.Copy(PathIn + "/__init__.py", PathOut + "/__init__.py");

                // Recuperation des templates
                codeError = 6;
                Console.WriteLine("-> Recuperation templates");
                Console.WriteLine("--> Création dossier \"templates\"");
                Directory.CreateDirectory(PathOut + "/templates");
                codeError = 7;
                tempofileName = Directory.GetFiles(PathIn + "/templates", "*.html");
                foreach (string file in tempofileName)
                {
                    Console.WriteLine("---> Copie \"" + file + "\"");
                    File.Copy(file, PathOut + file);
                }
                codeError = 8;
                Console.WriteLine("---> Recuperation des dossiers de template ");
                tempoDirName = Directory.GetDirectories(PathIn + "/templates");
                foreach (string dir in tempoDirName)
                {
                    Console.WriteLine("----> Création \"" + dir + "\"");
                    Directory.CreateDirectory(PathOut + dir);
                    tempofileName = Directory.GetFiles(dir, "*.html");
                    foreach (string file in tempofileName)
                    {
                        Console.WriteLine("-----> Copie \"" + file + "\"");
                        File.Copy(file, PathOut + file);
                    }
                }

                // Recuperation du Static
                codeError = 9;
                Console.WriteLine("-> Recuperation Static");
                Console.WriteLine("--> Création dossier \"Static\"");
                Directory.CreateDirectory(PathOut + "/Static");
                // Library d'Icone
                codeError = 10;
                Console.WriteLine("---> Création dossier \"_icone\"");
                Directory.CreateDirectory(PathOut + "/Static/_icone");
                tempofileName = Directory.GetFiles(PathIn + "/Static/_icone", "*.png");
                foreach (string file in tempofileName)
                {
                    Console.WriteLine("---> Copie \"" + file + "\"");
                    File.Copy(file, PathOut + file);
                }
                // Library
                codeError = 11;
                Console.WriteLine("---> Création dossier \"_Library\"");
                Directory.CreateDirectory(PathOut + "/Static/_Library");
                Console.WriteLine("----> Création dossier \"d\"");
                Directory.CreateDirectory(PathOut + "/Static/_Library/d");
                Console.WriteLine("-----> copie script externe");
                tempofileName = Directory.GetFiles(PathIn + "/Static/_Library/d", "*.js");
                foreach (string file in tempofileName)
                {
                    Console.WriteLine("-----> Copie \"" + file + "\"");
                    File.Copy(file, PathOut + file);
                }
                Console.WriteLine("-----> copie app.js");
                File.Copy(PathIn + "/Static/_Library/app.js", PathOut + "/Static/_Library/app.js");
                Console.WriteLine("-----> copie style.css");
                File.Copy(PathIn + "/Static/_Library/app.js", PathOut + "/Static/_Library/style.css");
                // Recuperation script page
                codeError = 12;
                RecuperationPage(PathIn + "/Static");

                // End
                Console.WriteLine("-> Recuperation terminer sans erreur");
                Console.WriteLine("-> dossier de sortie : " + PathOut);

                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("-> Arret de la recuperation code erreur : " + codeError);
                Console.WriteLine("--> " + e.Message);
                Console.ReadKey();
            }
        }
    }
}
