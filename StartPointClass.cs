using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ColorLightChanger
{


    public class StartPointClass : Form
    {
        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;

        private MenuItem ActivedMenuItem;
        private MenuItem DeactivedMenuItem;
        private MenuItem ExitMenuItem;

        private Boolean StatusLightsOn = false;
        private Boolean IsInit = false;
        private bool inited = false;
        private Button activedFormButton;
        private Button deactivedFormButton;
        private Label label_IP;
        private Button button_reload_lights;
        private TextBox textBoxIP;
        private Label label_lights;
        private DataGridView dataGridLights;
        private Label label_apikey;
        private TextBox textBoxAPIKey;
        private Label labelFallbackColor;
        private TextBox textBoxFallbackColor;
        private ColorLightChanger clc = new ColorLightChanger();
        private BindingSource lightRowBindingSource;
        private DataGridViewCheckBoxColumn isUsedDataGridViewCheckBoxColumn;
        private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private DataGridViewCheckBoxColumn isOnDataGridViewCheckBoxColumn;
        private System.ComponentModel.IContainer components;

        public bool AllowClose { get; set; }

        [STAThread]
        public static void Main()
        {
            Application.Run(new StartPointClass());
        }

        public StartPointClass()
        {
            InitializeComponent();
            // ContextMenu
            this.Icon = Images.flashlight;
            trayMenu = new ContextMenu();

            ActivedMenuItem = new MenuItem();
            DeactivedMenuItem = new MenuItem();
            ExitMenuItem = new MenuItem();

            ActivedMenuItem.Text = "Actived";
            DeactivedMenuItem.Text = "Deactived";
            ExitMenuItem.Text = "Exit";

            ActivedMenuItem.Click += new EventHandler(OnActived);
            DeactivedMenuItem.Click += new EventHandler(OnDeactived);
            ExitMenuItem.Click += new EventHandler(OnExit);

            trayMenu.MenuItems.Add(ActivedMenuItem);
            trayMenu.MenuItems.Add(DeactivedMenuItem);
            trayMenu.MenuItems.Add(ExitMenuItem);

            trayIcon = new NotifyIcon()
            {
                Text = "MovieMode",
                Icon = Images.flashlight,
                ContextMenu = trayMenu,
                Visible = true
            };
            trayIcon.DoubleClick += OnTaskbarDoubleClick;

            this.textBoxIP.Text = Config.defaultIP;
            this.textBoxAPIKey.Text = Config.defaultApiKey;
            this.textBoxFallbackColor.Text = Config.defaultcolor;
            this.OnStatusLightsOnChange(false);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.activedFormButton = new System.Windows.Forms.Button();
            this.deactivedFormButton = new System.Windows.Forms.Button();
            this.label_IP = new System.Windows.Forms.Label();
            this.button_reload_lights = new System.Windows.Forms.Button();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.label_lights = new System.Windows.Forms.Label();
            this.dataGridLights = new System.Windows.Forms.DataGridView();
            this.isUsedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isOnDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.lightRowBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label_apikey = new System.Windows.Forms.Label();
            this.textBoxAPIKey = new System.Windows.Forms.TextBox();
            this.labelFallbackColor = new System.Windows.Forms.Label();
            this.textBoxFallbackColor = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridLights)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lightRowBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // activedFormButton
            // 
            this.activedFormButton.Location = new System.Drawing.Point(90, 146);
            this.activedFormButton.Name = "activedFormButton";
            this.activedFormButton.Size = new System.Drawing.Size(139, 21);
            this.activedFormButton.TabIndex = 3;
            this.activedFormButton.Text = "Actived";
            this.activedFormButton.UseVisualStyleBackColor = true;
            this.activedFormButton.Click += new System.EventHandler(this.OnActived);
            // 
            // deactivedFormButton
            // 
            this.deactivedFormButton.Location = new System.Drawing.Point(235, 146);
            this.deactivedFormButton.Name = "deactivedFormButton";
            this.deactivedFormButton.Size = new System.Drawing.Size(137, 21);
            this.deactivedFormButton.TabIndex = 4;
            this.deactivedFormButton.Text = "Deactived";
            this.deactivedFormButton.UseVisualStyleBackColor = true;
            this.deactivedFormButton.Click += new System.EventHandler(this.OnDeactived);
            // 
            // label_IP
            // 
            this.label_IP.AutoSize = true;
            this.label_IP.Location = new System.Drawing.Point(161, 12);
            this.label_IP.Name = "label_IP";
            this.label_IP.Size = new System.Drawing.Size(17, 13);
            this.label_IP.TabIndex = 5;
            this.label_IP.Text = "IP";
            // 
            // button_reload_lights
            // 
            this.button_reload_lights.Location = new System.Drawing.Point(289, 9);
            this.button_reload_lights.Name = "button_reload_lights";
            this.button_reload_lights.Size = new System.Drawing.Size(83, 48);
            this.button_reload_lights.TabIndex = 6;
            this.button_reload_lights.Text = "Reload Lights";
            this.button_reload_lights.UseVisualStyleBackColor = true;
            this.button_reload_lights.Click += new System.EventHandler(this.onClickReload);
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(193, 9);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(88, 20);
            this.textBoxIP.TabIndex = 7;
            // 
            // label_lights
            // 
            this.label_lights.AutoSize = true;
            this.label_lights.Location = new System.Drawing.Point(12, 63);
            this.label_lights.Name = "label_lights";
            this.label_lights.Size = new System.Drawing.Size(35, 13);
            this.label_lights.TabIndex = 8;
            this.label_lights.Text = "Lights";
            // 
            // dataGridLights
            // 
            this.dataGridLights.AllowUserToAddRows = false;
            this.dataGridLights.AllowUserToDeleteRows = false;
            this.dataGridLights.AutoGenerateColumns = false;
            this.dataGridLights.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridLights.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.isUsedDataGridViewCheckBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.idDataGridViewTextBoxColumn,
            this.isOnDataGridViewCheckBoxColumn});
            this.dataGridLights.DataSource = this.lightRowBindingSource;
            this.dataGridLights.Location = new System.Drawing.Point(90, 63);
            this.dataGridLights.Name = "dataGridLights";
            this.dataGridLights.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridLights.Size = new System.Drawing.Size(282, 77);
            this.dataGridLights.TabIndex = 9;
            // 
            // isUsedDataGridViewCheckBoxColumn
            // 
            this.isUsedDataGridViewCheckBoxColumn.DataPropertyName = "isUsed";
            this.isUsedDataGridViewCheckBoxColumn.HeaderText = "Use it";
            this.isUsedDataGridViewCheckBoxColumn.Name = "isUsedDataGridViewCheckBoxColumn";
            this.isUsedDataGridViewCheckBoxColumn.Width = 40;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            this.nameDataGridViewTextBoxColumn.Width = 120;
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "id";
            this.idDataGridViewTextBoxColumn.HeaderText = "ID";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            this.idDataGridViewTextBoxColumn.Width = 40;
            // 
            // isOnDataGridViewCheckBoxColumn
            // 
            this.isOnDataGridViewCheckBoxColumn.DataPropertyName = "isOn";
            this.isOnDataGridViewCheckBoxColumn.HeaderText = "isOn";
            this.isOnDataGridViewCheckBoxColumn.Name = "isOnDataGridViewCheckBoxColumn";
            this.isOnDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isOnDataGridViewCheckBoxColumn.Width = 40;
            // 
            // lightRowBindingSource
            // 
            this.lightRowBindingSource.DataSource = typeof(LightRow);
            // 
            // label_apikey
            // 
            this.label_apikey.AutoSize = true;
            this.label_apikey.Location = new System.Drawing.Point(12, 36);
            this.label_apikey.Name = "label_apikey";
            this.label_apikey.Size = new System.Drawing.Size(45, 13);
            this.label_apikey.TabIndex = 10;
            this.label_apikey.Text = "API Key";
            // 
            // textBoxAPIKey
            // 
            this.textBoxAPIKey.Location = new System.Drawing.Point(90, 37);
            this.textBoxAPIKey.Name = "textBoxAPIKey";
            this.textBoxAPIKey.Size = new System.Drawing.Size(191, 20);
            this.textBoxAPIKey.TabIndex = 11;
            // 
            // labelFallbackColor
            // 
            this.labelFallbackColor.AutoSize = true;
            this.labelFallbackColor.Location = new System.Drawing.Point(12, 12);
            this.labelFallbackColor.Name = "labelFallbackColor";
            this.labelFallbackColor.Size = new System.Drawing.Size(74, 13);
            this.labelFallbackColor.TabIndex = 12;
            this.labelFallbackColor.Text = "Fallback Color";
            // 
            // textBoxFallbackColor
            // 
            this.textBoxFallbackColor.Location = new System.Drawing.Point(90, 9);
            this.textBoxFallbackColor.Name = "textBoxFallbackColor";
            this.textBoxFallbackColor.Size = new System.Drawing.Size(65, 20);
            this.textBoxFallbackColor.TabIndex = 13;
            // 
            // StartPointClass
            // 
            this.ClientSize = new System.Drawing.Size(384, 176);
            this.Controls.Add(this.textBoxFallbackColor);
            this.Controls.Add(this.labelFallbackColor);
            this.Controls.Add(this.textBoxAPIKey);
            this.Controls.Add(this.label_apikey);
            this.Controls.Add(this.dataGridLights);
            this.Controls.Add(this.label_lights);
            this.Controls.Add(this.textBoxIP);
            this.Controls.Add(this.button_reload_lights);
            this.Controls.Add(this.label_IP);
            this.Controls.Add(this.deactivedFormButton);
            this.Controls.Add(this.activedFormButton);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(400, 215);
            this.MinimumSize = new System.Drawing.Size(400, 215);
            this.Name = "StartPointClass";
            this.Text = "MovieMode for Philips Hue";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosingForm);
            this.Resize += new System.EventHandler(this.FormResize);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridLights)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lightRowBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void FormResize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                this.Hide();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false;
            ShowInTaskbar = false;
            base.OnLoad(e);
        }

        private void OnTaskbarDoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
        }

        private void OnStatusLightsOnChange(Boolean newStatus)
        {
            this.StatusLightsOn = newStatus;
            ActivedMenuItem.Visible = !StatusLightsOn;
            DeactivedMenuItem.Visible = StatusLightsOn;

            this.activedFormButton.Enabled = !StatusLightsOn;
            this.deactivedFormButton.Enabled = StatusLightsOn;
        }

        private void OnActived(object sender, EventArgs e)
        {
            this.clc.usingLights = new List<int>();
            foreach (LightRow row in this.lightRowBindingSource)
            {
                if (row != null)
                    this.clc.usingLights.Add(row.id);
            }
            this.clc.MovieModeOn();
            OnStatusLightsOnChange(true);
        }

        private void OnDeactived(object sender, EventArgs e)
        {
            this.clc.MovieModeOff();
            OnStatusLightsOnChange(false);
        }

        private void OnExit(object sender, EventArgs e)
        {
            this.AllowClose = true;
            Application.Exit();
        }

        private void OnClosingForm(object sender, FormClosingEventArgs e)
        {
            if (!this.AllowClose)
            {
                e.Cancel = true;
                Hide();
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                trayIcon.Dispose();
            }
            base.Dispose(isDisposing);
        }

        private void onClickReload(object sender, EventArgs e)
        {
            this.dataGridLights.Rows.Clear();
            Dictionary<int, Light> lights = clc.SetUp(this.textBoxIP.Text, this.textBoxAPIKey.Text);
            var i = 0;
            foreach (KeyValuePair<int, Light> light in lights)
            {
                this.lightRowBindingSource.Add(new LightRow(light.Value.name, light.Key, light.Value.state.on, false));
            }
        }
    }
}