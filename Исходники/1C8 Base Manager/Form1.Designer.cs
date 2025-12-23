namespace _1C8_Base_Manager
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.gridTemplates = new System.Windows.Forms.DataGridView();
            this.gridTarget = new System.Windows.Forms.DataGridView();
            this.comboUsers = new System.Windows.Forms.ComboBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.linkGithub = new System.Windows.Forms.LinkLabel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.comboComputers = new System.Windows.Forms.ComboBox();
            this.btnScan = new System.Windows.Forms.Button();
            this.lblLeft = new System.Windows.Forms.Label();
            this.lblRight = new System.Windows.Forms.Label();
            
            // Новые элементы для выбора источника
            this.txtSourcePath = new System.Windows.Forms.TextBox();
            this.btnSelectSource = new System.Windows.Forms.Button();
            this.btnDefaultSource = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.gridTemplates)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTarget)).BeginInit();
            this.SuspendLayout();
            
            // 
            // txtSourcePath (Путь к файлу-источнику)
            // 
            this.txtSourcePath.Location = new System.Drawing.Point(12, 25);
            this.txtSourcePath.Name = "txtSourcePath";
            this.txtSourcePath.ReadOnly = true; // Только для чтения
            this.txtSourcePath.Size = new System.Drawing.Size(300, 20);
            this.txtSourcePath.TabIndex = 100;
            
            // 
            // btnSelectSource (Кнопка "...")
            // 
            this.btnSelectSource.Location = new System.Drawing.Point(318, 23);
            this.btnSelectSource.Name = "btnSelectSource";
            this.btnSelectSource.Size = new System.Drawing.Size(30, 23);
            this.btnSelectSource.TabIndex = 101;
            this.btnSelectSource.Text = "...";
            this.btnSelectSource.UseVisualStyleBackColor = true;

            // 
            // btnDefaultSource (Кнопка "По умолчанию")
            // 
            this.btnDefaultSource.Location = new System.Drawing.Point(354, 23);
            this.btnDefaultSource.Name = "btnDefaultSource";
            this.btnDefaultSource.Size = new System.Drawing.Size(118, 23);
            this.btnDefaultSource.TabIndex = 102;
            this.btnDefaultSource.Text = "По умолчанию";
            this.btnDefaultSource.UseVisualStyleBackColor = true;

            // 
            // gridTemplates (Сдвинули чуть вниз)
            // 
            this.gridTemplates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTemplates.Location = new System.Drawing.Point(12, 55); 
            this.gridTemplates.Name = "gridTemplates";
            this.gridTemplates.Size = new System.Drawing.Size(460, 600);
            this.gridTemplates.TabIndex = 0;

            // 
            // lblLeft
            // 
            this.lblLeft.AutoSize = true;
            this.lblLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblLeft.Location = new System.Drawing.Point(12, 9);
            this.lblLeft.Name = "lblLeft";
            this.lblLeft.Size = new System.Drawing.Size(150, 13);
            this.lblLeft.TabIndex = 12;
            this.lblLeft.Text = "Источник баз (откуда):";

            // ---------------------------------------------------------
            // Правая часть (без изменений, но сдвинем чуть для красоты)
            // ---------------------------------------------------------
            
            // gridTarget
            this.gridTarget.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTarget.Location = new System.Drawing.Point(540, 55);
            this.gridTarget.Name = "gridTarget";
            this.gridTarget.Size = new System.Drawing.Size(460, 600);
            this.gridTarget.TabIndex = 1;

            // comboComputers
            this.comboComputers.FormattingEnabled = true;
            this.comboComputers.Location = new System.Drawing.Point(540, 25);
            this.comboComputers.Name = "comboComputers";
            this.comboComputers.Size = new System.Drawing.Size(150, 21);
            this.comboComputers.TabIndex = 10;
            this.comboComputers.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboComputers.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;

            // btnScan
            this.btnScan.Location = new System.Drawing.Point(700, 23);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(90, 23);
            this.btnScan.TabIndex = 11;
            this.btnScan.Text = "Подключиться";
            this.btnScan.UseVisualStyleBackColor = true;

            // comboUsers
            this.comboUsers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboUsers.FormattingEnabled = true;
            this.comboUsers.Location = new System.Drawing.Point(800, 25);
            this.comboUsers.Name = "comboUsers";
            this.comboUsers.Size = new System.Drawing.Size(200, 21);
            this.comboUsers.TabIndex = 3;

            // lblRight
            this.lblRight.AutoSize = true;
            this.lblRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblRight.Location = new System.Drawing.Point(540, 9);
            this.lblRight.Name = "lblRight";
            this.lblRight.Size = new System.Drawing.Size(115, 13);
            this.lblRight.TabIndex = 13;
            this.lblRight.Text = "Получатель (куда):";

            // Остальные кнопки
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnAdd.Location = new System.Drawing.Point(485, 300);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(40, 40);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "➡";
            this.btnAdd.UseVisualStyleBackColor = true;

            this.btnDelete.Location = new System.Drawing.Point(540, 665);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 23);
            this.btnDelete.TabIndex = 8;
            this.btnDelete.Text = "Удалить базу";
            this.btnDelete.UseVisualStyleBackColor = true;

            this.btnSave.Location = new System.Drawing.Point(650, 665);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 23);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Бэкап (Backup)";
            this.btnSave.UseVisualStyleBackColor = true;

            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(12, 695);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(38, 13);
            this.statusLabel.TabIndex = 6;
            this.statusLabel.Text = "Готов.";

            this.linkGithub.AutoSize = true;
            this.linkGithub.Location = new System.Drawing.Point(900, 695);
            this.linkGithub.Name = "linkGithub";
            this.linkGithub.Size = new System.Drawing.Size(40, 13);
            this.linkGithub.TabIndex = 7;
            this.linkGithub.TabStop = true;
            this.linkGithub.Text = "GitHub";

            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1014, 721);
            
            this.Controls.Add(this.btnDefaultSource);
            this.Controls.Add(this.btnSelectSource);
            this.Controls.Add(this.txtSourcePath);
            
            this.Controls.Add(this.lblRight);
            this.Controls.Add(this.lblLeft);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.comboComputers);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.linkGithub);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.comboUsers);
            this.Controls.Add(this.gridTarget);
            this.Controls.Add(this.gridTemplates);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "1C8 Manager (Admin Edition)";
            ((System.ComponentModel.ISupportInitialize)(this.gridTemplates)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTarget)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView gridTemplates;
        private System.Windows.Forms.DataGridView gridTarget;
        private System.Windows.Forms.ComboBox comboUsers;
        private System.Windows.Forms.ComboBox comboComputers;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.LinkLabel linkGithub;
        private System.Windows.Forms.Label lblLeft;
        private System.Windows.Forms.Label lblRight;
        
        // Новые переменные
        private System.Windows.Forms.TextBox txtSourcePath;
        private System.Windows.Forms.Button btnSelectSource;
        private System.Windows.Forms.Button btnDefaultSource;
    }
}