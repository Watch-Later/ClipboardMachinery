﻿using Caliburn.Micro;
using ClipboardMachinery.Common.Events;
using ClipboardMachinery.Components.Clip;
using ClipboardMachinery.Core.Repositories;
using ClipboardMachinery.Plumbing.Factories;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static ClipboardMachinery.Common.Events.ClipEvent;

namespace ClipboardMachinery.Pages {

    public abstract class ClipPageBase : Screen, IHandle<ClipEvent> {

        #region Properties

        public bool WatermarkIsVisible
            => ClipboardItems.Count == 0;

        public BindableCollection<ClipViewModel> ClipboardItems {
            get;
        }

        #endregion

        #region Fields

        protected readonly IDataRepository dataRepository;
        protected readonly IClipViewModelFactory clipVmFactory;
        protected bool AllowAddingClipsFromKeyboard = true;

        #endregion

        public ClipPageBase(IDataRepository dataRepository, IClipViewModelFactory clipVmFactory) {
            this.dataRepository = dataRepository;
            this.clipVmFactory = clipVmFactory;

            ClipboardItems = new BindableCollection<ClipViewModel>();
            ClipboardItems.CollectionChanged += OnClipboardItemsCollectionChanged;
        }

        #region Handlers

        private void OnClipboardItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            switch (e.Action) {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Reset:
                    NotifyOfPropertyChange(() => WatermarkIsVisible);
                    break;
            }
        }

        public async Task HandleAsync(ClipEvent message, CancellationToken cancellationToken) {
            switch(message.EventType) {
                case ClipEventType.Created:
                    if (!AllowAddingClipsFromKeyboard) {
                        return;
                    }

                    ClipboardItems.Insert(0, clipVmFactory.Create(message.Source));
                    break;

                case ClipEventType.Remove:
                    ClipViewModel clip = ClipboardItems.FirstOrDefault(vm => vm.Model == message.Source);
                    if (clip != null) {
                        ClipboardItems.Remove(clip);
                        await dataRepository.DeleteClip(clip.Model.Id);
                        clipVmFactory.Release(clip);
                    }
                    break;
            }
        }

        #endregion

    }

}
