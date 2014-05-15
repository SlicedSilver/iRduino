using iRduino.Classes;
using iRduino.Properties;
namespace Gui2
{
    
    partial class main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(main));
            this.StartButtonLabel = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.abrirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editarConfiguraçãoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.teste1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ConnectionStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.CurrentConfigurationLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.testarComunicaoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.arquivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configuracoesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ComPortBox = new System.Windows.Forms.ToolStripComboBox();
            this.sairToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // StartButtonLabel
            // 
            this.StartButtonLabel.Font = new System.Drawing.Font("Georgia", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartButtonLabel.Image = ((System.Drawing.Image)(resources.GetObject("StartButtonLabel.Image")));
            this.StartButtonLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.StartButtonLabel.Location = new System.Drawing.Point(12, 28);
            this.StartButtonLabel.Name = "StartButtonLabel";
            this.StartButtonLabel.Size = new System.Drawing.Size(385, 72);
            this.StartButtonLabel.TabIndex = 0;
            this.StartButtonLabel.Text = "Iniciar";
            this.StartButtonLabel.UseVisualStyleBackColor = true;
            this.StartButtonLabel.Click += new System.EventHandler(this.StartButtonLabel_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.BalloonTipText = "Continuamos rodando..";
            this.notifyIcon1.BalloonTipTitle = "LekoDuino";
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "LekoDuino";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.abrirToolStripMenuItem,
            this.editarConfiguraçãoToolStripMenuItem,
            this.teste1ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(116, 70);
            // 
            // abrirToolStripMenuItem
            // 
            this.abrirToolStripMenuItem.Name = "abrirToolStripMenuItem";
            this.abrirToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.abrirToolStripMenuItem.Text = "Mostrar";
            this.abrirToolStripMenuItem.Click += new System.EventHandler(this.abrirToolStripMenuItem_Click);
            // 
            // editarConfiguraçãoToolStripMenuItem
            // 
            this.editarConfiguraçãoToolStripMenuItem.Name = "editarConfiguraçãoToolStripMenuItem";
            this.editarConfiguraçãoToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            // 
            // teste1ToolStripMenuItem
            // 
            this.teste1ToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.teste1ToolStripMenuItem.Name = "teste1ToolStripMenuItem";
            this.teste1ToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.teste1ToolStripMenuItem.Text = "Sair";
            this.teste1ToolStripMenuItem.Click += new System.EventHandler(this.teste1ToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ConnectionStatusLabel,
            this.toolStripStatusLabel1,
            this.CurrentConfigurationLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 102);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(409, 24);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ConnectionStatusLabel
            // 
            this.ConnectionStatusLabel.Name = "ConnectionStatusLabel";
            this.ConnectionStatusLabel.Size = new System.Drawing.Size(0, 19);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(117, 19);
            this.toolStripStatusLabel1.Text = "Configuração Atual:";
            this.toolStripStatusLabel1.Click += new System.EventHandler(this.toolStripStatusLabel1_Click);
            // 
            // CurrentConfigurationLabel
            // 
            this.CurrentConfigurationLabel.Name = "CurrentConfigurationLabel";
            this.CurrentConfigurationLabel.Size = new System.Drawing.Size(0, 19);
            // 
            // testarComunicaoToolStripMenuItem
            // 
            this.testarComunicaoToolStripMenuItem.Name = "testarComunicaoToolStripMenuItem";
            this.testarComunicaoToolStripMenuItem.Size = new System.Drawing.Size(116, 23);
            this.testarComunicaoToolStripMenuItem.Text = "Testar Comunicao";
            this.testarComunicaoToolStripMenuItem.Click += new System.EventHandler(this.testarComunicaoToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.arquivoToolStripMenuItem,
            this.testarComunicaoToolStripMenuItem,
            this.ComPortBox,
            this.sairToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(409, 27);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // arquivoToolStripMenuItem
            // 
            this.arquivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configuracoesToolStripMenuItem});
            this.arquivoToolStripMenuItem.Name = "arquivoToolStripMenuItem";
            this.arquivoToolStripMenuItem.Size = new System.Drawing.Size(61, 23);
            this.arquivoToolStripMenuItem.Text = "Arquivo";
            // 
            // configuracoesToolStripMenuItem
            // 
            this.configuracoesToolStripMenuItem.Name = "configuracoesToolStripMenuItem";
            this.configuracoesToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.configuracoesToolStripMenuItem.Text = "Configurações";
            // 
            // ComPortBox
            // 
            this.ComPortBox.Name = "ComPortBox";
            this.ComPortBox.Size = new System.Drawing.Size(75, 23);
            this.ComPortBox.Text = "Porta COM";
            this.ComPortBox.SelectedIndexChanged += new System.EventHandler(this.ComPortBox_SelectedIndexChanged_1);
            // 
            // sairToolStripMenuItem
            // 
            this.sairToolStripMenuItem.Name = "sairToolStripMenuItem";
            this.sairToolStripMenuItem.Size = new System.Drawing.Size(38, 23);
            this.sairToolStripMenuItem.Text = "Sair";
            this.sairToolStripMenuItem.Click += new System.EventHandler(this.sairToolStripMenuItem_Click);
            // 
            // main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 126);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.StartButtonLabel);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "main";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LekoRacing2";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.Shown += new System.EventHandler(this.Main_Shown);
            this.Resize += new System.EventHandler(this.Main_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartButtonLabel;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ConnectionStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        public System.Windows.Forms.ToolStripStatusLabel CurrentConfigurationLabel;
        private System.Windows.Forms.ToolStripMenuItem testarComunicaoToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripComboBox ComPortBox;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem teste1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sairToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem abrirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem arquivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configuracoesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editarConfiguraçãoToolStripMenuItem;
    }
}

