namespace EmissaoAutomaticaNFSE.WebScrapingCore;

partial class EmissaoNFSe
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
        {
            this.parametrosLabel = new System.Windows.Forms.Label();
            this.tempoText = new System.Windows.Forms.TextBox();
            this.ativaDesativaNavegadorCheckBox = new System.Windows.Forms.CheckBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // parametrosLabel
            // 
            this.parametrosLabel.AutoSize = true;
            this.parametrosLabel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.parametrosLabel.ForeColor = System.Drawing.SystemColors.Control;
            this.parametrosLabel.Location = new System.Drawing.Point(11, 11);
            this.parametrosLabel.Name = "parametrosLabel";
            this.parametrosLabel.Size = new System.Drawing.Size(123, 16);
            this.parametrosLabel.TabIndex = 6;
            this.parametrosLabel.Text = "Tempo de execução";
            // 
            // tempoText
            // 
            this.tempoText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tempoText.Location = new System.Drawing.Point(14, 30);
            this.tempoText.Name = "tempoText";
            this.tempoText.Size = new System.Drawing.Size(498, 20);
            this.tempoText.Text = "300000";
            this.tempoText.TabIndex = 0;
            // 
            // ativaDesativaNavegadorCheckBox
            // 
            this.ativaDesativaNavegadorCheckBox.AutoSize = true;
            this.ativaDesativaNavegadorCheckBox.Checked = true;
            this.ativaDesativaNavegadorCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ativaDesativaNavegadorCheckBox.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ativaDesativaNavegadorCheckBox.ForeColor = System.Drawing.SystemColors.Control;
            this.ativaDesativaNavegadorCheckBox.Location = new System.Drawing.Point(14, 71);
            this.ativaDesativaNavegadorCheckBox.Name = "ativaDesativaNavegadorCheckBox";
            this.ativaDesativaNavegadorCheckBox.Size = new System.Drawing.Size(261, 20);
            this.ativaDesativaNavegadorCheckBox.TabIndex = 13;
            this.ativaDesativaNavegadorCheckBox.Text = "Ativa/Desativa visualização do navegador";
            this.ativaDesativaNavegadorCheckBox.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 97);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(500, 256);
            this.dataGridView1.TabIndex = 14;
            // 
            // EmissaoNFSE
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.ClientSize = new System.Drawing.Size(531, 365);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.ativaDesativaNavegadorCheckBox);
            this.Controls.Add(this.tempoText);
            this.Controls.Add(this.parametrosLabel);
            this.Name = "EmissaoNFSE";
            this.Text = "Emissão NFSE";
            this.Load += new System.EventHandler(this.EmissaoNFSE_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label parametrosLabel;
        private TextBox tempoText;
        private CheckBox ativaDesativaNavegadorCheckBox;
        private DataGridView dataGridView1;
}