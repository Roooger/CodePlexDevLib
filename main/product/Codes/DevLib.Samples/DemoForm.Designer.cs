﻿using DevLib.ModernUI.ComponentModel;
using DevLib.ModernUI.Forms;
namespace DevLib.Samples
{
    partial class DemoForm
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
            this.components = new System.ComponentModel.Container();
            this.modernStyleManager1 = new DevLib.ModernUI.ComponentModel.ModernStyleManager(this.components);
            this.modernToggle1 = new DevLib.ModernUI.Forms.ModernToggle();
            this.modernButton1 = new DevLib.ModernUI.Forms.ModernButton();
            this.modernDateTime1 = new DevLib.ModernUI.Forms.ModernDateTimePicker();
            this.modernComboBox1 = new DevLib.ModernUI.Forms.ModernComboBox();
            this.modernTabControl1 = new DevLib.ModernUI.Forms.ModernTabControl();
            this.modernTabPage1 = new DevLib.ModernUI.Forms.ModernTabPage();
            this.modernButton2 = new DevLib.ModernUI.Forms.ModernButton();
            this.modernTabPage2 = new DevLib.ModernUI.Forms.ModernTabPage();
            this.modernPanel1 = new DevLib.ModernUI.Forms.ModernPanel();
            this.modernButton3 = new DevLib.ModernUI.Forms.ModernButton();
            this.modernTile1 = new DevLib.ModernUI.Forms.ModernTile();
            this.modernTextBox1 = new DevLib.ModernUI.Forms.ModernTextBox();
            this.modernTabControl2 = new DevLib.ModernUI.Forms.ModernTabControl();
            this.modernTabPage3 = new DevLib.ModernUI.Forms.ModernTabPage();
            this.modernTabPage4 = new DevLib.ModernUI.Forms.ModernTabPage();
            ((System.ComponentModel.ISupportInitialize)(this.modernStyleManager1)).BeginInit();
            this.modernTabControl1.SuspendLayout();
            this.modernTabPage1.SuspendLayout();
            this.modernPanel1.SuspendLayout();
            this.modernTabControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // modernStyleManager1
            // 
            this.modernStyleManager1.ColorStyle = DevLib.ModernUI.Forms.ModernColorStyle.Pink;
            this.modernStyleManager1.Owner = this;
            this.modernStyleManager1.ThemeStyle = DevLib.ModernUI.Forms.ModernThemeStyle.Dark;
            // 
            // modernToggle1
            // 
            this.modernToggle1.AutoSize = true;
            this.modernToggle1.FontWeight = DevLib.ModernUI.Drawing.ModernFontWeight.Light;
            this.modernToggle1.Location = new System.Drawing.Point(247, 133);
            this.modernToggle1.Name = "modernToggle1";
            this.modernToggle1.Size = new System.Drawing.Size(80, 17);
            this.modernToggle1.TabIndex = 4;
            this.modernToggle1.Text = "Off";
            this.modernToggle1.UseSelectable = true;
            this.modernToggle1.UseVisualStyleBackColor = true;
            this.modernToggle1.CheckedChanged += new System.EventHandler(this.modernToggle1_CheckedChanged);
            // 
            // modernButton1
            // 
            this.modernButton1.Location = new System.Drawing.Point(23, 63);
            this.modernButton1.Name = "modernButton1";
            this.modernButton1.Size = new System.Drawing.Size(175, 44);
            this.modernButton1.TabIndex = 5;
            this.modernButton1.Text = "modernButton1";
            this.modernButton1.UseSelectable = true;
            this.modernButton1.UseVisualStyleBackColor = true;
            this.modernButton1.Click += new System.EventHandler(this.modernButton1_Click);
            // 
            // modernDateTime1
            // 
            this.modernDateTime1.FontSize = DevLib.ModernUI.Drawing.ModernFontSize.Small;
            this.modernDateTime1.FontWeight = DevLib.ModernUI.Drawing.ModernFontWeight.Light;
            this.modernDateTime1.Location = new System.Drawing.Point(108, 59);
            this.modernDateTime1.MinimumSize = new System.Drawing.Size(0, 29);
            this.modernDateTime1.Name = "modernDateTime1";
            this.modernDateTime1.Size = new System.Drawing.Size(200, 29);
            this.modernDateTime1.TabIndex = 0;
            // 
            // modernComboBox1
            // 
            this.modernComboBox1.FontSize = DevLib.ModernUI.Drawing.ModernFontSize.Medium;
            this.modernComboBox1.FormattingEnabled = true;
            this.modernComboBox1.ItemHeight = 23;
            this.modernComboBox1.Location = new System.Drawing.Point(127, 68);
            this.modernComboBox1.Name = "modernComboBox1";
            this.modernComboBox1.Size = new System.Drawing.Size(196, 29);
            this.modernComboBox1.TabIndex = 0;
            this.modernComboBox1.UseSelectable = true;
            // 
            // modernTabControl1
            // 
            this.modernTabControl1.Controls.Add(this.modernTabPage1);
            this.modernTabControl1.Controls.Add(this.modernTabPage2);
            this.modernTabControl1.FontSize = DevLib.ModernUI.Drawing.ModernFontSize.Small;
            this.modernTabControl1.Location = new System.Drawing.Point(49, 184);
            this.modernTabControl1.Name = "modernTabControl1";
            this.modernTabControl1.SelectedIndex = 0;
            this.modernTabControl1.Size = new System.Drawing.Size(299, 213);
            this.modernTabControl1.TabIndex = 6;
            this.modernTabControl1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.modernTabControl1.UseSelectable = true;
            // 
            // modernTabPage1
            // 
            this.modernTabPage1.Controls.Add(this.modernButton2);
            this.modernTabPage1.HighlightHorizontalScrollBarOnWheel = false;
            this.modernTabPage1.HighlightVerticalScrollBarOnWheel = false;
            this.modernTabPage1.HorizontalScrollBarSize = 10;
            this.modernTabPage1.Location = new System.Drawing.Point(4, 34);
            this.modernTabPage1.Name = "modernTabPage1";
            this.modernTabPage1.Size = new System.Drawing.Size(291, 175);
            this.modernTabPage1.TabIndex = 0;
            this.modernTabPage1.Text = "modernTabPage1";
            this.modernTabPage1.UseHorizontalBarColor = true;
            this.modernTabPage1.UseVerticalBarColor = true;
            this.modernTabPage1.VerticalScrollBarSize = 10;
            this.modernTabPage1.Click += new System.EventHandler(this.modernTabPage1_Click);
            // 
            // modernButton2
            // 
            this.modernButton2.Location = new System.Drawing.Point(79, 80);
            this.modernButton2.Name = "modernButton2";
            this.modernButton2.Size = new System.Drawing.Size(75, 23);
            this.modernButton2.TabIndex = 7;
            this.modernButton2.Text = "modernButton2";
            this.modernButton2.UseSelectable = true;
            this.modernButton2.UseVisualStyleBackColor = true;
            // 
            // modernTabPage2
            // 
            this.modernTabPage2.HighlightHorizontalScrollBarOnWheel = false;
            this.modernTabPage2.HighlightVerticalScrollBarOnWheel = false;
            this.modernTabPage2.HorizontalScrollBarSize = 10;
            this.modernTabPage2.Location = new System.Drawing.Point(4, 34);
            this.modernTabPage2.Name = "modernTabPage2";
            this.modernTabPage2.Size = new System.Drawing.Size(291, 175);
            this.modernTabPage2.TabIndex = 1;
            this.modernTabPage2.Text = "modernTabPage2";
            this.modernTabPage2.UseHorizontalBarColor = true;
            this.modernTabPage2.UseVerticalBarColor = true;
            this.modernTabPage2.VerticalScrollBarSize = 10;
            // 
            // modernPanel1
            // 
            this.modernPanel1.Controls.Add(this.modernButton3);
            this.modernPanel1.HighlightHorizontalScrollBarOnWheel = false;
            this.modernPanel1.HighlightVerticalScrollBarOnWheel = false;
            this.modernPanel1.HorizontalScrollBarSize = 10;
            this.modernPanel1.Location = new System.Drawing.Point(392, 50);
            this.modernPanel1.Name = "modernPanel1";
            this.modernPanel1.Size = new System.Drawing.Size(223, 141);
            this.modernPanel1.TabIndex = 7;
            this.modernPanel1.UseHorizontalBarColor = true;
            this.modernPanel1.UseVerticalBarColor = true;
            this.modernPanel1.VerticalScrollBarSize = 10;
            // 
            // modernButton3
            // 
            this.modernButton3.Location = new System.Drawing.Point(67, 50);
            this.modernButton3.Name = "modernButton3";
            this.modernButton3.Size = new System.Drawing.Size(75, 23);
            this.modernButton3.TabIndex = 2;
            this.modernButton3.Text = "modernButton3";
            this.modernButton3.UseSelectable = true;
            this.modernButton3.UseVisualStyleBackColor = true;
            // 
            // modernTile1
            // 
            this.modernTile1.ActiveControl = null;
            this.modernTile1.Location = new System.Drawing.Point(459, 249);
            this.modernTile1.Name = "modernTile1";
            this.modernTile1.PaintTileCount = false;
            this.modernTile1.Size = new System.Drawing.Size(144, 117);
            this.modernTile1.TabIndex = 8;
            this.modernTile1.Text = "modernTile1";
            this.modernTile1.UseSelectable = true;
            this.modernTile1.UseVisualStyleBackColor = true;
            // 
            // modernTextBox1
            // 
            this.modernTextBox1.Lines = new string[] {
        "3"};
            this.modernTextBox1.Location = new System.Drawing.Point(247, 84);
            this.modernTextBox1.MaxLength = 32767;
            this.modernTextBox1.Name = "modernTextBox1";
            this.modernTextBox1.PasswordChar = '\0';
            this.modernTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.modernTextBox1.SelectedText = "";
            this.modernTextBox1.Size = new System.Drawing.Size(123, 23);
            this.modernTextBox1.TabIndex = 9;
            this.modernTextBox1.Text = "3";
            this.modernTextBox1.UseSelectable = true;
            // 
            // modernTabControl2
            // 
            this.modernTabControl2.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.modernTabControl2.Controls.Add(this.modernTabPage3);
            this.modernTabControl2.Controls.Add(this.modernTabPage4);
            this.modernTabControl2.FontSize = DevLib.ModernUI.Drawing.ModernFontSize.Small;
            this.modernTabControl2.Location = new System.Drawing.Point(91, 423);
            this.modernTabControl2.Multiline = true;
            this.modernTabControl2.Name = "modernTabControl2";
            this.modernTabControl2.SelectedIndex = 0;
            this.modernTabControl2.Size = new System.Drawing.Size(650, 87);
            this.modernTabControl2.TabIndex = 10;
            this.modernTabControl2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.modernTabControl2.UseSelectable = true;
            // 
            // modernTabPage3
            // 
            this.modernTabPage3.HighlightHorizontalScrollBarOnWheel = true;
            this.modernTabPage3.HighlightVerticalScrollBarOnWheel = true;
            this.modernTabPage3.HorizontalScrollBarSize = 10;
            this.modernTabPage3.Location = new System.Drawing.Point(70, 4);
            this.modernTabPage3.Name = "modernTabPage3";
            this.modernTabPage3.ShowHorizontalScrollBar = true;
            this.modernTabPage3.ShowVerticalScrollBar = true;
            this.modernTabPage3.Size = new System.Drawing.Size(576, 79);
            this.modernTabPage3.TabIndex = 0;
            this.modernTabPage3.Text = "modernTabPage3";
            this.modernTabPage3.UseHorizontalBarColor = true;
            this.modernTabPage3.UseVerticalBarColor = true;
            this.modernTabPage3.VerticalScrollBarSize = 10;
            // 
            // modernTabPage4
            // 
            this.modernTabPage4.HighlightHorizontalScrollBarOnWheel = false;
            this.modernTabPage4.HighlightVerticalScrollBarOnWheel = false;
            this.modernTabPage4.HorizontalScrollBarSize = 10;
            this.modernTabPage4.Location = new System.Drawing.Point(70, 4);
            this.modernTabPage4.Name = "modernTabPage4";
            this.modernTabPage4.Size = new System.Drawing.Size(576, 79);
            this.modernTabPage4.TabIndex = 1;
            this.modernTabPage4.Text = "modernTabPage4";
            this.modernTabPage4.UseHorizontalBarColor = true;
            this.modernTabPage4.UseVerticalBarColor = true;
            this.modernTabPage4.VerticalScrollBarSize = 10;
            // 
            // DemoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackImage = global::DevLib.Samples.Properties.Resources.VS_logo;
            this.BackImageMaxSize = 40;
            this.BackImagePadding = new System.Windows.Forms.Padding(150, 16, 0, 0);
            this.ClientSize = new System.Drawing.Size(752, 629);
            this.ColorStyle = DevLib.ModernUI.Forms.ModernColorStyle.Pink;
            this.ControlBox = false;
            this.Controls.Add(this.modernTabControl2);
            this.Controls.Add(this.modernTextBox1);
            this.Controls.Add(this.modernTile1);
            this.Controls.Add(this.modernPanel1);
            this.Controls.Add(this.modernTabControl1);
            this.Controls.Add(this.modernButton1);
            this.Controls.Add(this.modernToggle1);
            this.Name = "DemoForm";
            this.StyleManager = this.modernStyleManager1;
            this.Text = "DemoForm";
            this.ThemeStyle = DevLib.ModernUI.Forms.ModernThemeStyle.Dark;
            this.UseMaximizeBox = false;
            this.UseMinimizeBox = false;
            ((System.ComponentModel.ISupportInitialize)(this.modernStyleManager1)).EndInit();
            this.modernTabControl1.ResumeLayout(false);
            this.modernTabPage1.ResumeLayout(false);
            this.modernPanel1.ResumeLayout(false);
            this.modernTabControl2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ModernStyleManager modernStyleManager1;
        private ModernToggle modernToggle1;
        private ModernButton modernButton1;


        private ModernDateTimePicker modernDateTime1;

        private ModernComboBox modernComboBox1;
        private ModernTabControl modernTabControl1;
        private ModernTabPage modernTabPage1;
        private ModernTabPage modernTabPage2;
        private ModernButton modernButton2;
        private ModernTile modernTile1;
        private ModernPanel modernPanel1;
        private ModernButton modernButton3;
        private ModernTextBox modernTextBox1;
        private ModernTabControl modernTabControl2;
        private ModernTabPage modernTabPage3;
        private ModernTabPage modernTabPage4;
    }
}