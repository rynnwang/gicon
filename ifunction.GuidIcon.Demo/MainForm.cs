using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ifunction.GuidIcon.Demo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Click event of the btn_RandomGuid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btn_RandomGuid_Click(object sender, EventArgs e)
        {
            text_Guid.Text = Guid.NewGuid().ToString();
        }

        private void text_Guid_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
