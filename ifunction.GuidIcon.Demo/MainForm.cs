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

        /// <summary>
        /// Handles the TextChanged event of the text_Guid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void text_Guid_TextChanged(object sender, EventArgs e)
        {
            Guid? guid = null;

            try
            {
                guid = new Guid(((TextBox)sender).Text);
            }
            catch { }

            if (guid != null)
            {
                GuidIconGenerator generator = new GuidIconGenerator(guid.Value);
                var bitmap = generator.GenerateIcon(guid.Value, (int)(400 / generator.IconSize));

                this.iconImage.Image = bitmap;
            }
        }
    }
}
