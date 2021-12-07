
namespace AlgoTrading.DQN.UserInterface
{
    partial class MainForm
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
            this.mainPlotView = new OxyPlot.WindowsForms.PlotView();
            this.setupPanel = new System.Windows.Forms.Panel();
            this.createDQNButton = new System.Windows.Forms.Button();
            this.loadDQNButton = new System.Windows.Forms.Button();
            this.pauseButton = new System.Windows.Forms.Button();
            this.overallPanel = new System.Windows.Forms.Panel();
            this.overallLabel = new System.Windows.Forms.Label();
            this.maxQLabel = new System.Windows.Forms.Label();
            this.maxQTextBox = new System.Windows.Forms.TextBox();
            this.totalMemoriesTextBox = new System.Windows.Forms.TextBox();
            this.memoriesCountLabel = new System.Windows.Forms.Label();
            this.epochCountLabel = new System.Windows.Forms.Label();
            this.totalEpochsTextBox = new System.Windows.Forms.TextBox();
            this.minRewardTextBox = new System.Windows.Forms.TextBox();
            this.minRewardLabel = new System.Windows.Forms.Label();
            this.maxRewardLabel = new System.Windows.Forms.Label();
            this.maxRewardTextBox = new System.Windows.Forms.TextBox();
            this.currentPanel = new System.Windows.Forms.Panel();
            this.currentEpisodesCountLabel = new System.Windows.Forms.Label();
            this.currentEpochLabel = new System.Windows.Forms.Label();
            this.currentMaxQLabel = new System.Windows.Forms.Label();
            this.currentMaxQTextBox = new System.Windows.Forms.TextBox();
            this.currentMemoriesCountTextBox = new System.Windows.Forms.TextBox();
            this.currentMemoriesCountLabel = new System.Windows.Forms.Label();
            this.currentEposidesCountTextBox = new System.Windows.Forms.TextBox();
            this.currentMinRewardTextBox = new System.Windows.Forms.TextBox();
            this.currentMinRewardLabel = new System.Windows.Forms.Label();
            this.currentMaxRewardLabel = new System.Windows.Forms.Label();
            this.currentMaxRewardTextBox = new System.Windows.Forms.TextBox();
            this.brokerStatPanel = new System.Windows.Forms.Panel();
            this.avgDurationPlotView = new OxyPlot.WindowsForms.PlotView();
            this.avgProfitPlotView = new OxyPlot.WindowsForms.PlotView();
            this.brokerLabel = new System.Windows.Forms.Label();
            this.currentSettingsPanel = new System.Windows.Forms.Panel();
            this.loadedStocksTextBox = new System.Windows.Forms.ListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.epsilonTextBox = new System.Windows.Forms.TextBox();
            this.minimumEpsilonTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.epsilonDecayTextBox = new System.Windows.Forms.TextBox();
            this.discountTextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.batchSizeTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.hiddenLayersTextBox = new System.Windows.Forms.TextBox();
            this.bufferSizeTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.learningRateTextBox = new System.Windows.Forms.TextBox();
            this.commissionTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.initialCapitalTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.plotUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.lossPlotView = new OxyPlot.WindowsForms.PlotView();
            this.epsilonPlotView = new OxyPlot.WindowsForms.PlotView();
            this.tradeCountPlotView = new OxyPlot.WindowsForms.PlotView();
            this.setupPanel.SuspendLayout();
            this.overallPanel.SuspendLayout();
            this.currentPanel.SuspendLayout();
            this.brokerStatPanel.SuspendLayout();
            this.currentSettingsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPlotView
            // 
            this.mainPlotView.Location = new System.Drawing.Point(281, 6);
            this.mainPlotView.Name = "mainPlotView";
            this.mainPlotView.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.mainPlotView.Size = new System.Drawing.Size(952, 295);
            this.mainPlotView.TabIndex = 0;
            this.mainPlotView.Text = "rewardPlot";
            this.mainPlotView.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.mainPlotView.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.mainPlotView.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // setupPanel
            // 
            this.setupPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.setupPanel.Controls.Add(this.createDQNButton);
            this.setupPanel.Controls.Add(this.loadDQNButton);
            this.setupPanel.Controls.Add(this.pauseButton);
            this.setupPanel.Location = new System.Drawing.Point(972, 594);
            this.setupPanel.Name = "setupPanel";
            this.setupPanel.Size = new System.Drawing.Size(260, 185);
            this.setupPanel.TabIndex = 24;
            // 
            // createDQNButton
            // 
            this.createDQNButton.Location = new System.Drawing.Point(2, 11);
            this.createDQNButton.Name = "createDQNButton";
            this.createDQNButton.Size = new System.Drawing.Size(254, 53);
            this.createDQNButton.TabIndex = 4;
            this.createDQNButton.Text = "Create";
            this.createDQNButton.UseVisualStyleBackColor = true;
            this.createDQNButton.Click += new System.EventHandler(this.createDQNButton_Click);
            // 
            // loadDQNButton
            // 
            this.loadDQNButton.Location = new System.Drawing.Point(2, 66);
            this.loadDQNButton.Name = "loadDQNButton";
            this.loadDQNButton.Size = new System.Drawing.Size(254, 53);
            this.loadDQNButton.TabIndex = 3;
            this.loadDQNButton.Text = "Load";
            this.loadDQNButton.UseVisualStyleBackColor = true;
            this.loadDQNButton.Click += new System.EventHandler(this.loadDQNButton_Click);
            // 
            // pauseButton
            // 
            this.pauseButton.Enabled = false;
            this.pauseButton.Location = new System.Drawing.Point(2, 121);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(254, 53);
            this.pauseButton.TabIndex = 2;
            this.pauseButton.Text = "Pause";
            this.pauseButton.UseVisualStyleBackColor = true;
            this.pauseButton.Click += new System.EventHandler(this.pauseButton_Click);
            // 
            // overallPanel
            // 
            this.overallPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.overallPanel.Controls.Add(this.overallLabel);
            this.overallPanel.Controls.Add(this.maxQLabel);
            this.overallPanel.Controls.Add(this.maxQTextBox);
            this.overallPanel.Controls.Add(this.totalMemoriesTextBox);
            this.overallPanel.Controls.Add(this.memoriesCountLabel);
            this.overallPanel.Controls.Add(this.epochCountLabel);
            this.overallPanel.Controls.Add(this.totalEpochsTextBox);
            this.overallPanel.Controls.Add(this.minRewardTextBox);
            this.overallPanel.Controls.Add(this.minRewardLabel);
            this.overallPanel.Controls.Add(this.maxRewardLabel);
            this.overallPanel.Controls.Add(this.maxRewardTextBox);
            this.overallPanel.Location = new System.Drawing.Point(280, 594);
            this.overallPanel.Name = "overallPanel";
            this.overallPanel.Size = new System.Drawing.Size(340, 185);
            this.overallPanel.TabIndex = 25;
            // 
            // overallLabel
            // 
            this.overallLabel.AutoSize = true;
            this.overallLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.overallLabel.Location = new System.Drawing.Point(6, 12);
            this.overallLabel.Name = "overallLabel";
            this.overallLabel.Size = new System.Drawing.Size(62, 20);
            this.overallLabel.TabIndex = 22;
            this.overallLabel.Text = "Overall";
            // 
            // maxQLabel
            // 
            this.maxQLabel.AutoSize = true;
            this.maxQLabel.Location = new System.Drawing.Point(6, 100);
            this.maxQLabel.Name = "maxQLabel";
            this.maxQLabel.Size = new System.Drawing.Size(81, 17);
            this.maxQLabel.TabIndex = 21;
            this.maxQLabel.Text = "Maximum Q";
            // 
            // maxQTextBox
            // 
            this.maxQTextBox.Location = new System.Drawing.Point(155, 97);
            this.maxQTextBox.Name = "maxQTextBox";
            this.maxQTextBox.ReadOnly = true;
            this.maxQTextBox.Size = new System.Drawing.Size(180, 22);
            this.maxQTextBox.TabIndex = 20;
            // 
            // totalMemoriesTextBox
            // 
            this.totalMemoriesTextBox.Location = new System.Drawing.Point(155, 153);
            this.totalMemoriesTextBox.Name = "totalMemoriesTextBox";
            this.totalMemoriesTextBox.ReadOnly = true;
            this.totalMemoriesTextBox.Size = new System.Drawing.Size(180, 22);
            this.totalMemoriesTextBox.TabIndex = 19;
            // 
            // memoriesCountLabel
            // 
            this.memoriesCountLabel.AutoSize = true;
            this.memoriesCountLabel.Location = new System.Drawing.Point(6, 156);
            this.memoriesCountLabel.Name = "memoriesCountLabel";
            this.memoriesCountLabel.Size = new System.Drawing.Size(110, 17);
            this.memoriesCountLabel.TabIndex = 18;
            this.memoriesCountLabel.Text = "Memories Count";
            // 
            // epochCountLabel
            // 
            this.epochCountLabel.AutoSize = true;
            this.epochCountLabel.Location = new System.Drawing.Point(6, 128);
            this.epochCountLabel.Name = "epochCountLabel";
            this.epochCountLabel.Size = new System.Drawing.Size(89, 17);
            this.epochCountLabel.TabIndex = 17;
            this.epochCountLabel.Text = "Epoch Count";
            // 
            // totalEpochsTextBox
            // 
            this.totalEpochsTextBox.Location = new System.Drawing.Point(155, 125);
            this.totalEpochsTextBox.Name = "totalEpochsTextBox";
            this.totalEpochsTextBox.ReadOnly = true;
            this.totalEpochsTextBox.Size = new System.Drawing.Size(180, 22);
            this.totalEpochsTextBox.TabIndex = 16;
            // 
            // minRewardTextBox
            // 
            this.minRewardTextBox.Location = new System.Drawing.Point(155, 68);
            this.minRewardTextBox.Name = "minRewardTextBox";
            this.minRewardTextBox.ReadOnly = true;
            this.minRewardTextBox.Size = new System.Drawing.Size(180, 22);
            this.minRewardTextBox.TabIndex = 15;
            // 
            // minRewardLabel
            // 
            this.minRewardLabel.AutoSize = true;
            this.minRewardLabel.Location = new System.Drawing.Point(6, 71);
            this.minRewardLabel.Name = "minRewardLabel";
            this.minRewardLabel.Size = new System.Drawing.Size(107, 17);
            this.minRewardLabel.TabIndex = 14;
            this.minRewardLabel.Text = "Minimal Reward";
            // 
            // maxRewardLabel
            // 
            this.maxRewardLabel.AutoSize = true;
            this.maxRewardLabel.Location = new System.Drawing.Point(6, 42);
            this.maxRewardLabel.Name = "maxRewardLabel";
            this.maxRewardLabel.Size = new System.Drawing.Size(118, 17);
            this.maxRewardLabel.TabIndex = 13;
            this.maxRewardLabel.Text = "Maximum Reward";
            // 
            // maxRewardTextBox
            // 
            this.maxRewardTextBox.Location = new System.Drawing.Point(155, 39);
            this.maxRewardTextBox.Name = "maxRewardTextBox";
            this.maxRewardTextBox.ReadOnly = true;
            this.maxRewardTextBox.Size = new System.Drawing.Size(180, 22);
            this.maxRewardTextBox.TabIndex = 12;
            // 
            // currentPanel
            // 
            this.currentPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.currentPanel.Controls.Add(this.currentEpisodesCountLabel);
            this.currentPanel.Controls.Add(this.currentEpochLabel);
            this.currentPanel.Controls.Add(this.currentMaxQLabel);
            this.currentPanel.Controls.Add(this.currentMaxQTextBox);
            this.currentPanel.Controls.Add(this.currentMemoriesCountTextBox);
            this.currentPanel.Controls.Add(this.currentMemoriesCountLabel);
            this.currentPanel.Controls.Add(this.currentEposidesCountTextBox);
            this.currentPanel.Controls.Add(this.currentMinRewardTextBox);
            this.currentPanel.Controls.Add(this.currentMinRewardLabel);
            this.currentPanel.Controls.Add(this.currentMaxRewardLabel);
            this.currentPanel.Controls.Add(this.currentMaxRewardTextBox);
            this.currentPanel.Location = new System.Drawing.Point(626, 594);
            this.currentPanel.Name = "currentPanel";
            this.currentPanel.Size = new System.Drawing.Size(340, 185);
            this.currentPanel.TabIndex = 26;
            // 
            // currentEpisodesCountLabel
            // 
            this.currentEpisodesCountLabel.AutoSize = true;
            this.currentEpisodesCountLabel.Location = new System.Drawing.Point(7, 128);
            this.currentEpisodesCountLabel.Name = "currentEpisodesCountLabel";
            this.currentEpisodesCountLabel.Size = new System.Drawing.Size(107, 17);
            this.currentEpisodesCountLabel.TabIndex = 34;
            this.currentEpisodesCountLabel.Text = "Episodes Count";
            // 
            // currentEpochLabel
            // 
            this.currentEpochLabel.AutoSize = true;
            this.currentEpochLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentEpochLabel.Location = new System.Drawing.Point(6, 12);
            this.currentEpochLabel.Name = "currentEpochLabel";
            this.currentEpochLabel.Size = new System.Drawing.Size(117, 20);
            this.currentEpochLabel.TabIndex = 33;
            this.currentEpochLabel.Text = "Current Epoch";
            // 
            // currentMaxQLabel
            // 
            this.currentMaxQLabel.AutoSize = true;
            this.currentMaxQLabel.Location = new System.Drawing.Point(6, 100);
            this.currentMaxQLabel.Name = "currentMaxQLabel";
            this.currentMaxQLabel.Size = new System.Drawing.Size(81, 17);
            this.currentMaxQLabel.TabIndex = 32;
            this.currentMaxQLabel.Text = "Maximum Q";
            // 
            // currentMaxQTextBox
            // 
            this.currentMaxQTextBox.Location = new System.Drawing.Point(155, 97);
            this.currentMaxQTextBox.Name = "currentMaxQTextBox";
            this.currentMaxQTextBox.ReadOnly = true;
            this.currentMaxQTextBox.Size = new System.Drawing.Size(180, 22);
            this.currentMaxQTextBox.TabIndex = 31;
            // 
            // currentMemoriesCountTextBox
            // 
            this.currentMemoriesCountTextBox.Location = new System.Drawing.Point(155, 153);
            this.currentMemoriesCountTextBox.Name = "currentMemoriesCountTextBox";
            this.currentMemoriesCountTextBox.ReadOnly = true;
            this.currentMemoriesCountTextBox.Size = new System.Drawing.Size(180, 22);
            this.currentMemoriesCountTextBox.TabIndex = 30;
            // 
            // currentMemoriesCountLabel
            // 
            this.currentMemoriesCountLabel.AutoSize = true;
            this.currentMemoriesCountLabel.Location = new System.Drawing.Point(6, 156);
            this.currentMemoriesCountLabel.Name = "currentMemoriesCountLabel";
            this.currentMemoriesCountLabel.Size = new System.Drawing.Size(110, 17);
            this.currentMemoriesCountLabel.TabIndex = 29;
            this.currentMemoriesCountLabel.Text = "Memories Count";
            // 
            // currentEposidesCountTextBox
            // 
            this.currentEposidesCountTextBox.Location = new System.Drawing.Point(155, 125);
            this.currentEposidesCountTextBox.Name = "currentEposidesCountTextBox";
            this.currentEposidesCountTextBox.ReadOnly = true;
            this.currentEposidesCountTextBox.Size = new System.Drawing.Size(180, 22);
            this.currentEposidesCountTextBox.TabIndex = 28;
            // 
            // currentMinRewardTextBox
            // 
            this.currentMinRewardTextBox.Location = new System.Drawing.Point(155, 68);
            this.currentMinRewardTextBox.Name = "currentMinRewardTextBox";
            this.currentMinRewardTextBox.ReadOnly = true;
            this.currentMinRewardTextBox.Size = new System.Drawing.Size(180, 22);
            this.currentMinRewardTextBox.TabIndex = 27;
            // 
            // currentMinRewardLabel
            // 
            this.currentMinRewardLabel.AutoSize = true;
            this.currentMinRewardLabel.Location = new System.Drawing.Point(6, 71);
            this.currentMinRewardLabel.Name = "currentMinRewardLabel";
            this.currentMinRewardLabel.Size = new System.Drawing.Size(107, 17);
            this.currentMinRewardLabel.TabIndex = 26;
            this.currentMinRewardLabel.Text = "Minimal Reward";
            // 
            // currentMaxRewardLabel
            // 
            this.currentMaxRewardLabel.AutoSize = true;
            this.currentMaxRewardLabel.Location = new System.Drawing.Point(6, 42);
            this.currentMaxRewardLabel.Name = "currentMaxRewardLabel";
            this.currentMaxRewardLabel.Size = new System.Drawing.Size(118, 17);
            this.currentMaxRewardLabel.TabIndex = 25;
            this.currentMaxRewardLabel.Text = "Maximum Reward";
            // 
            // currentMaxRewardTextBox
            // 
            this.currentMaxRewardTextBox.Location = new System.Drawing.Point(155, 39);
            this.currentMaxRewardTextBox.Name = "currentMaxRewardTextBox";
            this.currentMaxRewardTextBox.ReadOnly = true;
            this.currentMaxRewardTextBox.Size = new System.Drawing.Size(180, 22);
            this.currentMaxRewardTextBox.TabIndex = 24;
            // 
            // brokerStatPanel
            // 
            this.brokerStatPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.brokerStatPanel.Controls.Add(this.tradeCountPlotView);
            this.brokerStatPanel.Controls.Add(this.avgDurationPlotView);
            this.brokerStatPanel.Controls.Add(this.avgProfitPlotView);
            this.brokerStatPanel.Controls.Add(this.brokerLabel);
            this.brokerStatPanel.Location = new System.Drawing.Point(1239, 6);
            this.brokerStatPanel.Name = "brokerStatPanel";
            this.brokerStatPanel.Size = new System.Drawing.Size(554, 773);
            this.brokerStatPanel.TabIndex = 27;
            // 
            // avgDurationPlotView
            // 
            this.avgDurationPlotView.Location = new System.Drawing.Point(9, 283);
            this.avgDurationPlotView.Name = "avgDurationPlotView";
            this.avgDurationPlotView.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.avgDurationPlotView.Size = new System.Drawing.Size(529, 234);
            this.avgDurationPlotView.TabIndex = 37;
            this.avgDurationPlotView.Text = "plotView1";
            this.avgDurationPlotView.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.avgDurationPlotView.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.avgDurationPlotView.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // avgProfitPlotView
            // 
            this.avgProfitPlotView.Location = new System.Drawing.Point(9, 43);
            this.avgProfitPlotView.Name = "avgProfitPlotView";
            this.avgProfitPlotView.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.avgProfitPlotView.Size = new System.Drawing.Size(529, 234);
            this.avgProfitPlotView.TabIndex = 35;
            this.avgProfitPlotView.Text = "plotView1";
            this.avgProfitPlotView.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.avgProfitPlotView.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.avgProfitPlotView.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // brokerLabel
            // 
            this.brokerLabel.AutoSize = true;
            this.brokerLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.brokerLabel.Location = new System.Drawing.Point(6, 11);
            this.brokerLabel.Name = "brokerLabel";
            this.brokerLabel.Size = new System.Drawing.Size(59, 20);
            this.brokerLabel.TabIndex = 33;
            this.brokerLabel.Text = "Broker";
            // 
            // currentSettingsPanel
            // 
            this.currentSettingsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.currentSettingsPanel.Controls.Add(this.loadedStocksTextBox);
            this.currentSettingsPanel.Controls.Add(this.label7);
            this.currentSettingsPanel.Controls.Add(this.epsilonTextBox);
            this.currentSettingsPanel.Controls.Add(this.minimumEpsilonTextBox);
            this.currentSettingsPanel.Controls.Add(this.label8);
            this.currentSettingsPanel.Controls.Add(this.label9);
            this.currentSettingsPanel.Controls.Add(this.epsilonDecayTextBox);
            this.currentSettingsPanel.Controls.Add(this.discountTextBox);
            this.currentSettingsPanel.Controls.Add(this.label10);
            this.currentSettingsPanel.Controls.Add(this.label11);
            this.currentSettingsPanel.Controls.Add(this.batchSizeTextBox);
            this.currentSettingsPanel.Controls.Add(this.label2);
            this.currentSettingsPanel.Controls.Add(this.hiddenLayersTextBox);
            this.currentSettingsPanel.Controls.Add(this.bufferSizeTextBox);
            this.currentSettingsPanel.Controls.Add(this.label3);
            this.currentSettingsPanel.Controls.Add(this.label4);
            this.currentSettingsPanel.Controls.Add(this.learningRateTextBox);
            this.currentSettingsPanel.Controls.Add(this.commissionTextBox);
            this.currentSettingsPanel.Controls.Add(this.label5);
            this.currentSettingsPanel.Controls.Add(this.label6);
            this.currentSettingsPanel.Controls.Add(this.initialCapitalTextBox);
            this.currentSettingsPanel.Controls.Add(this.label1);
            this.currentSettingsPanel.Location = new System.Drawing.Point(4, 6);
            this.currentSettingsPanel.Name = "currentSettingsPanel";
            this.currentSettingsPanel.Size = new System.Drawing.Size(270, 773);
            this.currentSettingsPanel.TabIndex = 28;
            // 
            // loadedStocksTextBox
            // 
            this.loadedStocksTextBox.FormattingEnabled = true;
            this.loadedStocksTextBox.ItemHeight = 16;
            this.loadedStocksTextBox.Location = new System.Drawing.Point(7, 329);
            this.loadedStocksTextBox.Name = "loadedStocksTextBox";
            this.loadedStocksTextBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.loadedStocksTextBox.Size = new System.Drawing.Size(254, 436);
            this.loadedStocksTextBox.TabIndex = 42;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 243);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 17);
            this.label7.TabIndex = 41;
            this.label7.Text = "Initial Epsilon";
            // 
            // epsilonTextBox
            // 
            this.epsilonTextBox.Location = new System.Drawing.Point(156, 240);
            this.epsilonTextBox.Name = "epsilonTextBox";
            this.epsilonTextBox.ReadOnly = true;
            this.epsilonTextBox.Size = new System.Drawing.Size(105, 22);
            this.epsilonTextBox.TabIndex = 40;
            // 
            // minimumEpsilonTextBox
            // 
            this.minimumEpsilonTextBox.Location = new System.Drawing.Point(156, 296);
            this.minimumEpsilonTextBox.Name = "minimumEpsilonTextBox";
            this.minimumEpsilonTextBox.ReadOnly = true;
            this.minimumEpsilonTextBox.Size = new System.Drawing.Size(105, 22);
            this.minimumEpsilonTextBox.TabIndex = 39;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 299);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(113, 17);
            this.label8.TabIndex = 38;
            this.label8.Text = "Minimum Epsilon";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 271);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(98, 17);
            this.label9.TabIndex = 37;
            this.label9.Text = "Epsilon Decay";
            // 
            // epsilonDecayTextBox
            // 
            this.epsilonDecayTextBox.Location = new System.Drawing.Point(156, 268);
            this.epsilonDecayTextBox.Name = "epsilonDecayTextBox";
            this.epsilonDecayTextBox.ReadOnly = true;
            this.epsilonDecayTextBox.Size = new System.Drawing.Size(105, 22);
            this.epsilonDecayTextBox.TabIndex = 36;
            // 
            // discountTextBox
            // 
            this.discountTextBox.Location = new System.Drawing.Point(156, 211);
            this.discountTextBox.Name = "discountTextBox";
            this.discountTextBox.ReadOnly = true;
            this.discountTextBox.Size = new System.Drawing.Size(105, 22);
            this.discountTextBox.TabIndex = 35;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 214);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(63, 17);
            this.label10.TabIndex = 34;
            this.label10.Text = "Discount";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(7, 185);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(75, 17);
            this.label11.TabIndex = 33;
            this.label11.Text = "Batch Size";
            // 
            // batchSizeTextBox
            // 
            this.batchSizeTextBox.Location = new System.Drawing.Point(156, 182);
            this.batchSizeTextBox.Name = "batchSizeTextBox";
            this.batchSizeTextBox.ReadOnly = true;
            this.batchSizeTextBox.Size = new System.Drawing.Size(105, 22);
            this.batchSizeTextBox.TabIndex = 32;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 17);
            this.label2.TabIndex = 31;
            this.label2.Text = "Hidden Layers";
            // 
            // hiddenLayersTextBox
            // 
            this.hiddenLayersTextBox.Location = new System.Drawing.Point(156, 98);
            this.hiddenLayersTextBox.Name = "hiddenLayersTextBox";
            this.hiddenLayersTextBox.ReadOnly = true;
            this.hiddenLayersTextBox.Size = new System.Drawing.Size(105, 22);
            this.hiddenLayersTextBox.TabIndex = 30;
            // 
            // bufferSizeTextBox
            // 
            this.bufferSizeTextBox.Location = new System.Drawing.Point(156, 154);
            this.bufferSizeTextBox.Name = "bufferSizeTextBox";
            this.bufferSizeTextBox.ReadOnly = true;
            this.bufferSizeTextBox.Size = new System.Drawing.Size(105, 22);
            this.bufferSizeTextBox.TabIndex = 29;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 157);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 17);
            this.label3.TabIndex = 28;
            this.label3.Text = "Buffer Size";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 129);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 17);
            this.label4.TabIndex = 27;
            this.label4.Text = "Learning Rate";
            // 
            // learningRateTextBox
            // 
            this.learningRateTextBox.Location = new System.Drawing.Point(156, 126);
            this.learningRateTextBox.Name = "learningRateTextBox";
            this.learningRateTextBox.ReadOnly = true;
            this.learningRateTextBox.Size = new System.Drawing.Size(105, 22);
            this.learningRateTextBox.TabIndex = 26;
            // 
            // commissionTextBox
            // 
            this.commissionTextBox.Location = new System.Drawing.Point(156, 69);
            this.commissionTextBox.Name = "commissionTextBox";
            this.commissionTextBox.ReadOnly = true;
            this.commissionTextBox.Size = new System.Drawing.Size(105, 22);
            this.commissionTextBox.TabIndex = 25;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 17);
            this.label5.TabIndex = 24;
            this.label5.Text = "Commission";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 17);
            this.label6.TabIndex = 23;
            this.label6.Text = "Initial Capital";
            // 
            // initialCapitalTextBox
            // 
            this.initialCapitalTextBox.Location = new System.Drawing.Point(156, 40);
            this.initialCapitalTextBox.Name = "initialCapitalTextBox";
            this.initialCapitalTextBox.ReadOnly = true;
            this.initialCapitalTextBox.Size = new System.Drawing.Size(105, 22);
            this.initialCapitalTextBox.TabIndex = 22;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Current Settings";
            // 
            // plotUpdateTimer
            // 
            this.plotUpdateTimer.Interval = 5000;
            this.plotUpdateTimer.Tick += new System.EventHandler(this.plotUpdateTimer_Tick);
            // 
            // lossPlotView
            // 
            this.lossPlotView.Location = new System.Drawing.Point(281, 307);
            this.lossPlotView.Name = "lossPlotView";
            this.lossPlotView.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.lossPlotView.Size = new System.Drawing.Size(470, 271);
            this.lossPlotView.TabIndex = 29;
            this.lossPlotView.Text = "rewardPlot";
            this.lossPlotView.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.lossPlotView.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.lossPlotView.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // epsilonPlotView
            // 
            this.epsilonPlotView.Location = new System.Drawing.Point(763, 307);
            this.epsilonPlotView.Name = "epsilonPlotView";
            this.epsilonPlotView.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.epsilonPlotView.Size = new System.Drawing.Size(470, 271);
            this.epsilonPlotView.TabIndex = 30;
            this.epsilonPlotView.Text = "rewardPlot";
            this.epsilonPlotView.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.epsilonPlotView.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.epsilonPlotView.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // tradeCountPlowView
            // 
            this.tradeCountPlotView.Location = new System.Drawing.Point(9, 523);
            this.tradeCountPlotView.Name = "tradeCountPlowView";
            this.tradeCountPlotView.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.tradeCountPlotView.Size = new System.Drawing.Size(529, 234);
            this.tradeCountPlotView.TabIndex = 38;
            this.tradeCountPlotView.Text = "plotView1";
            this.tradeCountPlotView.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.tradeCountPlotView.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.tradeCountPlotView.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1805, 791);
            this.Controls.Add(this.epsilonPlotView);
            this.Controls.Add(this.lossPlotView);
            this.Controls.Add(this.currentSettingsPanel);
            this.Controls.Add(this.brokerStatPanel);
            this.Controls.Add(this.currentPanel);
            this.Controls.Add(this.overallPanel);
            this.Controls.Add(this.setupPanel);
            this.Controls.Add(this.mainPlotView);
            this.Name = "MainForm";
            this.Text = "DQN Learning Interface";
            this.setupPanel.ResumeLayout(false);
            this.overallPanel.ResumeLayout(false);
            this.overallPanel.PerformLayout();
            this.currentPanel.ResumeLayout(false);
            this.currentPanel.PerformLayout();
            this.brokerStatPanel.ResumeLayout(false);
            this.brokerStatPanel.PerformLayout();
            this.currentSettingsPanel.ResumeLayout(false);
            this.currentSettingsPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private OxyPlot.WindowsForms.PlotView mainPlotView;
        private System.Windows.Forms.Panel setupPanel;
        private System.Windows.Forms.Panel overallPanel;
        private System.Windows.Forms.Label overallLabel;
        private System.Windows.Forms.Label maxQLabel;
        private System.Windows.Forms.TextBox maxQTextBox;
        private System.Windows.Forms.TextBox totalMemoriesTextBox;
        private System.Windows.Forms.Label memoriesCountLabel;
        private System.Windows.Forms.Label epochCountLabel;
        private System.Windows.Forms.TextBox totalEpochsTextBox;
        private System.Windows.Forms.TextBox minRewardTextBox;
        private System.Windows.Forms.Label minRewardLabel;
        private System.Windows.Forms.Label maxRewardLabel;
        private System.Windows.Forms.TextBox maxRewardTextBox;
        private System.Windows.Forms.Panel currentPanel;
        private System.Windows.Forms.Label currentEpisodesCountLabel;
        private System.Windows.Forms.Label currentEpochLabel;
        private System.Windows.Forms.Label currentMaxQLabel;
        private System.Windows.Forms.TextBox currentMaxQTextBox;
        private System.Windows.Forms.TextBox currentMemoriesCountTextBox;
        private System.Windows.Forms.Label currentMemoriesCountLabel;
        private System.Windows.Forms.TextBox currentEposidesCountTextBox;
        private System.Windows.Forms.TextBox currentMinRewardTextBox;
        private System.Windows.Forms.Label currentMinRewardLabel;
        private System.Windows.Forms.Label currentMaxRewardLabel;
        private System.Windows.Forms.TextBox currentMaxRewardTextBox;
        private System.Windows.Forms.Button pauseButton;
        private System.Windows.Forms.Panel brokerStatPanel;
        private OxyPlot.WindowsForms.PlotView avgProfitPlotView;
        private System.Windows.Forms.Label brokerLabel;
        private System.Windows.Forms.Button createDQNButton;
        private System.Windows.Forms.Button loadDQNButton;
        private System.Windows.Forms.Panel currentSettingsPanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox hiddenLayersTextBox;
        private System.Windows.Forms.TextBox bufferSizeTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox learningRateTextBox;
        private System.Windows.Forms.TextBox commissionTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox initialCapitalTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox epsilonTextBox;
        private System.Windows.Forms.TextBox minimumEpsilonTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox epsilonDecayTextBox;
        private System.Windows.Forms.TextBox discountTextBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox batchSizeTextBox;
        private System.Windows.Forms.ListBox loadedStocksTextBox;
        private System.Windows.Forms.Timer plotUpdateTimer;
        private OxyPlot.WindowsForms.PlotView lossPlotView;
        private OxyPlot.WindowsForms.PlotView epsilonPlotView;
        private OxyPlot.WindowsForms.PlotView avgDurationPlotView;
        private OxyPlot.WindowsForms.PlotView tradeCountPlotView;
    }
}

