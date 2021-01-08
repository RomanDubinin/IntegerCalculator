﻿using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Calculation;
using Microsoft.Win32;

namespace CalculatorUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly StringBuilder expression;
        private readonly char[] operations = {'+', '-', '*', '/'};
        private readonly Calculator calculator;

        private string fileToCalculate;
        private string fileToSaveCalculations;
        private readonly BunchCalculator bunchCalculator;

        public MainWindow()
        {
            InitializeComponent();
            expression = new StringBuilder(500);
            calculator = new Calculator();
            bunchCalculator = new BunchCalculator(calculator);
        }

        private void Digit_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button) sender;
            AddDigit(b.Content.ToString());
        }

        private void Operation_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button) sender;
            AddOperation(b.Content.ToString());
        }

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            CalculateAndShowResult();
        }

        private void Backspace_Click(object sender, RoutedEventArgs e)
        {
            RemoveLastCharacter();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Delete();
        }

        private void Key_Click(object sender, KeyEventArgs e)
        {
            //todo add oem keys
            if (e.Key == Key.NumPad1) AddDigit("1");
            if (e.Key == Key.NumPad2) AddDigit("2");
            if (e.Key == Key.NumPad3) AddDigit("3");
            if (e.Key == Key.NumPad4) AddDigit("4");
            if (e.Key == Key.NumPad5) AddDigit("5");
            if (e.Key == Key.NumPad6) AddDigit("6");
            if (e.Key == Key.NumPad7) AddDigit("7");
            if (e.Key == Key.NumPad8) AddDigit("8");
            if (e.Key == Key.NumPad9) AddDigit("9");
            if (e.Key == Key.NumPad0) AddDigit("0");
            if (e.Key == Key.Add) AddOperation("+");
            if (e.Key == Key.Subtract) AddOperation("-");
            if (e.Key == Key.Multiply) AddOperation("*");
            if (e.Key == Key.Divide) AddOperation("/");
            if (e.Key == Key.Return) CalculateAndShowResult();
            if (e.Key == Key.Delete) Delete();
            if (e.Key == Key.Back) RemoveLastCharacter();
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                fileToCalculate = openFileDialog.FileName;
                OpenFileButton.Visibility = Visibility.Hidden;
                SaveFileButton.Visibility = Visibility.Visible;
            }
        }

        private async void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                fileToSaveCalculations = saveFileDialog.FileName;
                SaveFileButton.Visibility = Visibility.Hidden;
                await Task.Run(() => CalculateFile()).ConfigureAwait(true);
                OpenFileButton.Visibility = Visibility.Visible;
            }
        }

        private void AddDigit(string digit)
        {
            expression.Append(digit);
            ExpressionScreen.Text = expression.ToString();
            ScrollToEnd();
        }

        private void AddOperation(string operation)
        {
            if (expression.Length > 0 && operations.Contains(expression[^1]))
                expression.Remove(expression.Length-1, 1);

            expression.Append(operation);
            ExpressionScreen.Text = expression.ToString();
            ScrollToEnd();
        }

        private async void CalculateAndShowResult()
        {
            if (expression.Length == 0)
                return;

            Screen.IsEnabled = false;
            var calculationResult = await Task
                                          .Run(() => calculator.CalculateFromString(expression.ToString()))
                                          .ConfigureAwait(true);
            Screen.IsEnabled = true;
            expression.Clear();

            if (!calculationResult.Validation.IsSuccess)
            {
                ExpressionScreen.Text += "\n" + new string(' ', calculationResult.Validation.ErrorPosition) + "^";
                ErrorScreen.Text = calculationResult.Validation.ErrorMessage;
                AnswerScreen.Text = "";
            }
            else if (calculationResult.CalculationError != null)
            {
                ErrorScreen.Text = calculationResult.CalculationError;
                AnswerScreen.Text = "";
            }
            else
            {
                ErrorScreen.Text = "";
                AnswerScreen.Text = calculationResult.Result.ToString();
            }
        }

        private void CalculateFile()
        {
            var expressions = File.ReadLines(fileToCalculate);
            var results = bunchCalculator.Calculate(expressions);
            File.WriteAllLines(fileToSaveCalculations, results);
        }

        private void Delete()
        {
            expression.Clear();
            ExpressionScreen.Text = expression.ToString();
        }

        private void RemoveLastCharacter()
        {
            if (expression.Length > 0)
                expression.Remove(expression.Length-1, 1);

            ExpressionScreen.Text = expression.ToString();
        }

        private void ScrollToEnd()
        {
            //just because ScrollToEnd does not work
            ExpressionScreen.ScrollToHorizontalOffset(double.MaxValue);
        }
    }
}