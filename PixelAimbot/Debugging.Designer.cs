﻿namespace PixelAimbot
{
    partial class Debugging
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
            this.button2 = new System.Windows.Forms.Button();
            this.textBoxX = new System.Windows.Forms.TextBox();
            this.textBoxY = new System.Windows.Forms.TextBox();
            this.textBoxWidth = new System.Windows.Forms.TextBox();
            this.textBoxHeight = new System.Windows.Forms.TextBox();
            this.labelX = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnGetMinimap = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.buttonSelectPicture = new System.Windows.Forms.Button();
            this.buttonSelectMask = new System.Windows.Forms.Button();
            this.labelTreshold = new System.Windows.Forms.Label();
            this.trackBarTreshold = new System.Windows.Forms.TrackBar();
            this.buttonSelectArea = new System.Windows.Forms.Button();
            this.pictureBoxMask = new System.Windows.Forms.PictureBox();
            this.pictureBoxPicture = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMask)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(11, 261);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(314, 27);
            this.button2.TabIndex = 2;
            this.button2.Text = "Live";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBoxX
            // 
            this.textBoxX.Location = new System.Drawing.Point(225, 13);
            this.textBoxX.Name = "textBoxX";
            this.textBoxX.Size = new System.Drawing.Size(100, 20);
            this.textBoxX.TabIndex = 3;
            this.textBoxX.TextChanged += new System.EventHandler(this.textBoxX_TextChanged);
            // 
            // textBoxY
            // 
            this.textBoxY.Location = new System.Drawing.Point(225, 39);
            this.textBoxY.Name = "textBoxY";
            this.textBoxY.Size = new System.Drawing.Size(100, 20);
            this.textBoxY.TabIndex = 4;
            this.textBoxY.TextChanged += new System.EventHandler(this.textBoxY_TextChanged);
            // 
            // textBoxWidth
            // 
            this.textBoxWidth.Location = new System.Drawing.Point(225, 65);
            this.textBoxWidth.Name = "textBoxWidth";
            this.textBoxWidth.Size = new System.Drawing.Size(100, 20);
            this.textBoxWidth.TabIndex = 5;
            this.textBoxWidth.TextChanged += new System.EventHandler(this.textBoxWidth_TextChanged);
            // 
            // textBoxHeight
            // 
            this.textBoxHeight.Location = new System.Drawing.Point(225, 91);
            this.textBoxHeight.Name = "textBoxHeight";
            this.textBoxHeight.Size = new System.Drawing.Size(100, 20);
            this.textBoxHeight.TabIndex = 6;
            this.textBoxHeight.TextChanged += new System.EventHandler(this.textBoxHeight_TextChanged);
            // 
            // labelX
            // 
            this.labelX.AutoSize = true;
            this.labelX.Location = new System.Drawing.Point(11, 16);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(79, 13);
            this.labelX.TabIndex = 7;
            this.labelX.Text = "X Start Position";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Y Start Position";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Width";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Height";
            // 
            // btnGetMinimap
            // 
            this.btnGetMinimap.Location = new System.Drawing.Point(14, 153);
            this.btnGetMinimap.Name = "btnGetMinimap";
            this.btnGetMinimap.Size = new System.Drawing.Size(148, 27);
            this.btnGetMinimap.TabIndex = 11;
            this.btnGetMinimap.Text = "Get Minimap";
            this.btnGetMinimap.UseVisualStyleBackColor = true;
            this.btnGetMinimap.Click += new System.EventHandler(this.btnGetMinimap_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "png";
            // 
            // buttonSelectPicture
            // 
            this.buttonSelectPicture.Location = new System.Drawing.Point(63, 186);
            this.buttonSelectPicture.Name = "buttonSelectPicture";
            this.buttonSelectPicture.Size = new System.Drawing.Size(262, 27);
            this.buttonSelectPicture.TabIndex = 12;
            this.buttonSelectPicture.Text = "Select Picture";
            this.buttonSelectPicture.UseVisualStyleBackColor = true;
            this.buttonSelectPicture.Click += new System.EventHandler(this.buttonSelectPicture_Click);
            // 
            // buttonSelectMask
            // 
            this.buttonSelectMask.Location = new System.Drawing.Point(63, 219);
            this.buttonSelectMask.Name = "buttonSelectMask";
            this.buttonSelectMask.Size = new System.Drawing.Size(262, 27);
            this.buttonSelectMask.TabIndex = 13;
            this.buttonSelectMask.Text = "Select Mask";
            this.buttonSelectMask.UseVisualStyleBackColor = true;
            this.buttonSelectMask.Click += new System.EventHandler(this.buttonSelectMask_Click);
            // 
            // labelTreshold
            // 
            this.labelTreshold.AutoSize = true;
            this.labelTreshold.Location = new System.Drawing.Point(11, 123);
            this.labelTreshold.Name = "labelTreshold";
            this.labelTreshold.Size = new System.Drawing.Size(72, 13);
            this.labelTreshold.TabIndex = 17;
            this.labelTreshold.Text = "Treshold (0.7)";
            // 
            // trackBarTreshold
            // 
            this.trackBarTreshold.Location = new System.Drawing.Point(117, 117);
            this.trackBarTreshold.Maximum = 100;
            this.trackBarTreshold.Name = "trackBarTreshold";
            this.trackBarTreshold.Size = new System.Drawing.Size(211, 45);
            this.trackBarTreshold.TabIndex = 18;
            this.trackBarTreshold.Value = 70;
            this.trackBarTreshold.ValueChanged += new System.EventHandler(this.trackBarTreshold_Changed);
            // 
            // buttonSelectArea
            // 
            this.buttonSelectArea.Location = new System.Drawing.Point(178, 153);
            this.buttonSelectArea.Name = "buttonSelectArea";
            this.buttonSelectArea.Size = new System.Drawing.Size(147, 27);
            this.buttonSelectArea.TabIndex = 19;
            this.buttonSelectArea.Text = "Select Area";
            this.buttonSelectArea.UseVisualStyleBackColor = true;
            this.buttonSelectArea.Click += new System.EventHandler(this.buttonSelectArea_Click);
            // 
            // pictureBoxMask
            // 
            this.pictureBoxMask.Location = new System.Drawing.Point(14, 219);
            this.pictureBoxMask.Name = "pictureBoxMask";
            this.pictureBoxMask.Size = new System.Drawing.Size(43, 27);
            this.pictureBoxMask.TabIndex = 15;
            this.pictureBoxMask.TabStop = false;
            // 
            // pictureBoxPicture
            // 
            this.pictureBoxPicture.Location = new System.Drawing.Point(14, 186);
            this.pictureBoxPicture.Name = "pictureBoxPicture";
            this.pictureBoxPicture.Size = new System.Drawing.Size(43, 27);
            this.pictureBoxPicture.TabIndex = 14;
            this.pictureBoxPicture.TabStop = false;
            // 
            // Debugging
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 310);
            this.Controls.Add(this.buttonSelectArea);
            this.Controls.Add(this.btnGetMinimap);
            this.Controls.Add(this.trackBarTreshold);
            this.Controls.Add(this.labelTreshold);
            this.Controls.Add(this.pictureBoxMask);
            this.Controls.Add(this.pictureBoxPicture);
            this.Controls.Add(this.buttonSelectMask);
            this.Controls.Add(this.buttonSelectPicture);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelX);
            this.Controls.Add(this.textBoxHeight);
            this.Controls.Add(this.textBoxWidth);
            this.Controls.Add(this.textBoxY);
            this.Controls.Add(this.textBoxX);
            this.Controls.Add(this.button2);
            this.Name = "Debugging";
            this.Text = "Debugging";
            this.Load += new System.EventHandler(this.Debugging_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMask)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label labelX;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnGetMinimap;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button buttonSelectPicture;
        private System.Windows.Forms.Button buttonSelectMask;
        private System.Windows.Forms.PictureBox pictureBoxPicture;
        private System.Windows.Forms.PictureBox pictureBoxMask;
        private System.Windows.Forms.Label labelTreshold;
        private System.Windows.Forms.TrackBar trackBarTreshold;
        private System.Windows.Forms.Button buttonSelectArea;
        public System.Windows.Forms.TextBox textBoxX;
        public System.Windows.Forms.TextBox textBoxY;
        public System.Windows.Forms.TextBox textBoxWidth;
        public System.Windows.Forms.TextBox textBoxHeight;
    }
}