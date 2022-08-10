using System.Threading.Tasks;

namespace TradeBubble.ViewModels
{
    public abstract class PageViewModel : ViewModel
    {
        public bool IsLoaded { get; set; } = false;

        public virtual async Task InitializeAsync()
        {
            try
            {
                if (!IsLoaded)
                    await LoadData();

                IsLoaded = true;
            }
            catch
            {
                throw;
            }
        }

        protected abstract Task LoadData();
    }
}
