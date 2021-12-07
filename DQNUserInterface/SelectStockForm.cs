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
    public partial class SelectStockForm : Form
    {
        public DQNSetupPresenter Presenter { get; private set; }
        public SelectStockForm(DQNSetupPresenter setupPresenter)
        {
            InitializeComponent();
            Presenter = setupPresenter;           
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SelectStockForm_Shown(object sender, EventArgs e)
        {           
            BindingSource bindingSource = new BindingSource();
            bindingSource.Add(Presenter);
          
            intervalBox.Items.AddRange(Presenter.Intervals.Keys.ToArray());
            stockSymbolBox.DataBindings.Add("Text", bindingSource, "SelectedDownloadStock");

            UpdateBoxes();
        }

        void UpdateBoxes()
        {
            availableStocksBox.DataSource = null;
            loadedStocksBox.DataSource = null;

            Presenter.GetAvailableStocks();

            availableStocksBox.DataSource = Presenter.AvailableStocks;
            loadedStocksBox.DataSource = Presenter.AddedStocks;
        }

        private void downloadButton_Click(object sender, EventArgs e)
        {
            if (stockSymbolBox.Text == String.Empty)
                return;

            Presenter.SelectedIntervals = intervalBox.SelectedIndices.Cast<int>().ToList();
            Presenter.DownloadStock();

            Presenter.GetAvailableStocks();

            intervalBox.ClearSelected();
            stockSymbolBox.Text = String.Empty;

            UpdateBoxes();
        }

        private void addStockDataButton_Click(object sender, EventArgs e)
        {
            Presenter.SelectedAvailableStocks = availableStocksBox.SelectedIndices.Cast<int>().ToList();
            Presenter.MoveStockToLoaded();

            UpdateBoxes();
        }

        private void addAllButton_Click(object sender, EventArgs e)
        {
            Presenter.SelectedAvailableStocks = availableStocksBox.SelectedIndices.Cast<int>().ToList();
            Presenter.MoveAllStocksToLoaded();

            UpdateBoxes();
        }

        private void removeAddedButton_Click(object sender, EventArgs e)
        {
            Presenter.SelectedAddedStocks = loadedStocksBox.SelectedIndices.Cast<int>().ToList();
            Presenter.MoveStockFromLoaded();

            UpdateBoxes();
        }

        private void removeAllButton_Click(object sender, EventArgs e)
        {
            Presenter.SelectedAddedStocks = loadedStocksBox.SelectedIndices.Cast<int>().ToList();
            Presenter.MoveAllStocksFromLoaded();

            UpdateBoxes();
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            if(Presenter.AddedStocks.Count != 0)
            {
                Presenter.ConfirmStockDataSelection();              
                Close();
            }
               
        }
    }
}
