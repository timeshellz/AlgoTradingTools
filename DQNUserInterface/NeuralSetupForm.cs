using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlgoTrading.DQN.UserInterface
{
    public partial class NeuralSetupForm : Form
    {
        public DQNSetupPresenter Presenter { get; private set; }
        

        public NeuralSetupForm(DQNSetupPresenter setupPresenter)
        {
            InitializeComponent();
            Presenter = setupPresenter;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            bool formFill = AreFormsFilled();

            if(!formFill)
            {
                formFillLabel.Text = "Not all data provided.";
                formFillLabel.Visible = true;
            }
            else
            {
                

                SelectStockForm stockForm = new SelectStockForm(Presenter);
                stockForm.ShowDialog();

                Presenter.GenerateDQN();
                Close();
            }
        }

        bool AreFormsFilled()
        {
            foreach(Panel panel in Controls.OfType<Panel>())
            {
                List<TextBox> textBoxes = panel.Controls.OfType<TextBox>().ToList();

                if(textBoxes.Count > 0)
                {
                    if (textBoxes.Where(e => e.Text == String.Empty).Count() != 0)
                        return false;
                }
            }

            return true;
        }

        private void NeuralSetupForm_Shown(object sender, EventArgs e)
        {
            BindingSource bindingSource = new BindingSource();
            bindingSource.Add(Presenter);

            initialCapitalTextBox.DataBindings.Add("Text", bindingSource, nameof(Presenter.InitialCapital));
            commissionTextBox.DataBindings.Add("Text", bindingSource, nameof(Presenter.Commission));
            networkNameTextBox.DataBindings.Add("Text", bindingSource, nameof(Presenter.NetworkName));
            hiddenLayersTextBox.DataBindings.Add("Text", bindingSource, nameof(Presenter.HiddenLayers));
            learningRateTextBox.DataBindings.Add("Text", bindingSource, nameof(Presenter.LearningRate));
            agentNameTextBox.DataBindings.Add("Text", bindingSource, nameof(Presenter.AgentName));
            bufferSizeTextBox.DataBindings.Add("Text", bindingSource, nameof(Presenter.BufferSize));
            batchSizeTextBox.DataBindings.Add("Text", bindingSource, nameof(Presenter.BatchSize));
            stepCountTextBox.DataBindings.Add("Text", bindingSource, nameof(Presenter.StepCount));
            discountTextBox.DataBindings.Add("Text", bindingSource, nameof(Presenter.Discount));
            epsilonTextBox.DataBindings.Add("Text", bindingSource, nameof(Presenter.Epsilon));
            epsilonDecayTextBox.DataBindings.Add("Text", bindingSource, nameof(Presenter.EpsilonDecay));
            minEpsilonTextBox.DataBindings.Add("Text", bindingSource, nameof(Presenter.MinimumEpsilon));
            tauTextBox.DataBindings.Add("Text", bindingSource, nameof(Presenter.Tau));
        }

        private void NeuralSetupForm_Load(object sender, EventArgs e)
        {

        }
    }
}
