using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LR4_LogAn_Var14
{
    public partial class Form2 : Form
    {
        public Form2(List<Lexeme> lexemes)
        {
            InitializeComponent();
            dataGridViewLexemes.DataSource = lexemes;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

    }
}
