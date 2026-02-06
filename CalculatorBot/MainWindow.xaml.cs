using System.Windows;
using System.Windows.Controls;

namespace CalculatorBot
{
    public partial class MainWindow : Window
    {
        double firstNumber = 0;
        string currentOperator = "";
        bool operatorPressed = false;

        public MainWindow()
        {
            InitializeComponent();
            txtDisplay.Text = "";
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CalculatorButton_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string input = btn.Content.ToString();

            // ================= NUMBERS & DECIMAL =================
            if (char.IsDigit(input[0]) || input == ".")
            {
                txtDisplay.Text += input;
                operatorPressed = false;
                return;
            }

            // ================= CLEAR =================
            if (input == "C")
            {
                txtDisplay.Text = "";
                firstNumber = 0;
                currentOperator = "";
                operatorPressed = false;
                return;
            }

            // ================= BACKSPACE =================
            if (input == "⌫")
            {
                if (txtDisplay.Text.Length > 0)
                    txtDisplay.Text = txtDisplay.Text.Substring(0, txtDisplay.Text.Length - 1);
                return;
            }

            // ================= OPERATOR =================
            if (input == "+" || input == "−" || input == "×" || input == "÷")
            {
                if (!operatorPressed)
                {
                    firstNumber = double.Parse(txtDisplay.Text);
                    txtDisplay.Text += $" {input} ";
                    currentOperator = input;
                    operatorPressed = true;
                }
                return;
            }

            // ================= EQUALS =================
            if (input == "=")
            {
                CalculateResult();
            }
        }

        private void CalculateResult()
        {
            string[] parts = txtDisplay.Text.Split(' ');

            if (parts.Length < 3) return;

            double secondNumber = double.Parse(parts[2]);
            double result = 0;

            switch (currentOperator)
            {
                case "+":
                    result = firstNumber + secondNumber;
                    break;
                case "−":
                    result = firstNumber - secondNumber;
                    break;
                case "×":
                    result = firstNumber * secondNumber;
                    break;
                case "÷":
                    if (secondNumber == 0)
                    {
                        MessageBox.Show("Cannot divide by zero");
                        return;
                    }
                    result = firstNumber / secondNumber;
                    break;
            }

            txtDisplay.Text = result.ToString().Length > 12 ? result.ToString().Substring(0, 12) : result.ToString();
        }
    }
}
