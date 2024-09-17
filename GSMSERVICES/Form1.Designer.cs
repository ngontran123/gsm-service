namespace GSMSERVICES
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.materialTabControl1 = new MaterialSkin.Controls.MaterialTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.version_txt = new MaterialSkin.Controls.MaterialLabel();
            this.num_of_port = new MaterialSkin.Controls.MaterialLabel();
            this.dataGSM = new MaterialSkin.Controls.MaterialListView();
            this.stt = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.port = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.sdt = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.telco = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.serial_sim = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imei = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.main_balane = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.sub_balance = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.expire = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.note = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.messageListView = new MaterialSkin.Controls.MaterialListView();
            this.msg_stt = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.com = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.from_tel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.to_tel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.msg_date = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.message = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.otp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.languagesComboBox = new MaterialSkin.Controls.MaterialComboBox();
            this.materialLabel5 = new MaterialSkin.Controls.MaterialLabel();
            this.materialButton1 = new MaterialSkin.Controls.MaterialButton();
            this.baudrateComboBox = new MaterialSkin.Controls.MaterialComboBox();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.displayLabel = new MaterialSkin.Controls.MaterialLabel();
            this.materialCard2 = new MaterialSkin.Controls.MaterialCard();
            this.blueBtn = new MaterialSkin.Controls.MaterialRadioButton();
            this.amberBtn = new MaterialSkin.Controls.MaterialRadioButton();
            this.greenBtn = new MaterialSkin.Controls.MaterialRadioButton();
            this.materialLabel4 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel3 = new MaterialSkin.Controls.MaterialLabel();
            this.materialCard1 = new MaterialSkin.Controls.MaterialCard();
            this.materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            this.themeToogle = new MaterialSkin.Controls.MaterialSwitch();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.guna2AnimateWindow1 = new Guna.UI2.WinForms.Guna2AnimateWindow(this.components);
            this.materialTabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.materialCard2.SuspendLayout();
            this.materialCard1.SuspendLayout();
            this.SuspendLayout();
            // 
            // materialTabControl1
            // 
            this.materialTabControl1.Controls.Add(this.tabPage1);
            this.materialTabControl1.Controls.Add(this.tabPage2);
            this.materialTabControl1.Controls.Add(this.tabPage3);
            this.materialTabControl1.Cursor = System.Windows.Forms.Cursors.Default;
            this.materialTabControl1.Depth = 0;
            resources.ApplyResources(this.materialTabControl1, "materialTabControl1");
            this.materialTabControl1.ImageList = this.imageList1;
            this.materialTabControl1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialTabControl1.Multiline = true;
            this.materialTabControl1.Name = "materialTabControl1";
            this.materialTabControl1.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.White;
            this.tabPage1.Controls.Add(this.version_txt);
            this.tabPage1.Controls.Add(this.num_of_port);
            this.tabPage1.Controls.Add(this.dataGSM);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // version_txt
            // 
            resources.ApplyResources(this.version_txt, "version_txt");
            this.version_txt.Depth = 0;
            this.version_txt.MouseState = MaterialSkin.MouseState.HOVER;
            this.version_txt.Name = "version_txt";
            // 
            // num_of_port
            // 
            resources.ApplyResources(this.num_of_port, "num_of_port");
            this.num_of_port.Depth = 0;
            this.num_of_port.MouseState = MaterialSkin.MouseState.HOVER;
            this.num_of_port.Name = "num_of_port";
            // 
            // dataGSM
            // 
            resources.ApplyResources(this.dataGSM, "dataGSM");
            this.dataGSM.AutoArrange = false;
            this.dataGSM.AutoSizeTable = false;
            this.dataGSM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dataGSM.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGSM.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.stt,
            this.port,
            this.sdt,
            this.telco,
            this.status,
            this.serial_sim,
            this.imei,
            this.main_balane,
            this.sub_balance,
            this.expire,
            this.note});
            this.dataGSM.Depth = 0;
            this.dataGSM.FullRowSelect = true;
            this.dataGSM.HideSelection = false;
            this.dataGSM.MouseLocation = new System.Drawing.Point(-1, -1);
            this.dataGSM.MouseState = MaterialSkin.MouseState.OUT;
            this.dataGSM.Name = "dataGSM";
            this.dataGSM.OwnerDraw = true;
            this.dataGSM.UseCompatibleStateImageBehavior = false;
            this.dataGSM.View = System.Windows.Forms.View.Details;
            this.dataGSM.SelectedIndexChanged += new System.EventHandler(this.materialListView1_SelectedIndexChanged);
            // 
            // stt
            // 
            resources.ApplyResources(this.stt, "stt");
            // 
            // port
            // 
            resources.ApplyResources(this.port, "port");
            // 
            // sdt
            // 
            resources.ApplyResources(this.sdt, "sdt");
            // 
            // telco
            // 
            resources.ApplyResources(this.telco, "telco");
            // 
            // status
            // 
            resources.ApplyResources(this.status, "status");
            // 
            // serial_sim
            // 
            resources.ApplyResources(this.serial_sim, "serial_sim");
            // 
            // imei
            // 
            resources.ApplyResources(this.imei, "imei");
            // 
            // main_balane
            // 
            resources.ApplyResources(this.main_balane, "main_balane");
            // 
            // sub_balance
            // 
            resources.ApplyResources(this.sub_balance, "sub_balance");
            // 
            // expire
            // 
            resources.ApplyResources(this.expire, "expire");
            // 
            // note
            // 
            resources.ApplyResources(this.note, "note");
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.White;
            this.tabPage2.Controls.Add(this.messageListView);
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            // 
            // messageListView
            // 
            this.messageListView.AutoSizeTable = false;
            this.messageListView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.messageListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.messageListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.msg_stt,
            this.com,
            this.from_tel,
            this.to_tel,
            this.msg_date,
            this.message,
            this.otp});
            this.messageListView.Depth = 0;
            resources.ApplyResources(this.messageListView, "messageListView");
            this.messageListView.FullRowSelect = true;
            this.messageListView.HideSelection = false;
            this.messageListView.MouseLocation = new System.Drawing.Point(-1, -1);
            this.messageListView.MouseState = MaterialSkin.MouseState.OUT;
            this.messageListView.Name = "messageListView";
            this.messageListView.OwnerDraw = true;
            this.messageListView.UseCompatibleStateImageBehavior = false;
            this.messageListView.View = System.Windows.Forms.View.Details;
            // 
            // msg_stt
            // 
            resources.ApplyResources(this.msg_stt, "msg_stt");
            // 
            // com
            // 
            resources.ApplyResources(this.com, "com");
            // 
            // from_tel
            // 
            resources.ApplyResources(this.from_tel, "from_tel");
            // 
            // to_tel
            // 
            resources.ApplyResources(this.to_tel, "to_tel");
            // 
            // msg_date
            // 
            resources.ApplyResources(this.msg_date, "msg_date");
            // 
            // message
            // 
            resources.ApplyResources(this.message, "message");
            // 
            // otp
            // 
            resources.ApplyResources(this.otp, "otp");
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.languagesComboBox);
            this.tabPage3.Controls.Add(this.materialLabel5);
            this.tabPage3.Controls.Add(this.materialButton1);
            this.tabPage3.Controls.Add(this.baudrateComboBox);
            this.tabPage3.Controls.Add(this.materialLabel1);
            this.tabPage3.Controls.Add(this.displayLabel);
            this.tabPage3.Controls.Add(this.materialCard2);
            this.tabPage3.Controls.Add(this.materialLabel3);
            this.tabPage3.Controls.Add(this.materialCard1);
            resources.ApplyResources(this.tabPage3, "tabPage3");
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // languagesComboBox
            // 
            this.languagesComboBox.AutoResize = false;
            this.languagesComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.languagesComboBox.Depth = 0;
            this.languagesComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.languagesComboBox.DropDownHeight = 174;
            this.languagesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.languagesComboBox.DropDownWidth = 121;
            resources.ApplyResources(this.languagesComboBox, "languagesComboBox");
            this.languagesComboBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.languagesComboBox.FormattingEnabled = true;
            this.languagesComboBox.Items.AddRange(new object[] {
            resources.GetString("languagesComboBox.Items"),
            resources.GetString("languagesComboBox.Items1")});
            this.languagesComboBox.MouseState = MaterialSkin.MouseState.OUT;
            this.languagesComboBox.Name = "languagesComboBox";
            this.languagesComboBox.StartIndex = 0;
            this.languagesComboBox.SelectedIndexChanged += new System.EventHandler(this.languagesComboBox_SelectedIndexChanged);
            // 
            // materialLabel5
            // 
            resources.ApplyResources(this.materialLabel5, "materialLabel5");
            this.materialLabel5.Depth = 0;
            this.materialLabel5.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel5.Name = "materialLabel5";
            // 
            // materialButton1
            // 
            resources.ApplyResources(this.materialButton1, "materialButton1");
            this.materialButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.materialButton1.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.materialButton1.Depth = 0;
            this.materialButton1.HighEmphasis = true;
            this.materialButton1.Icon = null;
            this.materialButton1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialButton1.Name = "materialButton1";
            this.materialButton1.NoAccentTextColor = System.Drawing.Color.Empty;
            this.materialButton1.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.materialButton1.UseAccentColor = false;
            this.materialButton1.UseVisualStyleBackColor = true;
            this.materialButton1.Click += new System.EventHandler(this.materialButton1_Click);
            // 
            // baudrateComboBox
            // 
            this.baudrateComboBox.AutoResize = false;
            this.baudrateComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.baudrateComboBox.Depth = 0;
            this.baudrateComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.baudrateComboBox.DropDownHeight = 174;
            this.baudrateComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.baudrateComboBox.DropDownWidth = 121;
            resources.ApplyResources(this.baudrateComboBox, "baudrateComboBox");
            this.baudrateComboBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.baudrateComboBox.FormattingEnabled = true;
            this.baudrateComboBox.Items.AddRange(new object[] {
            resources.GetString("baudrateComboBox.Items"),
            resources.GetString("baudrateComboBox.Items1"),
            resources.GetString("baudrateComboBox.Items2"),
            resources.GetString("baudrateComboBox.Items3"),
            resources.GetString("baudrateComboBox.Items4"),
            resources.GetString("baudrateComboBox.Items5"),
            resources.GetString("baudrateComboBox.Items6")});
            this.baudrateComboBox.MouseState = MaterialSkin.MouseState.OUT;
            this.baudrateComboBox.Name = "baudrateComboBox";
            this.baudrateComboBox.StartIndex = 0;
            // 
            // materialLabel1
            // 
            resources.ApplyResources(this.materialLabel1, "materialLabel1");
            this.materialLabel1.Depth = 0;
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            // 
            // displayLabel
            // 
            resources.ApplyResources(this.displayLabel, "displayLabel");
            this.displayLabel.Depth = 0;
            this.displayLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.displayLabel.Name = "displayLabel";
            // 
            // materialCard2
            // 
            this.materialCard2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialCard2.Controls.Add(this.blueBtn);
            this.materialCard2.Controls.Add(this.amberBtn);
            this.materialCard2.Controls.Add(this.greenBtn);
            this.materialCard2.Controls.Add(this.materialLabel4);
            this.materialCard2.Depth = 0;
            this.materialCard2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            resources.ApplyResources(this.materialCard2, "materialCard2");
            this.materialCard2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCard2.Name = "materialCard2";
            // 
            // blueBtn
            // 
            resources.ApplyResources(this.blueBtn, "blueBtn");
            this.blueBtn.Depth = 0;
            this.blueBtn.MouseLocation = new System.Drawing.Point(-1, -1);
            this.blueBtn.MouseState = MaterialSkin.MouseState.HOVER;
            this.blueBtn.Name = "blueBtn";
            this.blueBtn.Ripple = true;
            this.blueBtn.TabStop = true;
            this.blueBtn.UseVisualStyleBackColor = true;
            this.blueBtn.CheckedChanged += new System.EventHandler(this.materialRadioButton3_CheckedChanged);
            // 
            // amberBtn
            // 
            resources.ApplyResources(this.amberBtn, "amberBtn");
            this.amberBtn.Depth = 0;
            this.amberBtn.MouseLocation = new System.Drawing.Point(-1, -1);
            this.amberBtn.MouseState = MaterialSkin.MouseState.HOVER;
            this.amberBtn.Name = "amberBtn";
            this.amberBtn.Ripple = true;
            this.amberBtn.TabStop = true;
            this.amberBtn.UseVisualStyleBackColor = true;
            this.amberBtn.CheckedChanged += new System.EventHandler(this.materialRadioButton2_CheckedChanged);
            // 
            // greenBtn
            // 
            resources.ApplyResources(this.greenBtn, "greenBtn");
            this.greenBtn.Depth = 0;
            this.greenBtn.MouseLocation = new System.Drawing.Point(-1, -1);
            this.greenBtn.MouseState = MaterialSkin.MouseState.HOVER;
            this.greenBtn.Name = "greenBtn";
            this.greenBtn.Ripple = true;
            this.greenBtn.TabStop = true;
            this.greenBtn.UseVisualStyleBackColor = true;
            this.greenBtn.CheckedChanged += new System.EventHandler(this.materialRadioButton1_CheckedChanged);
            // 
            // materialLabel4
            // 
            resources.ApplyResources(this.materialLabel4, "materialLabel4");
            this.materialLabel4.Depth = 0;
            this.materialLabel4.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel4.Name = "materialLabel4";
            // 
            // materialLabel3
            // 
            resources.ApplyResources(this.materialLabel3, "materialLabel3");
            this.materialLabel3.Depth = 0;
            this.materialLabel3.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel3.Name = "materialLabel3";
            // 
            // materialCard1
            // 
            this.materialCard1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialCard1.Controls.Add(this.materialLabel2);
            this.materialCard1.Controls.Add(this.themeToogle);
            this.materialCard1.Depth = 0;
            this.materialCard1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            resources.ApplyResources(this.materialCard1, "materialCard1");
            this.materialCard1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCard1.Name = "materialCard1";
            // 
            // materialLabel2
            // 
            resources.ApplyResources(this.materialLabel2, "materialLabel2");
            this.materialLabel2.Depth = 0;
            this.materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            // 
            // themeToogle
            // 
            resources.ApplyResources(this.themeToogle, "themeToogle");
            this.themeToogle.BackColor = System.Drawing.Color.White;
            this.themeToogle.Depth = 0;
            this.themeToogle.MouseLocation = new System.Drawing.Point(-1, -1);
            this.themeToogle.MouseState = MaterialSkin.MouseState.HOVER;
            this.themeToogle.Name = "themeToogle";
            this.themeToogle.Ripple = true;
            this.themeToogle.UseVisualStyleBackColor = false;
            this.themeToogle.CheckedChanged += new System.EventHandler(this.themeToogle_CheckedChanged);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "email.png");
            this.imageList1.Images.SetKeyName(1, "gsm.png");
            this.imageList1.Images.SetKeyName(2, "settings.png");
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.materialTabControl1);
            this.DrawerShowIconsWhenHidden = true;
            this.DrawerTabControl = this.materialTabControl1;
            this.Name = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.materialTabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.materialCard2.ResumeLayout(false);
            this.materialCard2.PerformLayout();
            this.materialCard1.ResumeLayout(false);
            this.materialCard1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private MaterialSkin.Controls.MaterialTabControl materialTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TabPage tabPage3;
        private MaterialSkin.Controls.MaterialSwitch themeToogle;
        private MaterialSkin.Controls.MaterialLabel materialLabel2;
        private MaterialSkin.Controls.MaterialCard materialCard1;
        private MaterialSkin.Controls.MaterialCard materialCard2;
        private MaterialSkin.Controls.MaterialRadioButton blueBtn;
        private MaterialSkin.Controls.MaterialRadioButton amberBtn;
        private MaterialSkin.Controls.MaterialRadioButton greenBtn;
        private MaterialSkin.Controls.MaterialLabel materialLabel4;
        private MaterialSkin.Controls.MaterialLabel materialLabel3;
        private MaterialSkin.Controls.MaterialListView dataGSM;
        private System.Windows.Forms.ColumnHeader stt;
        private System.Windows.Forms.ColumnHeader port;
        private System.Windows.Forms.ColumnHeader sdt;
        private System.Windows.Forms.ColumnHeader telco;
        private System.Windows.Forms.ColumnHeader status;
        private System.Windows.Forms.ColumnHeader imei;
        private System.Windows.Forms.ColumnHeader main_balane;
        private System.Windows.Forms.ColumnHeader sub_balance;
        private System.Windows.Forms.ColumnHeader expire;
        private System.Windows.Forms.ColumnHeader note;
        private MaterialSkin.Controls.MaterialLabel displayLabel;
        private MaterialSkin.Controls.MaterialListView messageListView;
        private System.Windows.Forms.ColumnHeader msg_stt;
        private System.Windows.Forms.ColumnHeader com;
        private System.Windows.Forms.ColumnHeader from_tel;
        private System.Windows.Forms.ColumnHeader to_tel;
        private System.Windows.Forms.ColumnHeader message;
        private System.Windows.Forms.ColumnHeader msg_date;
        private System.Windows.Forms.ColumnHeader otp;
        private MaterialSkin.Controls.MaterialComboBox baudrateComboBox;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialButton materialButton1;
        private MaterialSkin.Controls.MaterialLabel num_of_port;
        private Guna.UI2.WinForms.Guna2AnimateWindow guna2AnimateWindow1;
        private System.Windows.Forms.ColumnHeader serial_sim;
        private MaterialSkin.Controls.MaterialComboBox languagesComboBox;
        private MaterialSkin.Controls.MaterialLabel materialLabel5;
        private MaterialSkin.Controls.MaterialLabel version_txt;
    }
}

