﻿using System;
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
            var bitmap = GuidIconGenerator.GenerateBitmap(((TextBox)sender).Text, 400);
            this.iconImage.Image = bitmap;
        }

        /// <summary>
        /// Handles the Load event of the MainForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            text_Guid.Text = Guid.NewGuid().ToString();
        }
    }
}
