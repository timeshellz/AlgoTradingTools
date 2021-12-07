
namespace AlgoTrading.DQN.UserInterface
{
    partial class SelectStockForm
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
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.stockSymbolBox = new System.Windows.Forms.TextBox();
            this.addStockDataButton = new System.Windows.Forms.Button();
            this.availableStocksBox = new System.Windows.Forms.ListBox();
            this.label14 = new System.Windows.Forms.Label();
            this.downloadButton = new System.Windows.Forms.Button();
            this.removeAddedButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.addAllButton = new System.Windows.Forms.Button();
            this.removeAllButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.intervalBox = new System.Windows.Forms.ListBox();
            this.loadedStocksBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 20);
            this.label3.TabIndex = 15;
            this.label3.Text = "Choose Stocks";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 20);
            this.label1.TabIndex = 12;
            this.label1.Text = "New Stock";
            // 
            // stockSymbolBox
            // 
            this.stockSymbolBox.Location = new System.Drawing.Point(12, 69);
            this.stockSymbolBox.Name = "stockSymbolBox";
            this.stockSymbolBox.Size = new System.Drawing.Size(194, 22);
            this.stockSymbolBox.TabIndex = 11;
            // 
            // addStockDataButton
            // 
            this.addStockDataButton.Location = new System.Drawing.Point(223, 342);
            this.addStockDataButton.Name = "addStockDataButton";
            this.addStockDataButton.Size = new System.Drawing.Size(194, 39);
            this.addStockDataButton.TabIndex = 19;
            this.addStockDataButton.Text = "Add";
            this.addStockDataButton.UseVisualStyleBackColor = true;
            this.addStockDataButton.Click += new System.EventHandler(this.addStockDataButton_Click);
            // 
            // availableStocksBox
            // 
            this.availableStocksBox.FormattingEnabled = true;
            this.availableStocksBox.ItemHeight = 16;
            this.availableStocksBox.Location = new System.Drawing.Point(223, 69);
            this.availableStocksBox.Name = "availableStocksBox";
            this.availableStocksBox.Size = new System.Drawing.Size(194, 260);
            this.availableStocksBox.TabIndex = 18;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(219, 46);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(132, 20);
            this.label14.TabIndex = 17;
            this.label14.Text = "Available Stocks";
            // 
            // downloadButton
            // 
            this.downloadButton.Location = new System.Drawing.Point(12, 342);
            this.downloadButton.Name = "downloadButton";
            this.downloadButton.Size = new System.Drawing.Size(194, 67);
            this.downloadButton.TabIndex = 20;
            this.downloadButton.Text = "Download";
            this.downloadButton.UseVisualStyleBackColor = true;
            this.downloadButton.Click += new System.EventHandler(this.downloadButton_Click);
            // 
            // removeAddedButton
            // 
            this.removeAddedButton.Location = new System.Drawing.Point(426, 342);
            this.removeAddedButton.Name = "removeAddedButton";
            this.removeAddedButton.Size = new System.Drawing.Size(194, 39);
            this.removeAddedButton.TabIndex = 23;
            this.removeAddedButton.Text = "Remove";
            this.removeAddedButton.UseVisualStyleBackColor = true;
            this.removeAddedButton.Click += new System.EventHandler(this.removeAddedButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(423, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 20);
            this.label2.TabIndex = 21;
            this.label2.Text = "Added Stocks";
            // 
            // addAllButton
            // 
            this.addAllButton.Location = new System.Drawing.Point(223, 387);
            this.addAllButton.Name = "addAllButton";
            this.addAllButton.Size = new System.Drawing.Size(194, 22);
            this.addAllButton.TabIndex = 24;
            this.addAllButton.Text = "Add All";
            this.addAllButton.UseVisualStyleBackColor = true;
            this.addAllButton.Click += new System.EventHandler(this.addAllButton_Click);
            // 
            // removeAllButton
            // 
            this.removeAllButton.Location = new System.Drawing.Point(426, 387);
            this.removeAllButton.Name = "removeAllButton";
            this.removeAllButton.Size = new System.Drawing.Size(194, 22);
            this.removeAllButton.TabIndex = 25;
            this.removeAllButton.Text = "Remove All";
            this.removeAllButton.UseVisualStyleBackColor = true;
            this.removeAllButton.Click += new System.EventHandler(this.removeAllButton_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Location = new System.Drawing.Point(393, 422);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 31);
            this.acceptButton.TabIndex = 27;
            this.acceptButton.Text = "Accept";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(510, 422);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 31);
            this.cancelButton.TabIndex = 26;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // intervalBox
            // 
            this.intervalBox.FormattingEnabled = true;
            this.intervalBox.ItemHeight = 16;
            this.intervalBox.Location = new System.Drawing.Point(12, 98);
            this.intervalBox.Name = "intervalBox";
            this.intervalBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.intervalBox.Size = new System.Drawing.Size(194, 228);
            this.intervalBox.TabIndex = 28;
            // 
            // loadedStocksBox
            // 
            this.loadedStocksBox.FormattingEnabled = true;
            this.loadedStocksBox.ItemHeight = 16;
            this.loadedStocksBox.Location = new System.Drawing.Point(426, 69);
            this.loadedStocksBox.Name = "loadedStocksBox";
            this.loadedStocksBox.Size = new System.Drawing.Size(194, 260);
            this.loadedStocksBox.TabIndex = 29;
            // 
            // SelectStockForm
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(632, 465);
            this.Controls.Add(this.loadedStocksBox);
            this.Controls.Add(this.intervalBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.removeAllButton);
            this.Controls.Add(this.addAllButton);
            this.Controls.Add(this.removeAddedButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.downloadButton);
            this.Controls.Add(this.addStockDataButton);
            this.Controls.Add(this.availableStocksBox);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.stockSymbolBox);
            this.MaximumSize = new System.Drawing.Size(650, 512);
            this.MinimumSize = new System.Drawing.Size(650, 512);
            this.Name = "SelectStockForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Stock";
            this.Shown += new System.EventHandler(this.SelectStockForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox stockSymbolBox;
        private System.Windows.Forms.Button addStockDataButton;
        private System.Windows.Forms.ListBox availableStocksBox;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button downloadButton;
        private System.Windows.Forms.Button removeAddedButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button addAllButton;
        private System.Windows.Forms.Button removeAllButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ListBox intervalBox;
        private System.Windows.Forms.ListBox loadedStocksBox;
    }
}