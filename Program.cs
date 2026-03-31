using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BatchFileGenerator
{
    class Program
    {
        static List<string> commands = new List<string>();
        static bool includeEchoOff = true;

        static void Main(string[] args)
        {
            Console.Title = "Advanced Batch File Generator";
            Console.WriteLine("=== Advanced Batch File Generator (.NET Framework 4.7.2) ===\n");

            while (true)
            {
                ShowMenu();
                string choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1": AddCommand(); break;
                    case "2": ViewCommands(); break;
                    case "3": EditCommand(); break;
                    case "4": RemoveCommand(); break;
                    case "5": InsertCommand(); break;
                    case "6": GenerateBatchFile(); break;
                    case "7": ToggleEchoOff(); break;
                    case "10": Console.WriteLine("Exiting..."); return;
                    case "8": ExportProject(); break;
                    case "9": ImportProject(); break;
                    default: Console.WriteLine("Invalid choice. Press any key..."); Console.ReadKey(); break;
                }
            }
        }

        static void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Main Menu ===");
            Console.WriteLine("1. Add a command");
            Console.WriteLine("2. View commands");
            Console.WriteLine("3. Edit a command");
            Console.WriteLine("4. Remove a command");
            Console.WriteLine("5. Insert a command at position");
            Console.WriteLine("6. Generate batch file");
            Console.WriteLine($"7. Toggle '@echo off' (currently: {(includeEchoOff ? "ON" : "OFF")})");
            Console.WriteLine("8. Export project to file (.bsgbuildfile)");
            Console.WriteLine("9. Import project from file");
            Console.WriteLine("10. Exit");
            Console.Write("\nSelect option: ");
        }

        static void AddCommand()
        {
            Console.Clear();
            Console.WriteLine("=== Add Command ===");
            Console.WriteLine("1. Echo (print text)");
            Console.WriteLine("2. Run a program (start)");
            Console.WriteLine("3. Pause");
            Console.WriteLine("4. Comment (REM)");
            Console.WriteLine("5. Custom batch command");
            Console.WriteLine("6. Change directory (CD)");
            Console.WriteLine("7. Make directory (MD)");
            Console.WriteLine("8. Remove file/directory (DEL / RD)");
            Console.WriteLine("9. Copy file");
            Console.WriteLine("10. Move file");
            Console.WriteLine("11. Set environment variable (SET)");
            Console.WriteLine("12. IF condition (basic)");
            Console.WriteLine("13. FOR loop (basic)");
            Console.WriteLine("14. Call another batch file");
            Console.WriteLine("15. Exit / exit with code");
            Console.WriteLine("16. Color");
            Console.WriteLine("17. Title");
            Console.WriteLine("18. Choice (ask for input)");
            Console.WriteLine("19. Timeout / sleep");
            Console.WriteLine("20. Start with window style");
            Console.Write("Choose command type: ");

            string type = Console.ReadLine()?.Trim();
            string newCmd = "";

            switch (type)
            {
                case "1":
                    Console.Write("Enter text to echo: ");
                    newCmd = $"echo {Console.ReadLine()}";
                    break;
                case "2":
                    Console.Write("Enter program path (e.g., notepad.exe): ");
                    newCmd = $"start \"\" \"{Console.ReadLine()}\"";
                    break;
                case "3":
                    newCmd = "pause";
                    break;
                case "4":
                    Console.Write("Enter comment: ");
                    newCmd = $"REM {Console.ReadLine()}";
                    break;
                case "5":
                    Console.Write("Enter custom batch command: ");
                    newCmd = Console.ReadLine();
                    break;
                case "6":
                    Console.Write("Enter directory path: ");
                    newCmd = $"cd /d \"{Console.ReadLine()}\"";
                    break;
                case "7":
                    Console.Write("Enter directory name to create: ");
                    newCmd = $"md \"{Console.ReadLine()}\"";
                    break;
                case "8":
                    Console.Write("Enter path to delete (use /s /q for silent recursive): ");
                    string path = Console.ReadLine();
                    Console.Write("Delete directory recursively? (y/n): ");
                    if (Console.ReadLine()?.ToLower() == "y")
                        newCmd = $"rd /s /q \"{path}\"";
                    else
                        newCmd = $"del /f /q \"{path}\"";
                    break;
                case "9":
                    Console.Write("Source file: ");
                    string src = Console.ReadLine();
                    Console.Write("Destination: ");
                    string dst = Console.ReadLine();
                    newCmd = $"copy /y \"{src}\" \"{dst}\"";
                    break;
                case "10":
                    Console.Write("Source file: ");
                    string moveSrc = Console.ReadLine();
                    Console.Write("Destination: ");
                    string moveDst = Console.ReadLine();
                    newCmd = $"move /y \"{moveSrc}\" \"{moveDst}\"";
                    break;
                case "11":
                    Console.Write("Variable name: ");
                    string varName = Console.ReadLine();
                    Console.Write("Variable value: ");
                    string varVal = Console.ReadLine();
                    newCmd = $"set {varName}={varVal}";
                    break;
                case "12":
                    Console.Write("Condition (e.g., \"%var%==value\" or \"exist file.txt\"): ");
                    string condition = Console.ReadLine();
                    Console.Write("Command to execute if true: ");
                    string ifTrue = Console.ReadLine();
                    newCmd = $"if {condition} {ifTrue}";
                    break;
                case "13":
                    Console.Write("FOR loop syntax (e.g., /l %%i in (1,1,10) do echo %%i): ");
                    newCmd = $"for {Console.ReadLine()}";
                    break;
                case "14":
                    Console.Write("Path to batch file to call: ");
                    newCmd = $"call \"{Console.ReadLine()}\"";
                    break;
                case "15":
                    Console.Write("Exit code (optional, press Enter for none): ");
                    string code = Console.ReadLine();
                    newCmd = string.IsNullOrEmpty(code) ? "exit" : $"exit /b {code}";
                    break;
                case "16":
                    Console.Write("Color code (e.g., 0A for black background, green text): ");
                    newCmd = $"color {Console.ReadLine()}";
                    break;
                case "17":
                    Console.Write("Console title: ");
                    newCmd = $"title {Console.ReadLine()}";
                    break;
                case "18":
                    Console.Write("Choice prompt text: ");
                    string prompt = Console.ReadLine();
                    Console.Write("Options (e.g., YN, YNC): ");
                    string options = Console.ReadLine();
                    newCmd = $"choice /c {options} /m \"{prompt}\"";
                    break;
                case "19":
                    Console.Write("Seconds to wait: ");
                    string sec = Console.ReadLine();
                    newCmd = $"timeout /t {sec} /nobreak";
                    break;
                case "20":
                    Console.Write("Program to start: ");
                    string prog = Console.ReadLine();
                    Console.Write("Window style (min, max, hide): ");
                    string style = Console.ReadLine()?.ToLower();
                    string styleArg = "";
                    switch (style)
                    {
                        case "min":
                            styleArg = "/min";
                            break;
                        case "max":
                            styleArg = "/max";
                            break;
                        case "hide":
                            styleArg = "/b";
                            break;
                        default:
                            styleArg = "";
                            break;
                    }
                    newCmd = $"start {styleArg} \"\" \"{prog}\"";
                    break;
                default:
                    Console.WriteLine("Invalid type. No command added.");
                    Console.ReadKey();
                    return;
            }

            commands.Add(newCmd);
            Console.WriteLine("Command added.");
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }

        static void ViewCommands()
        {
            Console.Clear();
            Console.WriteLine("=== Current Commands ===");
            if (commands.Count == 0)
                Console.WriteLine("(No commands added yet)");
            else
            {
                for (int i = 0; i < commands.Count; i++)
                    Console.WriteLine($"{i + 1}. {commands[i]}");
            }
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }

        static void EditCommand()
        {
            if (commands.Count == 0)
            {
                Console.WriteLine("No commands to edit.");
                Console.ReadKey();
                return;
            }

            ViewCommands();
            Console.Write("Enter the number of the command to edit: ");
            if (int.TryParse(Console.ReadLine(), out int idx) && idx >= 1 && idx <= commands.Count)
            {
                Console.WriteLine($"Current command: {commands[idx - 1]}");
                Console.Write("Enter new command text: ");
                string newText = Console.ReadLine();
                if (!string.IsNullOrEmpty(newText))
                    commands[idx - 1] = newText;
                Console.WriteLine("Command updated.");
            }
            else
                Console.WriteLine("Invalid number.");
            Console.ReadKey();
        }

        static void RemoveCommand()
        {
            if (commands.Count == 0)
            {
                Console.WriteLine("No commands to remove.");
                Console.ReadKey();
                return;
            }

            ViewCommands();
            Console.Write("Enter the number of the command to remove: ");
            if (int.TryParse(Console.ReadLine(), out int idx) && idx >= 1 && idx <= commands.Count)
            {
                commands.RemoveAt(idx - 1);
                Console.WriteLine("Command removed.");
            }
            else
                Console.WriteLine("Invalid number.");
            Console.ReadKey();
        }

        static void InsertCommand()
        {
            Console.Clear();
            Console.WriteLine("=== Insert Command ===");
            if (commands.Count == 0)
            {
                Console.WriteLine("No commands yet. Use Add command first.");
                Console.ReadKey();
                return;
            }

            ViewCommands();
            Console.Write($"Enter position to insert at (1..{commands.Count + 1}): ");
            if (int.TryParse(Console.ReadLine(), out int pos) && pos >= 1 && pos <= commands.Count + 1)
            {
                commands.Insert(pos - 1, "##PLACEHOLDER##");
                Console.WriteLine("Now choose a command to insert (same types as Add):");
                AddCommand();
                if (commands[pos - 1] == "##PLACEHOLDER##")
                    commands.RemoveAt(pos - 1);
            }
            else
            {
                Console.WriteLine("Invalid position.");
                Console.ReadKey();
            }
        }

        static void GenerateBatchFile()
        {
            Console.Clear();
            Console.WriteLine("=== Generate Batch File ===");
            Console.Write("Enter filename (without extension, default 'script'): ");
            string fileName = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(fileName))
                fileName = "script";
            if (!fileName.EndsWith(".bat"))
                fileName += ".bat";

            try
            {
                using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.Default))
                {
                    if (includeEchoOff)
                        writer.WriteLine("@echo off");
                    writer.WriteLine(":: Batch file generated by Advanced BatchFileGenerator");
                    writer.WriteLine(":: Created on: " + DateTime.Now);
                    writer.WriteLine();

                    foreach (string cmd in commands)
                        writer.WriteLine(cmd);
                }
                Console.WriteLine($"\nBatch file '{fileName}' generated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError writing file: {ex.Message}");
            }
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }

        static void ToggleEchoOff()
        {
            includeEchoOff = !includeEchoOff;
            Console.WriteLine($"\n'@echo off' is now {(includeEchoOff ? "ON" : "OFF")}.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        static void ExportProject()
        {
            Console.Clear();
            Console.WriteLine("=== Export Project ===");
            Console.Write("Enter filename for export (without extension, default 'project'): ");
            string fileName = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(fileName))
                fileName = "project";
            if (!fileName.EndsWith(".bsgbuildfile"))
                fileName += ".bsgbuildfile";

            try
            {
                using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.UTF8))
                {
                    // File format: 
                    // line 1: version marker (e.g., BSGBUILDFILE_1_0)
                    // line 2: echo off flag (0 or 1)
                    // then each command on its own line
                    writer.WriteLine("BSGBUILDFILE_1_0");
                    writer.WriteLine(includeEchoOff ? "1" : "0");
                    foreach (string cmd in commands)
                        writer.WriteLine(cmd);
                }
                Console.WriteLine($"\nProject exported to '{fileName}' successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError exporting file: {ex.Message}");
            }
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }

        static void ImportProject()
        {
            Console.Clear();
            Console.WriteLine("=== Import Project ===");
            Console.Write("Enter filename to import (including .bsgbuildfile): ");
            string fileName = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(fileName))
            {
                Console.WriteLine("No filename provided.");
                Console.ReadKey();
                return;
            }

            if (!File.Exists(fileName))
            {
                Console.WriteLine($"File '{fileName}' does not exist.");
                Console.ReadKey();
                return;
            }

            try
            {
                string[] lines = File.ReadAllLines(fileName, Encoding.UTF8);
                if (lines.Length < 2)
                {
                    Console.WriteLine("Invalid file format (too short).");
                    Console.ReadKey();
                    return;
                }

                // Check version marker
                if (lines[0] != "BSGBUILDFILE_1_0")
                {
                    Console.WriteLine("Unsupported or unknown file version.");
                    Console.ReadKey();
                    return;
                }

                // Parse echo off flag
                bool newEchoOff = false;
                if (lines[1] == "1")
                    newEchoOff = true;
                else if (lines[1] == "0")
                    newEchoOff = false;
                else
                {
                    Console.WriteLine("Invalid echo off flag in file.");
                    Console.ReadKey();
                    return;
                }

                // Remaining lines are commands
                List<string> newCommands = new List<string>();
                for (int i = 2; i < lines.Length; i++)
                {
                    newCommands.Add(lines[i]);
                }

                // Ask for confirmation before replacing current project
                Console.WriteLine($"\nFound {newCommands.Count} commands and EchoOff = {(newEchoOff ? "ON" : "OFF")}.");
                Console.Write("Replace current project? (y/n): ");
                string confirm = Console.ReadLine()?.ToLower();
                if (confirm == "y")
                {
                    commands = newCommands;
                    includeEchoOff = newEchoOff;
                    Console.WriteLine("Project imported successfully!");
                }
                else
                {
                    Console.WriteLine("Import cancelled.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError importing file: {ex.Message}");
            }
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }
    }
}
