using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace LR4_LogAn_Var14
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //МАЛЬКОВА М.В.
        }


        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFilePath.Text = openFileDialog.FileName;
                }
            }
        }

        private void btnAnalyze_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilePath.Text))
            {
                MessageBox.Show("Пожалуйста, загрузите файл.");
                return;
            }

            try
            {
                string fileContent = File.ReadAllText(txtFilePath.Text);
                var lexemes = AnalyzeLexical(fileContent);
                Form2 form2 = new Form2(lexemes);
                form2.Show();
            }
            catch
            {
                MessageBox.Show("Ошибка при чтении файла.");
            }
        }

        private List<Lexeme> AnalyzeLexical(string input)
        {
            List<Lexeme> lexemes = new List<Lexeme>();
            int lexemeNumber = 1;

            string[] logicalOperators = { "or", "xor", "and", "not" };
            string[] constants = { "\'T\'", "\'F\'" };

            // Разделяем выражения по `;` (удаляем пустые элементы)
            string[] expressions = input.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var expression in expressions)
            {
                // Разбиваем по пробелам, `:=`, `(`, `)`
                string pattern = @"(:=|[()])|\s+";
                string[] tokens = Regex.Split(expression.Trim(), pattern)
                                   .Where(t => !string.IsNullOrEmpty(t) && !string.IsNullOrWhiteSpace(t))
                                   .ToArray();

                foreach (var token in tokens)
                {
                    if (lexemes.Any(l => l.Value == token))
                    {
                        continue;
                    }

                    Lexeme lexeme = new Lexeme { Number = lexemeNumber++, Value = token };

                    if (constants.Contains(token))
                    {
                        lexeme.Type = "Символьная константа";
                    }
                    // Проверка на знак присваивания
                    else if (token == ":=")
                    {
                        lexeme.Type = "Знак присваивания";
                    }
                    // Проверка на логические операции
                    else if (logicalOperators.Contains(token))
                    {
                        lexeme.Type = "Логическая операция";
                    }
                    // Проверка на скобки
                    else if (token == "(" || token == ")")
                    {
                        lexeme.Type = "Скобка";
                    }
                    // Проверка на комментарии
                    else if (token == "//")
                    {
                        lexeme.Type = "Знак комментария";
                    }
                    // Проверка на число
                    else if (int.TryParse(token, out _))
                    {
                        lexeme.Type = "Число";
                    }
                    // Проверка на идентификатор (последним, чтобы T и F не относились к ним)
                    else if (IsIdentifier(token))
                    {
                        lexeme.Type = "Идентификатор";
                    }
                    else
                    {
                        lexeme.Type = "Неизвестный токен";
                    }

                    lexemes.Add(lexeme);
                }
            }
            return lexemes;
        }

        private bool IsIdentifier(string token)
        {
            if (string.IsNullOrEmpty(token) || (!char.IsLetter(token[0]) && token[0] != '_'))
            {
                return false;
            }

            for (int i = 1; i < token.Length; i++)
            {
                if (!char.IsLetterOrDigit(token[i]) && token[i] != '_')
                {
                    return false;
                }
            }

            return true;
        }
    }
}
