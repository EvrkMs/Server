using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Soft.Safe
{
    public partial class AddForSafe : Form
    {
        public string SumTextBox { get; private set; }
        public AddForSafe()
        {
            InitializeComponent();
            sendSumButton.Click += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(sumTextBox.Text))
                {
                    MessageBox.Show("Пожалуйста, введите корректные данные.");
                    return;
                }

                SumTextBox = sumTextBox.Text;

                this.DialogResult = DialogResult.OK;
                this.Close();
            };
        }
    }
}
