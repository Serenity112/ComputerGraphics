﻿namespace ComputerGraphics
{
    partial class Form5
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.viewRotationX = new System.Windows.Forms.NumericUpDown();
            this.viewRotationY = new System.Windows.Forms.NumericUpDown();
            this.viewRotationZ = new System.Windows.Forms.NumericUpDown();
            this.spectatorX = new System.Windows.Forms.TextBox();
            this.spectatorY = new System.Windows.Forms.TextBox();
            this.spectatorZ = new System.Windows.Forms.TextBox();
            this.fieldOfView = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewRotationX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewRotationY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewRotationZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldOfView)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1449, 877);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(1524, 822);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(204, 50);
            this.button1.TabIndex = 1;
            this.button1.Text = "Отрисовка";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // viewRotationX
            // 
            this.viewRotationX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.viewRotationX.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.viewRotationX.Location = new System.Drawing.Point(1644, 68);
            this.viewRotationX.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.viewRotationX.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.viewRotationX.Name = "viewRotationX";
            this.viewRotationX.Size = new System.Drawing.Size(84, 24);
            this.viewRotationX.TabIndex = 2;
            this.viewRotationX.ValueChanged += new System.EventHandler(this.viewRotationX_ValueChanged);
            // 
            // viewRotationY
            // 
            this.viewRotationY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.viewRotationY.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.viewRotationY.Location = new System.Drawing.Point(1644, 94);
            this.viewRotationY.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.viewRotationY.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.viewRotationY.Name = "viewRotationY";
            this.viewRotationY.Size = new System.Drawing.Size(84, 24);
            this.viewRotationY.TabIndex = 3;
            this.viewRotationY.ValueChanged += new System.EventHandler(this.viewRotationY_ValueChanged);
            // 
            // viewRotationZ
            // 
            this.viewRotationZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.viewRotationZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.viewRotationZ.Location = new System.Drawing.Point(1644, 120);
            this.viewRotationZ.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.viewRotationZ.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.viewRotationZ.Name = "viewRotationZ";
            this.viewRotationZ.Size = new System.Drawing.Size(84, 24);
            this.viewRotationZ.TabIndex = 4;
            this.viewRotationZ.ValueChanged += new System.EventHandler(this.viewRotationZ_ValueChanged);
            // 
            // spectatorX
            // 
            this.spectatorX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spectatorX.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.spectatorX.Location = new System.Drawing.Point(1519, 68);
            this.spectatorX.Name = "spectatorX";
            this.spectatorX.Size = new System.Drawing.Size(82, 24);
            this.spectatorX.TabIndex = 5;
            this.spectatorX.Text = "0";
            // 
            // spectatorY
            // 
            this.spectatorY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spectatorY.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.spectatorY.Location = new System.Drawing.Point(1519, 94);
            this.spectatorY.Name = "spectatorY";
            this.spectatorY.Size = new System.Drawing.Size(82, 24);
            this.spectatorY.TabIndex = 6;
            this.spectatorY.Text = "0";
            // 
            // spectatorZ
            // 
            this.spectatorZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spectatorZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.spectatorZ.Location = new System.Drawing.Point(1519, 120);
            this.spectatorZ.Name = "spectatorZ";
            this.spectatorZ.Size = new System.Drawing.Size(82, 24);
            this.spectatorZ.TabIndex = 7;
            this.spectatorZ.Text = "0";
            // 
            // fieldOfView
            // 
            this.fieldOfView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fieldOfView.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.fieldOfView.Location = new System.Drawing.Point(1507, 244);
            this.fieldOfView.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.fieldOfView.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.fieldOfView.Name = "fieldOfView";
            this.fieldOfView.Size = new System.Drawing.Size(120, 26);
            this.fieldOfView.TabIndex = 8;
            this.fieldOfView.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(1507, 225);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(188, 18);
            this.label1.TabIndex = 9;
            this.label1.Text = "Поле зрения (в градусах):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(1510, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 36);
            this.label2.TabIndex = 9;
            this.label2.Text = "Координаты\r\nнаблюдателя:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(1635, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 36);
            this.label3.TabIndex = 9;
            this.label3.Text = "Направление\r\nвзгляда:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(1488, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 72);
            this.label4.TabIndex = 9;
            this.label4.Text = "X\r\nY\r\nZ";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1760, 901);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.fieldOfView);
            this.Controls.Add(this.spectatorZ);
            this.Controls.Add(this.spectatorY);
            this.Controls.Add(this.spectatorX);
            this.Controls.Add(this.viewRotationZ);
            this.Controls.Add(this.viewRotationY);
            this.Controls.Add(this.viewRotationX);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form5";
            this.Text = "Form5";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewRotationX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewRotationY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewRotationZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldOfView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown viewRotationX;
        private System.Windows.Forms.NumericUpDown viewRotationY;
        private System.Windows.Forms.NumericUpDown viewRotationZ;
        private System.Windows.Forms.TextBox spectatorX;
        private System.Windows.Forms.TextBox spectatorY;
        private System.Windows.Forms.TextBox spectatorZ;
        private System.Windows.Forms.NumericUpDown fieldOfView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}