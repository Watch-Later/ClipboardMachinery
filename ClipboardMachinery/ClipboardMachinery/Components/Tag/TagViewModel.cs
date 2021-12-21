using Caliburn.Micro;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using ClipboardMachinery.Components.DialogOverlay;
using Castle.Core;
using ClipboardMachinery.Core.TagKind;

namespace ClipboardMachinery.Components.Tag {

    public class TagViewModel : Screen {

        #region Properties

        [DoNotWire]
        public TagModel Model {
            get => model;
            private set {
                if (model == value) {
                    return;
                }

                if (model != null) {
                    model.PropertyChanged -= OnModelPropertyChanged;
                }

                if (value != null) {
                    value.PropertyChanged += OnModelPropertyChanged;
                }

                model = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => HasDescription);
                NotifyOfPropertyChange(() => BackgroundColor);
                // TODO: Read display value again
            }
        }

        public string DisplayValue {
            get => displayValue;
            private set {
                if (displayValue == value) {
                    return;
                }

                displayValue = value;
                NotifyOfPropertyChange();
            }
        }

        public bool HasDescription {
            get => !string.IsNullOrWhiteSpace(Model?.Description);
        }

        public Color BackgroundColor {
            get => Model?.Color.HasValue == true ? Model.Color.Value : Colors.Transparent;
        }

        #endregion

        #region Fields

        private readonly ITagKindManager tagKindManager;
        private readonly IDialogOverlayManager dialogOverlayManager;

        private TagModel model;
        private string displayValue;

        #endregion

        public TagViewModel(TagModel tagModel, ITagKindManager tagKindManager, IDialogOverlayManager dialogOverlayManager) {
            this.tagKindManager = tagKindManager;
            this.dialogOverlayManager = dialogOverlayManager;
            Model = tagModel;
        }

        #region Handlers

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken) {
            await base.OnInitializeAsync(cancellationToken);
            DisplayValue = Model?.Value != null ? await tagKindManager.GetText(Model.Kind, Model.Value) : null;
        }

        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken) {
            if (close) {
                Model = null;
            }

            return Task.CompletedTask;
        }

        private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e) {
            switch (e.PropertyName) {
                case nameof(TagModel.Value):
                    NotifyOfPropertyChange(() => DisplayValue);
                    break;

                case nameof(TagModel.Color):
                    NotifyOfPropertyChange(() => BackgroundColor);
                    break;

                case nameof(TagModel.Description):
                    NotifyOfPropertyChange(() => HasDescription);
                    break;
            }
        }

        #endregion

        #region Actions

        public void Edit() {
            dialogOverlayManager.OpenDialog(
                () => dialogOverlayManager.Factory.CreateTagEditor(Model),
                tagEditor => dialogOverlayManager.Factory.Release(tagEditor)
            );
        }

        #endregion

    }

}
