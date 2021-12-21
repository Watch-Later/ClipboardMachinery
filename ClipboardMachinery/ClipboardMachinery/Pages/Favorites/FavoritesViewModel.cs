using Caliburn.Micro;
using ClipboardMachinery.Common.Events;
using ClipboardMachinery.Common.Screen;
using ClipboardMachinery.Components.Navigator;
using ClipboardMachinery.Core.DataStorage;
using ClipboardMachinery.Core.DataStorage.Impl;
using ClipboardMachinery.Plumbing.Factories;

namespace ClipboardMachinery.Pages.Favorites {

    public class FavoritesViewModel : ClipPageBase, IScreenPage {

        #region IScreenPage

        public string Title { get; } = "Favorites";

        public string Icon { get; } = "IconStarFull";

        public byte Order { get; } = 2;

        #endregion

        public FavoritesViewModel(IDataRepository dataRepository, IEventAggregator eventAggregator, IClipFactory vmFactory) : base(15, dataRepository, eventAggregator, vmFactory) {
            ((ClipLazyProvider) DataProvider).ApplyTagFilter("category");
        }

        #region Logic

        protected override bool IsAllowedAddClipsFromKeyboard(ClipEvent message) {
            return false;
        }

        protected override bool IsClearingItemsWhenDeactivating() {
            return true;
        }

        #endregion

    }

}
