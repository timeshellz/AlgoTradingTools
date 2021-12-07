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
    public partial class SelectDQNForm : Form
    {
        public DQNSelectPresenter Presenter { get; private set; }
        public SelectDQNForm(DQNSelectPresenter setupPresenter)
        {
            Presenter = setupPresenter;
            InitializeComponent();
        }

        private void SelectDQNForm_Shown(object sender, EventArgs e)
        {
            dqnListBox.DataSource = Presenter.AvailableDQNs;
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            if(dqnListBox.SelectedItem.ToString() != null)
            {
                Presenter.SelectedDQN = dqnListBox.SelectedItem.ToString();
                Presenter.LoadSelectedDQN();

                Close();
            }           
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }
    }
}
