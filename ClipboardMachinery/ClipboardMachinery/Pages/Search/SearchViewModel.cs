using Caliburn.Micro;
using ClipboardMachinery.Components.Navigator;

namespace ClipboardMachinery.Pages.Search {

    public class SearchViewModel : Screen, IScreenPage {

        #region IScreenPage

        public string Title { get; } = "Search";

        public string Icon { get; } = "IconSearch";

        public byte Order { get; } = 3;

        #endregion

    }

}
