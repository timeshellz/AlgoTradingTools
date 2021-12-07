using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTrading.DQN.UserInterface
{
    public class DQNSelectPresenter
    {
        public DQNMainPresenter MainPresenter { get; private set; }

        public List<string> AvailableDQNs { get; set; }
        public string SelectedDQN { get; set; }

        public DQNSelectPresenter(DQNMainPresenter mainPresenter)
        {
            MainPresenter = mainPresenter;

            GetAvailableDQNs();
        }

        void GetAvailableDQNs()
        {
            AvailableDQNs = DQNMetaFileManager.GetAvailableMeta();
        }

        public void LoadSelectedDQN()
        {
            MainPresenter.LoadOldDQN(SelectedDQN + ".json");
        }
    }
}
