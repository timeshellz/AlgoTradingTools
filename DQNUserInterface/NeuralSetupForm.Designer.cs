
namespace AlgoTrading.DQN.UserInterface
{
    partial class NeuralSetupForm
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
            this.initialCapitalTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.commissionTextBox = new System.Windows.Forms.TextBox();
            this.brokerSettingsPanel = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.neuralSettingsPanel = new System.Windows.Forms.Panel();
            this.networkNameTextBox = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.learningRateTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.hiddenLayersTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.agentSettingsPanel = new System.Windows.Forms.Panel();
            this.tauTextBox = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.stepCountTextBox = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.agentNameTextBox = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.minEpsilonTextBox = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.epsilonDecayTextBox = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.epsilonTextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.discountTextBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.batchSizeTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.bufferSizeTextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.formFillLabel = new System.Windows.Forms.Label();
            this.brokerSettingsPanel.SuspendLayout();
            this.neuralSettingsPanel.SuspendLayout();
            this.agentSettingsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // initialCapitalTextBox
            // 
            this.initialCapitalTextBox.Location = new System.Drawing.Point(195, 36);
            this.initialCapitalTextBox.Name = "initialCapitalTextBox";
            this.initialCapitalTextBox.Size = new System.Drawing.Size(202, 22);
            this.initialCapitalTextBox.TabIndex = 0;
            this.initialCapitalTextBox.Tag = "Initial Capital";
            this.initialCapitalTextBox.Text = "600";
            this.initialCapitalTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Initial Capital";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Commission";
            // 
            // commissionTextBox
            // 
            this.commissionTextBox.Location = new System.Drawing.Point(195, 64);
            this.commissionTextBox.Name = "commissionTextBox";
            this.commissionTextBox.Size = new System.Drawing.Size(202, 22);
            this.commissionTextBox.TabIndex = 2;
            this.commissionTextBox.Tag = "Commission";
            this.commissionTextBox.Text = "0.03";
            this.commissionTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // brokerSettingsPanel
            // 
            this.brokerSettingsPanel.Controls.Add(this.label3);
            this.brokerSettingsPanel.Controls.Add(this.commissionTextBox);
            this.brokerSettingsPanel.Controls.Add(this.label2);
            this.brokerSettingsPanel.Controls.Add(this.initialCapitalTextBox);
            this.brokerSettingsPanel.Controls.Add(this.label1);
            this.brokerSettingsPanel.Location = new System.Drawing.Point(12, 12);
            this.brokerSettingsPanel.Name = "brokerSettingsPanel";
            this.brokerSettingsPanel.Size = new System.Drawing.Size(400, 91);
            this.brokerSettingsPanel.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Emulated Broker";
            // 
            // neuralSettingsPanel
            // 
            this.neuralSettingsPanel.Controls.Add(this.networkNameTextBox);
            this.neuralSettingsPanel.Controls.Add(this.label15);
            this.neuralSettingsPanel.Controls.Add(this.label4);
            this.neuralSettingsPanel.Controls.Add(this.learningRateTextBox);
            this.neuralSettingsPanel.Controls.Add(this.label5);
            this.neuralSettingsPanel.Controls.Add(this.hiddenLayersTextBox);
            this.neuralSettingsPanel.Controls.Add(this.label6);
            this.neuralSettingsPanel.Location = new System.Drawing.Point(12, 109);
            this.neuralSettingsPanel.Name = "neuralSettingsPanel";
            this.neuralSettingsPanel.Size = new System.Drawing.Size(400, 120);
            this.neuralSettingsPanel.TabIndex = 5;
            // 
            // networkNameTextBox
            // 
            this.networkNameTextBox.Location = new System.Drawing.Point(195, 36);
            this.networkNameTextBox.Name = "networkNameTextBox";
            this.networkNameTextBox.Size = new System.Drawing.Size(202, 22);
            this.networkNameTextBox.TabIndex = 15;
            this.networkNameTextBox.Tag = "Buffer Size";
            this.networkNameTextBox.Text = "New Network";
            this.networkNameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(45, 39);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(100, 17);
            this.label15.TabIndex = 16;
            this.label15.Text = "Network Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 20);
            this.label4.TabIndex = 4;
            this.label4.Text = "Neural Network";
            // 
            // learningRateTextBox
            // 
            this.learningRateTextBox.Location = new System.Drawing.Point(195, 92);
            this.learningRateTextBox.Name = "learningRateTextBox";
            this.learningRateTextBox.Size = new System.Drawing.Size(202, 22);
            this.learningRateTextBox.TabIndex = 2;
            this.learningRateTextBox.Tag = "Learning Rate";
            this.learningRateTextBox.Text = "0.01";
            this.learningRateTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(45, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 17);
            this.label5.TabIndex = 3;
            this.label5.Text = "Learning Rate";
            // 
            // hiddenLayersTextBox
            // 
            this.hiddenLayersTextBox.Location = new System.Drawing.Point(195, 64);
            this.hiddenLayersTextBox.Name = "hiddenLayersTextBox";
            this.hiddenLayersTextBox.Size = new System.Drawing.Size(202, 22);
            this.hiddenLayersTextBox.TabIndex = 0;
            this.hiddenLayersTextBox.Tag = "Hidden Layers";
            this.hiddenLayersTextBox.Text = "3";
            this.hiddenLayersTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(45, 67);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 17);
            this.label6.TabIndex = 1;
            this.label6.Text = "Hidden Layers";
            // 
            // agentSettingsPanel
            // 
            this.agentSettingsPanel.Controls.Add(this.tauTextBox);
            this.agentSettingsPanel.Controls.Add(this.label17);
            this.agentSettingsPanel.Controls.Add(this.stepCountTextBox);
            this.agentSettingsPanel.Controls.Add(this.label16);
            this.agentSettingsPanel.Controls.Add(this.agentNameTextBox);
            this.agentSettingsPanel.Controls.Add(this.label14);
            this.agentSettingsPanel.Controls.Add(this.minEpsilonTextBox);
            this.agentSettingsPanel.Controls.Add(this.label13);
            this.agentSettingsPanel.Controls.Add(this.epsilonDecayTextBox);
            this.agentSettingsPanel.Controls.Add(this.label12);
            this.agentSettingsPanel.Controls.Add(this.epsilonTextBox);
            this.agentSettingsPanel.Controls.Add(this.label10);
            this.agentSettingsPanel.Controls.Add(this.discountTextBox);
            this.agentSettingsPanel.Controls.Add(this.label11);
            this.agentSettingsPanel.Controls.Add(this.label7);
            this.agentSettingsPanel.Controls.Add(this.batchSizeTextBox);
            this.agentSettingsPanel.Controls.Add(this.label8);
            this.agentSettingsPanel.Controls.Add(this.bufferSizeTextBox);
            this.agentSettingsPanel.Controls.Add(this.label9);
            this.agentSettingsPanel.Location = new System.Drawing.Point(12, 235);
            this.agentSettingsPanel.Name = "agentSettingsPanel";
            this.agentSettingsPanel.Size = new System.Drawing.Size(400, 290);
            this.agentSettingsPanel.TabIndex = 6;
            // 
            // tauTextBox
            // 
            this.tauTextBox.Location = new System.Drawing.Point(195, 261);
            this.tauTextBox.Name = "tauTextBox";
            this.tauTextBox.Size = new System.Drawing.Size(202, 22);
            this.tauTextBox.TabIndex = 17;
            this.tauTextBox.Text = "0.01";
            this.tauTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(45, 264);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(33, 17);
            this.label17.TabIndex = 18;
            this.label17.Text = "Tau";
            // 
            // stepCountTextBox
            // 
            this.stepCountTextBox.Location = new System.Drawing.Point(195, 121);
            this.stepCountTextBox.Name = "stepCountTextBox";
            this.stepCountTextBox.Size = new System.Drawing.Size(202, 22);
            this.stepCountTextBox.TabIndex = 15;
            this.stepCountTextBox.Tag = "Batch Size";
            this.stepCountTextBox.Text = "10";
            this.stepCountTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(45, 124);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(78, 17);
            this.label16.TabIndex = 16;
            this.label16.Text = "Step Count";
            // 
            // agentNameTextBox
            // 
            this.agentNameTextBox.Location = new System.Drawing.Point(195, 37);
            this.agentNameTextBox.Name = "agentNameTextBox";
            this.agentNameTextBox.Size = new System.Drawing.Size(202, 22);
            this.agentNameTextBox.TabIndex = 13;
            this.agentNameTextBox.Tag = "Buffer Size";
            this.agentNameTextBox.Text = "New Agent";
            this.agentNameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(45, 40);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(86, 17);
            this.label14.TabIndex = 14;
            this.label14.Text = "Agent Name";
            // 
            // minEpsilonTextBox
            // 
            this.minEpsilonTextBox.Location = new System.Drawing.Point(195, 233);
            this.minEpsilonTextBox.Name = "minEpsilonTextBox";
            this.minEpsilonTextBox.Size = new System.Drawing.Size(202, 22);
            this.minEpsilonTextBox.TabIndex = 11;
            this.minEpsilonTextBox.Text = "0.1";
            this.minEpsilonTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(45, 236);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(113, 17);
            this.label13.TabIndex = 12;
            this.label13.Text = "Minimum Epsilon";
            // 
            // epsilonDecayTextBox
            // 
            this.epsilonDecayTextBox.Location = new System.Drawing.Point(195, 205);
            this.epsilonDecayTextBox.Name = "epsilonDecayTextBox";
            this.epsilonDecayTextBox.Size = new System.Drawing.Size(202, 22);
            this.epsilonDecayTextBox.TabIndex = 9;
            this.epsilonDecayTextBox.Text = "0.99";
            this.epsilonDecayTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(45, 208);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(98, 17);
            this.label12.TabIndex = 10;
            this.label12.Text = "Epsilon Decay";
            // 
            // epsilonTextBox
            // 
            this.epsilonTextBox.Location = new System.Drawing.Point(195, 177);
            this.epsilonTextBox.Name = "epsilonTextBox";
            this.epsilonTextBox.Size = new System.Drawing.Size(202, 22);
            this.epsilonTextBox.TabIndex = 7;
            this.epsilonTextBox.Tag = "Epsilon";
            this.epsilonTextBox.Text = "0.99";
            this.epsilonTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(45, 180);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(54, 17);
            this.label10.TabIndex = 8;
            this.label10.Text = "Epsilon";
            // 
            // discountTextBox
            // 
            this.discountTextBox.Location = new System.Drawing.Point(195, 149);
            this.discountTextBox.Name = "discountTextBox";
            this.discountTextBox.Size = new System.Drawing.Size(202, 22);
            this.discountTextBox.TabIndex = 5;
            this.discountTextBox.Tag = "Discount";
            this.discountTextBox.Text = "0.999";
            this.discountTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(45, 152);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(63, 17);
            this.label11.TabIndex = 6;
            this.label11.Text = "Discount";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(12, 10);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 20);
            this.label7.TabIndex = 4;
            this.label7.Text = "Agent";
            // 
            // batchSizeTextBox
            // 
            this.batchSizeTextBox.Location = new System.Drawing.Point(195, 93);
            this.batchSizeTextBox.Name = "batchSizeTextBox";
            this.batchSizeTextBox.Size = new System.Drawing.Size(202, 22);
            this.batchSizeTextBox.TabIndex = 2;
            this.batchSizeTextBox.Tag = "Batch Size";
            this.batchSizeTextBox.Text = "32";
            this.batchSizeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(45, 96);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 17);
            this.label8.TabIndex = 3;
            this.label8.Text = "Batch Size";
            // 
            // bufferSizeTextBox
            // 
            this.bufferSizeTextBox.Location = new System.Drawing.Point(195, 65);
            this.bufferSizeTextBox.Name = "bufferSizeTextBox";
            this.bufferSizeTextBox.Size = new System.Drawing.Size(202, 22);
            this.bufferSizeTextBox.TabIndex = 0;
            this.bufferSizeTextBox.Tag = "Buffer Size";
            this.bufferSizeTextBox.Text = "500000";
            this.bufferSizeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(45, 68);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 17);
            this.label9.TabIndex = 1;
            this.label9.Text = "Buffer Size";
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(302, 531);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(110, 31);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Location = new System.Drawing.Point(186, 531);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(110, 31);
            this.acceptButton.TabIndex = 9;
            this.acceptButton.Text = "Accept";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // formFillLabel
            // 
            this.formFillLabel.AutoSize = true;
            this.formFillLabel.Location = new System.Drawing.Point(13, 538);
            this.formFillLabel.Name = "formFillLabel";
            this.formFillLabel.Size = new System.Drawing.Size(129, 17);
            this.formFillLabel.TabIndex = 10;
            this.formFillLabel.Text = "Not All Forms Filled";
            this.formFillLabel.Visible = false;
            // 
            // NeuralSetupForm
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(418, 568);
            this.Controls.Add(this.formFillLabel);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.agentSettingsPanel);
            this.Controls.Add(this.neuralSettingsPanel);
            this.Controls.Add(this.brokerSettingsPanel);
            this.MaximumSize = new System.Drawing.Size(436, 615);
            this.MinimumSize = new System.Drawing.Size(436, 615);
            this.Name = "NeuralSetupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Setup";
            this.Load += new System.EventHandler(this.NeuralSetupForm_Load);
            this.Shown += new System.EventHandler(this.NeuralSetupForm_Shown);
            this.brokerSettingsPanel.ResumeLayout(false);
            this.brokerSettingsPanel.PerformLayout();
            this.neuralSettingsPanel.ResumeLayout(false);
            this.neuralSettingsPanel.PerformLayout();
            this.agentSettingsPanel.ResumeLayout(false);
            this.agentSettingsPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox initialCapitalTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox commissionTextBox;
        private System.Windows.Forms.Panel brokerSettingsPanel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel neuralSettingsPanel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox learningRateTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox hiddenLayersTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel agentSettingsPanel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox batchSizeTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox bufferSizeTextBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox epsilonTextBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox discountTextBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox epsilonDecayTextBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox minEpsilonTextBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Label formFillLabel;
        private System.Windows.Forms.TextBox networkNameTextBox;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox agentNameTextBox;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tauTextBox;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox stepCountTextBox;
        private System.Windows.Forms.Label label16;
    }
}