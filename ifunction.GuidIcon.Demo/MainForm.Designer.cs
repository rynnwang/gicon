namespace ifunction.GuidIcon.Demo
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.text_Guid = new System.Windows.Forms.TextBox();
            this.btn_RandomGuid = new System.Windows.Forms.Button();
            this.iconImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.iconImage)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Guid: ";
            // 
            // text_Guid
            // 
            this.text_Guid.Location = new System.Drawing.Point(66, 6);
            this.text_Guid.Name = "text_Guid";
            this.text_Guid.Size = new System.Drawing.Size(295, 25);
            this.text_Guid.TabIndex = 1;
            this.text_Guid.Text = "16EF7B9F-F797-4AC3-9D52-3C0E353C1924";
            this.text_Guid.TextChanged += new System.EventHandler(this.text_Guid_TextChanged);
            // 
            // btn_RandomGuid
            // 
            this.btn_RandomGuid.Location = new System.Drawing.Point(367, 7);
            this.btn_RandomGuid.Name = "btn_RandomGuid";
            this.btn_RandomGuid.Size = new System.Drawing.Size(77, 23);
            this.btn_RandomGuid.TabIndex = 2;
            this.btn_RandomGuid.Text = "Random";
            this.btn_RandomGuid.UseVisualStyleBackColor = true;
            this.btn_RandomGuid.Click += new System.EventHandler(this.btn_RandomGuid_Click);
            // 
            // iconImage
            // 
            this.iconImage.Location = new System.Drawing.Point(30, 37);
            this.iconImage.Name = "iconImage";
            this.iconImage.Size = new System.Drawing.Size(400, 400);
            this.iconImage.TabIndex = 3;
            this.iconImage.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 454);
            this.Controls.Add(this.iconImage);
            this.Controls.Add(this.btn_RandomGuid);
            this.Controls.Add(this.text_Guid);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "Guid Icon Generator";
            ((System.ComponentModel.ISupportInitialize)(this.iconImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox text_Guid;
        private System.Windows.Forms.Button btn_RandomGuid;
        private System.Windows.Forms.PictureBox iconImage;

    }
}

