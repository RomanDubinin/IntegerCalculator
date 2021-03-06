﻿using System;
using System.IO;
using System.Linq;
using Calculation;
using Extensions;

namespace BunchCalculatorConsole
{
    class Program
    {
        private static readonly string usage = "\tUsage: " +
                                               $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.exe " +
                                               "input_file_name " +
                                               "output_file_name";

        private static readonly string[] doReplaceCommands = {"Y", "y"};
        private static readonly string[] doNotReplaceCommands = { "N", "n" };

        static void Main(string[] args)
        {
            var (error, inputFile, outputFile) = ParseCommandLineArguments(args);

            if (error != null)
            {
                Console.WriteLine(error);
                return;
            }

            var directory = Path.GetDirectoryName(outputFile);
            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            var bunchCalculator = new BunchCalculator(new Calculator(new Validator(), new ExpressionNormalizer()));
            var expressions = File.ReadLines(inputFile);
            var results = bunchCalculator.Calculate(expressions);
            FileExtensions.WriteAllLines(outputFile, results);
        }

        private static (string error, string inputFile, string outputFile) ParseCommandLineArguments(string[] args)
        {
            if (args.Length != 2)
                return (usage, null, null);

            var inputFile = args[0];
            var outputFile = args[1];


            if (!File.Exists(inputFile))
                return ("\tThe input file does not exists.", null, null);

            if (FileExtensions.FilesAreSame(inputFile, outputFile))
            {
                return ("\tFiles are same. Choose other output file.", null, null);
            }

            if (File.Exists(outputFile))
            {
                Console.WriteLine("\tThe output file already exists.");
                Console.Write("Do you want to replace it ");
                var replaceCommand = "";
                while (!doReplaceCommands.Contains(replaceCommand) && !doNotReplaceCommands.Contains(replaceCommand))
                {
                    Console.Write("(Y/N)? ");
                    replaceCommand = Console.ReadLine();
                }

                if (doNotReplaceCommands.Contains(replaceCommand))
                {
                    return ("", null, null);
                }
            }

            if (!FileExtensions.IsFileAccessibleForWrite(outputFile))
            {
                return ($"\tFile {outputFile} is inaccessible to write. Choose other output file.", null, null);
            }

            return (null, inputFile, outputFile);
        }
    }
}