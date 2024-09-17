namespace GSMSERVICES
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.kryptonPalette1 = new Krypton.Toolkit.KryptonPalette(this.components);
            this.guna2AnimateWindow1 = new Guna.UI2.WinForms.Guna2AnimateWindow(this.components);
            this.Login = new MaterialSkin.Controls.MaterialButton();
            this.saveTokenSwitch = new MaterialSkin.Controls.MaterialSwitch();
            this.tokenTextBox = new MaterialSkin.Controls.MaterialTextBox2();
            this.loginLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // kryptonPalette1
            // 
            this.kryptonPalette1.BasePaletteMode = Krypton.Toolkit.PaletteMode.Office2010White;
            this.kryptonPalette1.ButtonStyles.ButtonForm.StateNormal.Back.Color1 = System.Drawing.Color.White;
            this.kryptonPalette1.ButtonStyles.ButtonForm.StateNormal.Back.Color2 = System.Drawing.Color.White;
            this.kryptonPalette1.ButtonStyles.ButtonForm.StateNormal.Border.DrawBorders = ((Krypton.Toolkit.PaletteDrawBorders)((((Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | Krypton.Toolkit.PaletteDrawBorders.Left) 
            | Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonPalette1.ButtonStyles.ButtonForm.StateNormal.Border.Width = 0;
            this.kryptonPalette1.ButtonStyles.ButtonForm.StatePressed.Back.Color1 = System.Drawing.Color.White;
            this.kryptonPalette1.ButtonStyles.ButtonForm.StatePressed.Back.Color2 = System.Drawing.Color.White;
            this.kryptonPalette1.ButtonStyles.ButtonForm.StatePressed.Border.DrawBorders = ((Krypton.Toolkit.PaletteDrawBorders)((((Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | Krypton.Toolkit.PaletteDrawBorders.Left) 
            | Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonPalette1.ButtonStyles.ButtonForm.StatePressed.Border.Width = 0;
            this.kryptonPalette1.ButtonStyles.ButtonForm.StateTracking.Back.Color1 = System.Drawing.Color.White;
            this.kryptonPalette1.ButtonStyles.ButtonForm.StateTracking.Back.Color2 = System.Drawing.Color.White;
            this.kryptonPalette1.ButtonStyles.ButtonForm.StateTracking.Border.DrawBorders = ((Krypton.Toolkit.PaletteDrawBorders)((((Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | Krypton.Toolkit.PaletteDrawBorders.Left) 
            | Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonPalette1.ButtonStyles.ButtonForm.StateTracking.Border.Width = 0;
            this.kryptonPalette1.FormStyles.FormMain.StateCommon.Back.Color1 = System.Drawing.Color.White;
            this.kryptonPalette1.FormStyles.FormMain.StateCommon.Back.Color2 = System.Drawing.Color.White;
            this.kryptonPalette1.FormStyles.FormMain.StateCommon.Border.DrawBorders = ((Krypton.Toolkit.PaletteDrawBorders)((((Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | Krypton.Toolkit.PaletteDrawBorders.Left) 
            | Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonPalette1.FormStyles.FormMain.StateCommon.Border.GraphicsHint = Krypton.Toolkit.PaletteGraphicsHint.None;
            this.kryptonPalette1.FormStyles.FormMain.StateCommon.Border.Rounding = 16F;
            this.kryptonPalette1.HeaderStyles.HeaderForm.StateCommon.Back.Color1 = System.Drawing.Color.White;
            this.kryptonPalette1.HeaderStyles.HeaderForm.StateCommon.Back.Color2 = System.Drawing.Color.White;
            this.kryptonPalette1.HeaderStyles.HeaderForm.StateCommon.ButtonEdgeInset = 12;
            this.kryptonPalette1.HeaderStyles.HeaderForm.StateCommon.ButtonPadding = new System.Windows.Forms.Padding(10, -1, -1, -1);
            // 
            // Login
            // 
            resources.ApplyResources(this.Login, "Login");
            this.Login.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.Login.Depth = 0;
            this.Login.HighEmphasis = true;
            this.Login.Icon = null;
            this.Login.MouseState = MaterialSkin.MouseState.HOVER;
            this.Login.Name = "Login";
            this.Login.NoAccentTextColor = System.Drawing.Color.Empty;
            this.Login.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.Login.UseAccentColor = false;
            this.Login.UseVisualStyleBackColor = true;
            this.Login.Click += new System.EventHandler(this.guna2Button1_Click);
            // 
            // saveTokenSwitch
            // 
            resources.ApplyResources(this.saveTokenSwitch, "saveTokenSwitch");
            this.saveTokenSwitch.Depth = 0;
            this.saveTokenSwitch.MouseLocation = new System.Drawing.Point(-1, -1);
            this.saveTokenSwitch.MouseState = MaterialSkin.MouseState.HOVER;
            this.saveTokenSwitch.Name = "saveTokenSwitch";
            this.saveTokenSwitch.Ripple = true;
            this.saveTokenSwitch.UseVisualStyleBackColor = true;
            this.saveTokenSwitch.CheckedChanged += new System.EventHandler(this.materialSwitch1_CheckedChanged_1);
            // 
            // tokenTextBox
            // 
            this.tokenTextBox.AnimateReadOnly = false;
            resources.ApplyResources(this.tokenTextBox, "tokenTextBox");
            this.tokenTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.tokenTextBox.Depth = 0;
            this.tokenTextBox.HideSelection = true;
            this.tokenTextBox.LeadingIcon = null;
            this.tokenTextBox.MaxLength = 32767;
            this.tokenTextBox.MouseState = MaterialSkin.MouseState.OUT;
            this.tokenTextBox.Name = "tokenTextBox";
            this.tokenTextBox.PasswordChar = '\0';
            this.tokenTextBox.ReadOnly = false;
            this.tokenTextBox.SelectedText = "";
            this.tokenTextBox.SelectionLength = 0;
            this.tokenTextBox.SelectionStart = 0;
            this.tokenTextBox.ShortcutsEnabled = true;
            this.tokenTextBox.TabStop = false;
            this.tokenTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.tokenTextBox.TrailingIcon = null;
            this.tokenTextBox.UseSystemPasswordChar = false;
            // 
            // loginLabel
            // 
            resources.ApplyResources(this.loginLabel, "loginLabel");
            this.loginLabel.Name = "loginLabel";
            this.loginLabel.Click += new System.EventHandler(this.loginLabel_Click);
            // 
            // LoginForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tokenTextBox);
            this.Controls.Add(this.saveTokenSwitch);
            this.Controls.Add(this.Login);
            this.Controls.Add(this.loginLabel);
            this.Name = "LoginForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoginForm_FormClosing);
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Krypton.Toolkit.KryptonPalette kryptonPalette1;
        private Guna.UI2.WinForms.Guna2AnimateWindow guna2AnimateWindow1;
        private MaterialSkin.Controls.MaterialButton Login;
        private MaterialSkin.Controls.MaterialSwitch saveTokenSwitch;
        private MaterialSkin.Controls.MaterialTextBox2 tokenTextBox;
        private System.Windows.Forms.Label loginLabel;
    }
}